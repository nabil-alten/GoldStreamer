using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using GoldStreamer.Models.ViewModels;
using localization.Helpers;
using Trirand.Web.Mvc;
using BusinessServices;

namespace GoldStreamer.Controllers
{
    [Internationalization]
    public class TradersComparisonController : Controller
    {
        private readonly UnitOfWork uow = new UnitOfWork();
        readonly TradersComparisonViewModel _tradersComparisonVm = new TradersComparisonViewModel();

       
        public ActionResult Index(int? traderId)
        {
            Session["MainTraderChartId"] = traderId;
            _tradersComparisonVm.TraderList = uow.TraderRepo.GetAll().Where(x => x.TypeFlag == 1 && x.ID != traderId).ToList();
            return View(_tradersComparisonVm);
        }
        public ActionResult ViewChart(int? trader, int? buySellType, int? datetype, int? pricetype, string chartType)
        {
            CalculateAverageBuySellService service = new CalculateAverageBuySellService();
            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Home");
            //string[] tradersIds = trader.Split(',');
           
            Trader sessionTraderObj = new Trader();
            Trader selectedTraderObj =new Trader();
            int? traderId = null;


            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Home");
            if (Session["MainTraderChartId"] != null)
                traderId = Convert.ToInt32(Session["MainTraderChartId"].ToString());
            else traderId = 0;
                _tradersComparisonVm.TraderList = uow.TraderRepo.GetAll().Where(x => x.TypeFlag == 1 && x.ID != traderId).ToList();
         
                if (trader == null)
                {
                    if (_tradersComparisonVm.TraderList.Count > 0)
                    {
                        trader = _tradersComparisonVm.TraderList[0].ID;
                    }
                    else
                        trader = 0;
                }
                if (traderId != null)
                {
                    sessionTraderObj = uow.TraderRepo.Find(traderId);
                    ViewBag.subtitle = Resources.General.ComparasonTraders +" "+ sessionTraderObj.Name + " " + Resources.General.OtherTraders;
          
                }
                   
                if (trader != null)
                    selectedTraderObj = uow.TraderRepo.Find(trader);
                if (string.IsNullOrEmpty(chartType)) chartType = "Line";
                ChartType _chartType = new ChartType();

                if (chartType == "Column") ViewBag.ChartType = ChartType.Column;
                if (chartType == "Line") ViewBag.ChartType = ChartType.Line;
                if (chartType == "Scatter") ViewBag.ChartType = ChartType.Scatter;
                if (datetype == null || datetype == null) datetype = 1;
                if (pricetype == null || pricetype == null) pricetype = 1;
                if (buySellType == 0 || buySellType == null) buySellType = 1;

                List<string> lstDays = new List<string>();
                decimal[] arrdataSessionTrader = { 0 };
                decimal[] arrdataSelectedTrader = { 0 };
                List<ChartPoint> lstdatabuy = new List<ChartPoint>();
                List<ChartPoint> lstdatasell = new List<ChartPoint>();


                if (datetype == 1)
                {
                    ViewBag.text = Resources.General.goldpricesweek + " " + DateTime.Now.AddDays(-6).ToShortDateString() + " " + Resources.General.to + DateTime.Now.ToShortDateString();
                    arrdataSessionTrader = new decimal[7];
                    arrdataSelectedTrader = new decimal[7];
                    for (var i = 6; i >= 0; i--)
                    {
                        lstDays.Add("(" + DateTime.Now.AddDays(-i).Day + "/" + DateTime.Now.AddDays(-i).Month + ")");
                        TraderPricesChart SessiontraderPriceChart = uow.TraderPricesChartRepo.GetDayPrices(DateTime.Now.AddDays(-i), traderId);
                        TraderPricesChart SelectedTraderPriceChart = uow.TraderPricesChartRepo.GetDayPrices(DateTime.Now.AddDays(-i), trader);
                        if (SessiontraderPriceChart == null)
                        {
                            arrdataSessionTrader[6 - i] = 0;
                        }
                        else
                        {
                            if (buySellType == 1)
                            {
                                arrdataSessionTrader[6 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SessiontraderPriceChart.BuyAverage.ToString())),2) :Math.Round( (decimal.Parse(SessiontraderPriceChart.BuyClose.ToString())),2);

                            }
                            else
                            {
                                arrdataSessionTrader[6 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SessiontraderPriceChart.SellAverage.ToString())),2) :Math.Round( (decimal.Parse(SessiontraderPriceChart.SellClose.ToString())),2);

                            }
                        }
                        if (SelectedTraderPriceChart == null)
                        {
                            arrdataSelectedTrader[6 - i] = 0;
                        }
                        else
                        {
                            if (buySellType == 1)
                            {
                                arrdataSelectedTrader[6 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SelectedTraderPriceChart.BuyAverage.ToString())),2) :Math.Round( (decimal.Parse(SelectedTraderPriceChart.BuyClose.ToString())),2);
                            }
                            else
                            {
                                arrdataSelectedTrader[6 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SelectedTraderPriceChart.SellAverage.ToString())),2) :Math.Round( (decimal.Parse(SelectedTraderPriceChart.SellClose.ToString())),2);
                            }

                        }
                    }
                }
                else if (datetype == 2)
                {
                    ViewBag.text = Resources.General.goldpricesmonth + " " + DateTime.Now.AddDays(-29).ToShortDateString()+" " + Resources.General.to + DateTime.Now.ToShortDateString();
                    arrdataSessionTrader = new decimal[30];
                    arrdataSelectedTrader = new decimal[30];
                    for (var i = 29; i >= 0; i--)
                    {
                        lstDays.Add(DateTime.Now.AddDays(-i).Day.ToString());
                        TraderPricesChart SessiontraderPriceChart = uow.TraderPricesChartRepo.GetDayPrices(DateTime.Now.AddDays(-i), traderId);
                        TraderPricesChart SelectedTraderPriceChart = uow.TraderPricesChartRepo.GetDayPrices(DateTime.Now.AddDays(-i), trader);
                        if (SessiontraderPriceChart == null)
                        {
                            arrdataSessionTrader[29 - i] = 0;
                        }
                        else
                        {
                            if (buySellType == 1)
                            {
                                arrdataSessionTrader[29 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SessiontraderPriceChart.BuyAverage.ToString())),2) :Math.Round( (decimal.Parse(SessiontraderPriceChart.BuyClose.ToString())),2);

                            }
                            else
                            {
                                arrdataSessionTrader[29 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SessiontraderPriceChart.SellAverage.ToString())),2) :Math.Round( (decimal.Parse(SessiontraderPriceChart.SellClose.ToString())),2);

                            }
                        }
                        if (SelectedTraderPriceChart == null)
                        {
                            arrdataSelectedTrader[29 - i] = 0;
                        }
                        else
                        {
                            if (buySellType == 1)
                            {
                                arrdataSelectedTrader[29 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SelectedTraderPriceChart.BuyAverage.ToString())),2) :Math.Round( (decimal.Parse(SelectedTraderPriceChart.BuyClose.ToString())),2);
                            }
                            else
                            {
                                arrdataSelectedTrader[29 - i] = pricetype == 1 ?Math.Round( (decimal.Parse(SelectedTraderPriceChart.SellAverage.ToString())),2) :Math.Round( (decimal.Parse(SelectedTraderPriceChart.SellClose.ToString())),2);
                            }

                        }

                    }
                }
                else if (datetype == 3)
                {
                    ViewBag.text = Resources.General.goldpricesmonth+" " + DateTime.Now.AddDays(-29).ToShortDateString()+" " + Resources.General.to + DateTime.Now.ToShortDateString();

                    arrdataSessionTrader = new decimal[4];
                    arrdataSelectedTrader = new decimal[4];
                    for (var i = 3; i >= 0; i--)
                    {
                        DateTime weekStart = DateTime.Now;
                        DateTime weekEnd = DateTime.Now;
                        decimal SessionavgBuy, SessionavgSell;
                        decimal SelectedavgBuy, SelectedavgSell;
                        decimal SessioncloseBuy, SessioncloseSell;
                        decimal SelectedcloseBuy, SelectedcloseSell;
                        service.GetWeekStartEnd(DateTime.Now.AddDays(-(i * 7)), out weekStart, out weekEnd);
                        lstDays.Add("(" + weekStart.Day + "/" + weekStart.Month + "-" + weekEnd.Day + "/" + weekEnd.Month + ")");
                        uow.TraderPricesChartRepo.GetAvgPricesInWeek(weekStart, weekEnd, traderId, out SessionavgBuy, out SessionavgSell);
                        uow.TraderPricesChartRepo.GetAvgPricesInWeek(weekStart, weekEnd, trader, out SelectedavgBuy, out SelectedavgSell);

                        uow.TraderPricesChartRepo.GetClosePricesInWeek(weekStart, weekEnd, traderId, out SessioncloseBuy, out SessioncloseSell);
                        uow.TraderPricesChartRepo.GetClosePricesInWeek(weekStart, weekEnd, trader, out SelectedcloseBuy, out SelectedcloseSell);
                        if (buySellType == 1)
                        {
                            arrdataSessionTrader[3 - i] = pricetype == 1 ?Math.Round( SessionavgBuy,2) :Math.Round( SessioncloseBuy,2);
                            arrdataSelectedTrader[3 - i] = pricetype == 1 ?Math.Round( SelectedavgBuy,2) :Math.Round( SelectedcloseBuy,2);
                        }
                        else
                        {
                            arrdataSessionTrader[3 - i] = pricetype == 1 ?Math.Round( SessionavgSell,2) :Math.Round( SessioncloseSell,2);
                            arrdataSelectedTrader[3 - i] = pricetype == 1 ?Math.Round( SelectedavgSell,2) :Math.Round( SelectedcloseSell,2);
                        }
                    }
                }
                else if (datetype == 4)
                {
                    ViewBag.text = Resources.General.goldpricesmonth+" " + DateTime.Now.AddDays(-364).ToShortDateString()+" " + Resources.General.to + DateTime.Now.ToShortDateString();
                    arrdataSessionTrader = new decimal[12];
                    arrdataSelectedTrader = new decimal[12];
                    for (var i = 11; i >= 0; i--)
                    {
                        DateTime MonthStart = DateTime.Now;
                        DateTime MonthEnd = DateTime.Now;
                        decimal SessionavgBuy, SessionavgSell;
                        decimal SelectedavgBuy, SelectedavgSell;
                        decimal SessioncloseBuy, SessioncloseSell;
                        decimal SelectedcloseBuy, SelectedcloseSell;
                        service.GetMonthStartEnd(DateTime.Now.AddDays(-(i * 30)), out MonthStart, out MonthEnd);

                        lstDays.Add("(" + MonthStart.Day + "/" + MonthStart.Month + "-" + MonthEnd.Day + "/" + MonthEnd.Month + ")");
                        uow.TraderPricesChartRepo.GetAvgPricesInWeek(MonthStart, MonthEnd, traderId, out SessionavgBuy, out SessionavgSell);
                        uow.TraderPricesChartRepo.GetAvgPricesInWeek(MonthStart, MonthEnd, trader, out SelectedavgBuy, out SelectedavgSell);

                        uow.TraderPricesChartRepo.GetClosePricesInWeek(MonthStart, MonthEnd, traderId, out SessioncloseBuy, out SessioncloseSell);
                        uow.TraderPricesChartRepo.GetClosePricesInWeek(MonthStart, MonthEnd, trader, out SelectedcloseBuy, out SelectedcloseSell);
                        if (buySellType == 1)
                        {
                            arrdataSessionTrader[11 - i] = pricetype == 1 ?Math.Round( SessionavgBuy,2) :Math.Round( SessioncloseBuy,2);
                            arrdataSelectedTrader[11 - i] = pricetype == 1 ?Math.Round( SelectedavgBuy,2) :Math.Round( SelectedcloseBuy,2);
                        }
                        else 
                        {
                            arrdataSessionTrader[11 - i] = pricetype == 1 ?Math.Round( SessionavgSell,2) :Math.Round( SessioncloseSell,2);
                            arrdataSelectedTrader[11 - i] = pricetype == 1 ?Math.Round( SelectedavgSell,2) :Math.Round( SelectedcloseSell,2);
                        }
                    }
                }
                ViewBag.Categories = lstDays;
                ViewBag.Series = new List<ChartSeriesSettings>
                    {
                        new ChartSeriesSettings
                        {
                            Name =sessionTraderObj==null?"": sessionTraderObj.Name,
                            Data = new List<ChartPoint>().FromCollection(arrdataSessionTrader)

                        },
                        new ChartSeriesSettings
                        {
                            Name =selectedTraderObj==null?"اختر تاجر": selectedTraderObj.Name,
                            Data = new List<ChartPoint>().FromCollection(arrdataSelectedTrader)
                        }
      
                    };
          
            return View("_Chart");
           
        }
	}
} 