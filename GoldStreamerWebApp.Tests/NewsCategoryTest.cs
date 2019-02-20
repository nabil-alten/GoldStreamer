using System;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.DomainClasses;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class NewsCategoryTest
    {
        public UnitOfWork _uow;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }
        
        public int AddNewCategory()
        {
            NewsMainCategory newMainCategory = new NewsMainCategory()
            {
                MainCategoryName = "NewMainCategory",
                MainCategoryOrder = 1,
            };
            _uow.NewsMainCategoryRepo.AddNewsMainCategory(newMainCategory);
            _uow.SaveChanges();

       
            NewsCategory category = new NewsCategory()
            {
                CategoryName = "NewCategory",
                CategoryOrder = 1,
                MainCategoryId = newMainCategory.ID,
            };

            _uow.NewsCategoryRepo.AddNewsCategory(category);
            _uow.SaveChanges();
            return category.Id;
        }


        [TestMethod]
        public void CanGetCategoryByMainCategory()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            AddNewCategory();
           int mainCategoryID = _uow.NewsMainCategoryRepo.FindNewsMainCategoryByName("NewMainCategory").ID;
           Assert.AreEqual(1, _uow.NewsCategoryRepo.GetByMainCategory(mainCategoryID).Count);
        }

        [TestMethod]
        public void CanGetListMainCategories()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewCategory();
            Assert.AreEqual(1, _uow.NewsCategoryRepo.GetNewsCatetoryList("",null).Count);
        }

        [TestMethod]
        public void CanAddNewCategory()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            AddNewCategory();
            Assert.AreEqual(1, _uow.NewsCategoryRepo.GetNewsCatetoryList("",null).Count);
        }

        [TestMethod]
        public void CanUpdateNewCategory()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewCategory();

            NewsCategory mainCategory = _uow.NewsCategoryRepo.Find(ID);
            mainCategory.CategoryName = "UpdatedCategory";
            _uow.NewsCategoryRepo.Update(mainCategory);
            _uow.SaveChanges();
            Assert.AreEqual("UpdatedCategory", _uow.NewsCategoryRepo.Find(ID).CategoryName);
        }

        [TestMethod]
        public void CanDeleteQuestionGroup()
        {
            _uow.NewsMainCategoryRepo.DeleteAll();
            _uow.SaveChanges();
            int ID = AddNewCategory();

            _uow.NewsCategoryRepo.DeleteByID(ID);
            _uow.SaveChanges();

            Assert.AreEqual(null, _uow.NewsCategoryRepo.Find(ID));
        }

        [TestMethod]
        public void CanCheckNameExsists()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewCategory();
            int mainCategoryID = _uow.NewsCategoryRepo.Find(ID).MainCategoryId;
            int isExsists = _uow.NewsCategoryRepo.CheckNameExists("NewCategory", ID, mainCategoryID);
            Assert.AreEqual(0, isExsists);
        }

        [TestMethod]
        public void CanCheckOrderExsists()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewCategory();
            int mainCategoryID = _uow.NewsCategoryRepo.Find(ID).MainCategoryId;
            int isExsists = _uow.NewsCategoryRepo.CheckOrderExists(1, ID, mainCategoryID);
            Assert.AreEqual(0, isExsists);
        }

        [TestMethod]
        public void CanFindCategoryByID ()
        {
            _uow.NewsMainCategoryRepo.DeleteAllMainCategories();
            _uow.SaveChanges();
            int ID = AddNewCategory();
            NewsCategory newsCategory =  _uow.NewsCategoryRepo.FindNewsCategoryByID(ID);
            Assert.AreNotEqual(null, newsCategory);
        }
    }
}
