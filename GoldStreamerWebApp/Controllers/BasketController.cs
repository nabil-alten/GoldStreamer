using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using BusinessServices;
using DAL;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using localization.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using Resources;

namespace GoldStreamer.Controllers
{
    [AdvancedAuthorize,Authorize(Roles = "CanManageBasket,Admin")]
    [Internationalization]
    public class BasketController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        BasketService basketService = new BasketService();

        public ActionResult Index(string sortOrder)
        {
        //    if (Session["traderID"] == null)
        //        return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult _Add([Bind(Include = "Name")]Basket basket)
        {
            try
            {
                basket.BasketOwner = GetLoggedTraderOrUser();// Convert.ToInt32(Session["traderID"].ToString());
                if (ModelState.IsValid)
                {
                    if (uow.BasketRepo.CheckNameExists(basket.Name, basket.BasketOwner) == 1)
                        return new HttpStatusCodeResult(1);

                    uow.BasketRepo.Add(basket);
                    uow.SaveChanges();

                    return Json(new { basket }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return new HttpStatusCodeResult(404, Messages.BasketNameVal);
                }
            }
            catch
            {
                return View();
            }
        }

        public PartialViewResult _list(int? page, string sortOrder)
        {
            int traderId = GetLoggedTraderOrUser();//Convert.ToInt32(Session["traderID"].ToString());

            ViewBag.nameSorting = String.IsNullOrEmpty(sortOrder) ? "nameDsc" : "";
            ViewBag.buyPriceSorting = sortOrder == "BuyAsc" ? "BuyDesc" : "BuyAsc";
            ViewBag.sellPriceSorting = sortOrder == "SellAsc" ? "SellDesc" : "SellAsc";
            ViewBag.timeSorting = sortOrder == "TimeAsc" ? "TimeDesc" : "TimeAsc";

            List<Basket> vmBasketLst = basketService.List(sortOrder, traderId);

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);

            return PartialView("_listVM", vmBasketLst.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UpdatePrice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["BasketID"] = id;
            return RedirectToAction("Index", "BasketPrices");
        }
        public ActionResult PricesHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["BasketID"] = id;
            return RedirectToAction("BasketPricesHistoryIndex", "BasketPrices");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                basketService.DeleteBasket(id);
                return RedirectToAction("_list");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult BasketAssignedTraders(int? id, int owner)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            Session["BasketID"] = id;
            Session["owner"] = owner;
            List<Trader> mediumTraders = uow.BasketTradersRepo.GetAssignedUnAssignedUsers(Convert.ToInt32(id));// uow.TraderRepo.GetRegisteredTrader(2);
            return View("AssignUsers", mediumTraders);
        }

        public ActionResult Update(int[] userIds, string basketName)
        {
            //if (Session["BasketID"] == null)
            //    return RedirectToAction("Index", "Home");

            if (uow.BasketRepo.CheckNameExists(basketName, Convert.ToInt32(Session["owner"]), Convert.ToInt32(Session["BasketID"])) == 1)
                return new HttpStatusCodeResult(1);

            int basketId = (int)Session["BasketID"];
            basketService.SaveAssignedUsers(basketId, userIds, basketName);

            return Json(JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult _EditName()
        {
            var basketId = (int)Session["BasketID"];
            return PartialView("_edit", uow.BasketRepo.Find(basketId));
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
