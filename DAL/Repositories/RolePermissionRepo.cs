using BLL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using BLL.SecurityClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;
using System.Data.SqlClient;
using System.Transactions;
using System.Data.Common;

namespace DAL.Repositories
{
    public class RolePermissionRepo<T> : BaseRepo<T> where T : class
    {
        GoldStreamerContext _context = null;
        public RolePermissionRepo(GoldStreamerContext dbContext)
        {
            _context = dbContext;
        }

        public List<ApplicationRole> GetAllApplicationRoles ()
        {
         ApplicationDbContext UsersContext = new ApplicationDbContext();
         return UsersContext.Set<ApplicationRole>().ToList();
        }



        public void SaveAssignedRolesInTransaction(Guid[] rolesIds, int typeId)
        {
            TraderRepo<Trader> traderRepo = new TraderRepo<Trader>(_context);
            List<Trader> tradersList = traderRepo.GetAllByType(typeId);
            var UsersContext = new ApplicationDbContext();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    //01- Delete old roles
                    DeleteRolePermissionsByTypeID(typeId);
                   // _context.SaveChanges();

                    //02- Add new roles
                    if (rolesIds != null)
                    {
                        foreach (var id in rolesIds)
                        {
                            RolePermission newRolePermissiion = new RolePermission();
                            newRolePermissiion.AspNetRolesID = id;
                            newRolePermissiion.UserTypeID = typeId;
                            AddNewRolePermissions(newRolePermissiion);
                        }
                    }
                    _context.SaveChanges();


                    //using ()
                    //{
                        //03- Assign new roles to users in AspNetUserRoles
                      
                        foreach (var trader in tradersList)
                        {
                            List<ApplicationUser> applicationUsers = GetApplicationUsersByTraderID(trader.ID, UsersContext);
                            foreach (var appUser in applicationUsers)
                            {
                                DeleteAllUserRoles(appUser.Id, UsersContext);
                                if (rolesIds != null)
                                {
                                    foreach (var roleID in rolesIds)
                                    {
                                        ApplicationUserRole appUserRole = new ApplicationUserRole();
                                        appUserRole.UserId = appUser.Id;
                                        appUserRole.RoleId = roleID.ToString();
                                        AddNewUserRoles(appUserRole, UsersContext);
                                    }
                                }
                            }
                        }
                        UsersContext.SaveChanges();
                   // }
                    scope.Complete();
                }

                catch
                {
                    scope.Dispose();
                }
            }
        }



        public List<ApplicationUserRole> GetAllApplicationUserRoles()
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            return UsersContext.Set<ApplicationUserRole>().ToList();
        }

        public Guid GetApplicationRoleByName(string name)
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            return Guid.Parse(UsersContext.Set<ApplicationRole>().FirstOrDefault(r => r.Name == name).Id);
        }
        public Guid GetApplicationUserByName(string name)
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            return Guid.Parse(UsersContext.Set<ApplicationUser>().FirstOrDefault(r => r.UserName == name).Id);
        }

        public List<RolePermission> GetAllRolePermisions()
        {
            return _context.Set<RolePermission>().ToList();
        }

        public List<RolePermission> FindRolePermissionByTypeAndRole(Guid AspNetRolesID , int typeId)
        {
            return _context.Set<RolePermission>().Where(r => r.AspNetRolesID == AspNetRolesID && r.UserTypeID == typeId).ToList();
        }

        public List<ApplicationRole> GetAssignedUnassigedRoles(int p)
        {
            throw new NotImplementedException();
        }

        public void AddNewRolePermissions(RolePermission newRolePermission)
        {
            _context.RolePermission.Add(newRolePermission);
        }
        public void DeleteRolePermissionsByTypeID(int typeID)
        {
            List<RolePermission> deletedRoles = GetRolePermissionsByTypeID(typeID);
            _context.RolePermission.RemoveRange(deletedRoles);
        }
        public List<RolePermission> GetRolePermissionsByTypeID(int typeID)
        {
            return _context.Set<RolePermission>().Where(r => r.UserTypeID == typeID).ToList();
        }

        public List<ApplicationUser> GetApplicationUsersByTraderID(int traderID, ApplicationDbContext UsersContext)
        {
            return UsersContext.Set<ApplicationUser>().Where(r => r.TraderId == traderID).ToList();
        }

        public void DeleteAllUserRoles(string Id, ApplicationDbContext UsersContext)
        {
            List<ApplicationUserRole> userRoles = UsersContext.Set<ApplicationUserRole>().Where(r => r.UserId == Id).ToList();
            UsersContext.Set<ApplicationUserRole>().RemoveRange(userRoles);
            UsersContext.SaveChanges();
        }
        public void AddNewUserRoles(ApplicationUserRole newAppRole, ApplicationDbContext UsersContext)
        {
            UsersContext.Set<ApplicationUserRole>().Add(newAppRole);
            UsersContext.SaveChanges();
        }
       
        public void AddNewUserRoles(ApplicationUserRole newAppRole)
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            UsersContext.Set<ApplicationUserRole>().Add(newAppRole);
            UsersContext.SaveChanges();
        }

        public ApplicationUserRole GetApplicationUserRole(string userID, string roleID)
        { 
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            return (UsersContext.Set<ApplicationUserRole>().FirstOrDefault(r => r.UserId == userID && r.RoleId == roleID));
            
        }
        public ApplicationUserRole GetApplicationUserRoleByUserID(string userID)
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            return (UsersContext.Set<ApplicationUserRole>().FirstOrDefault(r => r.UserId == userID));

        }
        public void AddNewRole (ApplicationRole newRole)
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            UsersContext.Set<ApplicationRole>().Add(newRole);
            UsersContext.SaveChanges();
        }
        public void DeleteRoleByRoleName(string roleName)
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            ApplicationRole deletedRole = UsersContext.Set<ApplicationRole>().Where(r => r.Name == roleName).SingleOrDefault();
            if (deletedRole != null)
            {
                UsersContext.Set<ApplicationRole>().Remove(deletedRole);
                UsersContext.SaveChanges();
            }
        }
    }

}
