using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace GoldStreamerWebApp.Tests
{
    [TestClass]
    public class TraderRepoTest : IDisposable
    {
        UnitOfWork uow = null;

        [TestInitialize]
        public void Setup()
        {
            uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
        }

        private Trader AddTrader(String name)
        {
            Trader traderObj = new Trader
            {
                Name = name,
                Family = name,
                Gender = true,
                Governorate = 1,
                City = 1,
                District = 1,
                TypeFlag = 1,
                SortOrder = 99,
                Email = name + "@techvision.com",
                //ReEmail = name + "@techvision.com",
                Mobile = "903572656856"
            };
            return traderObj;
        }

        private void DeleteTraders()
        {
            uow.TradePricesRepo.DeleteAll();
            uow.BasketPricesRepo.DeleteAll();
            uow.BasketTradersRepo.DeleteAll();
            uow.BasketRepo.DeleteAll();

            uow.FavorateListRepo.DeleteAll();
            uow.TraderFavRepo.DeleteAll();

            uow.UsersRepo.DeleteAll();

            uow.TraderRepo.DeleteList(26);
            uow.SaveChanges();
        }

        [TestMethod]
        public void CanAddTrader()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");

            uow.TraderRepo.Add(traderObj);
            //uow.TraderRepo.AddAgain(TraderObj);
            uow.SaveChanges();

            List<Trader> traders = uow.TraderRepo.GetAll();
            Assert.AreEqual(26, traders.Count);
        }

        
        [Ignore]
        [TestMethod]
        public void CanDeleteAll()
        {
            uow.TraderRepo.DeleteAll();
            uow.TradePricesRepo.DeleteAll();
            uow.SaveChanges();
            Assert.AreEqual(uow.TraderRepo.GetAll().Count, 0);
        }
        [TestMethod]
        public void CanFindSuperTraderByName()
        {
            Trader superTrader = uow.TraderRepo.FindTraderById(1);
            int hisID = superTrader.ID;

            Assert.AreEqual(uow.TraderRepo.SearchSpecificType(superTrader.Name, 1)[0].ID, hisID);
        }
        [TestMethod]
        public void CanGetAllTradersByType()
        {
            List<Trader> lst = uow.TraderRepo.GetAll();
            int superTraderCnt = lst.Where(x => x.TypeFlag == 1).ToList().Count;
            Assert.AreEqual(superTraderCnt, uow.TraderRepo.GetAllByType(1).Count);
        }

        [TestMethod]
        public void CanCheckEmailExistsBefore1()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();

            Trader trader = AddTrader("testTrader");
            int Res=uow.TraderRepo.CheckEmailExists(trader.Email, trader.ID);
            Assert.AreEqual(1,Res);
        }

        [TestMethod]
        public void CanCheckEmailExistsBefore0()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            traderObj.Phone = "53426235";

            int Res = uow.TraderRepo.CheckEmailExists(traderObj.Email, traderObj.ID);
            Assert.AreEqual(0, Res);
        }

        [TestMethod]
        public void CanCheckOrderExistsBefore1()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();

            Trader trader = AddTrader("testTrader");
            int Res = uow.TraderRepo.CheckOrderExists(traderObj.SortOrder, trader.ID);
            Assert.AreEqual(1, Res);
        }

        [TestMethod]
        public void CanCheckOrderExistsBefore0()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            traderObj.Phone = "53426235";

            int Res = uow.TraderRepo.CheckOrderExists(traderObj.SortOrder, traderObj.ID);
            Assert.AreEqual(0, Res);
        }

        [TestMethod]
        public void CanUpdateTrader()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            traderObj.Phone = "53426235";
            uow.SaveChanges();

            Trader t = uow.TraderRepo.Find(traderObj.ID);
            Assert.AreEqual(t.Phone, "53426235");

        }

        [TestMethod]
        public void CanCheckDuplicateTraderNameNoDuplicate()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            Trader trader1= AddTrader("testTrader1");
            uow.TraderRepo.Add(trader1);

            int res = uow.TraderRepo.CheckNameExists(trader1);
  
            Assert.AreEqual(0, res);

        }

        [TestMethod]
        public void CanCheckDuplicateTraderNameHasDuplicate()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            Trader trader1 = AddTrader("testTrader");
            uow.TraderRepo.Add(trader1);

            int res = uow.TraderRepo.CheckNameExists(trader1);

            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void CanCheckDuplicateTraderNameEditSameObject()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();

            int res = uow.TraderRepo.CheckNameExists(traderObj);

            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void CanCheckDuplicateTraderMobileHasDuplicate()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            Trader trader1 = AddTrader("testTrader");
            uow.TraderRepo.Add(trader1);

            int res = uow.TraderRepo.CheckMobileExists(trader1);

            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void CanCheckDuplicateTraderMobileEditSameObject()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            traderObj.Mobile = "24565478956342";
            int res = uow.TraderRepo.CheckMobileExists(traderObj);

            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void CanCheckDuplicateTraderShopNameHasDuplicate()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            traderObj.ShopName = "www";
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            Trader trader1 = AddTrader("testTrader");
            trader1.ShopName = "www";
            uow.TraderRepo.Add(trader1);

            int res = uow.TraderRepo.CheckShopNameExists(trader1);

            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void CanCheckDuplicateTraderShopNameEditSameObject()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            //traderObj.ShopName = "weew";
            int res = uow.TraderRepo.CheckShopNameExists(traderObj);

            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void CanCheckDuplicateTraderPhoneEditSameObject()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            traderObj.Phone = "24565478956342";
            int res = uow.TraderRepo.CheckPhoneExists(traderObj);

            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void CanCheckDuplicateTraderPhoneHasDuplicate()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            traderObj.Phone = "222333444";
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            Trader trader1 = AddTrader("testTrader");
            trader1.Phone = "222333444";
            uow.TraderRepo.Add(trader1);

            int res = uow.TraderRepo.CheckPhoneExists(trader1);

            Assert.AreEqual(1, res);
        }
        
        [TestMethod]
        public void CanGetLastTraderOrder()
        {
            //uow.TradePricesRepo.DeleteAll();
            //uow.TraderRepo.DeleteList(26);
            //uow.SaveChanges();
            DeleteTraders();
            Trader traderObj = AddTrader("testTrader");
            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();

            int order = uow.TraderRepo.GetNextTraderOrder();
            Assert.AreEqual(100, order);

        }

        #region IDisposable Implementation

        protected bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release disposable objects used by this instance here.

                    if (uow != null)
                        uow.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
