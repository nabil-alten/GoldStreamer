using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.Repositories;
using DAL.UnitOfWork;
using GoldStreamer.Models.ViewModels;
using GoldStreamerWebApp.Models;

using CaptchaMvc.HtmlHelpers;

namespace GoldStreamerWebApp.Controllers
{
    public partial class AccountController
    {
        private UnitOfWork uow = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Register()
        {
            var Governorate = uow.GovernorateRepo.GetAllGovernorates();
            ViewBag.govs = Governorate; //new SelectList(Governorate, "ID", "Name");

            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser() { UserName = model.UserName, Token = Guid.NewGuid(), Email = model.Email };

        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            SendConfirmation(user);
        //            await SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        public async Task<ActionResult> SendConfirmation(ApplicationUser user)
        {
            //var code = Guid.NewGuid();
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userName = user.UserName, code = user.Token }, protocol: Request.Url.Scheme);

            //hlpr.sendMail(new Message());
            Mail objMail = new Mail { Destination = user.Email, Subject = "subject", Body =  GetBodyContent(" تأكيد الحساب" , "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>")};
            await GoldStreamer.Helpers.UsefulMethods.SendAsync(objMail);
            //await UserManager.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
            return View("DisplayEmail", new LoginViewModel { Resend = false });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ReSendConfirmation(LoginViewModel user1)
        {
            var user = await UserManager.FindByNameAsync(user1.UserName);

            user.Token = Guid.NewGuid();
            user.TokenCreationDate = DateTime.Now;
            await UserManager.UpdateAsync(user);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userName = user.UserName, code = user.Token }, protocol: Request.Url.Scheme);

            Mail objMail = new Mail { Destination = user.Email, Subject = "subject", Body = GetBodyContent(" تأكيد الحساب", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>") };
            await GoldStreamer.Helpers.UsefulMethods.SendAsync(objMail);
            ViewBag.Message = @Resources.General.ResendMessage;
            return View("DisplayEmail", new LoginViewModel { Resend = true });
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userName, string code)
        {//ValidTokenHours
            var validTokenHours = Convert.ToDouble(ConfigurationManager.AppSettings["ValidTokenHours"]);
            var result = false;
            if (userName == null || code == null)
            {
                return View("Error");
            }
            var user = await UserManager.FindByNameAsync(userName);

            if (user.IsVerified)
                ViewBag.Message = Resources.Messages.VerifiedBefore;
            else if (user.TokenCreationDate.AddHours(validTokenHours) < DateTime.Now)
            {
                ViewBag.Message = Resources.Messages.TokenExpired;
                return View("DisplayEmail", new LoginViewModel { UserName = userName, Resend = true });
            }
            else if (user.Token != Guid.Parse(code))
                ViewBag.Message = Resources.Messages.NewTokenGenerated;
            else
            {
                result = true; //await UserManager.ConfirmEmailAsync(userId, code);
                user.IsVerified = true;
                await UserManager.UpdateAsync(user);
            }
            return View(result ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult _TraderRegister()
        {
            var Governorate = uow.GovernorateRepo.GetAllGovernorates();
            ViewBag.govs = new SelectList(Governorate, "ID", "Name");

            return View("_TraderRegister", new TraderRegisterViewModel() { TypeFlag = 2, Governorates = Governorate });
        }

        [AllowAnonymous]
        public ActionResult _UserRegister()
        {
            var Governorate = uow.GovernorateRepo.GetAllGovernorates();
            ViewBag.govs = new SelectList(Governorate, "ID", "Name");

            return View("_UserRegister", new TraderRegisterViewModel() { TypeFlag = 3, Governorates = Governorate });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TraderRegister(TraderRegisterViewModel traderVm)
        {
            if (ModelState.IsValid)
            {
                var Governorate = uow.GovernorateRepo.GetAllGovernorates();
                ViewBag.govs = new SelectList(Governorate, "ID", "Name");
                traderVm.Governorates = Governorate;

                if (!this.IsCaptchaValid(Resources.Messages.CaptchaError))
                {
                    @ViewBag.Capatcha = Resources.Messages.CaptchaError;
                    if (traderVm.TypeFlag == 2)
                        return View("_TraderRegister", traderVm);
                    return View("_UserRegister", traderVm);
                }

                Trader trader = Mapper.Map<TraderRegisterViewModel, Trader>(traderVm);
                if (uow.TraderRepo.CheckEmailExists(trader.Email, trader.ID) == 1)
                {
                    ViewBag.error = Resources.Messages.EmailExists;

                    if (traderVm.TypeFlag == 2)
                        return View("_TraderRegister", traderVm);
                    return View("_UserRegister", traderVm);

                    //return RedirectToAction("_TraderRegister", traderVm);
                    //return PartialView("_TraderRegister",traderVm);
                }

                trader.IsActive = true;
                uow.TraderRepo.Add(trader);

                var user = new ApplicationUser() { UserName = traderVm.UserName, Token = Guid.NewGuid(), TokenCreationDate = DateTime.Now, Email = traderVm.Email, IsActive = true };

                var result = await UserManager.CreateAsync(user, traderVm.Password);

                if (result.Succeeded)
                {
                    uow.SaveChanges();
                    user.TraderId = trader.ID;
                    await UserManager.UpdateAsync(user);
                    await SendConfirmation(user);

                    //Assign roles 
                    List<RolePermission> listRoles = uow.RolePermissionRepo.GetRolePermissionsByTypeID(trader.TypeFlag);
                    foreach (var role in listRoles)
                    {
                        ApplicationUserRole appUserRole = new ApplicationUserRole();
                        appUserRole.UserId = user.Id;
                        appUserRole.RoleId = role.AspNetRolesID.ToString();

                        uow.RolePermissionRepo.AddNewUserRoles(appUserRole);
                    }


                    return View("DisplayEmail");
                    //await SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                    ViewBag.error = ((string[])(result.Errors))[0].Contains("Name") ? Resources.Messages.UserNameRepeated : ((string[])(result.Errors))[0];
                }
            }

            // If we got this far, something failed, redisplay form
            if (traderVm.TypeFlag == 2)
            {
                return View("_TraderRegister", traderVm);
            }
            else return View("_UserRegister", traderVm);
        }

        [AllowAnonymous]
        public ActionResult GetCity(int govId)
        {
            List<City> city = uow.CityRepo.GetGovCities(govId);
            city.Insert(0, new City { Code = "", Name = "اختر", ID = 0 });

            return Json(new { city }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetRegion(int cityId)
        {
            List<Region> district = uow.RegionRepo.GetCityRegions(cityId);
            district.Insert(0, new Region { Code = "", Name = "اختر", ID = 0 });
            return Json(new { district }, JsonRequestBehavior.AllowGet);
        }

    }
}