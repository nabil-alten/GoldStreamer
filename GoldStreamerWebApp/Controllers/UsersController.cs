using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using GoldStreamer.Helpers;
using GoldStreamer.Models.ViewModels;
using GoldStreamerWebApp.Models.ViewModels;
using localization.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace GoldStreamer.Controllers
{
    [AdvancedAuthorize, Authorize(Roles = "Admin")]
    [Internationalization]
    public class UsersController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; private set; }
       public IUserStore<ApplicationUser> store{get;set;} 
        private UnitOfWork uow = new UnitOfWork();
      private  ApplicationDbContext UsersContext = new ApplicationDbContext();

        public UsersController()
        {

            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UsersContext));
        }
        //
        // GET: /Users/
        public ActionResult Index()
        {
            if (Session["traderID"] == null)
                return RedirectToAction("Index", "Home");

            int traderId = Convert.ToInt32(Session["traderID"].ToString());
            // List<Users> lstusers =  uow.UsersRepo.GetByTraderId(traderId);


            ApplicationDbContext UsersContext = new ApplicationDbContext();

            List<ApplicationUser> userList = UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(o => o.UserName).ToList();


            return View(userList);
        }
        public ActionResult ActiveInActiveUsersIndex(string UserName, string FullName, int? Type, string sortOrder, int? pageSize)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSort = String.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : "";
            ViewBag.UserNameSort = sortOrder == "UserNameAsc" ? "UserName_desc" : "UserNameAsc";

            ViewBag.PageSize = pageSize;
            //  ViewBag.searchText = searchText;
            ViewBag.PageSize = pageSize;
            ViewBag.UserName = UserName;
            ViewBag.fullName = FullName;
            ViewBag.Type = Type;
            int x = 0;
            int y = Convert.ToInt32(pageSize);
            PageMethod(out x, out y);
          //  ApplicationUserViewModel traderVm = new ApplicationUserViewModel();
            List<ApplicationUserViewModel> lsttraderVm = new List<ApplicationUserViewModel>();
            //ApplicationUser trader = Mapper.Map<ApplicationUserViewModel, ApplicationUser>(traderVm);
            if (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(FullName) || (Type != null && Type != 0))
            {

                List<ApplicationUser> filteredAppUsers = uow.UsersRepo.BlockUnBolckSearch(UserName);
                List<Trader> filteredUsers = uow.UsersRepo.GetAllTradersAndUsersSearch(FullName, Type);
                foreach (ApplicationUser obj in filteredAppUsers)
                {
                    foreach (Trader trad in filteredUsers)
                    {
                        if (obj.TraderId == trad.ID)
                        {
                            ApplicationUserViewModel traderVm = new ApplicationUserViewModel();
                            traderVm.ID = obj.Id;
                            traderVm.TraderId = trad.ID;
                            traderVm.UserName = obj.UserName;
                            traderVm.typeFlg = trad.TypeFlag;
                            traderVm.Fullname = trad.Name + " " + trad.Family;
                            traderVm.IsActive = obj.IsActive;
                            lsttraderVm.Add(traderVm);
                        }
                    }
                }
                List<ApplicationUserViewModel> sorted = SortBy(sortOrder, lsttraderVm);
                ViewBag.count = sorted.Count();
                return View(sorted.ToPagedList(x, y));
            }
            else
            {
                List<ApplicationUser> filteredAppUsers = uow.UsersRepo.BlockUnBolckList();
                List<Trader> filteredUsers = uow.UsersRepo.GetAllTradersAndUsers();
                
                foreach (ApplicationUser obj in filteredAppUsers)
                {
                    foreach (Trader trad in filteredUsers)
                    {
                        if (obj.TraderId == trad.ID)
                        {
                            ApplicationUserViewModel traderVm = new ApplicationUserViewModel();
                            traderVm.ID = obj.Id;
                            traderVm.TraderId = trad.ID;
                            traderVm.UserName = obj.UserName;
                            traderVm.typeFlg = trad.TypeFlag;
                            traderVm.Fullname = trad.Name + " " + trad.Family;
                            traderVm.IsActive = obj.IsActive;
                            lsttraderVm.Add(traderVm);
                        }
                    }
                }
            }
            return View(SortBy(sortOrder, lsttraderVm).ToPagedList(x, y));
        }
        public PartialViewResult _ActiveInActiveList(string UserName, string FullName, int? Type, string sortOrder, int? pageSize, int? pageNumber)
        {

            ViewBag.PageSize = pageSize;
            ViewBag.UserName = UserName;
            ViewBag.fullName = FullName;
            ViewBag.Type = Type;
            ViewBag.PageNumber = pageNumber;

            //List<ApplicationUser> AlluSERS = uow.UsersRepo.BlockUnBolckList().ToList();
            //List<ApplicationUser> USERS = SortBy("FirstName_asc", AlluSERS);
            int x = 0;
            int y = Convert.ToInt32(pageSize);
            if(pageNumber==null)
            {
                x = 0;
            }
            else
            {
                x =(int) pageNumber;
            }
            if (pageNumber == 0) pageNumber = 1;
            PageMethod(out x , out y);
            ApplicationUserViewModel traderVm = new ApplicationUserViewModel();
            List<ApplicationUserViewModel> lsttraderVm = new List<ApplicationUserViewModel>();
           // ApplicationUser trader = Mapper.Map<ApplicationUserViewModel, ApplicationUser>(traderVm);
            if (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(FullName) || (Type != null&&Type!=0) )
            {
                List<ApplicationUser> filteredAppUsers = uow.UsersRepo.BlockUnBolckSearch( UserName);
                List<Trader> filteredUsers = uow.UsersRepo.GetAllTradersAndUsersSearch(FullName,Type);
               foreach(ApplicationUser obj in filteredAppUsers)
               {
                   foreach(Trader trad in filteredUsers)
                   {
                       if(obj.TraderId==trad.ID)
                       {
                           traderVm = new ApplicationUserViewModel();
                           traderVm.ID = obj.Id;
                           traderVm.TraderId = trad.ID;
                           traderVm.UserName = obj.UserName;
                           traderVm.typeFlg = trad.TypeFlag;
                           traderVm.Fullname = trad.Name + " " + trad.Family;
                           traderVm.IsActive = obj.IsActive;
                           lsttraderVm.Add(traderVm);
                       }
                   }
               }
               List<ApplicationUserViewModel> sorted = SortBy(sortOrder, lsttraderVm);
                ViewBag.count = sorted.Count();
                return PartialView("_ActiveInActiveList", sorted.OrderBy(o => o.UserName).ToPagedList(x, y));
            }
            else
            {
                List<ApplicationUser> filteredAppUsers = uow.UsersRepo.BlockUnBolckList();
                List<Trader> filteredUsers = uow.UsersRepo.GetAllTradersAndUsers();
                foreach (ApplicationUser obj in filteredAppUsers)
                {
                    foreach (Trader trad in filteredUsers)
                    {
                        if (obj.TraderId == trad.ID)
                        {
                            traderVm = new ApplicationUserViewModel();
                            traderVm.ID = obj.Id;
                            traderVm.TraderId = trad.ID;
                            traderVm.UserName = obj.UserName;
                            traderVm.typeFlg = trad.TypeFlag;
                            traderVm.Fullname = trad.Name + " " + trad.Family;
                            traderVm.IsActive = obj.IsActive;
                            lsttraderVm.Add(traderVm);
                        }
                    }
                }
            }
            ViewBag.count = lsttraderVm.Count();
            return PartialView("_ActiveInActiveList", lsttraderVm.ToPagedList(x, y));
        }
        public async Task<ActionResult> ActiveSelected(string[] ids, int? Activate, int pageNumber)
        {
            ApplicationUser userObj = new ApplicationUser();
            ViewBag.PageNumber = pageNumber;
            if(ids.Count()==0)
            {
                return new HttpStatusCodeResult(2);
            }
            foreach(string id in ids)
            {
               userObj = UsersContext.Users.Where(j => j.Id == id).First();
               if (Activate == 1)
               {
                   userObj.IsActive = true;
               }
               else if (Activate == 2)
               {
                  
                   userObj.IsActive = false;
               }
            }


            UsersContext.Entry(userObj).State = EntityState.Modified;
            await UsersContext.SaveChangesAsync();
                int x = 0;
                int y = 10;
                PageMethod(out x, out y);
                return PartialView("_ActiveInActiveList", new List<ApplicationUserViewModel>().ToPagedList(x, y));

        }
        public ActionResult _List(int? page)
        {
            if (Session["traderID"] == null)
                return new HttpStatusCodeResult(5);

            int traderId = Convert.ToInt32(Session["traderID"].ToString());
            List<Users> lstusers = uow.UsersRepo.GetByTraderId(traderId).OrderBy(p => p.UserName).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);
            ApplicationDbContext UsersContext = new ApplicationDbContext();
            List<ApplicationUser> userList = UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(o => o.UserName).ToList();
            ViewBag.PageNumber = pageNumber;
            return PartialView("_list", userList.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Search(string sortOrder, string searchtxt, int? page)
        {

            if (Session["traderID"] == null)
                return new HttpStatusCodeResult(5);

            int traderId = Convert.ToInt32(Session["traderID"].ToString());
            List<Users> UsersLst = new List<Users>();
            ApplicationDbContext UsersContext = new ApplicationDbContext();

            List<ApplicationUser> userList = new List<ApplicationUser>(); //UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(p => p.UserName).ToList();

            if (searchtxt != string.Empty)
            {
                userList = UsersContext.Users.Where(j => j.TraderId == traderId).Where(w => w.UserName.Contains(searchtxt.ToLower()) || w.PasswordHash.Contains(searchtxt.ToLower())).OrderBy(p => p.UserName).ToList();

            }
            else
            {
                userList = UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(p => p.UserName).ToList();
            }
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.searchtxt = searchtxt;
            return PartialView("_List", userList.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult _Add()
        {
            return PartialView("_Add");
        }

        [HttpPost]
        public async Task<ActionResult> _Add(string userName)
        {
            if (Session["traderID"] == null)
                return new HttpStatusCodeResult(5);

            bool check = uow.UsersRepo.IsUserNameExist(userName);
            if (check == true)
            {
                return new HttpStatusCodeResult(2);
            }
            string generatedPW = UsefulMethods.GenerateNewPassword();
            PasswordHasher hasher = new PasswordHasher();
            string hashed = hasher.HashPassword(generatedPW);
            ApplicationDbContext UsersContext = new ApplicationDbContext();

            ApplicationUser userObj = new ApplicationUser();
            int traderId = Convert.ToInt32(Session["traderID"].ToString());
            userObj.UserName = userName;
            userObj.TraderId = traderId;
            userObj.PasswordHash = generatedPW; //hashed;
            userObj.DefaultPassword = generatedPW;
            userObj.IsActive = true;
            userObj.NeedReset = true;
            userObj.IsVerified = true;
            userObj.TokenCreationDate = DateTime.Now;
            if (UsersContext.Users.Where(j => j.TraderId == traderId).ToList().Count() == 0) 
            { userObj.MainUser = true; }
            else userObj.MainUser = false;
            IdentityManager im = new IdentityManager();
            im.CreateUser(userObj, userObj.PasswordHash);

            //Assign Roles
            Trader traderObj = uow.TraderRepo.FindTraderById(traderId);
            List<RolePermission> listRoles = uow.RolePermissionRepo.GetRolePermissionsByTypeID(traderObj.TypeFlag);
            foreach (var role in listRoles)
            {
                ApplicationUserRole appUserRole = new ApplicationUserRole();
                appUserRole.UserId = userObj.Id;
                appUserRole.RoleId = role.AspNetRolesID.ToString();

                uow.RolePermissionRepo.AddNewUserRoles(appUserRole);
            }

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = 1;
            ViewBag.pageSize = pageSize;

            List<ApplicationUser> userList = UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(o => o.UserName).ToList();
            return PartialView("_List", userList.ToPagedList(pageNumber, pageSize));

        }

        [HttpPost]
        public async Task<ActionResult> UnActivateUsr(string statusId, int? tradId, int pageNumber)
        {
            int? traderId = 0;
            ViewBag.PageNumber = pageNumber;
            if (tradId == null)
            {
                if (Session["traderID"] == null)
                    return new HttpStatusCodeResult(5);
                else
                {
                    traderId = Convert.ToInt32(Session["traderID"].ToString());
                }
            }
            else
            {
                traderId = tradId;
            }
            var cut_id = statusId;
            var status = cut_id.Substring(0, 1);
            var id = cut_id.Substring(2, cut_id.Length - 2);
            ApplicationUser userObj = UsersContext.Users.Where(j => j.TraderId == traderId && j.Id == id).First();
            if (status == "1")
            {
                userObj.IsActive = true;
            }
            else
            {
               // await UserManager.UpdateSecurityStampAsync(userObj.Id);
                userObj.IsActive = false;
            }
            UsersContext.Entry(userObj).State = EntityState.Modified;
            await UsersContext.SaveChangesAsync();

            List<ApplicationUser> userList = UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(o => o.UserName).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
         //   int pageNumber = 1;
            ViewBag.pageSize = pageSize;
            if (tradId == null)
            {
                return PartialView("_List", userList.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                int x = 0;
                int y = 10;
               
                PageMethod(out x, out y);
                return PartialView("_ActiveInActiveList", new List<ApplicationUserViewModel>().ToPagedList(pageNumber, pageSize));
            }

        }

        public async Task<ActionResult> GeneratePassword(string id)
        {
            ApplicationDbContext UsersContext = new ApplicationDbContext();

            if (Session["traderID"] == null)
                return new HttpStatusCodeResult(5);
            int traderId = Convert.ToInt32(Session["traderID"].ToString());

            var Db = new ApplicationDbContext();
            ApplicationUser userObj = Db.Users.Where(j => j.TraderId == traderId && j.Id == id).First();
            string createdPass = UsefulMethods.GenerateNewPassword();
            PasswordHasher hasher = new PasswordHasher();
            userObj.PasswordHash = hasher.HashPassword(createdPass);
            userObj.NeedReset = true;
            userObj.DefaultPassword = createdPass;
            //var result = await UserManager.UpdateAsync(userObj);

            Db.Entry(userObj).State = EntityState.Modified;
            await Db.SaveChangesAsync();

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = 1;
            ViewBag.pageSize = pageSize;

            List<ApplicationUser> userList = UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(o => o.UserName).ToList();
            return PartialView("_List", userList.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult RestPassword(string UserName, string CurrPassword, string NewPassword)
        {

            Users objUser = new Users();
            List<Users> user = uow.UsersRepo.GetByUserName(UserName);
            if (user.Count == 0)
            {
                return new HttpStatusCodeResult(1);
            }
            bool check = uow.UsersRepo.IsUserOldPasswordExist(user[0].Id, CurrPassword);
            if (check == false)
            {
                return new HttpStatusCodeResult(2);
            }
            objUser = uow.UsersRepo.Find(user[0].Id);

            objUser.Password = NewPassword;
            objUser.NeedReset = false;
            uow.UsersRepo.Update(objUser);
            uow.SaveChanges();
            return View("ChangePassword", new UsersViewModel());
        }






        public async Task<ActionResult> EditMainLogin(string userId)
        {
            int traderId ;
            try
            {
                 traderId = Convert.ToInt32(Session["traderID"].ToString());

            }
            catch (Exception ex) { return null; }

            try
            {
                ApplicationUser oldMinUser = UsersContext.Users.First(j => j.TraderId == traderId && j.MainUser == true);
                oldMinUser.MainUser = false;
                UsersContext.Entry(oldMinUser).State = EntityState.Modified;
                await UsersContext.SaveChangesAsync();
            }
            catch (Exception x) { }
            ApplicationUser newMinUser = UsersContext.Users.First(j => j.Id == userId && j.TraderId == traderId);
            newMinUser.MainUser = true;
            UsersContext.Entry(newMinUser).State = EntityState.Modified;
            await UsersContext.SaveChangesAsync();

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            int pageNumber = 1;
            ViewBag.pageSize = pageSize;

            List<ApplicationUser> userList = UsersContext.Users.Where(j => j.TraderId == traderId).OrderBy(o => o.UserName).ToList();
            return PartialView("_List", userList.ToPagedList(pageNumber, pageSize));

        }

        public void PageMethod(out int pageNum, out int pageSiz)
        {
            string page = Request.QueryString["PageNumber"];
            string pageSize = Request.QueryString["pageSize"];
            if (page == null)
            {
                page = "1";
            }
            else
            {
                page = Request.QueryString["PageNumber"].ToString();
            }
            if (pageSize == null || pageSize == ConfigurationManager.AppSettings["DefaultPageSize"].ToString())
            {
                pageSiz = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"].ToString());
            }
            else
            {
                pageSiz = int.Parse(pageSize.ToString());
            }
            pageNum = 0;
            if (!string.IsNullOrEmpty(page))
            {
                pageNum = ((int?)int.Parse(page) ?? 1);
            }

        }
        public List<ApplicationUserViewModel> SortBy(String sortedby, List<ApplicationUserViewModel> Users)
        {
            switch (sortedby)
            {
                case "FirstName_desc":
                    Users = Users.OrderByDescending(s => s.Fullname).ToList();
                    break;

                case "UserName_desc":
                    Users = Users.OrderByDescending(s => s.UserName).ToList();
                    break;

                case "UserNameAsc":
                    Users = Users.OrderBy(s => s.UserName).ToList();
                    break;


                default:
                    Users = Users.OrderBy(s => s.Fullname).ToList();
                    break;
            }

            return Users;
        }
    }
}