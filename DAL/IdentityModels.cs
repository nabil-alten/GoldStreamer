using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.SecurityClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL
{
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
            var rm = new RoleManager<ApplicationRole>(
                new RoleStore<ApplicationRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }
        public bool CreateRole(string name,string desc)
        {
            var rm = new RoleManager<ApplicationRole>(
                new RoleStore<ApplicationRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new ApplicationRole(name, desc));
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