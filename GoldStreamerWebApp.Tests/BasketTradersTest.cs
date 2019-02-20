using System.Collections.Generic;
using System.Linq;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class BasketTradersTest
    {
        UnitOfWork _uow;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }

        private Basket AddBasket(int owner, string name)
        {
            Basket tp = new Basket()
            {
                BasketOwner = owner,
                Name = name,
                IsDeleted = false
            };
            return tp;
        }

        private Trader AddTrader(string name, int type)
        {
            return new Trader
            {
                Name = name,
                Family = "",
                Governorate = 1,
                City = 1,
                District = 1,
                Gender = true,
                TypeFlag = type,
                SortOrder = 66,
                IsActive = true
            };
        }

        public BasketTraders AddBasketTrader(int traderId,int basketId)
        {
            BasketTraders bt = new BasketTraders()
            {
                TraderId = traderId,BasketId =basketId
            };
            return bt;
        }

        private void DeleteAddedTraders()
        {
           _uow.TraderRepo.DeleteList(26);
        }

        [TestMethod]
        public void CanAddBasketTraders()
        {
            _uow.BasketTradersRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();
            DeleteAddedTraders();
            Basket basket1 = AddBasket(1, "Basketeleven");
            Trader t1 = AddTrader("t1", 2);
            _uow.TraderRepo.Add(t1);
            _uow.BasketRepo.Add(basket1);
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID,basket1.ID));
            _uow.SaveChanges();
            Assert.AreEqual(1, _uow.BasketTradersRepo.GetAll().Count);
        }
        [TestMethod]
        public void CanGetBasketTraders()
        {
            _uow.BasketTradersRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();
            DeleteAddedTraders();
            Basket basket = AddBasket(1, "BasketOne");
            Basket basket1 = AddBasket(1, "Basketeleven");
            _uow.BasketRepo.Add(basket);
            _uow.BasketRepo.Add(basket1);
            Trader t1 = AddTrader("t1", 2);
            Trader t2 = AddTrader("t2", 2);
            _uow.TraderRepo.Add(t1);
            _uow.TraderRepo.Add(t2);
            _uow.SaveChanges();

            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket.ID));
            _uow.BasketTradersRepo.Add(AddBasketTrader(t2.ID, basket1.ID));
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket.ID));
            _uow.SaveChanges();

            List<BasketTraders> bt = _uow.BasketTradersRepo.GetBasketUsers(basket.ID);

            Assert.AreEqual(2, bt.Count);
        }     
        [TestMethod]
        public void CanDeleteBasketTraders()
        {
            _uow.BasketTradersRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();
            DeleteAddedTraders();

            Basket basket = AddBasket(1, "BasketOne");
            Basket basket1 = AddBasket(1, "Basketeleven");
            _uow.BasketRepo.Add(basket);
            _uow.BasketRepo.Add(basket1);
            Trader t1 = AddTrader("t1", 2);
            _uow.TraderRepo.Add(t1);
            _uow.SaveChanges();
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket.ID));
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket1.ID));
            _uow.SaveChanges();

            _uow.BasketTradersRepo.DeleteBasketUsers(basket.ID);
            _uow.SaveChanges();
            Assert.AreEqual(1, _uow.BasketTradersRepo.GetAll().Count);
        }

        [TestMethod]
        public void CanCheckAssignedUnassignedTraders()
        {
            _uow.BasketTradersRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();
            DeleteAddedTraders();

            Basket basket = AddBasket(1, "BasketOne");
            Basket basket1 = AddBasket(1, "Basketeleven");
            _uow.BasketRepo.Add(basket);
            _uow.BasketRepo.Add(basket1);
            Trader t1 = AddTrader("t1", 2);
            _uow.TraderRepo.Add(t1);
            _uow.SaveChanges();
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket.ID));
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket1.ID));
            _uow.SaveChanges();

            List<Trader> ts= _uow.BasketTradersRepo.GetAssignedUnAssignedUsers(basket.ID).Where(t=>t.ID==t1.ID).ToList();

            Assert.AreEqual(0, ts.Count);


        }

        [TestMethod]
        public void CanCheckAssignedUnassignedTraders1()
        {
            _uow.BasketTradersRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();
            _uow.UsersRepo.DeleteAll();
            DeleteAddedTraders();

            Basket basket = AddBasket(1, "BasketOne");
            Basket basket1 = AddBasket(1, "Basketeleven");
            Basket basket2 = AddBasket(2, "Basket3");
            _uow.BasketRepo.Add(basket);
            _uow.BasketRepo.Add(basket1);
            _uow.BasketRepo.Add(basket2);
            Trader t1 = AddTrader("t1", 2);
            _uow.TraderRepo.Add(t1);
            _uow.SaveChanges();
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket.ID));
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket1.ID));
            _uow.SaveChanges();

            List<Trader> ts = _uow.BasketTradersRepo.GetAssignedUnAssignedUsers(basket2.ID).Where(t => t.ID == t1.ID).ToList();

            Assert.AreEqual(1, ts.Count);


        }

        [TestMethod]
        public void CanCheckAssignedUnassignedTradersForSameOwner()
        {
            _uow.BasketTradersRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();
            DeleteAddedTraders();

            Basket basket = AddBasket(1, "BasketOne");
            Basket basket1 = AddBasket(1, "Basketeleven");
            Basket basket2 = AddBasket(1, "Basket3");
            _uow.BasketRepo.Add(basket);
            _uow.BasketRepo.Add(basket1);
            Trader t1 = AddTrader("t1", 2);
            Trader t2 = AddTrader("t2", 2);
            _uow.TraderRepo.Add(t1);
            _uow.TraderRepo.Add(t2);
            _uow.SaveChanges();
            _uow.BasketTradersRepo.Add(AddBasketTrader(t1.ID, basket.ID));
            _uow.BasketTradersRepo.Add(AddBasketTrader(t2.ID, basket1.ID));
            _uow.SaveChanges();
            _uow.BasketRepo.Add(basket2);
            _uow.BasketRepo.DeleteBasket(basket.ID);
            _uow.SaveChanges();
            List<Trader> ts = _uow.BasketTradersRepo.GetAssignedUnAssignedUsers(basket2.ID).Where(t => t.ID == t1.ID||t.ID==t2.ID).ToList();

            Assert.AreEqual(0, ts.Count);


        }
    }
}
