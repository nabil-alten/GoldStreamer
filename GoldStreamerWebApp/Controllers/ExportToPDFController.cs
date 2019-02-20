using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using GoldStreamer.Models.ViewModels;
using localization.Helpers;
using Rotativa.MVC;

namespace GoldStreamer.Controllers
{

    [AllowAnonymous]
    [Internationalization]
    public class ExportToPDFController : Controller
    {
        readonly UnitOfWork _uow = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult TraderPricesHistoryPrint(DateTime? date, bool print,int tr)
        {
            /*
            Session["traderID"] = Session["traderID"] == null ? tr.ToString() : Session["traderID"].ToString();
            int traderId = Convert.ToInt32(Session["traderID"] == null ? "1" : Session["traderID"].ToString());
            */
            int traderId = tr;
            Trader trader = _uow.TraderRepo.FindTraderById(traderId);
            List<TraderPrices> pricesLst = _uow.TradePricesRepo.GetDayPrices(date.Value, traderId).OrderByDescending(p => p.priceDate).ToList();
            List<TraderPricesPrintVM> traderPricesVM = Mapper.Map<List<TraderPrices>, List<TraderPricesPrintVM>>(pricesLst);
            Session["date"] = date;
            traderPricesVM[0].TraderName = trader.Name;
            traderPricesVM[0].Date = date.Value;
            traderPricesVM[0].print = print;

            return View("TraderPricesPrint", traderPricesVM);
        }

        [AllowAnonymous]
        public ActionResult GeneratePDF(int traderId)
        {
            try
            {
                return new ActionAsPdf(
                     "TraderPricesHistoryPrint",
                     new { date = Convert.ToDateTime(Session["date"].ToString()), print = true, tr = traderId }) { FileName = "TraderPrices.pdf" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        public ActionResult _BasketBestPrice()
        {
            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");

            DateTime date = DateTime.Now;
            if (TempData["SelectedDate"] != null)
            {
                date = DateTime.Parse(TempData["SelectedDate"].ToString());
            }

            int basketId = Convert.ToInt32(Session["BasketID"].ToString());

            decimal minBuy = _uow.BasketPricesRepo.GetBasketMinBuyPrice(basketId, date, 2);
            ViewData["minBuy"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", minBuy);
            decimal maxSell = _uow.BasketPricesRepo.GetBasketMaxSellPrice(basketId, date, 2);
            ViewData["maxSell"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", maxSell);

            return PartialView("_BestPrices");
        }
        public ActionResult GenerateBasketPDF(int basketId)
        {
            return new ActionAsPdf(
                 "BasketPricesHistoryPrint",
                 new { date = Convert.ToDateTime(Session["date"].ToString()), print = true, basket = basketId }) { FileName = "TraderPrices.pdf" };
        }

        public PartialViewResult _BestPrices()
        {
            DateTime date = DateTime.Now.Date;
            if (TempData["SelectedDate"] != null)
            {
                date = DateTime.Parse(TempData["SelectedDate"].ToString());
            }
            Session["traderID"] = Session["traderID"] == null ? "1" : Session["traderID"].ToString();//for pdf authorization issue
            int traderId = Convert.ToInt32(Session["traderID"].ToString());
            decimal minBuy = _uow.TradePricesRepo.GetBasketMaxBuyPrice(traderId, date);

            ViewData["minBuy"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", minBuy);
            decimal maxSell = _uow.TradePricesRepo.GetBasketMinSellPrice(traderId, date);

            ViewData["maxSell"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", maxSell);
            return PartialView("_BestPrices");
        }


        [AllowAnonymous]
        public ActionResult BasketPricesHistoryPrint(DateTime? date, bool print, int basket)
        {
            Session["BasketID"] = Session["BasketID"] == null ? basket.ToString() : Session["BasketID"].ToString();
            int traderBasketId = Convert.ToInt32(Session["BasketID"].ToString());
            Basket b = _uow.BasketRepo.Find(traderBasketId);

            if (date == null)
                date = DateTime.Now;

            List<BasketPrices> pricesLst = _uow.BasketPricesRepo.FindAllBasketPricesByDate(traderBasketId, date).OrderByDescending(p => p.PriceDate).ToList();
            List<BasketPricesPrintVM> basketPricesVM = Mapper.Map<List<BasketPrices>, List<BasketPricesPrintVM>>(pricesLst);
            Session["date"] = date;
            basketPricesVM[0].BasketName = b.Name;
            basketPricesVM[0].Date = date.Value;
            basketPricesVM[0].print = print;

            return View("BasketPricesPrint", basketPricesVM);
        }

    }
}