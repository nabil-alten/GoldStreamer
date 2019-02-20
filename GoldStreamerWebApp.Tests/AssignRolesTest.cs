using System;
using System.Collections.Generic;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class AssignRolesTest
    {
        UnitOfWork uow = null;
        [TestInitialize]
        public void Setup()
        {
            uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }

        public string AddNewRole()
        {
            ApplicationRole newRole = new ApplicationRole() 
            {
                Name = "NewRole",
                Description = "Test",
            };
            uow.RolePermissionRepo.AddNewRole(newRole);
            return newRole.Id;
        }
        public int AddNewRolePermission()
        {
            RolePermission newRolePermission = new RolePermission()
            {
                AspNetRolesID = uow.RolePermissionRepo.GetApplicationRoleByName("Admin"),
                UserTypeID = 2,

            };
            uow.RolePermissionRepo.AddNewRolePermissions(newRolePermission);
            uow.SaveChanges();
            return newRolePermission.ID;
        }



        
        [TestMethod]
        public void CanGetRoleByName()
        {
            Guid appRoleID=uow.RolePermissionRepo.GetApplicationRoleByName("Admin");
            RolePermission newRolePermission = new RolePermission()
            {
                AspNetRolesID =appRoleID,
                UserTypeID = 1,
            };

            uow.RolePermissionRepo.AddNewRolePermissions(newRolePermission);
            uow.SaveChanges();
            Assert.IsNotNull(newRolePermission.ID);
        }
       
        [TestMethod]
        public void CanAddNewRolePermission ()
        {
            int ID = AddNewRolePermission();
            Assert.IsNotNull(ID);
        }
        
        [TestMethod]
        public void CanDeleteRolePermissionByTypeID()
        {
            uow.RolePermissionRepo.DeleteRolePermissionsByTypeID(2);
            uow.SaveChanges();
            
            List<RolePermission> rolePermissionsList = uow.RolePermissionRepo.GetRolePermissionsByTypeID(2);
            Assert.AreEqual(0,rolePermissionsList.Count);
        }
      
        [TestMethod]
        public void CanGetRolePermissionsByTypeID()
        {
            uow.RolePermissionRepo.DeleteRolePermissionsByTypeID(2);
            uow.SaveChanges();
            int ID = AddNewRolePermission();

            List<RolePermission> rolePermissionsList = uow.RolePermissionRepo.GetRolePermissionsByTypeID(2);
            Assert.IsNotNull(rolePermissionsList);
        }
        
        [TestMethod]
        public void CanGetApplicationUsersByTraderID()
        {
            var UsersContext = new ApplicationDbContext();
            List<ApplicationUser> appUsers = uow.RolePermissionRepo.GetApplicationUsersByTraderID(0, UsersContext);
            Assert.IsNotNull(appUsers);
        }
        
        [TestMethod]
        public void CanDeleteAllUserRoles()
        {
            var UsersContext = new ApplicationDbContext();
            Guid UserID = uow.RolePermissionRepo.GetApplicationUserByName("Admin");
            uow.RolePermissionRepo.DeleteAllUserRoles(UserID.ToString(), UsersContext);
            Assert.IsNull(uow.RolePermissionRepo.GetApplicationUserRoleByUserID(UserID.ToString()));
        }

        
        [TestMethod]
        public void CanAddUserRoles()
        {
            Guid userID = uow.RolePermissionRepo.GetApplicationUserByName("Admin");
            uow.RolePermissionRepo.DeleteRoleByRoleName("AddUserRole");

            ApplicationRole newRole = new ApplicationRole()
            {
                Name = "AddUserRole",
                Description = "CanAddUserRole",
            };
            uow.RolePermissionRepo.AddNewRole(newRole);

            string RoleID = newRole.Id;
            ApplicationUserRole newUserRole = new ApplicationUserRole()
            {
                UserId = userID.ToString(),
                RoleId = RoleID,
            };
            uow.RolePermissionRepo.AddNewUserRoles(newUserRole);
            uow.SaveChanges();

            Assert.IsNotNull (uow.RolePermissionRepo.GetApplicationUserRole(userID.ToString(),RoleID));
        }

    }
}
