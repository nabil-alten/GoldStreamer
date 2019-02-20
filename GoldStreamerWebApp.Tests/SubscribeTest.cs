using BLL.DomainClasses;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class SubscribeTest
    {
        UnitOfWork _uow;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
        }
        public void CleanUp()
        {
            _uow.SubscribeRepo.DeleteAll();
        }
        public Subscribe SetSubscribe(string Name, string Email, bool sub) 
        {

            Subscribe SubscribeObj = new Subscribe();
            SubscribeObj.ID = Guid.NewGuid();
            SubscribeObj.Name = Name;
            SubscribeObj.Email = Email;
            SubscribeObj.IsSubscribe = sub;
            return SubscribeObj;
        }
        [TestMethod]
        public void CanAddSubscribe()
        {
            _uow.SubscribeRepo.DeleteAll();
            _uow.SaveChanges();

            _uow.SubscribeRepo.Add(SetSubscribe("Test1", "Dina@yahoo.com", true));
            _uow.SubscribeRepo.Add(SetSubscribe("Test2", "Dina@gmail.com", true));
            _uow.SaveChanges();
            Assert.AreEqual(2, _uow.SubscribeRepo.GetAll().Count);
        }
        [TestMethod]
        public void CanUpdateSubscribe()
        {
            _uow.SubscribeRepo.DeleteAll();
            _uow.SaveChanges();

            Subscribe obj1 = SetSubscribe("Test1", "Dina@yahoo.com", true);

            _uow.SubscribeRepo.Add(obj1);
            _uow.SubscribeRepo.Add(SetSubscribe("Test2", "Dina@gmail.com", true));
            _uow.SaveChanges();
            obj1.IsSubscribe = false;
            _uow.SubscribeRepo.Update(obj1);
            _uow.SaveChanges();
            obj1 = _uow.SubscribeRepo.Find(obj1.ID);
            Assert.AreEqual(false, obj1.IsSubscribe);
        }
        [TestMethod]
        public void CanGetSubscribed()
        {
            _uow.SubscribeRepo.DeleteAll();
            _uow.SaveChanges();

            _uow.SubscribeRepo.Add(SetSubscribe("Test1", "Dina@yahoo.com", true));
            _uow.SubscribeRepo.Add(SetSubscribe("Test2", "Dina@gmail.com", true));
            _uow.SubscribeRepo.Add(SetSubscribe("Test3", "Dina@hotmail.com", false));
            _uow.SubscribeRepo.Add(SetSubscribe("Test4", "Dina@test.com", false));
            _uow.SaveChanges();
            Assert.AreEqual(2, _uow.SubscribeRepo.GetAllSubscribes().Count);
        }
        [TestMethod]
        public void checkEmailExist()
        {
            _uow.SubscribeRepo.DeleteAll();
            _uow.SaveChanges();

            _uow.SubscribeRepo.Add(SetSubscribe("Test1", "Dina@yahoo.com", true));
            _uow.SubscribeRepo.Add(SetSubscribe("Test2", "Dina@gmail.com", true));
            _uow.SubscribeRepo.Add(SetSubscribe("Test3", "Dina@hotmail.com", false));
            _uow.SubscribeRepo.Add(SetSubscribe("Test4", "Dina@test.com", false));
            _uow.SaveChanges();
            int check = _uow.SubscribeRepo.CheckEmailExists("Dina@yahoo.com");
            Assert.AreEqual(1, check);
        }
        [TestMethod]
        public void checkEmailNotExist()
        {
            _uow.SubscribeRepo.DeleteAll();
            _uow.SaveChanges();

            _uow.SubscribeRepo.Add(SetSubscribe("Test1", "Dina@yahoo.com", true));
            _uow.SubscribeRepo.Add(SetSubscribe("Test2", "Dina@gmail.com", true));
            _uow.SubscribeRepo.Add(SetSubscribe("Test3", "Dina@hotmail.com", false));
            _uow.SubscribeRepo.Add(SetSubscribe("Test4", "Dina@test.com", false));
            _uow.SaveChanges();
            int check = _uow.SubscribeRepo.CheckEmailExists("Dina@techvision-eg.com");
            Assert.AreEqual(0, check);
        }
    }
}
