using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using localization.Helpers;
using PagedList;

namespace GoldStreamer.Controllers
{
    [AdvancedAuthorize, Authorize(Roles = "Admin")]
    [Internationalization]
    public class SuperTraderController : Controller
    {
        readonly UnitOfWork _uow = new UnitOfWork();
        public ActionResult Index()
        {
            List<Trader> lstSuperTraders = _uow.TraderRepo.GetAllByType(1);
            return View(lstSuperTraders);

        }
        public ActionResult _List(int? page)
        {
            List<Trader> lstSuperTraders = _uow.TraderRepo.GetAllByType(1).OrderBy(p => p.SortOrder).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);

            return PartialView("_list", lstSuperTraders.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Search(string sortOrder, string searchtxt, int? page)
        {
            List<Trader> superTradersLst;
            if (searchtxt != string.Empty)
            {
                superTradersLst = _uow.TraderRepo.SearchSpecificType(searchtxt, 1).OrderBy(p => p.SortOrder).ToList();
            }
            else
            {
                superTradersLst = _uow.TraderRepo.GetAllByType(1).OrderBy(p => p.SortOrder).ToList();
            }
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.searchtxt = searchtxt;
            return PartialView("_List", superTradersLst.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UnActivateSuperTrader(string statusId)
        {
            var cutId = statusId;
            var status = cutId.Substring(0, 1);
            var id = cutId.Substring(2, cutId.Length - 2);           
            Trader sTrader = _uow.TraderRepo.FindTraderById( int.Parse(id));

            if (status == "1")
            {
                sTrader.IsActive = true;
            }
            else
            {
                sTrader.IsActive = false;
            }

            _uow.TraderRepo.Update(sTrader);
            _uow.SaveChanges();

            List<Trader> superTraders = _uow.TraderRepo.GetAllByType(1).OrderBy(p => p.SortOrder).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = 1;
            ViewBag.pageSize = pageSize;

            return PartialView("_List", superTraders.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult AddUsers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = _uow.TraderRepo.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            Session["traderID"] = id;
            return RedirectToAction("Index", "Users");
        }
    }
}