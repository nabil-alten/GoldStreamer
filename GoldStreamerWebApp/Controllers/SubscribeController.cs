using BLL.DomainClasses;
using DAL.UnitOfWork;
using localization.Helpers;
using RTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoldStreamer.Filters;

namespace GoldStreamer.Controllers
{

    [Internationalization]
    public class SubscribeController : Controller
    {
        //
        // GET: /Subscribe/
        UnitOfWork uow = new UnitOfWork();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult UnsubscribeFaild()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult UnSubscribe(Guid? id)
        {
            if (id != null)
            {
                Subscribe obj = uow.SubscribeRepo.Find(id);
                if (obj != null)
                {
                    obj.IsSubscribe = false;
                    uow.SubscribeRepo.Remove(obj.ID);
                    uow.SaveChanges();
                    return View("UnSubscribe");
                }
                else
                {
                    return View("UnsubscribeFaild");
                }
            }
            else
            {
                return View("UnsubscribeFaild");
            }
        }
        [AllowAnonymous]
        public ActionResult _Add()
        {
            Subscribe newObj = new Subscribe();
            return PartialView("_Add", newObj);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult _Add([Bind(Include = "Name ,Email")] Subscribe subscribe)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    subscribe.Name = subscribe.Name.TrimEnd(' ');
                    subscribe.Email = subscribe.Email.TrimEnd(' ');
                    int res = 0;
                    if (uow.SubscribeRepo.CheckEmailExists(subscribe.Email) == 1)
                    {
                        res = 1;
                    }
                    else
                    {
                        subscribe.ID = Guid.NewGuid();
                        subscribe.IsSubscribe = true;
                        uow.SubscribeRepo.Add(subscribe);
                        uow.SaveChanges();
                        res = 0;
                    }

                    if (res == 0)
                        return Json(new HttpStatusCodeResult(HttpStatusCode.OK, Resources.Messages.subscribesuccess));
                    else if (res == 1)
                        return Json(new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, Resources.Messages.EmailExists));
                    else
                        throw new Exception();
                }
                else
                    throw new Exception();

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                //throw new Exception(ExchangeWebApp.Helpers.ExceptionHelper.ParseErrors(ModelState),ex);
                return Json(new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [ValidateInput(false)]
        protected Editor PrepairEditor(Action<Editor> oninit)
        {
            //Editor editor = new Editor(System.Web.HttpContext.Current, "editor");

            //editor.ClientFolder = "/richtexteditor/";
            //editor.ContentCss = "/Content/example.css";
            ////editor.ClientFolder = "/Content/richtexteditor/";
            ////editor.ClientFolder = "/Scripts/richtexteditor/";

            //editor.Text = "";

            //editor.AjaxPostbackUrl = Url.Action("EditorAjaxHandler");

            //if (oninit != null) oninit(editor);

            ////try to handle the upload/ajax requests
            //bool isajax = editor.MvcInit();

            //if (isajax)
            //    return editor;

            ////load the form data if any
            //if (this.Request.HttpMethod == "POST")
            //{
            //    string formdata = this.Request.Form[editor.Name];
            //    if (formdata != null)
            //        editor.LoadFormData(formdata);
            //}

            ////render the editor to ViewBag.Editor
            //ViewBag.Editor = editor.MvcGetString();

            //return editor;
            return null;
        }
        [ValidateInput(false)]
        public ActionResult EditorAjaxHandler()
        {
            PrepairEditor(delegate(Editor editor)
            {

            });
            return new EmptyResult();
        }

        [ValidateInput(false)]
        public ActionResult AdminSubscribe()
        {
            PrepairEditor(delegate(Editor editor)
            {

            });
            return View();
        }
        [AdvancedAuthorize, Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> AdminSubscribe(string subject, string content = "")
        {
            try
            {
                //Editor theeditor = PrepairEditor(delegate(Editor editor)
                //{

                //});
                //if (theeditor.XHTML == "")
                //{
                //    return new HttpStatusCodeResult(2);
                //}
                List<Subscribe> lst = uow.SubscribeRepo.GetAllSubscribes();
                foreach (Subscribe obj in lst)
                {
                    var callbackUrl = Url.Action("UnSubscribe", "Subscribe", new { id = obj.ID }, protocol: Request.Url.Scheme);

                    Mail objMail = new Mail { Destination = obj.Email, Subject = subject, Body = GetBodyContent(subject, content + "<br> Unsubscribe by clicking this link: <a href=\"" + callbackUrl + "\">link</a>") };

                    await GoldStreamer.Helpers.UsefulMethods.SendAsync(objMail, true);
                }
                //  return new HttpStatusCodeResult(HttpStatusCode.OK, Resources.Messages.Saved);
                ViewBag._content = Resources.Messages.sent;
               // theeditor.Text = "";
                return View("AdminSubscribe", new Subscribe());
            }
            catch (Exception ex)
            {
                ViewBag._content = "";
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        public string GetBodyContent(string title, string content)
        {
            string html = System.IO.File.ReadAllText(Server.MapPath("~/MailTemplate/MailTemplate.html"));
            string htmlBody = html.Replace("#Titile#", title);
            string body = htmlBody.Replace("#TitleContent#", content);


            return body;
        }
    }
}