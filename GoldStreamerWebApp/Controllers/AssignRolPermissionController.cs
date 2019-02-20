using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using GoldStreamer.Models.ViewModels;
using localization.Helpers;

namespace GoldStreamer.Controllers
{
    [Internationalization]
    [AdvancedAuthorize,Authorize(Roles = "Admin")]

    public class AssignRolPermissionController : Controller
    {
        private UnitOfWork uow = null;
        private RolePermissionViewModel rolePermisionsVM = null;
        public AssignRolPermissionController()
        {
            uow = new UnitOfWork();
            rolePermisionsVM = new RolePermissionViewModel();
        }
        //
        // GET: /AssignRolPermission/

        public ActionResult Index()
        {
            List<ApplicationRole> applicationRolesList = uow.RolePermissionRepo.GetAllApplicationRoles();
            List<RolePermission> rolePermissions = uow.RolePermissionRepo.GetAllRolePermisions();
            List<RolePermissionViewModel> rolePermissionVMList = new List<RolePermissionViewModel>();
            foreach (var item in applicationRolesList)
            {
                if (item.Name != "Admin")
                {
                    rolePermisionsVM = new RolePermissionViewModel();
                    rolePermisionsVM.Id = item.Id;
                    rolePermisionsVM.Name = item.Description;
                    rolePermisionsVM.Selected = false;
                    rolePermisionsVM.Description = "";
                    rolePermissionVMList.Add(rolePermisionsVM);
                }
            }

            foreach (var x in rolePermissionVMList)
            {
                if (x.Name != "Admin")
                {
                    if (uow.RolePermissionRepo.FindRolePermissionByTypeAndRole(Guid.Parse(x.Id), 1).Count() > 0)
                    {
                        x.Selected = true;
                    }
                }

            }
            return View(rolePermissionVMList);
        }

        public ActionResult Update(Guid[] rolesIds, string typeId)
        {
            SaveAssignedroles(rolesIds, int.Parse(typeId));
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult _List()
        {
            List<ApplicationRole> applicationRolesList = uow.RolePermissionRepo.GetAllApplicationRoles();
            List<RolePermission> rolePermissions = uow.RolePermissionRepo.GetAllRolePermisions();
            List<RolePermissionViewModel> rolePermissionVMList = new List<RolePermissionViewModel>();
            foreach (var item in applicationRolesList)
            {
                if (item.Name != "Admin")
                {
                    rolePermisionsVM = new RolePermissionViewModel();
                    rolePermisionsVM.Id = item.Id;
                    rolePermisionsVM.Name = item.Description;
                    rolePermisionsVM.Selected = false;
                    rolePermisionsVM.Description = "";
                    rolePermissionVMList.Add(rolePermisionsVM);
                }
            }

            foreach (var x in rolePermissionVMList)
            {
                if (x.Name != "Admin")
                {
                    if (uow.RolePermissionRepo.FindRolePermissionByTypeAndRole(Guid.Parse(x.Id), 1).Count() > 0)
                    {
                        x.Selected = true;
                    }
                }

            }
            return PartialView("_List",rolePermissionVMList);
        }
        public void SaveAssignedroles(Guid[] rolesIds, int typeId)
        {
            uow.RolePermissionRepo.SaveAssignedRolesInTransaction(rolesIds, typeId);
        }

        public ActionResult LoadAssignedRoles(int? typeId)
        {
             List<RolePermission> rolePermissions = uow.RolePermissionRepo.GetRolePermissionsByTypeID(typeId.Value);
            List<ApplicationRole> allRoles= uow.RolePermissionRepo.GetAllApplicationRoles();

            List<RolePermissionViewModel> rolePermissionVMList = new List<RolePermissionViewModel>();

            foreach (var item in allRoles)
            {
                if (item.Name != "Admin")
                {
                    rolePermisionsVM = new RolePermissionViewModel();
                    rolePermisionsVM.Id = item.Id;
                    rolePermisionsVM.Name = item.Description;
                    rolePermisionsVM.Description = "";
                    rolePermissionVMList.Add(rolePermisionsVM);

                    if (IsRolePermissionExsists(item, rolePermissions))
                        rolePermisionsVM.Selected = true;
                    else
                        rolePermisionsVM.Selected = false;
                }
            }

            return PartialView("_List",rolePermissionVMList);
        }

        private bool IsRolePermissionExsists(ApplicationRole applicationRole, List<RolePermission> roles)
        {
            RolePermission role = roles.Where(s => s.AspNetRolesID == Guid.Parse(applicationRole.Id)).SingleOrDefault();
            if (role == null)
                return false;
            else
                return true;
        }

    }
}