using System.Web.Mvc;

namespace GoldStreamer.Controllers
{
    [AllowAnonymous]
    public class RolePermissionController : Controller
    {
        // GET: RolePermission
        public ActionResult Index()
        {
            return View();
        }
    }
}