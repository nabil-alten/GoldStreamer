using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.UnitOfWork;
using BLL.DomainClasses;
using DAL;
using BusinessServices;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class QuestionGroupTest
    {
        UnitOfWork _uow;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
        }
        public int AddNewQuestionGroup()
        {
            QuestionGroup newQuestionGroup = new QuestionGroup()
            {
                GroupName = "NewGroup",
                GroupOrder = 1,
            };
            _uow.QuestionGroupRepo.AddNewQuestionGroup(newQuestionGroup);
            _uow.SaveChanges();
            return newQuestionGroup.ID;
        }
        public Question AddQuestion(string question, string answer, int groupOrder, string groupName, int GroupID)
        {

            Question questionObj = new Question();
            questionObj.QuestionText = question;
            questionObj.Answer = answer;
            questionObj.ID = new int();
            questionObj.QuestionGroup = groupName;
            questionObj.QuestionGroupId = GroupID;

            return questionObj;
        }


        [TestMethod]
        public void CanGetListQuestionGroups()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();
            int ID = AddNewQuestionGroup();

            Assert.AreEqual(1, _uow.QuestionGroupRepo.GetQuestionGroupList("").Count);
        }

        [TestMethod]
        public void CanAddNewQuestionGroup()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();
            AddNewQuestionGroup();
            Assert.AreEqual(1, _uow.QuestionGroupRepo.GetQuestionGroupList("").Count);
        }
        
        [TestMethod]
        public void CanUpdateQuestionGroup()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();
            int ID = AddNewQuestionGroup();
            
            QuestionGroup questionGroup = _uow.QuestionGroupRepo.Find(ID);
            questionGroup.GroupName = "UpdatedGroup";
            _uow.QuestionGroupRepo.Update(questionGroup);
            _uow.SaveChanges();
            Assert.AreEqual("UpdatedGroup",_uow.QuestionGroupRepo.Find(ID).GroupName);
        }

        [TestMethod]
        public void CanDeleteQuestionGroup()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();
            int ID = AddNewQuestionGroup();

            _uow.QuestionGroupRepo.DeleteByID(ID);
            _uow.SaveChanges();

            Assert.AreEqual(null, _uow.QuestionGroupRepo.Find(ID));
        }

         [TestMethod]
        public void CanCheckNameExsists ()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();
            int ID = AddNewQuestionGroup();
            int isExsists = _uow.QuestionGroupRepo.CheckNameExists("NewGroup", ID);
            Assert.AreEqual(0, isExsists);
        }

        [TestMethod]
        public void CanCheckOrderExsists()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();
            int ID = AddNewQuestionGroup();
            int isExsists = _uow.QuestionGroupRepo.CheckOrderExists (1, ID);
            Assert.AreEqual(0, isExsists);
        }

        [TestMethod]
        public void HasAssignedQuestionsTest()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();
            int ID = AddNewQuestionGroup();
            _uow.QuestionRepo.Add(AddQuestion("كيفية التسجيل ؟", "إضغط على زر التسجيل",1, "التسجيل", ID));
            _uow.SaveChanges();

            bool result = _uow.QuestionGroupRepo.hasAssignedQuestions(ID);
            Assert.AreEqual(true, result);
        }
    }
}
