using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BLL.DomainClasses;
using BusinessServices;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class PriceViewerTest
    {
        UnitOfWork _uow;
        private FavListService _favLstSrvc;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
            _favLstSrvc = new FavListService();
        }
        private void DeleteAllPrices()
        {
            _uow.TradePricesRepo.DeleteAll();
        }
        private TraderPrices AddPriceToTrader(int traderId, decimal buyPrice, decimal sellPrice,DateTime date)
        {
            TraderPrices tp = new TraderPrices()
            {
                TraderID = traderId,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                priceDate = date,
            };
            return tp;
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
        private Basket AddTestBasket()
        {
            Basket b = new Basket()
            {
                BasketOwner = 1,
                Name = "SuperTraderA BasketB",
                IsDeleted = false
            };
            _uow.BasketRepo.Add(b);
            return b;
        }
        private void ClearReatedTables()
        {
            _uow.BasketPricesRepo.DeleteAll();
            _uow.BasketTradersRepo.DeleteAll();
            _uow.BasketRepo.DeleteAll();
            _uow.SaveChanges();
        }
        [TestMethod]
        public void CanGetCurrentBuy()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 12.8M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.SaveChanges();

            decimal currentBuy = _uow.PriceViewerRepo.GetCurrentBuy(1);

            Assert.AreEqual(1.10, double.Parse( currentBuy.ToString(CultureInfo.InvariantCulture) ));
        }
        [TestMethod]
        public void CanGetCurrentSell()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.00M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.SaveChanges();

            decimal currentSell = _uow.PriceViewerRepo.GetCurrentSell(1);

            Assert.AreEqual(2.00, double.Parse(currentSell.ToString(CultureInfo.InvariantCulture)));
        }
        [TestMethod]
        public void CanGetMaxBuyPerTrader()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.10M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            decimal maxBuyPerTrader = _uow.PriceViewerRepo.GetMaxBuyPerTrader(1);

            Assert.AreEqual( 1.90, double.Parse(maxBuyPerTrader.ToString(CultureInfo.InvariantCulture)));
        }
        [TestMethod]
        public void CanGetMinSellPerTrader()
        {
            DeleteAllPrices();
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.10M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            decimal minSellPerTrader = _uow.PriceViewerRepo.GetMinSellPerTrader(1);

            Assert.AreEqual(2.10, double.Parse(minSellPerTrader.ToString(CultureInfo.InvariantCulture)));
        }
        [TestMethod]
        public void CanGetOpenBuy()
        {
            DeleteAllPrices();
            TraderPrices yesterdayBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.50M, sellPrice: 2.50M, date: DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.10M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            decimal openBuy = _uow.PriceViewerRepo.GetOpenBuy(1);

            Assert.AreEqual(1.10, double.Parse(openBuy.ToString(CultureInfo.InvariantCulture)));
        }
        [TestMethod]
        public void CanGetOpenSell()
        {
            DeleteAllPrices();
            TraderPrices yesterdayBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.50M, sellPrice: 2.50M, date: DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.10M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            decimal openSell = _uow.PriceViewerRepo.GetOpenSell(1);

            Assert.AreEqual(2.10, double.Parse(openSell.ToString(CultureInfo.InvariantCulture)));
        }
        [TestMethod]
        public void CanGetTraderWithMaxBuy()
        {
            DeleteAllPrices();
            TraderPrices yesterdayBalance = AddPriceToTrader(traderId: 1, buyPrice: 2.50M, sellPrice: 2.50M, date: DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.10M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 2, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            int traderId = _uow.PriceViewerRepo.GetTraderWithMaxBuy();

            Assert.AreEqual(2 , traderId);
        }
        [TestMethod]
        public void CanGetTraderWithMinSell()
        {
            DeleteAllPrices();
            TraderPrices yesterdayBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.50M, sellPrice: 2.00M, date: DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.50M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 2, buyPrice: 1.90M, sellPrice: 2.30M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            int traderId = _uow.PriceViewerRepo.GetTraderWithMinSell();

            Assert.AreEqual(2, traderId);
        }
        [TestMethod]
        public void CanGetLastPriceDate()
        {
            DeleteAllPrices();
            TraderPrices yesterdayBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.50M, sellPrice: 2.00M, date: DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.50M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 2, buyPrice: 1.90M, sellPrice: 2.30M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            DateTime lastPriceDate = _uow.PriceViewerRepo.GetLastPriceDate(1);

            Assert.AreEqual(DateTime.Now.ToShortDateString(), lastPriceDate.ToShortDateString());
        }
        [TestMethod]
        public void CanGetAllPrices()
        {
            DeleteAllPrices();
            TraderPrices yesterdayBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.50M, sellPrice: 2.00M, date: DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.10M, sellPrice: 2.50M, date: DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(traderId: 2, buyPrice: 1.90M, sellPrice: 2.30M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

           List<Prices> pricesLst = (List<Prices>)_uow.PriceViewerRepo.GetAllPrices(0);
           Assert.AreEqual(2.5 , double.Parse(pricesLst[0].CurrentSell.ToString(CultureInfo.InvariantCulture)));
        }
        [TestMethod]
        public void CanGetTradersWithMaxBuy()
        {
            DeleteAllPrices();
            TraderPrices firstBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now);
            TraderPrices newerBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now.AddHours(1));
            TraderPrices secondBalance = AddPriceToTrader(traderId: 2, buyPrice: 1.90M, sellPrice: 2.50M, date: DateTime.Now);
            TraderPrices thirddayBalance = AddPriceToTrader(traderId: 3, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(firstBalance);
            _uow.TradePricesRepo.Add(newerBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.TradePricesRepo.Add(thirddayBalance);
            _uow.SaveChanges();

            List<TraderPrices> allWithMaxBuy = _uow.PriceViewerRepo.GetTradersWithMaxBuy(_uow.TraderRepo.GetAll());

            Assert.AreEqual(3, allWithMaxBuy.Count);
        }
        [TestMethod]
        public void CanGetTradersWithMinSell()
        {
            DeleteAllPrices();
            TraderPrices firstBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.50M, date: DateTime.Now);
            TraderPrices newerBalance = AddPriceToTrader(traderId: 1, buyPrice: 1.90M, sellPrice: 2.90M, date: DateTime.Now.AddHours(1));
            TraderPrices secondBalance = AddPriceToTrader(traderId: 2, buyPrice: 1.90M, sellPrice: 2.50M, date: DateTime.Now);
            TraderPrices thirddayBalance = AddPriceToTrader(traderId: 3, buyPrice: 1.90M, sellPrice: 2.50M, date: DateTime.Now);
            _uow.TradePricesRepo.Add(firstBalance);
            _uow.TradePricesRepo.Add(newerBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.TradePricesRepo.Add(thirddayBalance);
            _uow.SaveChanges();

            List<TraderPrices> allWithMaxBuy = _uow.PriceViewerRepo.GetTradersWithMinSell(_uow.TraderRepo.GetAll());

            Assert.AreEqual(2, allWithMaxBuy.Count);
        }
        [TestMethod]
        public void CanCheckTraderInBasket()
        {
            _uow.BasketPricesRepo.DeleteAll();
            _uow.BasketTradersRepo.DeleteAll();
            _uow.BasketRepo.DeleteAll();

            Basket basket = new Basket
            {
                BasketOwner = 1,
                Name = "basketA",
                IsDeleted = false
            };
            _uow.BasketRepo.Add(basket);
            List<Trader> normalUsers = _uow.TraderRepo.Search(x => x.TypeFlag == 3).ToList();
            BasketTraders bt = new BasketTraders
            {
                TraderId = normalUsers[0].ID,
                BasketId = basket.ID
            };
            _uow.BasketTradersRepo.Add(bt);
            _uow.SaveChanges();
            Assert.AreEqual(basket.ID, _uow.PriceViewerRepo.CheckTraderInBasket(1, normalUsers[0].ID));
        }
        [TestMethod]
        public void CanGetBasketBuyPrice()
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
            _uow.TradePricesRepo.Add(tp);

            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now, true);
            _uow.BasketPricesRepo.Add(aPrice);
            _uow.BasketPricesRepo.Add(bPrice);
            _uow.SaveChanges();

            var lastValue = _uow.BasketPricesRepo.GetLastBuySellPriceToday(b.ID, "b");
            Assert.AreEqual(1500.0M, lastValue);
        }
        [TestMethod]
        public void CanGetBasketSellPrice()
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
            _uow.TradePricesRepo.Add(tp);

            BasketPrices aPrice = AddPriceToTraderBasket(b.ID, 1400.0M, 7000.0M, DateTime.Now.AddDays(-1), false);
            BasketPrices bPrice = AddPriceToTraderBasket(b.ID, 1500.0M, 8000.0M, DateTime.Now , true );
            _uow.BasketPricesRepo.Add(aPrice);
            _uow.BasketPricesRepo.Add(bPrice);
            _uow.SaveChanges();

            var lastValue = _uow.BasketPricesRepo.GetLastBuySellPriceToday(b.ID, "s");
            Assert.AreEqual(8000.0M, lastValue);
        }
        [TestMethod]
        public void CanGetAllPricesFavOnly()
        {
            DeleteAllPrices();
            int[] superTraderIds = new int[2];
            superTraderIds[0] = 1;
            superTraderIds[1] = 2;
            _favLstSrvc.SaveAssignedUsers(11, superTraderIds);
        
            TraderPrices yesterdayBalance = AddPriceToTrader(1,  1.50M,  2.00M, DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader( 1, 1.10M, 2.50M, DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(2, 1.90M, 2.30M, DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            List<Prices> pricesLst = (List<Prices>)_uow.PriceViewerRepo.GetAllPrices(11, true);
            Assert.AreEqual(2, pricesLst.Count);
        }

        private void DeleteTraders()
        {
            _uow.TradePricesRepo.DeleteAll();
            _uow.BasketPricesRepo.DeleteAll();
            _uow.BasketTradersRepo.DeleteAll();
            _uow.BasketRepo.DeleteAll();

            _uow.FavorateListRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();

            _uow.UsersRepo.DeleteAll();

            _uow.TraderRepo.DeleteList(26);
            _uow.SaveChanges();
        }

        [TestMethod]
        public void CanGetAllPricesWithoutFav()
        {
            DeleteAllPrices();
          DeleteTraders();
            TraderPrices yesterdayBalance = AddPriceToTrader(1, 1.50M, 2.00M, DateTime.Now.AddDays(-1));
            TraderPrices openingBalance = AddPriceToTrader(1, 1.10M, 2.50M, DateTime.Now);
            TraderPrices secondBalance = AddPriceToTrader(2, 1.90M, 2.30M, DateTime.Now);
            _uow.TradePricesRepo.Add(yesterdayBalance);
            _uow.TradePricesRepo.Add(openingBalance);
            _uow.TradePricesRepo.Add(secondBalance);
            _uow.SaveChanges();

            List<Prices> pricesLst = (List<Prices>)_uow.PriceViewerRepo.GetAllPrices(1);
            Assert.AreEqual(10, pricesLst.Count);
        }
        [TestMethod]
        public void CanGetCurrentBestSellForApplicationHeader()
        {
            DeleteAllPrices();
            TraderPrices aBalance = AddPriceToTrader(1, 1.50M, 2.00M, DateTime.Now);
            TraderPrices bBalance = AddPriceToTrader(2, 1.10M, 2.50M, DateTime.Now);
            TraderPrices cBalance = AddPriceToTrader(3, 1.90M, 2.30M, DateTime.Now);
            _uow.TradePricesRepo.Add(aBalance);
            _uow.TradePricesRepo.Add(bBalance);
            _uow.TradePricesRepo.Add(cBalance);
            _uow.SaveChanges();

            decimal bestSell = _uow.PriceViewerRepo.GetCurrentBestSell();
            Assert.AreEqual(2.00M, bestSell);
        }
        [TestMethod]
        public void CanGetCurrentBestBuyForApplicationHeader()
        {
            DeleteAllPrices();
            TraderPrices aBalance = AddPriceToTrader(1, 1.50M, 2.00M, DateTime.Now);
            TraderPrices bBalance = AddPriceToTrader(2, 1.10M, 2.50M, DateTime.Now);
            TraderPrices cBalance = AddPriceToTrader(3, 1.90M, 2.30M, DateTime.Now);
            _uow.TradePricesRepo.Add(aBalance);
            _uow.TradePricesRepo.Add(bBalance);
            _uow.TradePricesRepo.Add(cBalance);
            _uow.SaveChanges();

            decimal bestSell = _uow.PriceViewerRepo.GetCurrentBestBuy();
            Assert.AreEqual(1.90M, bestSell);
        }
    }
}
