using System.Collections.Generic;
using System.Web.Mvc;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using localization.Helpers;

namespace GoldStreamer.Controllers
{
    [AdvancedAuthorize,Authorize(Roles = "CanViewPrices,Admin")]
    [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
    [Internationalization]
    public class PricerController : Controller
    {
        readonly UnitOfWork _uow = new UnitOfWork();
        public ActionResult Index()
        {
            if (Session["traderID"] == null)
            {
                return GoHome();
            }
            return View();
        }       
        public ActionResult PricesHistory(int? basketId)
        {
            if (basketId == null)
            {
                return GoHome();
            }
            Session["BasketID"] = basketId;
            return RedirectToAction("BasketPricesHistoryIndex", "BasketPrices");
        }
        public ActionResult TraderPricesHistory(int? traderId)
        {
            if (traderId == null)
            {
                return GoHome();
            }
            Session["traderHistoryID"] = traderId;
            return RedirectToAction("TraderPricesHistoryIndex", "TraderPrices");
        }
        public ActionResult GetPrices()
        {
            if (Session["traderID"] == null)
            {
                return GoHome();
            }

            int traderId = int.Parse(Session["traderID"].ToString());
            IEnumerable<Prices> priceList = _uow.PriceViewerRepo.GetAllPrices(traderId);
            foreach (var item in priceList)
            {
                if(item.TradersMinSell)
                    ViewData["maxSellVal"] = string.Format("{0:0,0.00;(0:0,0.00);----}", item.CurrentSell);
                if (item.TradersMaxBuy)
                    ViewData["minBuyVal"] = string.Format("{0:0,0.00;(0:0,0.00);----}", item.CurrentBuy);

            }
            return PartialView("_PricesList", priceList);
        }
        public ActionResult GetPricesFav()
        {
            if (Session["traderID"] == null)
            {
                return GoHome();
            }

            int traderId = int.Parse(Session["traderID"].ToString());
            IEnumerable<Prices> priceList = _uow.PriceViewerRepo.GetAllPrices(traderId, true );
            //foreach (var item in priceList)
            //{
            //    if (item.TradersMinSell)
            //        ViewData["maxSellVal"] = string.Format("{0:0,0.00;(0:0,0.00);----}", item.CurrentSell);
            //    if (item.TradersMaxBuy)
            //        ViewData["minBuyVal"] = string.Format("{0:0,0.00;(0:0,0.00);----}", item.CurrentBuy);

            //}
            return PartialView("_PricesListFav", priceList);
        }

        [AdvancedAuthorize, Authorize(Roles = "CanViewFavPrices,Admin")]
        public ActionResult FavListPrices()
        {
            if (Session["traderID"] == null)
            {
                return GoHome();
            }

            int traderId = int.Parse(Session["traderID"].ToString());
            bool hasFav = _uow.TraderFavRepo.HasFavorite(traderId);
            if (!hasFav)
                return View("AssignFav");
            
            return View("IndexFav");
        }
        
        private ActionResult GoHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}