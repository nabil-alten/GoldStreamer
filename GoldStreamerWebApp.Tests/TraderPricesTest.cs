using System;
using System.Collections.Generic;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoldStreamerWebApp.Tests
{
    [TestClass]
    public class TraderPricesTest
    {
        UnitOfWork uow = null;
        [TestInitialize]
        public void Setup()
        {
            uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }
        private void DeleteAllPrices()
        {
            uow.TradePricesRepo.DeleteAll();
           // uow.SaveChanges();
        }

        private TraderPrices AddPriceToTrader(int traderID, decimal buyPrice, decimal sellPrice,DateTime date)
        {
            TraderPrices tp = new TraderPrices()
            {
                TraderID = traderID,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                priceDate = date,
            };
            return tp;
        }

        [TestMethod]
        public void CanAddPrice()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M,DateTime.Now);
            uow.TradePricesRepo.Add(openingBalance);
            uow.SaveChanges();

            Assert.AreEqual(1, uow.TradePricesRepo.GetAll().Count);
        }

        [TestMethod]
        public void CanGetAllTraderPrices()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M,DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 1234.9M, 5345.8M,DateTime.Now);
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.SaveChanges();

            Assert.AreEqual(2, uow.TradePricesRepo.GetAll().Count);
        }

        [TestMethod]
        public void CanGetAllTodayTraderPrices()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));

            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();

            Assert.AreEqual(2, uow.TradePricesRepo.FindByTrader(1).Count);
        }

        [TestMethod]
        public void CanGetTheLastBuyPriceInDay()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.8M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();
            var buy="" ;
            TraderPrices emptyBuyPrice = AddPriceToTrader(1, buy == "" ? uow.TradePricesRepo.GetLastBuySellPriceToday(1, "b") : Convert.ToDecimal(buy), 23.3M, DateTime.Now);

            uow.TradePricesRepo.Add(emptyBuyPrice);
            uow.SaveChanges();
            decimal x=Convert.ToDecimal( uow.TradePricesRepo.Find(emptyBuyPrice.ID).BuyPrice);
            Assert.AreEqual(114.9M, x);

        }

        [TestMethod]
        public void CanGetTheLastSellPriceInDay()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.8M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();
            var sell = "";
            TraderPrices emptyBuyPrice = AddPriceToTrader(1, 23.3M, sell == "" ? uow.TradePricesRepo.GetLastBuySellPriceToday(1, "s") : Convert.ToDecimal(sell), DateTime.Now);

            uow.TradePricesRepo.Add(emptyBuyPrice);
            uow.SaveChanges();
            decimal x =Convert.ToDecimal( uow.TradePricesRepo.Find(emptyBuyPrice.ID).SellPrice);
            Assert.AreEqual(5345.8M, x);

        }

        [TestMethod]
        public void CanGetTraderMinBuyPrice()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.8M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();

            decimal price= uow.TradePricesRepo.GetTraderMinBuyPrice(1);
            Assert.AreEqual(12.9M, price);
        }

        [TestMethod]
        public void CanGetTraderMaxSellPrice()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.88M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();

            decimal price = uow.TradePricesRepo.GetTraderMaxSellPrice(1);
            Assert.AreEqual(5345.88M, price);
        }

        [TestMethod]
        public void CanCheckPricesNoPriceForTodayAndNullBuy()
        {
            DeleteAllPrices();
            uow.SaveChanges();
            TraderPrices openingBalance = new TraderPrices(){TraderID=1};

            int res= uow.TradePricesRepo.CheckPrices("", "2", openingBalance);
            Assert.AreEqual(2, res);

        }

        [TestMethod]
        public void CanCheckPricesNoPriceForTodayAndNullSell()
        {
            DeleteAllPrices();
            uow.SaveChanges();
            TraderPrices openingBalance = new TraderPrices() { TraderID = 1 };

            int res = uow.TradePricesRepo.CheckPrices("2", "", openingBalance);
            Assert.AreEqual(3, res);

        }

        [TestMethod]
        public void CanCheckPriceswithPriceForTodayAndNullSell()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            uow.TradePricesRepo.Add(openingBalance);
            uow.SaveChanges();

            TraderPrices price1 = new TraderPrices() { TraderID = 1 };

            int res = uow.TradePricesRepo.CheckPrices("2", "", price1);
            Assert.AreEqual(0, res);

        }

        [TestMethod]
        public void CanGetTraderPricesForCertainDay()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1244.9M, 56865.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.TradePricesRepo.Add(AddPriceToTrader(1, 122.9M, 5333.8M, DateTime.Now.AddDays(-1)));
            uow.SaveChanges();

            List<TraderPrices> tp= uow.TradePricesRepo.GetDayPrices(DateTime.Now.AddDays(-1), 1);

            Assert.AreEqual(2,tp.Count);

        }

        [TestMethod]
        public void CanGetTraderMinSellPrice()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.8M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();

            decimal price = uow.TradePricesRepo.GetBasketMinSellPrice(1,DateTime.Now.Date);
            Assert.AreEqual(12.8M, price);
        }

        [TestMethod]
        public void CanGetTraderMaxBuyPrice()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now);
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.88M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();

            decimal price = uow.TradePricesRepo.GetBasketMaxBuyPrice(1, DateTime.Now.Date);
            Assert.AreEqual(114.9M, price);
        }
        [TestMethod]
        public void CanGetTraderMinSellPriceDayBefore()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now.AddDays(-1));
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.8M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();

            decimal price = uow.TradePricesRepo.GetBasketMinSellPrice(1, DateTime.Now.Date.AddDays(-1));
            Assert.AreEqual(5.8M, price);
        }

        [TestMethod]
        public void CanGetTraderMaxBuyPriceDayBefore()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(1, 12.9M, 12.8M, DateTime.Now.AddDays(-1));
            TraderPrices price1 = AddPriceToTrader(1, 114.9M, 5345.88M, DateTime.Now);
            TraderPrices price2 = AddPriceToTrader(1, 1234.9M, 5345.8M, DateTime.Now.AddDays(-1));
            uow.TradePricesRepo.Add(openingBalance);
            uow.TradePricesRepo.Add(price1);
            uow.TradePricesRepo.Add(price2);
            uow.SaveChanges();

            decimal price = uow.TradePricesRepo.GetBasketMaxBuyPrice(1, DateTime.Now.Date.AddDays(-1));
            Assert.AreEqual(1234.9M, price);
        }

        [TestMethod]
        public void delUsers()
        {
            uow.UsersRepo.DeleteAll();            
        }
    }
}
