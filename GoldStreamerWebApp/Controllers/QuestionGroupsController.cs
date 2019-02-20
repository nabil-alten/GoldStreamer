using BLL.DomainClasses;
using BusinessServices;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Resources;
using localization.Helpers;

namespace GoldStreamer.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Admin")]
    [Internationalization]
    public class QuestionGroupsController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        QuestionGroupService questionGroupService = new QuestionGroupService();


        //
        // GET: /QuestionGroups/
        public ActionResult QuestionGroups()
        {
            return View();
        }

        
        //public PartialViewResult _List(int? page, string sortOrder)
        //{
        //    ViewBag.groupNameSorting = String.IsNullOrEmpty(sortOrder) ? "nameDsc" : "";
        //    ViewBag.buyPriceSorting = sortOrder == "nameAsc" ? "nameDesc" : "nameAsc";
        //    ViewBag.sellPriceSorting = sortOrder == "orderAsc" ? "orderDesc" : "orderAsc";

        //    List<QuestionGroup> questionGroupLst = questionGroupService.ListQuestionGroups(sortOrder);

            
        //    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
        //    int pageNumber = (page ?? 1);
        //    return PartialView("_AllGroups", questionGroupLst.ToPagedList(pageNumber, pageSize));
             
        //    return PartialView("_List", questionGroupLst);
        //}


        public PartialViewResult _List(string searchText)
        {
            //IE 9
            if (!string.IsNullOrEmpty(searchText))
                if (searchText.Equals("بحث..."))
                    searchText = string.Empty;

            ViewBag.GroupsSearchText = searchText;
            List<QuestionGroup> questionGroupLst = uow.QuestionGroupRepo.GetQuestionGroupList(searchText);
            return PartialView("_List", questionGroupLst);
        }

        [HttpPost]
        public ActionResult _Add(QuestionGroup _questionGroup)
        {
            try
            {
                if (uow.QuestionGroupRepo.CheckNameExists(_questionGroup.GroupName, null) == 1)
                    return new HttpStatusCodeResult(1);
                if (uow.QuestionGroupRepo.CheckOrderExists(_questionGroup.GroupOrder, null) == 1)
                    return new HttpStatusCodeResult(2);

                uow.QuestionGroupRepo.AddNewQuestionGroup(_questionGroup);
                uow.SaveChanges();
                
                return PartialView("_List", uow.QuestionGroupRepo.GetAll());
            }
            catch
            {
                return View();
            }
        }

        public ActionResult _Edit(string name, int order)
        {
            return View();
        }

        public JsonResult GetQuestionGroupInfo(int id)
        {
            QuestionGroup group = uow.QuestionGroupRepo.Find(id);
            string result = "Name:" + group.GroupName + ",Order:" + group.GroupOrder;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateQuestionGroup(QuestionGroup _questionGroup)
        {
            try
            {
                if (uow.QuestionGroupRepo.CheckNameExists(_questionGroup.GroupName, _questionGroup.ID) == 1)
                    return new HttpStatusCodeResult(1);
                if (uow.QuestionGroupRepo.CheckOrderExists(_questionGroup.GroupOrder, _questionGroup.ID) == 1)
                    return new HttpStatusCodeResult(2);
               
                uow.QuestionGroupRepo.UpdateQuestionGroup(_questionGroup);
                uow.SaveChanges();

                return PartialView("_List", uow.QuestionGroupRepo.GetAll());
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Delete(int id)
        {
            if (!uow.QuestionGroupRepo.hasAssignedQuestions(id))
            {
                uow.QuestionGroupRepo.DeleteByID(id);
                uow.SaveChanges();
            }
            else
                return new HttpStatusCodeResult(1, Resources.Messages.DeleteQuestionGroupVal);

            return PartialView("_List", uow.QuestionGroupRepo.GetAll());
        }
	}
}  