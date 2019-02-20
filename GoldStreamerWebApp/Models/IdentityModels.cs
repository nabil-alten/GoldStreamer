using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoldStreamerWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int TraderId { get; set; }
        public bool IsActive { get; set; }
        public bool NeedReset { get; set; }
        public bool MainUser { get; set; } 
        public bool IsVerified { get; set; }
        public Guid Token { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public bool IsLockedOut { get; set; }

    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("GoldStreamerContext")
        {
        }

        //public System.Data.Entity.DbSet<GoldStreamerWebApp.Models.ViewModels.UserViewModel> UserViewModels { get; set; }

        //public System.Data.Entity.DbSet<GoldStreamerWebApp.Models.Company> Companies { get; set; }

        //public System.Data.Entity.DbSet<GoldStreamerWebApp.Models.Country> Countries { get; set; }
    }
    public class IdentityManager
    {
        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }
        public bool UpdateUser(ApplicationUser userObj)
        {
            var um = new UserManager<ApplicationUser>(
                          new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Update(userObj);
            um.UpdateAsync(userObj);
            return idResult.Succeeded;
        }
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }
        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.Role.Name);
            }
        }
    }
}