using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.Repositories;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using localization.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using Resources;
using Microsoft.AspNet.SignalR;

namespace GoldStreamer.Controllers
{
    [Internationalization]
    public class BasketPricesController : Controller
    {
        readonly UnitOfWork _uow = new UnitOfWork();
        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageBasketPrices,Admin")]
        public ActionResult Index(int? page)
        {
            //if (Session["BasketID"] == null)
            //    return RedirectToAction("Index", "Trader");

            int traderBasketId = Convert.ToInt32(Session["BasketID"].ToString());
            if (string.IsNullOrEmpty(traderBasketId.ToString()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        [AdvancedAuthorize,System.Web.Mvc.Authorize(Roles = "CanViewBasketPricesHistory,Admin")]
        public ActionResult BasketPricesHistoryIndex(int? page, int? id)
        {

            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");

            int traderBasketId = Convert.ToInt32(Session["BasketID"].ToString());
            if (string.IsNullOrEmpty(traderBasketId.ToString()))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanViewBasketPricesHistory,Admin")]
        public ActionResult _BasketPricesHistory(int? page)
        {
            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");

            int traderBasketId = Convert.ToInt32(Session["BasketID"].ToString());
            List<BasketPrices> pricesLst = _uow.BasketPricesRepo.FindAllTodayBasketPrices(traderBasketId).OrderByDescending(p => p.PriceDate).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);

            return PartialView("_BasketPricesHistory", pricesLst.ToPagedList(pageNumber, pageSize));
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageBasketPrices,CanManageBasketPrices,Admin")]
        public ActionResult _list(int? page)
        {
            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");

            int traderBasketId = Convert.ToInt32(Session["BasketID"].ToString());
            List<BasketPrices> pricesLst = _uow.BasketPricesRepo.FindAllTodayBasketPrices(traderBasketId).OrderByDescending(p => p.PriceDate).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);

            return PartialView("_list", pricesLst.ToPagedList(pageNumber, pageSize));
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanViewBasketPricesHistory,Admin")]
        public ActionResult Search(string sortOrder, string searchtxt, int? page)
        {

            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");

            int traderBasketId = Convert.ToInt32(Session["BasketID"].ToString());
            List<BasketPrices> pricesLst = _uow.BasketPricesRepo.SearchByDate(traderBasketId, DateTime.Parse(searchtxt)).OrderByDescending(p => p.PriceDate).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.searchtxt = searchtxt;
            TempData["SelectedDate"] = searchtxt;
            return PartialView("_BasketPricesHistory", pricesLst.ToPagedList(pageNumber, pageSize));
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageBasketPrices,CanViewBasketPricesHistory,Admin")]
        public ActionResult _BestPrice()
        {
            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");

            DateTime date = DateTime.Now;
            if (TempData["SelectedDate"] != null)
            {
                date = DateTime.Parse(TempData["SelectedDate"].ToString());
            }
            int traderId = GetLoggedTraderOrUser();//Convert.ToInt32(Session["traderID"].ToString());
            int basketId = Convert.ToInt32(Session["BasketID"].ToString());
            Trader objTrader = _uow.TraderRepo.FindTraderById(traderId);
            decimal minBuy = _uow.BasketPricesRepo.GetBasketMinBuyPrice(basketId, date, objTrader.TypeFlag);
            ViewData["minBuy"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", minBuy);
            decimal maxSell = _uow.BasketPricesRepo.GetBasketMaxSellPrice(basketId, date, objTrader.TypeFlag);
            ViewData["maxSell"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", maxSell);

            return PartialView("_BestPrice");
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageBasketPrices,Admin")]
        public ActionResult Create(string sellPrice, string buyPrice)
        {
            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");
            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Home");

            int traderId = GetLoggedTraderOrUser();//Convert.ToInt32(Session["traderID"].ToString());
            Trader objTrader = _uow.TraderRepo.FindTraderById(traderId);
            int basketId = Convert.ToInt32(Session["BasketID"].ToString());
            if (sellPrice == "0.00" || buyPrice == "0.00")
                return new HttpStatusCodeResult(1, Messages.ZeroPrice);

            BasketPrices basketPrice = new BasketPrices() { BasketID = basketId, PriceDate = DateTime.Now, IsCurrent = true };
            if (string.IsNullOrEmpty(sellPrice) || string.IsNullOrEmpty(buyPrice))
            {
                int objState = _uow.BasketPricesRepo.CheckPrices(buyPrice, sellPrice, basketPrice);

                if (sellPrice == "" && buyPrice == "")
                {
                    if (objState > 0)
                        return new HttpStatusCodeResult(3, Messages.NoPriceBefore);

                    return new HttpStatusCodeResult(2, Messages.nullablePrices);
                }
                if (objState == 2 || objState == 3)
                {
                    return new HttpStatusCodeResult(3, Messages.NoPriceBefore);
                }
                else
                {
                    basketPrice.BuyPrice = buyPrice == "" ? _uow.BasketPricesRepo.GetLastBuySellPriceToday(basketPrice.BasketID, "b") : Convert.ToDecimal(buyPrice);
                    basketPrice.SellPrice = sellPrice == "" ? _uow.BasketPricesRepo.GetLastBuySellPriceToday(basketPrice.BasketID, "s") : Convert.ToDecimal(sellPrice);
                }
            }
            else
            {
                basketPrice.BuyPrice = Convert.ToDecimal(buyPrice);
                basketPrice.SellPrice = Convert.ToDecimal(sellPrice);
            }
            if (ModelState.IsValid)
            {
                _uow.BasketPricesRepo.Add(basketPrice);
                _uow.BasketPricesRepo.SetCurrentFlag(basketPrice.BasketID, basketPrice.ID);
                _uow.SaveChanges();

                decimal minBuy = _uow.BasketPricesRepo.GetBasketMinBuyPrice(basketId, DateTime.Now, objTrader.TypeFlag);
                decimal maxSell = _uow.BasketPricesRepo.GetBasketMaxSellPrice(basketId, DateTime.Now, objTrader.TypeFlag);

                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<PricesHub>();
                context.Clients.All.updatePrices();

                return Json(new { traderprice = basketPrice, maxSell, minBuy }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(404, Messages.UpdateFailed);
            }
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageBasketPrices,Admin")]
        public ActionResult ValidateIsBetterPrice(string sellPrice, string buyPrice)
        {


            if (Session["BasketID"] == null)
                return RedirectToAction("Index", "Home");

            int res = 0;
            if (string.IsNullOrEmpty(sellPrice) && string.IsNullOrEmpty(buyPrice))
                res = 1;

            int basketId = Convert.ToInt32(Session["BasketID"].ToString());
            Basket basket = _uow.BasketRepo.Find(basketId);
            List<TraderPrices> traderPrices = _uow.TradePricesRepo.FindByTrader(basket.BasketOwner);
            TraderPrices currentPrice = traderPrices.LastOrDefault();
            decimal userBuyPrice = 0M, userSellPrice = 0M;

            if (currentPrice == null)
                res = 1;

            if (res != 1)
            {
                if (string.IsNullOrEmpty(buyPrice))
                    userBuyPrice = _uow.BasketPricesRepo.GetLastBuySellPriceToday(basketId, "b");
                else
                {
                    var string2Decimal = Helpers.UsefulMethods.string2decimal(buyPrice);
                    if (string2Decimal != null)
                        userBuyPrice = string2Decimal.Value;
                }

                if (string.IsNullOrEmpty(sellPrice))
                    userSellPrice = _uow.BasketPricesRepo.GetLastBuySellPriceToday(basketId, "s");
                else
                {
                    var string2Decimal = Helpers.UsefulMethods.string2decimal(sellPrice);
                    if (string2Decimal != null)
                        userSellPrice = string2Decimal.Value;
                }

                // ReSharper disable once PossibleNullReferenceException
                if (userBuyPrice < currentPrice.BuyPrice)
                    res = 2;
                else if (userSellPrice > currentPrice.SellPrice)
                    res = 3;
                if (userBuyPrice == currentPrice.BuyPrice && userSellPrice == currentPrice.SellPrice)
                    res = 4;
            }

            return Json(new
            {
                isBetter = res,
                crntBuy = currentPrice == null ? 0 : currentPrice.BuyPrice,
                crntSell = currentPrice == null ? 0 : currentPrice.SellPrice
            }, JsonRequestBehavior.AllowGet);
        }

        private int GetLoggedTraderOrUser()
        {
            var um = new UserManager<ApplicationUser>(
                               new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(User.Identity.GetUserId());

            return user.TraderId;
        }
    }
}
