using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using GoldStreamer.Models.ViewModels;
using localization.Helpers;
using Microsoft.AspNet.Identity;

namespace GoldStreamer.Controllers
{
   // [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    [AdvancedAuthorize, Authorize]
    [Internationalization]
    public class TraderController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        // GET: /Trader/
        [HttpGet]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            string userName = User.Identity.Name;

            return View("Index", uow.TraderRepo.GetRegisteredTrader(1));
        }

        public ActionResult STraderProfile()
        {
            var Governorate = uow.GovernorateRepo.GetAllGovernorates();
            //govs.Insert(0,new Governorate(){Code="",Name="اختر",ID = 0});
            ViewBag.govs = new SelectList(Governorate, "ID", "Name");

            ViewBag.Order = uow.TraderRepo.GetNextTraderOrder();
            return View("BigTraderProfile");
        }
        
        public ActionResult Edit(int id)
        {
            var Governorate = uow.GovernorateRepo.GetAllGovernorates();
            //govs.Insert(0,new Governorate(){Code="",Name="اختر",ID = 0});
            ViewBag.govs = new SelectList(Governorate, "ID", "Name");

            Trader t = uow.TraderRepo.FindTraderById(id);
            if (t.Governorate != null)
            {
                List<City> city = uow.CityRepo.GetGovCities((int)t.Governorate);
                //city.Insert(0, new City { Code = "", Name = "اختر", ID = 0 });
                ViewBag.City = new SelectList(city, "ID", "Name", t.City);
                if (t.City != null)
                {
                    List<Region> district = uow.RegionRepo.GetCityRegions((int) t.City);
                    //district.Insert(0, new Region { Code = "", Name = "اختر", ID = 0 });
                    ViewBag.District = new SelectList(district, "ID", "Name", t.District);
                }
            }

            TraderViewModel traderVM = Mapper.Map<Trader, TraderViewModel>(t);
            traderVM.ReEmail = traderVM.Email;

            // t.ReEmail = t.Email;
            return View("Edit", traderVM);
        }

        [HttpPost]
        public ActionResult Edit(Trader trader)
        {
            if (uow.TraderRepo.CheckNameExists(trader) == 1)
                return new HttpStatusCodeResult(3);
            if (uow.TraderRepo.CheckPhoneExists(trader) == 1)
                return new HttpStatusCodeResult(6);
            if (uow.TraderRepo.CheckMobileExists(trader) == 1)
                return new HttpStatusCodeResult(4);
            if (uow.TraderRepo.CheckEmailExists(trader.Email, trader.ID) == 1)
                return new HttpStatusCodeResult(1);
            if (uow.TraderRepo.CheckOrderExists(trader.SortOrder, trader.ID) == 1)
                return new HttpStatusCodeResult(2);

            //if (uow.TraderRepo.CheckShopNameExists(trader) == 1)
            //    return new HttpStatusCodeResult(5);


            uow.TraderRepo.Update(trader);
            uow.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Add(Trader trader)
        {
            try
            {
                //basket.BasketOwner = Convert.ToInt32(Session["traderID"].ToString());
                //if (ModelState.IsValid)
                //{
                if (uow.TraderRepo.CheckNameExists(trader) == 1)
                    return new HttpStatusCodeResult(3);
                if (uow.TraderRepo.CheckPhoneExists(trader) == 1)
                    return new HttpStatusCodeResult(6);
                if (uow.TraderRepo.CheckMobileExists(trader) == 1)
                    return new HttpStatusCodeResult(4);
                if (uow.TraderRepo.CheckEmailExists(trader.Email, trader.ID) == 1)
                    return new HttpStatusCodeResult(1);
                if (uow.TraderRepo.CheckOrderExists(trader.SortOrder, trader.ID) == 1)
                    return new HttpStatusCodeResult(2);

                //if (uow.TraderRepo.CheckShopNameExists(trader) == 1)
                //    return new HttpStatusCodeResult(5);

                uow.TraderRepo.Add(trader);
                uow.SaveChanges();

                return Json(new { trader }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return new HttpStatusCodeResult(404, Messages.BasketNameVal);
                //}
            }
            catch
            {
                return View("BigTraderProfile");
            }
        }
        // GET: /Trader/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = uow.TraderRepo.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            Session["traderName"] = trader.Name;
            Session["traderID"] = id;
            return RedirectToAction("Index", "TraderPrices");
        }
        [HttpGet]
        public ActionResult ShowPriceList()
        {
            return View(uow.TraderRepo.GetRegisteredTrader(2));
        }
        public ActionResult PriceDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = uow.TraderRepo.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            Session["traderID"] = id;
            return RedirectToAction("Index", "Pricer");
        }
        public ActionResult Baskets(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = uow.TraderRepo.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            Session["traderName"] = trader.Name;
            Session["traderID"] = id;
            return RedirectToAction("Index", "Basket");
        }

        public ActionResult FavList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = uow.TraderRepo.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            Session["traderID"] = id;
            return RedirectToAction("FavListPrices", "Pricer");
        }

        public ActionResult FavListAssign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = uow.TraderRepo.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            Session["traderID"] = id;
            return RedirectToAction("AssignTraders", "FavorateList");
        }
        public ActionResult AddUsers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = uow.TraderRepo.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            Session["traderID"] = id;
            return RedirectToAction("Index", "Users");
        }

        public ActionResult GetCity(int govId)
        {
            List<City> city = uow.CityRepo.GetGovCities(govId);
            city.Insert(0, new City { Code = "", Name = "اختر", ID = 0 });

            return Json(new { city }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRegion(int cityId)
        {
            List<Region> district = uow.RegionRepo.GetCityRegions(cityId);
            district.Insert(0, new Region { Code = "", Name = "اختر", ID = 0 });
            return Json(new { district }, JsonRequestBehavior.AllowGet);
        }

    }
}
