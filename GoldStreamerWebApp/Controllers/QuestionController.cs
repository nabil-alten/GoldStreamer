using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using GoldStreamer.Filters;
using localization.Helpers;
using PagedList;
using Resources;
using GoldStreamer.Models.ViewModels;
using System.Threading.Tasks;

namespace GoldStreamer.Controllers
{
    [AdvancedAuthorize, Authorize(Roles = "Admin")]
    [Internationalization]
    public class QuestionController : Controller
    {
        private UnitOfWork uow = new UnitOfWork();
        //
        // GET: /Question/
        public ActionResult Index(string searchText, int? isActive, bool? UserQuestion, bool? NoCategoryQuestions, bool? NoAnswersQuestions, bool? QuestionsNoAnswer, int? pageSize)
        {
            if(pageSize == null || pageSize == 0)
            {
                pageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"].ToString());
            }
            ViewBag.PageSize = pageSize;
            ViewBag.isActive = isActive;
            ViewBag.UserQuestion = UserQuestion;
            ViewBag.NoCategoryQuestions = NoCategoryQuestions;
            ViewBag.NoAnswersQuestions = NoAnswersQuestions;
            ViewBag.QuestionsNoAnswer = QuestionsNoAnswer;
            
            ViewBag.SearchText = searchText;
            ViewBag.Groups = uow.QuestionGroupRepo.GetAll();
            List<Question> questionList = uow.QuestionRepo.GetAll();

            foreach (var question in questionList)
            {
                if (question.QuestionGroupId != null)
                {
                    question.QuestionGroup = uow.QuestionGroupRepo.Find(question.QuestionGroupId).GroupName;
                }
            }
            int x = 0;
            int y = Convert.ToInt32(pageSize);
            PageMethod(out x, out y);
            return View(questionList.ToPagedList(x, y));
        }
        [AllowAnonymous]
        public ActionResult UserQuestion()
        {
            return View();
        }
        public PartialViewResult _List(string searchText, int? isActive, bool? UserQuestion, bool? NoCategoryQuestions, bool? NoAnswersQuestions, bool? QuestionsNoAnswer, int pageNumber, int pageSize)
        {
            ViewBag.PageSize = pageSize;
            ViewBag.isActive = isActive;
            ViewBag.UserQuestion = UserQuestion;
            ViewBag.NoCategoryQuestions = NoCategoryQuestions;
            ViewBag.NoAnswersQuestions = NoAnswersQuestions;
            ViewBag.QuestionsNoAnswer = QuestionsNoAnswer;
            ViewBag.pageNumber = pageNumber;
            ViewBag.SearchText = searchText;
            int x = pageNumber;
            int y = pageSize;
            if (pageNumber == 0) pageNumber = 1;
            PageMethod(out x, out y);
            List<Question> questionList = null;
            if (string.IsNullOrEmpty(searchText) && UserQuestion==false && isActive==3)
            {
                questionList = uow.QuestionRepo.GetAll();
            }
            else
            {
                questionList = uow.QuestionRepo.Search(searchText,isActive,UserQuestion,NoCategoryQuestions,NoAnswersQuestions,QuestionsNoAnswer);
            }

            foreach (var question in questionList)
            {
                if (question.QuestionGroupId != null)
                {
                    question.QuestionGroup = uow.QuestionGroupRepo.Find(question.QuestionGroupId).GroupName;
                }
            }
            return PartialView("_List", questionList.ToPagedList(x, y));
        }
        public ActionResult UnActivateQuestion(string statusId,int confirm, int pageNumber)
        {

            ViewBag.PageNumber = pageNumber;

            var cut_id = statusId;
            var status = cut_id.Substring(0, 1);
            var id = cut_id.Substring(2, cut_id.Length - 2);
            Question QuestionObj = uow.QuestionRepo.Find(int.Parse(id.ToString()));
           
            if (status == "1")
            {
                if (QuestionObj.QuestionGroupId == null || QuestionObj.Answer == null || QuestionObj.Answer =="")
                {
                    return new HttpStatusCodeResult(9);
                }
                QuestionObj.IsActive = true;
                if(QuestionObj.IsRepeated)
                {
                    List<Question> lstQustions = uow.QuestionRepo.GetActiveQuestions(QuestionObj);
                    if (lstQustions.Count > 0 && confirm == 0)
                    {
                        return new HttpStatusCodeResult(8);
                    }
                    else if(lstQustions.Count>0 && confirm==1)
                    {
                        uow.QuestionRepo.ChangeActiveAssignment(QuestionObj);
                    }
                }
            }
            else
            {
                QuestionObj.IsActive = false;
            }
            uow.QuestionRepo.Update(QuestionObj);
            uow.SaveChanges();
            List<Question> lst =new List<Question>();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            //   int pageNumber = 1;
            ViewBag.pageSize = pageSize;
            return PartialView("_List", lst.ToPagedList(pageNumber, pageSize));
        }
        public async Task<ActionResult> SendQuestionReply(int Id, int confirm, int pageNumber)
        {

            ViewBag.PageNumber = pageNumber;
            Question QuestionObj = uow.QuestionRepo.Find(Id);
                if (QuestionObj.IsReplied && confirm==0)
                {
                    return new HttpStatusCodeResult(9);
                }
            if(QuestionObj.Answer==null)
            {
                return new HttpStatusCodeResult(5);
            }
            QuestionObj.IsReplied = true;
            uow.QuestionRepo.Update(QuestionObj);
            uow.SaveChanges();
            Mail objMail = new Mail { Destination = QuestionObj.Email, Subject = "أسئلة الزوار - موقع الصاغة", Body = GetBodyContent("أسئلة الزوار - موقع الصاغة", "مرحباً " + QuestionObj.Name + "، <br /><br />" + "لقد تم استقبال سؤالكم الذي كان نصه" + "<br /><br />" + QuestionObj.QuestionText + "<br /><br />" + "اليكم الرد التالي" + "<br /><br />" + QuestionObj.Answer + "<br /><br />" + "<a href=\"" + QuestionObj.VideoLink + "\">" + QuestionObj.VideoLink + "</a>") };

            await GoldStreamer.Helpers.UsefulMethods.SendAsync(objMail, true);
            List<Question> lst = new List<Question>();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            //   int pageNumber = 1;
            ViewBag.pageSize = pageSize;
            return PartialView("_List", lst.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult _Add()
        {
            ViewBag.Groups = uow.QuestionGroupRepo.GetAll();
            Question newobj=new Question();
            return PartialView("_Add",newobj );
        }
        
        public ActionResult Save(int groupId, string questionText, string answerText, string videoLink, int questionId, int? pageSize, int? pageNumber)
        {
            Question question = new Question();
            int x = 1;
            int y = Convert.ToInt32(pageSize);
            if (y == 0 ) y = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"].ToString());
            //PageMethod(out x, out y);
            if (questionId == 0)
            {
                question.QuestionText = questionText.Trim();
                if (answerText == ""||answerText==null)
                {
                    question.Answer = null;
                }
                else
                {
                    question.Answer = answerText.Trim();
                }
                question.VideoLink = videoLink.Trim();
                if (groupId != 0)
                {
                    question.QuestionGroupId = groupId;
                }

                else
                {
                    question.QuestionGroupId = null;
                }
                //question.QuestionGroup = uow.QuestionGroupRepo.Find(groupId).GroupName;
                question.ReadingCounter = 0;
                question.IsActive =question.QuestionGroupId !=null; 
                uow.QuestionRepo.Add(question);
            }

            else
            {
               question = uow.QuestionRepo.Find( questionId);
               
               question.QuestionText = questionText.Trim();
               if (answerText == "" || answerText == null)
                {
                    question.Answer = null;
                }
                else
                {
                    question.Answer = answerText.Trim();
                }
              
               question.VideoLink = videoLink.Trim();
                if(groupId!=0)
                {
                    question.QuestionGroupId = groupId;
                }
              
                else
                {
                    question.QuestionGroupId = null;
                }
                
                //question.QuestionGroup = uow.QuestionGroupRepo.Find(groupId).GroupName;
                uow.QuestionRepo.Update(question);
            }

            if (uow.QuestionRepo.CheckIfQuestionExist(question.QuestionText, questionId) == true && !question.IsUserQuestion)
            {
                return Json(new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, Messages.BasketNameDuplicate));
            }
                
            if (question.IsActive && (answerText == "" || answerText == null || groupId == 0 || question.Answer == null || question.QuestionGroupId == null))
                {
                    return Json(new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, Resources.Messages.Canotsavequestion));
                }
                uow.SaveChanges();
                List<Question> questionList = uow.QuestionRepo.GetAll();
                ViewBag.PageSize = pageSize;
                foreach (var questionItem in questionList)
                {
                    if (questionItem.QuestionGroupId != null)
                    {
                        questionItem.QuestionGroup = uow.QuestionGroupRepo.Find(questionItem.QuestionGroupId).GroupName;
                    }
                }
                return PartialView("_List", questionList.ToPagedList(x , y));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SaveUserQuestion([Bind(Include = "Name ,Email,QuestionText")] Question question)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    question.Name = question.Name.TrimEnd(' ');
                    question.Email = question.Email.TrimEnd(' ');
                    question.QuestionText = question.QuestionText.TrimEnd(' ');
                    int res = 0;
                    int maxId=uow.QuestionRepo.CheckUserQuestionExistMax(question.QuestionText);
                    if (maxId > 0)
                    {
                        Question obj = uow.QuestionRepo.Find(maxId);
                        if (obj.Answer != null && obj.Answer!="")
                        {

                            Mail objMail = new Mail { Destination = question.Email, Subject = "أسئلة الزوار - موقع الصاغة", Body = GetBodyContent("أسئلة الزوار - موقع الصاغة", "مرحباً " + question.Name + "، <br /><br />" + "لقد تم استقبال سؤالكم الذي كان نصه" + "<br /><br />" + question.QuestionText + "<br /><br />" + "اليكم الرد التالي" + "<br /><br />" + obj.Answer + "<br /><br />" + "<a href=\"" + obj.VideoLink + "\">" + obj.VideoLink + "</a>") };
                            await GoldStreamer.Helpers.UsefulMethods.SendAsync(objMail, true);
                            res = 0;
                        } 
                        else
                        {
                            question.IsRepeated = true;
                            question.IsActive = false;
                            question.IsReplied = false;
                            question.IsUserQuestion = true;
                            question.ReadingCounter = 0;
                            uow.QuestionRepo.Add(question);
                            uow.SaveChanges();
                            res = 0;
                        }
                    }
                    else
                    {
                        question.IsActive = false;
                        question.IsReplied = false;
                        question.IsUserQuestion = true;
                        question.ReadingCounter = 0;
                        uow.QuestionRepo.Add(question);
                        uow.SaveChanges();
                        res = 0;
                    }

                    if (res == 0)
                        return Json(new HttpStatusCodeResult(HttpStatusCode.OK, Resources.Messages.sent));
                    else if (res == 1)
                        return Json(new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, Resources.Messages.BasketNameDuplicate));
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
        public ActionResult _Edit(int? questionId)
        {
            ViewBag.Groups = uow.QuestionGroupRepo.GetAll();
            Question question = uow.QuestionRepo.Find(int.Parse(questionId.ToString()));
            return PartialView("_Add", question);

        }


        public ActionResult Delete(int? questionId, int pageSize, int pageNumber)
        {
          
            //int x = 0;
            //int y = pageSize;
            //PageMethod(out x, out y);
             pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            ViewBag.Groups = uow.QuestionGroupRepo.GetAll();
            Question question = uow.QuestionRepo.Find(int.Parse(questionId.ToString()));
            uow.QuestionRepo.Remove(question.ID);
            uow.SaveChanges();
            List<Question> questionList = uow.QuestionRepo.GetAll();
            foreach (var questionItem in questionList)
            {
                if (questionItem.QuestionGroupId != null)
                {
                    questionItem.QuestionGroup = uow.QuestionGroupRepo.Find(questionItem.QuestionGroupId).GroupName;
                }
            }
            if (questionList.ToPagedList(pageNumber, pageSize).Count == 0)
            {
                pageNumber = pageNumber - 1;
            }
            ViewBag.PageNumber = pageNumber;
            return PartialView("_List", questionList.ToPagedList(pageNumber , pageSize));

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
            if (pageSize == null || pageSize == ConfigurationManager.AppSettings["DefaultPageSize"].ToString() )
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

        public string GetBodyContent(string title, string content)
        {
            string html = System.IO.File.ReadAllText(Server.MapPath("~/MailTemplate/MailTemplate.html"));
            string htmlBody = html.Replace("#Titile#", title);
            string body = htmlBody.Replace("#TitleContent#", content);


            return body;
        }

	}
}