using System;
using System.Web;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace GoldStreamer.Helpers
{
    public class UserNavigation
    {
        static UnitOfWork _uow = new UnitOfWork();


        public static bool IsAdmin()
        {
            return (HttpContext.Current.User.IsInRole("Admin"));

        }

        public static bool IsSuperTrader()
        {
            if (GetUserType() == 1)
                return true;
            return false;
        }

        public static bool IsTrader()
        {
            if (GetUserType() == 2)
                return true;
            return false;
        }

        public static bool IsUser()
        {
            if (GetUserType() == 3)
                return true;
            return false;
        }

        private static int GetUserType()

        {
            var um = new UserManager<ApplicationUser>(
                            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            try
            {
                 var user = um.FindById(HttpContext.Current.User.Identity.GetUserId());
                 return _uow.TraderRepo.FindTraderById(user.TraderId).TypeFlag;
            }
            catch (Exception)
            {
                return 0;
            }
           
         
        }
      
    }
}