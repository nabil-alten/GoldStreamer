using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Web.Mvc;
using DAL.Hubs;
using DAL.Repositories;
using localization.Helpers;
using Microsoft.AspNet.SignalR;

namespace GoldStreamer.Controllers
{
   [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
    [AllowAnonymous]
    [Internationalization]
    public class GlobalPriceController : Controller
    {
        private readonly GlobalPriceRepository _repo = new GlobalPriceRepository();
        private readonly UnitOfWork _uow = new UnitOfWork(new GoldStreamerContext());
        public ActionResult Index()
        {
            return View(_uow.GlobalPriceRepo.GetAll());
        }
        public ActionResult Get() 
        {
            Dollar dollarPrice =  _uow.DollarRepo.GetCurrentDollarPrice();
            IEnumerable<GlobalPrice> data = _repo.GetData(dollarPrice.SellPrice);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}