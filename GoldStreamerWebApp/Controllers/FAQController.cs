using localization.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.UnitOfWork;
using GoldStreamer.Models.ViewModels;

namespace GoldStreamer.Controllers
{
    [AllowAnonymous]
    [Internationalization]
    public class FAQController : Controller
    {
        private UnitOfWork _uow = new UnitOfWork();
        FAQViewModel faqVm = new FAQViewModel();
        public ActionResult Index()
        {
            faqVm.QuestionGroupList = _uow.QuestionGroupRepo.GetAllGroupsWithActiveQuestions();
            return View(faqVm);
        }
        public ActionResult _Accordion()
        {
            faqVm.QuestionsList = _uow.QuestionRepo.GetAll().Where(x => x.IsActive).ToList();
            return PartialView("_Accordion", faqVm);
        }
        public ActionResult GetFaqByQuestionGroup(int groupId)
        {
            faqVm.QuestionsList = _uow.QuestionRepo.GetAll().Where(x => x.IsActive && x.QuestionGroupId == groupId).ToList();
            return PartialView("_Accordion", faqVm);
        }


    }
}