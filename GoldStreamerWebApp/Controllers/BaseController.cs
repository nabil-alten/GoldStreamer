using System.Web.Mvc;
using DAL.UnitOfWork;

namespace GoldStreamer.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BaseController : Controller
    {
        readonly UnitOfWork _uow = new UnitOfWork();
        public ActionResult DoShowBest()
        {
            ViewData["maxSell"] = _uow.PriceViewerRepo.GetCurrentBestSell();
            ViewData["minBuy"] = _uow.PriceViewerRepo.GetCurrentBestBuy();
            return PartialView("_showBestPrices");
        }
	}
}