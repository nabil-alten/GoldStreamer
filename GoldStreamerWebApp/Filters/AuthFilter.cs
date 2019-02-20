using System.Web.Mvc;
using System.Web.Mvc.Filters;
using BLL.SecurityClasses;
using DAL;
using GoldStreamer.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoldStreamer.Filters
{
    public class AdvancedAuthorize : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            
            if (filterContext.Principal.Identity.Name != "")
            {
                var um = new UserManager<ApplicationUser>(
                                new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = um.FindById(filterContext.Principal.Identity.GetUserId());

                if (user != null)
                if(!user.IsActive && user.UserName.ToLower() != "admin")
                    filterContext.Result =
                        new RedirectResult(@"~/" + CultureHelper.GetCurrentCulture() + "/Redirector/NotActiveUser", false);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            
        }
    }
}