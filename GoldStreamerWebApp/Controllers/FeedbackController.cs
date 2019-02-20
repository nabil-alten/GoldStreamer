using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using localization.Helpers;

namespace GoldStreamer.Controllers
{
    [AllowAnonymous]
    [Internationalization]
    public class FeedbackController : Controller
    {
        private UnitOfWork uow = new UnitOfWork();
        //
        // GET: /Feedback/
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendFeedback(Feedback feedbackObj)
        {
            uow.FeedbackRepo.Add(feedbackObj);
            uow.SaveChanges();

            Mail objMail = new Mail { Destination = feedbackObj.Email, Subject = Resources.General.FeedbackSubject, Body = GetBodyContent(Resources.General.FeedbackSubject, Resources.Messages.FeedbackAutoMail) };
            await GoldStreamer.Helpers.UsefulMethods.SendAsync(objMail);
                  
            return View("confirmFeedback");
        }
        public ActionResult UsageTerms()
        {
            return View();
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