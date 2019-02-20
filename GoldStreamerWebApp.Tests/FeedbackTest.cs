using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;
using BusinessServices;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class FeedbackTest
    {
        UnitOfWork _uow;



        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
        }


        [TestCleanup]
        public void CleanData()
        {
            _uow.FeedbackRepo.DeleteAll();
            _uow.SaveChanges();
        }

        public Feedback SetFeedback(string name, string email,string subject, string comment)
        {
            Feedback fb = new Feedback();
            fb.ID = new int();
            fb.Name = name;
            fb.Email = email;
            fb.Comment = comment;
            fb.Subject = subject;

            return fb;
        }


        [TestMethod]
        public void CanAddFeedback()
        {
           _uow.FeedbackRepo.Add( SetFeedback("Radwa", "eng_radwa_alaa@yahoo.com","my feedback", "this is awesome  website "));
           _uow.SaveChanges();
           Assert.AreEqual(1 , _uow.FeedbackRepo.GetAll().Count);

        }
    }
}
