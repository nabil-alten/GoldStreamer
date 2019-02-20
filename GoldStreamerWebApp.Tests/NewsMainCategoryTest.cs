using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class NewsMainCategoryTest
    {
        public UnitOfWork _uow;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }
        public int AddNewMainCategory()
        {
            NewsMainCategory newMainCategory = new NewsMainCategory()
            {
                MainCategoryName = "NewMainCategory",
                MainCategoryOrder = 1,
            };
            _uow.NewsMainCategoryRepo.AddNewsMainCategory(newMainCategory);
            _uow.SaveChanges();
            return newMainCategory.ID;
        }

        
        [TestMethod]
        public void CanGetListMainCategories()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewMainCategory();

            Assert.AreEqual(1, _uow.NewsMainCategoryRepo.GetNewsMainCatetoryList("").Count);
        }

        [TestMethod]
        public void CanAddNewMainCategory()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            AddNewMainCategory();
            Assert.AreEqual(1, _uow.NewsMainCategoryRepo.GetNewsMainCatetoryList("").Count);
        }

        [TestMethod]
        public void CanAddNewMainCategoryTest()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            AddNewMainCategory();
            Assert.AreEqual(true, _uow.NewsMainCategoryRepo.CanAddMainCategory("2"));
        }

        [TestMethod]
        public void CanUpdateNewMainCategory()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewMainCategory();

            NewsMainCategory mainCategory = _uow.NewsMainCategoryRepo.Find(ID);
            mainCategory.MainCategoryName = "UpdatedMainCategory";
            _uow.NewsMainCategoryRepo.Update(mainCategory);
            _uow.SaveChanges();
            Assert.AreEqual("UpdatedMainCategory", _uow.NewsMainCategoryRepo.Find(ID).MainCategoryName);
        }

        [TestMethod]
        public void CanDeleteQuestionGroup()
        {
            _uow.NewsMainCategoryRepo.DeleteAll();
            _uow.SaveChanges();
            int ID = AddNewMainCategory();

            _uow.NewsMainCategoryRepo.DeleteByID(ID);
            _uow.SaveChanges();

            Assert.AreEqual(null, _uow.NewsMainCategoryRepo.Find(ID));
        }

        [TestMethod]
        public void CanCheckNameExsists()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewMainCategory();
            int isExsists = _uow.NewsMainCategoryRepo.CheckNameExists("NewMainCategory", ID);
            Assert.AreEqual(0, isExsists);
        }

        [TestMethod]
        public void CanCheckOrderExsists()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewMainCategory();
            int isExsists = _uow.NewsMainCategoryRepo.CheckOrderExists(1, ID);
            Assert.AreEqual(0, isExsists);
        }

        [TestMethod]
        public void CanDeleteAllMainCategories()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            Assert.AreEqual(0, _uow.NewsMainCategoryRepo.GetNewsMainCatetoryList("").Count);
        }


    }
}
