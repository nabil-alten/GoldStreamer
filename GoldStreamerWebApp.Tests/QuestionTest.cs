using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using BusinessServices;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class QuestionTest
    {
        UnitOfWork _uow;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
        }


        public Question SetQuestion(string question , string answer , string groupName , int groupOrder )
        {
            QuestionGroup questionGroup =  new QuestionGroup();
            questionGroup.GroupName = groupName;
            questionGroup.GroupOrder = groupOrder;
            questionGroup.ID = 1;
            _uow.QuestionGroupRepo.AddNewQuestionGroup(questionGroup);

            _uow.SaveChanges();
            Question questionObj =  new Question();
            questionObj.QuestionText = question;
            questionObj.Answer = answer;
            questionObj.ID = 1;
            questionObj.QuestionGroupId = questionGroup.ID;

            questionObj.QuestionGroup = _uow.QuestionGroupRepo.Find(questionObj.QuestionGroupId).GroupName;
            questionObj.ReadingCounter = 0;
            return questionObj;
        }


        [TestMethod]
        public void CanAddQuestion()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();

            _uow.QuestionRepo.Add(SetQuestion("كيفية التسجيل ؟", "إضغط على زر التسجيل", "Registeration", 1));
            _uow.SaveChanges();
            Assert.AreEqual(1, _uow.QuestionRepo.GetAll().Count);
        }

        [TestMethod]
        public void CanFindByGroupID()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();

            _uow.QuestionRepo.Add(SetQuestion("كيفية التسجيل ؟", "إضغط على زر التسجيل", "التسجيل", 1));
            _uow.SaveChanges();
            Assert.AreEqual(1, _uow.QuestionRepo.FindByGroupId(_uow.QuestionGroupRepo.GetAll().First().ID).Count());
        }

        [TestMethod]
        public void CanUpdateQuestion()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();

            _uow.QuestionRepo.Add(SetQuestion("كيفية التسجيل ؟", "إضغط على زر التسجيل", "التسجيل", 1));
            _uow.SaveChanges();
            Question question = _uow.QuestionRepo.GetAll().First();
            question.QuestionText = "registeration";
            _uow.QuestionRepo.Update(question);
            Assert.AreEqual("registeration", _uow.QuestionRepo.GetAll().First().QuestionText);
        }

        [TestMethod]
        public void CanDeleteQuestion()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();

            CanAddQuestion();
            _uow.QuestionRepo.Remove(_uow.QuestionRepo.GetAll().First().ID);
            _uow.SaveChanges();

            Assert.AreEqual(0, _uow.QuestionRepo.GetAll().Count());
        }


        [TestMethod]
        public void CanSearch()
        {
            _uow.QuestionGroupRepo.DeleteAllQuestionGroups();
            _uow.SaveChanges();

            CanAddQuestion();
            _uow.SaveChanges();

            Assert.AreEqual(1, _uow.QuestionRepo.Search("تسجيل", 2, false, false, false, false).Count());
        }
    }
}
