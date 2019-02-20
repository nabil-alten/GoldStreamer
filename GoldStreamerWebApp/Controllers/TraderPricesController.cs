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
using Microsoft.AspNet.SignalR;
using PagedList;
using Resources;

namespace GoldStreamer.Controllers
{
    [Internationalization]
    public class TraderPricesController : Controller
    {
        readonly UnitOfWork _uow = new UnitOfWork();

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageTraderPrices,Admin")]
        public ActionResult Index(int? page)
        {
            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Home");

            //User.Identity.GetUserId();
            int tid = GetLoggedTraderOrUser();// Convert.ToInt32(Session["traderID"].ToString());
            if (string.IsNullOrEmpty(tid.ToString()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageTraderPrices,Admin")]
        public PartialViewResult _list(int? page)
        {
            //ViewBag.TraderID = traderID;
            int traderId = GetLoggedTraderOrUser();//Convert.ToInt32(Session["traderID"].ToString());
            List<TraderPrices> pricesLst = _uow.TradePricesRepo.FindByTrader(traderId).OrderByDescending(p => p.priceDate).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);

            return PartialView("_list", pricesLst.ToPagedList(pageNumber, pageSize));
        }


        public PartialViewResult Add()
        {
            return PartialView("_Add");
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageTraderPrices,CanViewTraderPricesHistory,Admin")]
        public PartialViewResult _BestPrice()
        {
            DateTime date = DateTime.Now.Date;
            if (TempData["SelectedDate"] != null)
            {
                date = DateTime.Parse(TempData["SelectedDate"].ToString());
            }
            int traderId = 0;
            if (Session["traderHistoryID"] == null)
            {
                traderId = GetLoggedTraderOrUser();
            }
            else
            {
                traderId = Convert.ToInt32(Session["traderHistoryID"].ToString());
            }

            
            decimal minBuy = _uow.TradePricesRepo.GetTraderMinBuyPrice(traderId);

            ViewData["minBuy"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", minBuy);
            decimal maxSell = _uow.TradePricesRepo.GetTraderMaxSellPrice(traderId);

            ViewData["maxSell"] = string.Format("{0:0,0.00;(0:0,0.00);0.00}", maxSell);
            return PartialView("_BestPrice");
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "CanManageTraderPrices,Admin")]
        [HttpPost]
        public ActionResult Create(string sellPrice, string buyPrice)
        {
            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Home");

            int traderId = GetLoggedTraderOrUser();// Convert.ToInt32(Session["traderID"].ToString());
            if (sellPrice == "0.00" || buyPrice == "0.00")
                return new HttpStatusCodeResult(1, Messages.ZeroPrice);


            TraderPrices traderprice = new TraderPrices() { TraderID = traderId, priceDate = DateTime.Now };
            if (string.IsNullOrEmpty(sellPrice) || string.IsNullOrEmpty(buyPrice))
            {
                int objState = _uow.TradePricesRepo.CheckPrices(buyPrice, sellPrice, traderprice);

                if (sellPrice == "" && buyPrice == "" && objState > 0)
                    return new HttpStatusCodeResult(3, Messages.NoPriceBefore);
                else if (sellPrice == "" && buyPrice == "" && objState == 0)
                    return new HttpStatusCodeResult(2, Messages.nullablePrices);
                if (objState == 2 || objState == 3)
                {
                    return new HttpStatusCodeResult(3, Messages.NoPriceBefore);
                }
                else
                {
                    traderprice.BuyPrice = buyPrice == "" ? _uow.TradePricesRepo.GetLastBuySellPriceToday(traderprice.TraderID, "b") : Convert.ToDecimal(buyPrice);
                    traderprice.SellPrice = sellPrice == "" ? _uow.TradePricesRepo.GetLastBuySellPriceToday(traderprice.TraderID, "s") : Convert.ToDecimal(sellPrice);
                }
            }
            else
            {
                traderprice.BuyPrice = Convert.ToDecimal(buyPrice);
                traderprice.SellPrice = Convert.ToDecimal(sellPrice);
            }
            if (ModelState.IsValid)
            {
                traderprice.priceDate = DateTime.Now.ToLocalTime();
                _uow.TradePricesRepo.Add(traderprice);
                List<TraderPrices> lst = _uow.TradePricesRepo.GetDayPrices(traderprice.priceDate, traderprice.TraderID);
                TraderPricesChart newobj = _uow.TraderPricesChartRepo.GetDayPrices(traderprice.priceDate, traderprice.TraderID);
               
                    decimal buyAvg = 0;
                    decimal sellAvg = 0;
                    if (newobj == null)
                    {
                        newobj = new TraderPricesChart();
                    }
                    if (lst.Count == 0)
                    {
                        newobj.BuyAverage = traderprice.BuyPrice;
                        newobj.SellAverage = traderprice.SellPrice;
                    }
                    else
                    {
                        foreach (TraderPrices price in lst)
                        {
                            buyAvg += decimal.Parse(price.BuyPrice.ToString());
                            sellAvg += decimal.Parse(price.SellPrice.ToString());
                        }
                        newobj.BuyAverage = (buyAvg + traderprice.BuyPrice) / (lst.Count + 1);
                        newobj.SellAverage = (sellAvg + traderprice.SellPrice) / (lst.Count + 1);
                    }
                    newobj.BuyClose = traderprice.BuyPrice;
                    newobj.SellClose = traderprice.SellPrice;
                    if (newobj.ID == 0)
                    {
                        newobj.TraderID = traderprice.TraderID;
                        newobj.Date = traderprice.priceDate;
                        _uow.TraderPricesChartRepo.Add(newobj);

                    }
                    else if (newobj.ID != 0)
                    {
                        _uow.TraderPricesChartRepo.Update(newobj);
                    }
                       
                

                _uow.SaveChanges();

                decimal minBuy = _uow.TradePricesRepo.GetTraderMinBuyPrice(traderId);
                decimal maxSell = _uow.TradePricesRepo.GetTraderMaxSellPrice(traderId);

                //call the hub to broadcast the price changes to all clients
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<PricesHub>();
                context.Clients.All.updatePrices();
                context.Clients.All.updateHeader();

                return Json(new { traderprice, maxSell, minBuy }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(404, Messages.UpdateFailed);
            }
        }


        [AdvancedAuthorize,System.Web.Mvc.Authorize(Roles = "Admin,CanViewTraderPricesHistory")]
        public ActionResult TraderPricesHistoryIndex(int? page, int? id)
        {
            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Trader");
            int traderId = 0;
            if (Session["traderHistoryID"] == null)
            {
                traderId = GetLoggedTraderOrUser();
            }
            else
            {
                traderId = Convert.ToInt32(Session["traderHistoryID"].ToString());
            }//Convert.ToInt32(Session["traderID"].ToString());
            if (string.IsNullOrEmpty(traderId.ToString()))
            {
                return RedirectToAction("Index", "Trader");
            }
            return View();
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "Admin,CanViewTraderPricesHistory")]
        public ActionResult _TraderPricesHistory(int? page)
        {
            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Home");
            int traderId = 0;
            if(Session["traderHistoryID"]==null)
            {
                traderId = GetLoggedTraderOrUser(); 
            }
            else
            {
                traderId = Convert.ToInt32(Session["traderHistoryID"].ToString());
            }
           //Convert.ToInt32(Session["traderHistoryID"].ToString());
            List<TraderPrices> pricesLst = _uow.TradePricesRepo.FindByTrader(traderId).OrderByDescending(p => p.priceDate).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);

            return PartialView("_TraderPricesHistory", pricesLst.ToPagedList(pageNumber, pageSize));
        }

        [AdvancedAuthorize, System.Web.Mvc.Authorize(Roles = "Admin,CanViewTraderPricesHistory")]
        public ActionResult Search(string sortOrder, string searchtxt, int? page)
        {

            //if (Session["traderID"] == null)
            //    return RedirectToAction("Index", "Trader");
            int traderBasketId = 0;
            if (Session["traderHistoryID"] == null)
            {
                traderBasketId = GetLoggedTraderOrUser();
            }
            else
            {
                traderBasketId = Convert.ToInt32(Session["traderHistoryID"].ToString());
            }
            //int traderBasketId = GetLoggedTraderOrUser(); //Convert.ToInt32(Session["traderID"].ToString());
            List<TraderPrices> pricesLst = _uow.TradePricesRepo.GetDayPrices(DateTime.Parse(searchtxt), traderBasketId).OrderByDescending(p => p.priceDate).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.searchtxt = searchtxt;
            TempData["SelectedDate"] = searchtxt;
            return PartialView("_TraderPricesHistory", pricesLst.ToPagedList(pageNumber, pageSize));
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
