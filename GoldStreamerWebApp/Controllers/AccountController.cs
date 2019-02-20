using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using GoldStreamer.Helpers;
using GoldStreamerWebApp.Models;
using localization.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Resources;

namespace GoldStreamerWebApp.Controllers
{
    //[OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    [Internationalization]
    public partial class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Session["userObj"] = null;
            Session["Counter"] = 0;
            Session["lastUser"] = null;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);

                if (user != null)
                {
                    if (user.TraderId > 0 && !uow.TraderRepo.FindTraderById(user.TraderId).IsActive)
                    {
                        ModelState.AddModelError("", Messages.Userinactive);
                        return View(model);
                    }
                    if (!user.IsActive)
                    {
                        ModelState.AddModelError("", Messages.Userinactive);
                        return View(model);
                    }
                    else if (user.IsVerified != true)
                    {
                        model.Resend = true;
                        ViewBag.Message = General.ResendMessage;
                        return View("DisplayEmail", model);
                    }
                    //else if (user.IsLockedOut == true)
                    //{
                    //    return View("LockedUser", model);
                    //}

                    else if (user.NeedReset == true)
                    {
                        //await SignInAsync(user, model.RememberMe);
                        LoginViewModel loginViewModel = new LoginViewModel();
                        loginViewModel = model;
                        return RedirectToAction("Manage", "Account", model);
                    }

                    else
                    {
                        await SignInAsync(user, model.RememberMe);
                       





                        return RedirectToLocal(returnUrl);

                    }
                }
                else
                {
                    // userbyName = await UserManager.FindByNameAsync(model.UserName);

                    var userbyName = await UserManager.FindByNameAsync(model.UserName);
                    if (userbyName != null)
                    {
                        userbyName.AccessFailedCount += 1;

                        /*
                        if (userbyName.AccessFailedCount >= Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedCount"]))
                        {
                            userbyName.IsLockedOut = true;

                            UserManager.Update(userbyName);
                            return View("LockedUser", model);

                        }*/
                        UserManager.Update(userbyName);
                    }

                    ModelState.AddModelError("", Messages.InvalidUserName);


                }
            }

            // If we got this far, something failed, redisplay form
            return View ( model);

        }

        [AllowAnonymous]
        public ActionResult Manage(ManageMessageId? message, LoginViewModel model)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.PasswordCantBeSame ? "Please creat different password"

                : "";
            ViewBag.HasLocalPassword = ViewBag.HasLocalPassword == null ? HasPassword(model.UserName) : ViewBag.HasLocalPassword;
            ViewBag.UserName = model.UserName;

            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = true;
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);

                if (!user.NeedReset)
                {
                    ViewBag.Message = Messages.PasswordChangedBefore;
                    return View("Error");
                }

                if (user.DefaultPassword != null && user.NeedReset)
                {

                    IdentityResult result = new IdentityResult();
                    if (model.OldPassword != model.NewPassword)
                    {

                        result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            //string UserName = User.Identity.Name;
                            //user = await UserManager.FindAsync(UserName, model.NewPassword);
                            user.NeedReset = false;
                            user.DefaultPassword = null;
                            user.AccessFailedCount = 0;
                            UserManager.Update(user);
                            ViewBag.message = General.PasswordChanged;
                            await SignInAsync(user, false);
                            return RedirectToAction("GeneralPurpose", "Home", new MessagesVM { Message = General.PasswordChanged });
                            //return View("GeneralPurpose");
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                    else
                    {
                        //new & old are the same
                        ModelState.AddModelError("", Messages.InvalidUserName);
                        IdentityResult error = new IdentityResult();
                        AddErrors(error);

                    }

                }
                else if (user.DefaultPassword == null && user.NeedReset)
                {
                    // User does not have a password so remove any validation errors caused by a missing OldPassword field
                    ModelState state = ModelState["OldPassword"];
                    if (state != null)
                    {
                        state.Errors.Clear();
                    }


                    IdentityResult result = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.NeedReset = false;
                        UserManager.Update(user);
                        ViewBag.message = General.PasswordChanged;
                        await SignInAsync(user, false);
                        return RedirectToAction("GeneralPurpose", "Home", new MessagesVM { Message = Messages.passwordSet });
                        //return View("GeneralPurpose");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess,model= new LoginViewModel{UserName = user.UserName}});
                    }
                    else
                    {
                        AddErrors(result);
                        ViewBag.HasLocalPassword = false;
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);


        }

        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string UserName,string code)
        {
            ViewBag.HasLocalPassword = false;
            ViewBag.UserName = UserName;
            var user = await UserManager.FindByNameAsync(UserName);
            var validTokenHours = Convert.ToDouble(ConfigurationManager.AppSettings["ValidTokenHours"]);
            if (!user.NeedReset)
            {
                ViewBag.Message = Messages.PasswordChangedBefore;
                return View("Error");
            }

            if (user.TokenCreationDate.AddHours(validTokenHours) < DateTime.Now)
            {
                ModelState.AddModelError("", Messages.TokenExpired);
                return View("ForgetPassword");
            }
            else if (user.Token != Guid.Parse(code))
            {
                //ViewBag.Message = Resources.Messages.NewTokenGenerated;
                ModelState.AddModelError("", Messages.NewTokenGenerated);
                return View("ForgetPassword");
            }
            ManageUserViewModel mu = new ManageUserViewModel { UserName = UserName, OldPassword = "8965893" };
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View("Manage", mu);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        private void AddErrors(IdentityResult result)
        {
            try
            {
                if (result.Errors.First() != "")
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", Messages.SamePasswordError);
            }

        }
        private bool HasPassword(string userName)
        {
            var user = UserManager.FindByName(userName);
            if (user != null)
            {
                return user.DefaultPassword != null;
            }
            return false;
        }
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error,
            PasswordCantBeSame
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult> SendNewPassword(LoginViewModel model)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                int userType = uow.TraderRepo.FindTraderById(user.TraderId).TypeFlag;
                if (user != null && userType != 1)
                {
                    if (!user.IsVerified || !user.IsActive)
                    {
                        ModelState.AddModelError("", !user.IsVerified ? Resources.Messages.VerifyAccount : Resources.Messages.Userinactive);
                        return View("ForgetPassword", model);
                    }
                    string destination = user.Email;
                    string generatedPW = UsefulMethods.GenerateNewPassword();
                    PasswordHasher hasher = new PasswordHasher();
                    string hashed = hasher.HashPassword(generatedPW);
                    //user.DefaultPassword = generatedPW;
                    user.PasswordHash = null;
                    user.Token = Guid.NewGuid();
                    user.NeedReset = true;
                    user.TokenCreationDate = DateTime.Now;
                    UserManager.Update(user);

                    model.Resend = false;
                    model.UserName = General.CheckMail;
                    //send new password mail .

                    var callbackUrl = Url.Action("ForgotPassword", "Account", new { userName = user.UserName, code = user.Token }, protocol: Request.Url.Scheme);
                    Mail objMail = new Mail { Destination = user.Email, Subject = "Forget Password", Body = GetBodyContent("كلمة السر الجديدة" , "Please click this link to set your new password: <a href=\"" + callbackUrl + "\">link</a>")  };
                    await UsefulMethods.SendAsync(objMail);
                    return View("DisplayEmail", model);
                }
                else
                {
                    ModelState.AddModelError("", userType == 1 ? Resources.Messages.SuperTraderType : Resources.Messages.UserInCorrect);
                    return View("ForgetPassword", model);
                }
            }
            else
            {
                ModelState.AddModelError("", user == null ? Resources.Messages.verifyUserName : Resources.Messages.userNotExsist);
                return View("ForgetPassword", model);
            }

        }


        public string GetBodyContent(string title , string content )
        {
            string html = System.IO.File.ReadAllText(Server.MapPath("~/MailTemplate/MailTemplate.html"));
         string htmlBody = html.Replace("#Titile#", title);
         string body= htmlBody.Replace("#TitleContent#", content);


            return body;
        }

        //[AdvancedAuthorize,Authorize(Roles = "Admin")]


    }
}