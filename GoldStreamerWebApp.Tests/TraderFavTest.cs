using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.DomainClasses;
using DAL;
using DAL.UnitOfWork;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
    public class TraderFavTest
    {
        UnitOfWork _uow;
        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork(new DAL.GoldStreamerContext(), DropDB: false);
        }

        public TraderFavorites AddTraderFavorite(int owner)
        {
            return new TraderFavorites { FavOwnerId = owner };
        }
        public void DeleteData()
        {
            _uow.FavorateListRepo.DeleteAll();
            _uow.TraderFavRepo.DeleteAll();
        }

        [TestMethod]
        public void CanCheckTraderHasFavoriteList()
        {
            DeleteData();
            TraderFavorites traderFav1 = AddTraderFavorite(1);
            _uow.SaveChanges();
          
            bool res = _uow.TraderFavRepo.HasFavorite(1);
            Assert.AreEqual(false, res);
        }
        [TestMethod]
        public void CanGetFavorateIdByOwnerId()
        {
            DeleteData();
            TraderFavorites traderFav1 = AddTraderFavorite(1);
            _uow.TraderFavRepo.Add(traderFav1);
            _uow.SaveChanges();

            Assert.AreEqual(traderFav1.Id, _uow.TraderFavRepo.FindByOwner(1));

        }
    }
}
