using System.Web.Mvc;
using localization.Helpers;

namespace ExchangeWebApp.Controllers
{
    [AllowAnonymous]
    [Internationalization]
    public class RedirectorController : Controller
    {
        public ActionResult NoPermission()
        {
            return View("NoPermission");
        }
        public ActionResult NotActiveUser()
        {
            return View("NotActiveUser");
        }
        public ActionResult NoPermissionToEditProfile()
        {
            return View("NoPermissionToEditProfile");
        }
    }
}