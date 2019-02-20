using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoldStreamerWebApp.Tests
{
    [TestClass]
    public class BasketPricesTest
    {
        public UnitOfWork Uow;
        [TestInitialize]
        public void Setup()
        {
            Uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }
        private void ClearReatedTables()
        {
            Uow.BasketPricesRepo.DeleteAll();
            Uow.BasketTradersRepo.DeleteAll();
            Uow.BasketRepo.DeleteAll();
            Uow.SaveChanges();
        }
        private Basket AddTestBasket()
        {
            Basket b = new Basket()
            {
                BasketOwner = 1,
                Name = "SuperTraderA BasketB",
                IsDeleted = false
            };
            Uow.BasketRepo.Add(b);
            return b;
        }
        private void DeleteAllPrices()
        {
            Uow.BasketPricesRepo.DeleteAll();
        }
        private BasketPrices AddPriceToTraderBasket(int traderBasketId, decimal buyPrice, decimal sellPrice, DateTime date, bool isCurrent)
        {
            BasketPrices bp = new BasketPrices()
            {
                BasketID = traderBasketId,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                PriceDate = date,
                IsCurrent = isCurrent
            };
            return bp;
        }
        [TestMethod]
        public void CanDeleteAllBasketPrices()
        {
            DeleteAllPrices();
            Uow.SaveChanges();

            Assert.AreEqual(0, Uow.BasketPricesRepo.GetAll().Count);
        }
        [TestMethod]
        public void CanAddTraderBasketPrice()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices openingBasketBalance = AddPriceToTraderBasket(b.ID, 12.9M, 12.8M, DateTime.Now,true);
            Uow.BasketPricesRepo.Add(openingBasketBalance);
            Uow.SaveChanges();

            Assert.AreEqual(1, Uow.BasketPricesRepo.GetAll().Count);
        }
        [TestMethod]
        public void CanGetCurrentBasketPrices()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();

            BasketPrices openingBalance = AddPriceToTraderBasket(b.ID, 12.9M, 12.8M, DateTime.Now,false);
            BasketPrices antoherPrice = AddPriceToTraderBasket(b.ID, 1234.9M, 5345.8M, DateTime.Now,true);
            Uow.BasketPricesRepo.Add(openingBalance);
            Uow.BasketPricesRepo.Add(antoherPrice);
            Uow.SaveChanges();

            Assert.AreEqual(1234.9M, Uow.BasketPricesRepo.FindByBasketId(b.ID).BuyPrice);
        }
        [TestMethod]
        public void CanGetAllTodayTraderPrices()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();

            BasketPrices yesterdayPrice = AddPriceToTraderBasket(b.ID, 1234.0M, 5345.8M, DateTime.Now.AddDays(-1), true);
            BasketPrices todayPrice = AddPriceToTraderBasket(b.ID, 1234.9M, 5345.8M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(yesterdayPrice);
            Uow.BasketPricesRepo.Add(todayPrice);
            Uow.SaveChanges();

            Assert.AreEqual(1234.9M, Uow.BasketPricesRepo.FindByBasketId(b.ID).BuyPrice);
        }
        [TestMethod]
        public void CanSearchTraderPricesByDate()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();

            BasketPrices yesterdayPrice = AddPriceToTraderBasket(b.ID, 1234.0M, 5345.8M, DateTime.Now.AddDays(-1), true);
            BasketPrices todayPrice = AddPriceToTraderBasket(b.ID, 1234.9M, 5345.8M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(yesterdayPrice);
            Uow.BasketPricesRepo.Add(todayPrice);
            Uow.SaveChanges();

            Assert.AreEqual(1234.9M, Uow.BasketPricesRepo.SearchByDate(b.ID,DateTime.Now)[0].BuyPrice);
        }
        [TestMethod]
        public void CanGetCurrentBasketPricesByTraderId()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1234.0M, 321.5M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.SaveChanges();

            BasketPrices basketPrice = Uow.BasketPricesRepo.FindByBasketId(b.ID);
            Assert.AreEqual(1234.0M, basketPrice.BuyPrice.Value);
        }
        [TestMethod]
        public void CanGetNewBasketPricesByTraderId()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now,false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1200.0M, 3000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.SaveChanges();

            BasketPrices basketPrice = Uow.BasketPricesRepo.FindByBasketId(b.ID);
            Assert.AreEqual(1200.0M, basketPrice.BuyPrice);

        }
        [TestMethod]
        public void CanGetBasketMinBuyPriceTrader()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now, false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now, false);
            BasketPrices cPrice = AddPriceToTraderBasket(b.ID, 1600.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();
            decimal minBuy = Uow.BasketPricesRepo.GetBasketMinBuyPrice(b.ID,DateTime.Now,1);
            Assert.AreEqual(1400.0M, minBuy);
        }
        [TestMethod]
        public void CanGetBasketMinBuyPriceRegUser()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now, false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now, false);
            BasketPrices cPrice = AddPriceToTraderBasket(b.ID, 1600.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();
            decimal minBuy = Uow.BasketPricesRepo.GetBasketMinBuyPrice(b.ID, DateTime.Now, 3);
            Assert.AreEqual(1600.0M, minBuy);
        }
        [TestMethod]
        public void CanGetBasketMinBuyPriceNormalTrader()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now, false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now, false);
            BasketPrices cPrice = AddPriceToTraderBasket(b.ID, 1600.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();
            decimal minBuy = Uow.BasketPricesRepo.GetBasketMinBuyPrice(b.ID, DateTime.Now, 2);
            Assert.AreEqual(1600.0M, minBuy);
        }
        [TestMethod]
        public void CanGetBasketMaxSellPriceTrader()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now, false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now, false);
            BasketPrices cPrice = AddPriceToTraderBasket(b.ID, 1600.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();
            decimal maxSell = Uow.BasketPricesRepo.GetBasketMaxSellPrice(b.ID, DateTime.Now, 1);
            Assert.AreEqual(9000.0M, maxSell);
        }
        [TestMethod]
        public void CanGetBasketMaxSellPriceUser()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now, false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now, false);
            BasketPrices cPrice = AddPriceToTraderBasket(b.ID, 1600.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();
            decimal maxSell = Uow.BasketPricesRepo.GetBasketMaxSellPrice(b.ID, DateTime.Now, 3);
            Assert.AreEqual(7000.0M, maxSell);
        }
        [TestMethod]
        public void CanGetLastBuySellPriceToday()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            TraderPrices tp = new TraderPrices()
            {
                TraderID = 1,
                BuyPrice = 100M,
                SellPrice = 200M,
                priceDate = DateTime.Now
            };
            Uow.TradePricesRepo.Add(tp);
           
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now.AddDays(-1), false);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.SaveChanges();

            var lastValue = Uow.BasketPricesRepo.GetLastBuySellPriceToday(b.ID, "s");
            Assert.AreEqual(200M, lastValue);
        }

        [TestMethod]
        public void CanGetBuyOrSellFromTraderGeneralPricesList()
        {
            ClearReatedTables();
            TraderPrices tp = new TraderPrices()
            {
                TraderID = 1,
                BuyPrice = 100M,
                SellPrice = 200M,
                priceDate = DateTime.Now
            };
            Uow.TradePricesRepo.Add(tp);
            Basket basket = AddTestBasket();

            BasketPrices aPrice = AddPriceToTraderBasket(basket.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(basket.ID, 1500.0M, 8000.0M, DateTime.Now.AddDays(-1), false);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.SaveChanges();

            var state = Uow.BasketPricesRepo.CheckPrices("", "", aPrice);
            Assert.AreEqual(0, state);
        }
        [TestMethod]
        public void CanReportNullBuyFromTraderBasketPrices()
        {
            ClearReatedTables();
            Uow.TradePricesRepo.DeleteAll();
            Basket basket = AddTestBasket();

            BasketPrices aPrice = AddPriceToTraderBasket(basket.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(basket.ID, 1500.0M, 8000.0M, DateTime.Now.AddDays(-1), false);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.SaveChanges();

            var state = Uow.BasketPricesRepo.CheckPrices("", "999", aPrice);
            Assert.AreEqual(2, state);
        }
        [TestMethod]
        public void CanReportNullSellFromTraderBasketPrices()
        {
            ClearReatedTables();
            Uow.TradePricesRepo.DeleteAll();
            Basket basket = AddTestBasket();

            BasketPrices aPrice = AddPriceToTraderBasket(basket.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(basket.ID, 1500.0M, 8000.0M, DateTime.Now.AddDays(-1), false);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.SaveChanges();

            var state = Uow.BasketPricesRepo.CheckPrices("999", "", aPrice);
            Assert.AreEqual(3, state);
        }
        [TestMethod]
        public void CangetBuyFromBasketBeforeLookingInGeneralTraderPricesList()
        {
            ClearReatedTables();
            Uow.TradePricesRepo.DeleteAll();
            Basket basket = AddTestBasket();

            BasketPrices aPrice = AddPriceToTraderBasket(basket.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(basket.ID, 1500.0M, 8000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices cPrice = AddPriceToTraderBasket(basket.ID, 1700.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();

            var state = Uow.BasketPricesRepo.CheckPrices("", "999", aPrice);
            Assert.AreEqual(1700.0M, aPrice.BuyPrice);
        }
        [TestMethod]
        public void CangetSellFromBasketBeforeLookingInGeneralTraderPricesList()
        {
            ClearReatedTables();
            Uow.TradePricesRepo.DeleteAll();
            Basket basket = AddTestBasket();

            BasketPrices aPrice = AddPriceToTraderBasket(basket.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(basket.ID, 1500.0M, 8000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices cPrice = AddPriceToTraderBasket(basket.ID, 1700.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();

            var state = Uow.BasketPricesRepo.CheckPrices("999", "", aPrice);
            Assert.AreEqual(9000.0M, aPrice.SellPrice);
        }

        [TestMethod]
        public void CanSetCurrentFlag()
        {
            ClearReatedTables();
            Basket basket = AddTestBasket();

            BasketPrices aPrice = AddPriceToTraderBasket(basket.ID, 1400.0M, 7000.0M, DateTime.Now, false);
            BasketPrices bPrice = AddPriceToTraderBasket(basket.ID, 1500.0M, 8000.0M, DateTime.Now, true);
            BasketPrices cPrice = AddPriceToTraderBasket(basket.ID, 1700.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();
            Uow.BasketPricesRepo.SetCurrentFlag(basket.ID, cPrice.ID );
            
            DateTime today = DateTime.Now.Date;
            List<BasketPrices> list = Uow.BasketPricesRepo.GetAll().Where(x => x.BasketID == basket.ID && x.PriceDate > today && x.IsCurrent).ToList();
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void CanFindAllTodayBasketPrices()
        {
            ClearReatedTables();
            Basket b = AddTestBasket();
            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now, false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now, false);
            BasketPrices cPrice = AddPriceToTraderBasket(b.ID, 1600.0M, 9000.0M, DateTime.Now, true);
            Uow.BasketPricesRepo.Add(aPrice);
            Uow.BasketPricesRepo.Add(bPrice);
            Uow.BasketPricesRepo.Add(cPrice);
            Uow.SaveChanges();
            List<BasketPrices> list = Uow.BasketPricesRepo.FindAllTodayBasketPrices(b.ID);
            Assert.AreEqual(3, list.Count);
        }
    }
}
