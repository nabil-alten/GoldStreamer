using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.UnitOfWork;
using BLL.DomainClasses;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class GlobalPriceTest
    {
        private UnitOfWork _uow;
        
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
        }
        [TestCleanup]
        public void CleanUp()
        {
            _uow.GlobalPriceRepo.DeleteAll();
            _uow.SaveChanges();
        }

        [TestMethod]
        public void CanGetAll()
        {
            CleanUp();
            GlobalPrice newGlobalPrice = new GlobalPrice() 
            {
                Ask = 100,
                Bid = 100,
                CaptureTime= DateTime.Now,
                Close= 100,
                High = 100,
                Low= 50,
                Open= 50,
            };
            _uow.GlobalPriceRepo.Add(newGlobalPrice);
            _uow.SaveChanges();

            List<GlobalPrice> globalPrices= _uow.GlobalPriceRepo.GetAll();
            Assert.AreNotEqual(globalPrices.Count, 0); 
        }
    }
}
