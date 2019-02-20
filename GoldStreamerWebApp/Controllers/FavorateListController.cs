using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL.DomainClasses;
using BusinessServices;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using localization.Helpers;

namespace GoldStreamer.Controllers
{
    [AdvancedAuthorize, Authorize(Roles = "CanManageFavoriteList,Admin")]
    [Internationalization]
    public class FavorateListController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        FavListService FavorateTraderLstService = new FavListService();

        //
        // GET: /FavorateList/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AssignTraders()
        {
            if (Session["traderID"] == null)
                return RedirectToAction("Index", "Home");

            List<Trader> ts = uow.FavorateListRepo.GetAssignedUnAssignedUsers(Convert.ToInt32(Session["traderID"]));
            return View("AssignUsers", ts);
        }

        public ActionResult Update(int[] userIds)
        {
            if (Session["traderID"] == null)
                return RedirectToAction("Index", "Home");

            int ownerId = (int)Session["traderID"];
            FavorateTraderLstService.SaveAssignedUsers(ownerId, userIds);
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}
