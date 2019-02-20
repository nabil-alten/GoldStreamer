using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class NewsTest
    {
        public UnitOfWork _uow;

        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }
        public void CleanUp()
        {
            _uow.NewsRepo.DeleteAll();
        }
        public int AddNewCategory(string mainCategoryName, string categoryName)
        {
            NewsMainCategory newMainCategory = new NewsMainCategory()
            {
                MainCategoryName = mainCategoryName,
                MainCategoryOrder = 1,
            };
            _uow.NewsMainCategoryRepo.AddNewsMainCategory(newMainCategory);
            _uow.SaveChanges();


            NewsCategory category = new NewsCategory()
            {
                CategoryName = categoryName,
                CategoryOrder = 1,
                MainCategoryId = newMainCategory.ID,
            };

            _uow.NewsCategoryRepo.AddNewsCategory(category);
            _uow.SaveChanges();
            return category.Id;
        }

        public News AddNews(int category, DateTime date)
        {
            News news = new News
            {
                Title = "Technology Today",
                Summary = "small details",
                Body = "hahaha",
                Date = date,
                CategoryId = category,
                IsActive = true,
                IsLatest = true
            };
            return news;
        }

        [TestMethod]
        public void CanGetNewsByCategory()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");
            int ID2 = AddNewCategory("Politic", "Politic");

            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID2, DateTime.Now);

            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.SaveChanges();
            List<News> newsList = _uow.NewsRepo.GetBySubCategory(ID1);

            Assert.AreEqual(1, newsList.Count);
        }

        [TestMethod]
        public void CanGetNewsByAllCategories()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");
            int ID2 = AddNewCategory("Politic", "Politic");

            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID2, DateTime.Now);

            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.SaveChanges();
            List<News> newsList = _uow.NewsRepo.GetBySubCategory(null);

            Assert.AreEqual(2, newsList.Count);
        }

        [TestMethod]
        public void CanGetNewsById()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");
            int ID2 = AddNewCategory("Politic", "Politic");

            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID2, DateTime.Now);

            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.SaveChanges();
            News newsObj = _uow.NewsRepo.Find(news1.Id);

            Assert.IsNotNull(newsObj);
        }

        [TestMethod]
        public void canGetTop5ViewsInSubCategory()
        {
            _uow.NewsRepo.DeleteAll();
            int categoryID = AddNewCategory("MainCategory", "Category");

            News news1 = AddNews(categoryID, DateTime.Now);
            News news2 = AddNews(categoryID, DateTime.Now.AddSeconds(2));
            News news3 = AddNews(categoryID, DateTime.Now.AddMinutes(5));
            News news4 = AddNews(categoryID, DateTime.Now.AddMinutes(7));
            News news5 = AddNews(categoryID, DateTime.Now.AddMinutes(9));
            News news6 = AddNews(categoryID, DateTime.Now.AddMinutes(15));
            news1.ViewCount = 1;
            news2.ViewCount = 2;
            news3.ViewCount = 3;
            news4.ViewCount = 3;
            news5.ViewCount = 4;
            news6.ViewCount = 5;
          

            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.NewsRepo.Add(news3);
            _uow.NewsRepo.Add(news4);
            _uow.NewsRepo.Add(news5);
            _uow.NewsRepo.Add(news6);
            _uow.SaveChanges();

            List<News> newsObj = _uow.NewsRepo.GetTopViewsBySubCategory(categoryID);

            Assert.AreNotEqual(news1, newsObj.First());
        }
        [TestMethod]
        public void CanDelete()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");
            int ID2 = AddNewCategory("Politic", "Politic");

            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID2, DateTime.Now);

            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.SaveChanges();
            List<News> List = _uow.NewsRepo.GetBySubCategory(ID1);
            _uow.NewsRepo.Delete(news1);
            _uow.SaveChanges();
            List<News> newsList = _uow.NewsRepo.GetBySubCategory(ID1);
            Assert.AreEqual(1, List.Count);
            Assert.AreEqual(0, newsList.Count);
        }
        [TestMethod]
        public void CanSearchbyAny()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");
            int ID2 = AddNewCategory("Politic", "Politic");

            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID2, DateTime.Now);
            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            
            _uow.SaveChanges();
            List<News> List = _uow.NewsRepo.SearchByAny("", ID1, 0, null);
         
            Assert.AreEqual(1, List.Count);  
        }

        [TestMethod]
        public void CanGetMonthNews()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");
            int ID2 = AddNewCategory("Politic", "Politic");

            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID2, DateTime.Now.AddMonths(-2));

            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.SaveChanges();
            List<News> newsList = _uow.NewsRepo.GetMonthNews(DateTime.Now);

            Assert.AreEqual(1, newsList.Count);
        }
        [TestMethod]
        public void CanGetMonthNews1()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");
            int ID2 = AddNewCategory("Politic", "Politic");

            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID2, DateTime.Now.AddMonths(-2));
            News news3 = AddNews(ID2, DateTime.Now.AddMonths(-2));
            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.NewsRepo.Add(news3);
            _uow.SaveChanges();
            List<News> newsList = _uow.NewsRepo.GetMonthNews(DateTime.Now.AddMonths(-2));

            Assert.AreEqual(2, newsList.Count);
        }

        [TestMethod]
        public void CanGetRelatedNews()
        {
            _uow.NewsRepo.DeleteAll();
            int ID1 = AddNewCategory("Gold", "Gold");

            News news = AddNews(ID1, DateTime.Now);
            News news1 = AddNews(ID1, DateTime.Now);
            News news2 = AddNews(ID1, DateTime.Now.AddSeconds(2));
            News news3 = AddNews(ID1, DateTime.Now.AddMinutes(5));
            News news4 = AddNews(ID1, DateTime.Now.AddMinutes(7));
            News news5 = AddNews(ID1, DateTime.Now.AddMinutes(9));
            News news6 = AddNews(ID1, DateTime.Now.AddMinutes(15));
            news1.ViewCount = 1;
            news2.ViewCount = 2;
            news3.ViewCount = 3;
            news4.ViewCount = 3;
            news5.ViewCount = 4;
            news6.ViewCount = 5;

            _uow.NewsRepo.Add(news);
            _uow.NewsRepo.Add(news1);
            _uow.NewsRepo.Add(news2);
            _uow.NewsRepo.Add(news3);
            _uow.NewsRepo.Add(news4);
            _uow.NewsRepo.Add(news5);
            _uow.NewsRepo.Add(news6);
            _uow.SaveChanges();

            List<News> newsLst = _uow.NewsRepo.GetRelatedNews(ID1, news.Id);
            Assert.AreEqual(news6,newsLst[0]);

        }

    }
}
