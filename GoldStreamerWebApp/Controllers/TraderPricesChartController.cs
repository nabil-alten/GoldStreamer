using BLL.DomainClasses;
using BusinessServices;
using DAL.UnitOfWork;
using GoldStreamer.Helpers;
using localization.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;

namespace GoldStreamer.Controllers
{
    [AllowAnonymous]
    [Internationalization]
    public class TraderPricesChartController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        //
        // GET: /TraderPricesChart/
        public ActionResult Index(int? traderId)
        {
            Session["MainTraderChartId"] = traderId;
            return View();
        }

        public ActionResult GlobalPricesIndex()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ChartsList()
        {
            List<TraderPricesChart> lst = uow.TraderPricesChartRepo.getALLPrices();
            return View(lst);
        }
        public ActionResult _GoldPricesChart(string chartType, int? pricetype, int? datetype, int global)
        {
           int? traderId = null;
            ViewBag.subtitle = Resources.General.globalmarket;
            decimal dollarSellPrice = uow.DollarRepo.GetDollarSellPrice();
            if (global == 0 && Session["MainTraderChartId"] != null)
            {
                if (Session["MainTraderChartId"] == null)
                    return RedirectToAction("Index", "Home");
                traderId = Convert.ToInt32(Session["MainTraderChartId"].ToString());
                Trader objtrad = uow.TraderRepo.Find(int.Parse(traderId.ToString()));
                ViewBag.subtitle = objtrad.Name;
            }
           

            CalculateAverageBuySellService service = new CalculateAverageBuySellService();

            if (string.IsNullOrEmpty(chartType)) chartType = "Line";
            ChartType _chartType = new ChartType();
            if (chartType == "Column") ViewBag.ChartType = ChartType.Column;
            if (chartType == "Line") ViewBag.ChartType = ChartType.Line;
            if (chartType == "Scatter") ViewBag.ChartType = ChartType.Scatter;
            if (datetype == null) datetype = 1;
            if (pricetype == null) pricetype = 1;
            //if (chartType == "column") ViewBag.ChartType = ChartType.Column;
            List<string> lstDays = new List<string>();
            decimal[] arrdatabuy = { 0 };
            decimal[] arrdatasell = { 0 };
            List<ChartPoint> lstdatabuy = new List<ChartPoint>();
            List<ChartPoint> lstdatasell = new List<ChartPoint>();

            if (datetype == 1)
            {
                ViewBag.text =Resources.General.goldpricesweek+" " + DateTime.Now.AddDays(-6).ToShortDateString()+" " + Resources.General.to + DateTime.Now.ToShortDateString();
                arrdatasell = new decimal[7];
                arrdatabuy = new decimal[7];
                for (var i = 6; i >= 0; i--)
                {
                    lstDays.Add("(" + DateTime.Now.AddDays(-i).Day.ToString() + "/" + DateTime.Now.AddDays(-i).Month.ToString() + ")");
                    TraderPricesChart obj = uow.TraderPricesChartRepo.GetDayPrices(DateTime.Now.AddDays(-i), traderId);
                    if (obj != null)
                    {
                        if (global == 1)
                        {
                            arrdatabuy[6 - i] = pricetype == 1 ? Math.Round(((decimal.Parse(obj.BuyAverage.ToString()))*dollarSellPrice), 2) : Math.Round(((decimal.Parse(obj.BuyClose.ToString()))*dollarSellPrice), 2);
                            arrdatasell[6 - i] = pricetype == 1 ? Math.Round(((decimal.Parse(obj.SellAverage.ToString()))*dollarSellPrice), 2) : Math.Round(((decimal.Parse(obj.SellClose.ToString()))*dollarSellPrice), 2);
                        }
                        else
                        {
                            arrdatabuy[6 - i] = pricetype == 1 ? Math.Round((decimal.Parse(obj.BuyAverage.ToString())), 2) : Math.Round((decimal.Parse(obj.BuyClose.ToString())), 2);
                            arrdatasell[6 - i] = pricetype == 1 ? Math.Round((decimal.Parse(obj.SellAverage.ToString())), 2) : Math.Round((decimal.Parse(obj.SellClose.ToString())), 2);
                        }

                    }
                    else
                    {
                        arrdatabuy[6-i] = 0;
                        arrdatasell[6-i] = 0;
                    }
                }
            }
            else if (datetype == 2)
            {
                ViewBag.text = Resources.General.goldpricesmonth+" " + DateTime.Now.AddDays(-29).ToShortDateString()+" " + Resources.General.to + DateTime.Now.ToShortDateString();
                arrdatasell = new decimal[30];
                arrdatabuy = new decimal[30];
                for (var i = 29; i >= 0; i--)
                {
                    lstDays.Add(DateTime.Now.AddDays(-i).Day.ToString());
                    TraderPricesChart obj = uow.TraderPricesChartRepo.GetDayPrices(DateTime.Now.AddDays(-i), traderId);
                    if (obj != null)
                    {
                        if (global == 1)
                        {
                            arrdatabuy[29 - i] = pricetype == 1 ? Math.Round(((decimal.Parse(obj.BuyAverage.ToString())) * dollarSellPrice), 2) : Math.Round(((decimal.Parse(obj.BuyClose.ToString())) * dollarSellPrice), 2);
                            arrdatasell[29 - i] = pricetype == 1 ? Math.Round(((decimal.Parse(obj.SellAverage.ToString())) * dollarSellPrice), 2) : Math.Round(((decimal.Parse(obj.SellClose.ToString())) * dollarSellPrice), 2);
                        }
                        else
                        {
                            arrdatabuy[29 - i] = pricetype == 1 ? Math.Round((decimal.Parse(obj.BuyAverage.ToString())), 2) : Math.Round((decimal.Parse(obj.BuyClose.ToString())), 2);
                            arrdatasell[29 - i] = pricetype == 1 ? Math.Round((decimal.Parse(obj.SellAverage.ToString())), 2) : Math.Round((decimal.Parse(obj.SellClose.ToString())), 2);
                        }
                    }
                    else
                    {
                        arrdatabuy[29-i] = 0;
                        arrdatasell[29-i] = 0;
                    }
                }
            }
            else if (datetype == 3)
            {
                ViewBag.text = Resources.General.goldpricesmonth+" " + DateTime.Now.AddDays(-29).ToShortDateString()+" " + Resources.General.to + DateTime.Now.ToShortDateString();
                arrdatasell = new decimal[4];
                arrdatabuy = new decimal[4];
                for (var i = 3; i >= 0; i--)
                {
                    DateTime weekStart = DateTime.Now;
                    DateTime weekEnd = DateTime.Now;
                    decimal avgBuy, avgSell;
                    decimal closeBuy, closeSell;
                    service.GetWeekStartEnd(DateTime.Now.AddDays(-(i * 7)), out weekStart, out weekEnd);

                    lstDays.Add("(" + weekStart.Day + "/" + weekStart.Month + "-" + weekEnd.Day + "/" + weekEnd.Month + ")");
                    uow.TraderPricesChartRepo.GetAvgPricesInWeek(weekStart, weekEnd, traderId, out avgBuy, out avgSell);
                    uow.TraderPricesChartRepo.GetClosePricesInWeek(weekStart, weekEnd, traderId, out closeBuy, out closeSell);
                    if (global == 1)
                    {
                        arrdatabuy[3 - i] = pricetype == 1 ? Math.Round((avgBuy * dollarSellPrice), 2) : Math.Round((closeBuy * dollarSellPrice), 2);
                        arrdatasell[3 - i] = pricetype == 1 ? Math.Round((avgSell * dollarSellPrice), 2) : Math.Round((closeSell * dollarSellPrice), 2);
                    }
                    else
                    {
                        arrdatabuy[3 - i] = pricetype == 1 ? Math.Round(avgBuy, 2) : Math.Round(closeBuy, 2);
                        arrdatasell[3 - i] = pricetype == 1 ? Math.Round(avgSell, 2) : Math.Round(closeSell, 2);
                    }
                }
            }
            else if (datetype == 4)
            {
                ViewBag.text = Resources.General.goldpricesmonth+" " + DateTime.Now.AddDays(-364).ToShortDateString()+" " + Resources.General.to + DateTime.Now.ToShortDateString();
                arrdatasell = new decimal[12];
                arrdatabuy = new decimal[12];
                for (var i = 11; i >= 0; i--)
                {
                    DateTime MonthStart = DateTime.Now;
                    DateTime MonthEnd = DateTime.Now;
                    decimal avgBuy, avgSell;
                    decimal closeBuy, closeSell;
                    service.GetMonthStartEnd(DateTime.Now.AddDays(-(i * 30)), out MonthStart, out MonthEnd);

                    lstDays.Add("(" + MonthStart.Day + "/" + MonthStart.Month + "-" + MonthEnd.Day + "/" + MonthEnd.Month + ")");
                    uow.TraderPricesChartRepo.GetAvgPricesInWeek(MonthStart, MonthEnd, traderId, out avgBuy, out avgSell);
                    uow.TraderPricesChartRepo.GetClosePricesInWeek(MonthStart, MonthEnd, traderId, out closeBuy, out closeSell);
                    if (global == 1)
                    {
                        arrdatabuy[11 - i] = pricetype == 1 ? Math.Round((avgBuy*dollarSellPrice), 2) : Math.Round((closeBuy*dollarSellPrice), 2);
                        arrdatasell[11 - i] = pricetype == 1 ? Math.Round((avgSell*dollarSellPrice), 2) : Math.Round((closeSell*dollarSellPrice), 2);
                    }
                    else
                    {
                        arrdatabuy[11 - i] = pricetype == 1 ? Math.Round(avgBuy, 2) : Math.Round(closeBuy, 2);
                        arrdatasell[11 - i] = pricetype == 1 ? Math.Round(avgSell, 2) : Math.Round(closeSell, 2);
                    }
                }
            }
            ViewBag.Categories = lstDays;

            ViewBag.Data = new List<ChartPoint>().FromCollection(
                               arrdatabuy);

            ViewBag.Series = new List<ChartSeriesSettings>
                    {
                        new ChartSeriesSettings
                        {
                            Name = Resources.General.Buy,
                            Data = new List<ChartPoint>().FromCollection(arrdatabuy)

                        },
                        new ChartSeriesSettings
                        {
                            Name = Resources.General.Sell,
                            Data = new List<ChartPoint>().FromCollection(arrdatasell)
                        }
      
                    };
            return View("_GoldPricesChart");
        }

    }
}