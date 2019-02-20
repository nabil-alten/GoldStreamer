using System;
using System.Collections.Generic;
using BLL;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeWebApp.Tests
{
    [TestClass]
    public class BasketTest
    {
        UnitOfWork uow = null;
        [TestInitialize]
        public void Setup()
        {
            uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
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

        private BasketPrices AddBasketPrice(int basket, decimal buyPrice, decimal sellPrice)
        {
            BasketPrices bp = new BasketPrices()
            {
                BasketID = basket,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                IsCurrent = true,
                PriceDate = DateTime.Now
            };
            return bp;
        }

        private BasketTraders AddBasketTrader(int traderId, int basketId)
        {
            BasketTraders bt = new BasketTraders()
            {
                TraderId = traderId,
                BasketId = basketId
            };
            return bt;
        }

        private void DeleteAllBaskets()
        {
            DeleteAllBasketsPrices();
            uow.BasketTradersRepo.DeleteAll();
            uow.BasketRepo.DeleteAll();

            // uow.SaveChanges();
        }

        private void DeleteAllBasketsPrices()
        {
            uow.BasketPricesRepo.DeleteAll();
            // uow.SaveChanges();
        }

        private void DeleteBasketTraders(int basketId)
        {
            uow.BasketTradersRepo.DeleteBasketUsers(basketId);
        }

        [TestMethod]
        public void CanAdd()
        {
            DeleteAllBaskets();
            uow.BasketRepo.Add(AddBasket(1, "BasketOne"));
            uow.SaveChanges();
            Assert.AreEqual(1, uow.BasketRepo.GetAll().Count);

        }
        [TestMethod]
        public void CanCheckExistingNameFalse()
        {
            DeleteAllBaskets();
            Basket basket = AddBasket(1, "BasketOne");
            uow.BasketRepo.Add(basket);
            uow.SaveChanges();
            Basket basket1 = AddBasket(1, "Basket");
            int res = uow.BasketRepo.CheckNameExists(basket1.Name, basket1.BasketOwner);
            Assert.AreEqual(res, 0);
        }

        [TestMethod]
        public void CanCheckExistingNameTrue()
        {
            DeleteAllBaskets();
            Basket basket = AddBasket(1, "Basket");
            uow.BasketRepo.Add(basket);
            uow.SaveChanges();
            Basket basket1 = AddBasket(1, "Basket");
            int res = uow.BasketRepo.CheckNameExists(basket1.Name, basket1.BasketOwner);
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void CanCheckExistingNameForDifferentTraders()
        {
            DeleteAllBaskets();
            Basket basket = AddBasket(1, "Basket");
            uow.BasketRepo.Add(basket);
            uow.SaveChanges();
            Basket basket1 = AddBasket(2, "Basket");
            int res = uow.BasketRepo.CheckNameExists(basket1.Name, basket1.BasketOwner);
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void CanGetBasketByBigTraderId()
        {
            DeleteAllBaskets();
            Basket basket = AddBasket(1, "BasketOne");
            Basket basket1 = AddBasket(1, "Basketeleven");
            Basket basket2 = AddBasket(2, "BasketTwo");
            uow.BasketRepo.Add(basket);
            uow.BasketRepo.Add(basket1);
            uow.BasketRepo.Add(basket2);
            uow.SaveChanges();
            List<Basket> bskt = uow.BasketRepo.GetByBigTraderId(1);

            Assert.AreEqual(2, bskt.Count);
        }

        [TestMethod]
        public void CanDeleteBasket()
        {
            DeleteAllBaskets();
            Basket basket = AddBasket(1, "BasketOne");
            Basket basket1 = AddBasket(1, "Basketeleven");
            Basket basket2 = AddBasket(2, "BasketTwo");
            uow.BasketRepo.Add(basket);
            uow.BasketRepo.Add(basket1);
            uow.BasketRepo.Add(basket2);
            uow.SaveChanges();

            AddBasketTrader(6, basket.ID);
            AddBasketTrader(7, basket.ID);
            AddBasketTrader(7, basket1.ID);
            uow.SaveChanges();

            uow.BasketRepo.DeleteBasket(basket.ID);
            uow.SaveChanges();
            Assert.AreEqual(2, uow.BasketRepo.GetAllBaskerts().Count);
        }
        [TestMethod]
        public void CanCheckTraderBasketLastPrice()
        {
            AddBasketPrices();

            UnitOfWork u = new UnitOfWork();
            List<Basket> b = u.BasketRepo.GetTraderBasketsPrice(1);

            Assert.AreEqual(55.01M, b[0].BasketPrices[0].BuyPrice);
        }
        private void AddBasketPrices()
        {
            DeleteAllBaskets();


            var basket = AddBasket(1, "BasketOne");
            uow.BasketRepo.Add(basket);
            uow.SaveChanges();
            BasketPrices bp = AddBasketPrice(basket.ID, 12.01M, 12.02M);
            BasketPrices bp5 = AddBasketPrice(basket.ID, 55.01M, 55.02M);
            uow.BasketPricesRepo.Add(bp);
            bp.IsCurrent = false;
            uow.BasketPricesRepo.Add(bp5);
            uow.SaveChanges();
        }
    }
}
