using System.Web.Mvc;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.UnitOfWork;
using GoldStreamer.Controllers;
using GoldStreamer.Helpers;
using GoldStreamerWebApp.Models;
using localization.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Trirand.Web.Mvc;
using System;
using BusinessServices;
using System.Collections.Generic;

namespace GoldStreamerWebApp.Controllers
{
    [AllowAnonymous]
    [Internationalization]
    public class HomeController : BaseController
    {
        private readonly UnitOfWork _uow = new UnitOfWork();
       
        public ActionResult Index()
        {
            ViewBag.Date = DateTime.Now;
            var um = new UserManager<ApplicationUser>(
                          new UserStore<ApplicationUser>(new ApplicationDbContext()));

            //if (GoldStreamer.Helpers.Roles.IsAdmin()) return RedirectToAction("Index", "Home");
            if (GoldStreamer.Helpers.UserNavigation.IsSuperTrader())
            {
                Trader trader = _uow.TraderRepo.Find(um.FindById(User.Identity.GetUserId()).TraderId);
                if (trader == null)
                {
                    return HttpNotFound();
                }
                Session["traderID"] = um.FindById(User.Identity.GetUserId()).TraderId;

                return View("SuperTraderDashboard");
            }
            else if (GoldStreamer.Helpers.UserNavigation.IsTrader())
            {
                Trader trader = _uow.TraderRepo.Find(um.FindById(User.Identity.GetUserId()).TraderId);
                if (trader == null)
                {
                    return HttpNotFound();
                }
                Session["traderID"] = um.FindById(User.Identity.GetUserId()).TraderId;

                return View("TraderDashboard");
            }
            else if (GoldStreamer.Helpers.UserNavigation.IsAdmin())
            {
                return View("AdminDashboard");
            }
            return View("Index");
        }

        public ActionResult Home()
        {
            ViewBag.Date = DateTime.Now;

            return View("Index");
        }
        public ActionResult TraderDashboard()
        {
            return View("TraderDashboard");
        }

        public ActionResult AdminDashboard()
        {
            return View("AdminDashboard");
        }
        public ActionResult SuperTraderDashboard()
        {
            return View("SuperTraderDashboard");
        }

        public decimal GetGoldSellPrice()
        {
            decimal goldPriceInDollar = _uow.GlobalPriceRepo.GetGoldSellPrice();

            Dollar dollarPrice = _uow.DollarRepo.GetCurrentDollarPrice();
            decimal localGoldPrice = Math.Round(goldPriceInDollar * dollarPrice.SellPrice, 2);
            return localGoldPrice;
        }

        public decimal GetGoldBuyPrice()
        {
            decimal goldPriceInDollar = _uow.GlobalPriceRepo.GetGoldBuyPrice();
            Dollar dollarPrice = _uow.DollarRepo.GetCurrentDollarPrice();
            decimal localGoldPrice = Math.Round(goldPriceInDollar * dollarPrice.SellPrice, 2);
            return localGoldPrice;
        }

        public decimal GetDollarSellPrice()
        {
            Dollar currentValue = _uow.DollarRepo.GetCurrentDollarPrice();
            decimal ret = _uow.DollarRepo.GetDollarSellPrice();
            return ret;
        }

        public decimal GetDollarBuyPrice()
        {
            decimal ret = _uow.DollarRepo.GetDollarBuyPrice();
            return ret ;
        }

        public string GetCurentDollarPrice()
        {
            Dollar currentValue =  _uow.DollarRepo.GetCurrentDollarPrice();
            string result = "buy:" + currentValue.BuyPrice + ",Sell:" + currentValue.SellPrice;
            return result;
        }

        public ActionResult ShowPopup()
        {

            var msg = "hello";
            ViewData["Msg"] = msg;
            return ShowPopup();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();

        }

        public void ChangeLanguage(string CurrentUrl)
        {
            string NewURL = "";
            if (!CurrentUrl.Contains("en-us") && !CurrentUrl.Contains("ar-eg"))
                NewURL = "~/en-us";
            else if (CurrentUrl.Contains("en-us"))
                NewURL = CurrentUrl.Replace("en-us", "ar-eg");
            else
                NewURL = CurrentUrl.Replace("ar-eg", "en-us");
            Response.Redirect(NewURL);
        }

        public ActionResult GeneralPurpose( string message)
        {
            return View("GeneralPurpose", new MessagesVM { Message = message });
        }

        [ChildActionOnly]
        [AllowAnonymous]
        //[OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult _News()
        {
            UnitOfWork uow = new UnitOfWork();
            return PartialView("_News",uow.NewsRepo.GetOrderedNews());

        }
        [Authorize]
        public ActionResult Chat()
        {
            ViewBag.Date = DateTime.Now;

            return View("Chat");
        
        }

        public ActionResult PrivacyPolicy()
        {

            return View("PrivacyPolicy");

        }
    }
}