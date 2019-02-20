using AutoMapper;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.UnitOfWork;
using GoldStreamer.Helpers;
using GoldStreamer.Models.ViewModels;
using GoldStreamerWebApp.Models;
using localization.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldStreamer.Controllers
{
    [System.Web.Mvc.Authorize]
    [Internationalization]
    public class EditProfileController : Controller
    {
        //
        // GET: /EditProfile/
        UnitOfWork _uow = new UnitOfWork();

        public ActionResult Index()
        {
            Trader loggedInUser = GetLoggedTraderOrUser();
            if (loggedInUser != null)   // if not admin
            {
                int userType = loggedInUser.TypeFlag;
                if (userType == 1)
                {
                    var um = new UserManager<ApplicationUser>(
                                   new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var user = um.FindById(User.Identity.GetUserId());

                    if (user.MainUser)
                        return EditSuperTraderProfile();
                    else
                    {
                        return View("../Redirector/NoPermissionToEditProfile");
                    }
                }
                else if (userType == 2)
                    return EditTraderProfile();
                else if (userType == 3)
                    return EditUserProfile();
                else
                    return View();
            }
            else
                return View("../Redirector/NoPermissionToEditProfile");
        }


        private Trader GetLoggedTraderOrUser()
        {
            var um = new UserManager<ApplicationUser>(
                               new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(User.Identity.GetUserId());

            return _uow.TraderRepo.FindTraderById(user.TraderId);
        }

        public ActionResult EditUserProfile()
        {
            var um = new UserManager<ApplicationUser>(
                               new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(User.Identity.GetUserId());

            Trader traderUser = GetLoggedTraderOrUser();

            return View("EditUserProfile", new EditProfileViewModel()
            {
                UserName = traderUser.Name,
                Family = traderUser.Family,
                Gender = traderUser.Gender,
                Phone = traderUser.Phone,
                Mobile = traderUser.Mobile,
                Name = user.UserName,
                Email = traderUser.Email,
                ReEmail = traderUser.Email,
                IsPublicProfile = traderUser.IsPublicProfile,
                IsRegistered = traderUser.IsRegistered,
                TypeFlag = 2,
            });
        }
        public ActionResult EditTraderProfile()
        {
            var um = new UserManager<ApplicationUser>(
                              new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(User.Identity.GetUserId());

            Trader traderUser = GetLoggedTraderOrUser();

            var Governorate = _uow.GovernorateRepo.GetAllGovernorates();
            var Cities = _uow.CityRepo.GetAllCities();
            var Districs = _uow.RegionRepo.GetAllRegions();

            return View("EditTraderProfile", new EditProfileViewModel() 
            { 
              UserName = traderUser.Name, 
              Family = traderUser.Family, 
              Gender = traderUser.Gender, 
              Phone = traderUser.Phone, 
              Mobile = traderUser.Mobile,
              Name = user.UserName,
              Email = traderUser.Email,
              ReEmail = traderUser.Email,
              TypeFlag = 2,
              Governorates = Governorate,
              Cities = Cities,
              Districts = Districs,
              City = traderUser.City,
              IsRegistered = traderUser.IsRegistered,
              IsPublicProfile = traderUser.IsPublicProfile,
              District = traderUser.District,
              Governorate = traderUser.Governorate
            });
        }
        public ActionResult EditSuperTraderProfile()
        {
            var um = new UserManager<ApplicationUser>(
                              new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(User.Identity.GetUserId());

            Trader superTraderUser = GetLoggedTraderOrUser();
            var Governorate = _uow.GovernorateRepo.GetAllGovernorates();
            var Cities = _uow.CityRepo.GetAllCities();
            var Districs = _uow.RegionRepo.GetAllRegions();

            
            EditProfileViewModel traderVM = new EditProfileViewModel() 
            {
              UserName = superTraderUser.Name, 
              Family = superTraderUser.Family, 
              Gender = superTraderUser.Gender, 
              Phone = superTraderUser.Phone, 
              Mobile = superTraderUser.Mobile,
              Name = user.UserName,
              Email = superTraderUser.Email,
              ReEmail = superTraderUser.Email,
              TypeFlag = 2,
              ShopName = superTraderUser.ShopName,
              Governorates = Governorate,
              Cities = Cities,
              Districts = Districs,
              City = superTraderUser.City,
              IsPublicProfile = superTraderUser.IsPublicProfile,
              IsRegistered = superTraderUser.IsRegistered,
              District = superTraderUser.District,
              Governorate = superTraderUser.Governorate,
              SortOrder = superTraderUser.SortOrder,
            };
            

            return View("EditSuperTraderProfile", traderVM);
        }


        [HttpPost]
        public ActionResult UpdateUserProfile(Trader updatedUser, string Password, string OldPassword, string ConfirmPassword)
        {
            if (ValidatePassword(OldPassword, Password, ConfirmPassword))
            {
                Trader loggedInTrader = GetLoggedTraderOrUser();
                updatedUser.ID = loggedInTrader.ID;
                updatedUser.TypeFlag = 3;
                updatedUser.IsActive = true;
                /*
                if (_uow.TraderRepo.CheckNameExists(updatedUser) == 1)
                    return new HttpStatusCodeResult(3);
                if (_uow.TraderRepo.CheckPhoneExists(updatedUser) == 1)
                    return new HttpStatusCodeResult(6);
                if (_uow.TraderRepo.CheckMobileExists(updatedUser) == 1)
                    return new HttpStatusCodeResult(4);
                */

                if (_uow.TraderRepo.CheckEmailExists(updatedUser.Email, updatedUser.ID) == 1)
                    return new HttpStatusCodeResult(1);


                if (!string.IsNullOrEmpty(Password))
                {
                    var um = new UserManager<ApplicationUser>(
                                       new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var user = um.FindById(User.Identity.GetUserId());

                    PasswordHasher hasher = new PasswordHasher();
                    string HasSucceed = hasher.VerifyHashedPassword(user.PasswordHash, OldPassword).ToString();

                    if (HasSucceed.Equals("Success"))
                    {
                        user.PasswordHash = hasher.HashPassword(Password);
                        var result = um.UpdateAsync(user);
                    }
                    else
                        return new HttpStatusCodeResult(7);
                }

                _uow.TraderRepo.Update(updatedUser);
                _uow.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return new HttpStatusCodeResult(8);
        }

        [HttpPost]
        public ActionResult UpdateTraderProfile(Trader traderProfile, string Password, string OldPassword, string ConfirmPassword)
        {
            if (ValidatePassword(OldPassword, Password, ConfirmPassword))
            {
                Trader loggedInTrader = GetLoggedTraderOrUser();
                traderProfile.ID = loggedInTrader.ID;
                traderProfile.TypeFlag = 2;
                traderProfile.IsActive = true;

                /*
                if (_uow.TraderRepo.CheckNameExists(traderProfile) == 1)
                    return new HttpStatusCodeResult(3);
                if (_uow.TraderRepo.CheckPhoneExists(traderProfile) == 1)
                    return new HttpStatusCodeResult(6);
                if (_uow.TraderRepo.CheckMobileExists(traderProfile) == 1)
                    return new HttpStatusCodeResult(4);
                */

                if (_uow.TraderRepo.CheckEmailExists(traderProfile.Email, traderProfile.ID) == 1)
                    return new HttpStatusCodeResult(1);


                if (!string.IsNullOrEmpty(Password))
                {
                    var um = new UserManager<ApplicationUser>(
                                      new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var user = um.FindById(User.Identity.GetUserId());

                    PasswordHasher hasher = new PasswordHasher();
                    string HasSucceed = hasher.VerifyHashedPassword(user.PasswordHash, OldPassword).ToString();

                    if (HasSucceed.Equals("Success"))
                    {
                        user.PasswordHash = hasher.HashPassword(Password);
                        var result = um.UpdateAsync(user);
                    }
                    else
                        return new HttpStatusCodeResult(7);

                }

                _uow.TraderRepo.Update(traderProfile);
                _uow.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return new HttpStatusCodeResult(8);
        }

        [HttpPost]
        public ActionResult UpdateSuperTrader(Trader trader, string OldPassword, string Password, string ConfirmPassword)
        {
            if (ValidatePassword(OldPassword, Password, ConfirmPassword))
            {

                Trader loggedInTrader = GetLoggedTraderOrUser();
                trader.ID = loggedInTrader.ID;
                trader.TypeFlag = 1;
                trader.IsActive = true;

                /*
                if (_uow.TraderRepo.CheckPhoneExists(trader) == 1)
                    return new HttpStatusCodeResult(6);
                if (_uow.TraderRepo.CheckMobileExists(trader) == 1)
                    return new HttpStatusCodeResult(4);
                if (_uow.TraderRepo.CheckOrderExists(trader.SortOrder, trader.ID) == 1)
                    return new HttpStatusCodeResult(2);
                */
                if (_uow.TraderRepo.CheckNameExists(trader) == 1)
                    return new HttpStatusCodeResult(3);
                if (_uow.TraderRepo.CheckEmailExists(trader.Email, trader.ID) == 1)
                    return new HttpStatusCodeResult(1);


                if (!string.IsNullOrEmpty(Password))
                {
                    var um = new UserManager<ApplicationUser>(
                                       new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var user = um.FindById(User.Identity.GetUserId());

                    PasswordHasher hasher = new PasswordHasher();
                    string HasSucceed = hasher.VerifyHashedPassword(user.PasswordHash, OldPassword).ToString();

                    if (HasSucceed.Equals("Success"))
                    {
                        user.PasswordHash = hasher.HashPassword(Password);
                        var result = um.UpdateAsync(user);
                    }
                    else
                        return new HttpStatusCodeResult(7);
                }

                _uow.TraderRepo.Update(trader);
                _uow.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return new HttpStatusCodeResult(8);
        }

        private bool ValidatePassword(string OldPassword, string Password, string ConfirmPassword)
        {
            if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && !string.IsNullOrEmpty(OldPassword))
                return true;
            if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword) && string.IsNullOrEmpty(OldPassword))
                return true;
            if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword) && !string.IsNullOrEmpty(OldPassword))
                return true;
            else
                return false;
        }
	}
}