using System;
using BLL.DomainClasses;
using BusinessServices;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class CalculateAverageBuySellTest
    {
        public UnitOfWork _uow;
        public CalculateAverageBuySellService _calc;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
            _calc = new CalculateAverageBuySellService();
        }

        public void InsertTraderPricesChart(int traderId, DateTime date, decimal buyAvg, decimal sellAvg, decimal buyClose, decimal sellClose)
        {
            TraderPricesChart traderPricesChartObj = new TraderPricesChart();
            traderPricesChartObj.TraderID = traderId;
            traderPricesChartObj.Date = date;
            traderPricesChartObj.BuyAverage = buyAvg;
            traderPricesChartObj.SellAverage = sellAvg;
            traderPricesChartObj.BuyClose = buyClose;
            traderPricesChartObj.SellClose = sellClose;
            _uow.TraderPricesChartRepo.Add(traderPricesChartObj);
        }
        [TestMethod]
        public void CalculateBuyInWeek()
        {
            _uow.TraderPricesChartRepo.DeleteAll();
            InsertTraderPricesChart(1, DateTime.Parse("06/20/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/21/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/22/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/23/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/24/2015"), 7, 16, 7, 19);
            InsertTraderPricesChart(1, DateTime.Parse("06/25/2015"), 6, 13, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/26/2015"), 8, 14, 5, 21);
            InsertTraderPricesChart(1, DateTime.Parse("06/27/2015"), 2, 12, 4, 22);
            InsertTraderPricesChart(1, DateTime.Parse("06/28/2015"), 3, 11, 8, 23);
            InsertTraderPricesChart(1, DateTime.Parse("06/29/2015"), 5, 15, 9, 24);
            _uow.SaveChanges();

            DateTime weekStart, weekEnd;
            decimal avgBuy, avgSell;
            _calc.GetWeekStartEnd(DateTime.Parse("06/29/2015"), out weekStart, out weekEnd);
            _uow.TraderPricesChartRepo.GetAvgPricesInWeek(weekStart, weekEnd,1, out avgBuy, out avgSell);
            Assert.AreEqual(5.142857M, avgBuy);
        }

        [TestMethod]
        public void CalculateBuyCloseInWeek()
        {
            _uow.TraderPricesChartRepo.DeleteAll();
            InsertTraderPricesChart(1, DateTime.Parse("06/20/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/21/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/22/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/23/2015"), 5, 15, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/24/2015"), 7, 16, 7, 19);
            InsertTraderPricesChart(1, DateTime.Parse("06/25/2015"), 6, 13, 6, 20);
            InsertTraderPricesChart(1, DateTime.Parse("06/26/2015"), 8, 14, 5, 21);
            InsertTraderPricesChart(1, DateTime.Parse("06/27/2015"), 2, 12, 4, 22);
            InsertTraderPricesChart(1, DateTime.Parse("06/28/2015"), 3, 11, 8, 23);
            InsertTraderPricesChart(1, DateTime.Parse("06/29/2015"), 5, 15, 9, 24);
            _uow.SaveChanges();

            DateTime weekStart, weekEnd;
            decimal avgBuy, avgSell,closeBuy,closeSell;
            _calc.GetWeekStartEnd(DateTime.Parse("06/29/2015"), out weekStart, out weekEnd);
            _uow.TraderPricesChartRepo.GetClosePricesInWeek(weekStart, weekEnd,1, out closeBuy, out closeSell);
            Assert.AreEqual(6.428571M, closeBuy);
        }
    }
}
