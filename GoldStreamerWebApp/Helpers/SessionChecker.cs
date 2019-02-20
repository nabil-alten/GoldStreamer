using System.Web.Mvc;
using DAL.UnitOfWork;
using GoldStreamerWebApp.Helpers;
using GoldStreamerWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoldStreamer.Helpers
{
    public class SessionCheckerAttribute : ActionFilterAttribute
    {
        private UnitOfWork _uow = null;
        public SessionCheckerAttribute()
        {
            _uow = new UnitOfWork();
        }
        /*
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //handle general action checks for authorized users, openDaily and currentShift
            if (!filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower()
                .Equals("Login".ToLower())
                && !filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower()
                .Equals("Home".ToLower())
                && !filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower()
                .Equals("Redirector".ToLower())
                && !filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower()
                .Equals("Notification".ToLower())
                )
            {
                string s = User.Identity.GetUserId();
                CheckPermissions(filterContext);
            }
        }
        private void CheckPermissions(ActionExecutingContext filterContext, string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            
            var user = um.FindById(userId);
            bool havePermssion = _uow.RolePermissionRepo.
                CheckRoleHasPermssion(user, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            if (!havePermssion)
            {
                filterContext.Result = new RedirectResult(@"~/" + CultureHelper.GetCurrentCulture() + "/Redirector/NoPermission", false);
            }
        }
    */
    }
}