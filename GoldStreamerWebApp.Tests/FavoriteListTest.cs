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
    public class FavoriteListTest
    {
        UnitOfWork _uow;
        private FavListService favLstSrvc;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
            favLstSrvc = new FavListService();
        }

        public TraderFavorites AddTraderFavorite(int owner)
        {
            return new TraderFavorites { FavOwnerId = owner };
        }

        public FavoriteList AddFavoriteList(int TFav_Id, int ST_Id)
        {
            return new FavoriteList { SuperTraderId = ST_Id, TraderFavoriteId = TFav_Id };
        }
   
        public void DeleteData()
        {
            _uow.TradePricesRepo.DeleteAll();
            _uow.BasketPricesRepo.DeleteAll();
            _uow.BasketTradersRepo.DeleteAll();
            _uow.BasketRepo.DeleteAll();

            _uow.FavorateListRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();

            _uow.UsersRepo.DeleteAll();

            _uow.TraderRepo.DeleteList(26);
            _uow.SaveChanges();
        }
        private Trader AddTrader(string name, int type)
        {
            return new Trader
            {
                Name = name,
                Family = "",
                Governorate = 1,
                City = 1,
                District = 1,
                Gender =true,
                TypeFlag = type,
                SortOrder = 66
            };
        }

        private void DeleteAddedTraders()
        {
            _uow.TraderRepo.DeleteList(26);
        }

        [TestMethod]
        public void CanGetAssignedSuperTraders()
        {
            DeleteData();
            TraderFavorites traderFav1 = AddTraderFavorite(1);
            _uow.TraderFavRepo.Add(traderFav1);

            FavoriteList f1 = AddFavoriteList(traderFav1.Id, 2);
            FavoriteList f2 = AddFavoriteList(traderFav1.Id, 3);
            _uow.FavorateListRepo.Add(f1);
            _uow.FavorateListRepo.Add(f2);
            _uow.SaveChanges();

            List<Trader> t = _uow.FavorateListRepo.GetAssignedTraders(1);
            Assert.AreEqual(2, t.Count);
        }

        [TestMethod]
        public void CanGetAssignedUnassignedSTradersBigTraderEnter()
        {
            DeleteData();

            TraderFavorites traderFav1 = AddTraderFavorite(1);
            TraderFavorites traderFav2 = AddTraderFavorite(2);
            _uow.TraderFavRepo.Add(traderFav1);
            _uow.TraderFavRepo.Add(traderFav2);
            _uow.SaveChanges();
            FavoriteList f1 = AddFavoriteList(traderFav1.Id, 2);
            FavoriteList f2 = AddFavoriteList(traderFav1.Id, 3);
            _uow.FavorateListRepo.Add(f1);
            _uow.FavorateListRepo.Add(f2);

            FavoriteList f3 = AddFavoriteList(traderFav2.Id, 3);
            _uow.FavorateListRepo.Add(f3);


            _uow.SaveChanges();
            UnitOfWork uow = new UnitOfWork(new GoldStreamerContext());

            List<Trader> t = uow.FavorateListRepo.GetAssignedUnAssignedUsers(1);
            Assert.AreEqual(1, t[0].FavoriteList.Count);
            Assert.AreEqual(1, t[1].FavoriteList.Count);
        }

        [TestMethod]
        public void CanGetAssignedUnassignedSTradersSmallTraderEnter()
        {
            DeleteData();
            DeleteAddedTraders();
            Trader tdr= AddTrader("tst", 2);
            Trader tdr1 = AddTrader("tst1", 2);
            _uow.TraderRepo.Add(tdr);
            _uow.TraderRepo.Add(tdr1);
            _uow.SaveChanges();
            TraderFavorites traderFav1 = AddTraderFavorite(tdr.ID);
            TraderFavorites traderFav2 = AddTraderFavorite(tdr1.ID);
            _uow.TraderFavRepo.Add(traderFav1);
            _uow.TraderFavRepo.Add(traderFav2);
            _uow.SaveChanges();
            FavoriteList f1 = AddFavoriteList(traderFav1.Id, 2);
            FavoriteList f2 = AddFavoriteList(traderFav1.Id, 3);
            _uow.FavorateListRepo.Add(f1);
            _uow.FavorateListRepo.Add(f2);

            FavoriteList f3 = AddFavoriteList(traderFav2.Id, 3);
            _uow.FavorateListRepo.Add(f3);


            _uow.SaveChanges();
            UnitOfWork uow = new UnitOfWork(new GoldStreamerContext());

            List<Trader> t = uow.FavorateListRepo.GetAssignedUnAssignedUsers(tdr.ID);
            Assert.AreEqual(1, t[1].FavoriteList.Count);
            Assert.AreEqual(1, t[2].FavoriteList.Count);
        }

        [TestMethod]
        public void CanGetAssignedUnassignedSTradersFirstTimeBigTraderEnter()
        {
            DeleteData();
            DeleteAddedTraders();
            _uow.SaveChanges();

            UnitOfWork uow = new UnitOfWork(new GoldStreamerContext());

            List<Trader> t = uow.FavorateListRepo.GetAssignedUnAssignedUsers(1);
            Assert.AreEqual(9, t.Count);
        }

        [TestMethod]
        public void CanGetAssignedUnassignedSTradersFirstTimeSmallTraderEnter()
        {
            DeleteData();
            DeleteAddedTraders();
            Trader tdr = AddTrader("tst", 2);
            _uow.TraderRepo.Add(tdr);
            _uow.SaveChanges();

            UnitOfWork uow = new UnitOfWork(new GoldStreamerContext());

            List<Trader> t = uow.FavorateListRepo.GetAssignedUnAssignedUsers(tdr.ID);
            Assert.AreEqual(10, t.Count);
        }
        [TestMethod]
        public void CanDeleteSomeFavorateTraders()
        {
            DeleteData();

            TraderFavorites traderFav1 = AddTraderFavorite(1);
            TraderFavorites traderFav2 = AddTraderFavorite(2);
            _uow.TraderFavRepo.Add(traderFav1);
            _uow.TraderFavRepo.Add(traderFav2);
            _uow.SaveChanges();
            FavoriteList f1 = AddFavoriteList(traderFav1.Id, 2);
            FavoriteList f2 = AddFavoriteList(traderFav1.Id, 3);
            _uow.FavorateListRepo.Add(f1);
            _uow.FavorateListRepo.Add(f2);

            FavoriteList f3 = AddFavoriteList(traderFav2.Id, 3);
            _uow.FavorateListRepo.Add(f3);
            _uow.SaveChanges();

            _uow.FavorateListRepo.DeleteFavorateTraders(traderFav1.Id);
            _uow.SaveChanges();
            Assert.AreEqual(1, _uow.FavorateListRepo.GetAll().Count);
        }

        [TestMethod]
        public void CanDeleteAllFavorateTraders()
        {
            DeleteData();

            TraderFavorites traderFav1 = AddTraderFavorite(1);
            _uow.TraderFavRepo.Add(traderFav1);
            _uow.SaveChanges();

            FavoriteList f1 = AddFavoriteList(traderFav1.Id, 2);
            FavoriteList f2 = AddFavoriteList(traderFav1.Id, 3);
            _uow.FavorateListRepo.Add(f1);
            _uow.FavorateListRepo.Add(f2);
            _uow.SaveChanges();

            favLstSrvc.SaveAssignedUsers(1, null);

            Assert.AreEqual(0, _uow.FavorateListRepo.GetAll().Count);
            Assert.AreEqual(0,_uow.TraderFavRepo.GetAll().Count);
        }



    }
}
