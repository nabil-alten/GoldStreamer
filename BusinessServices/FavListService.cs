using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;
using DAL.UnitOfWork;

namespace BusinessServices
{
    public class FavListService
    {
        private readonly UnitOfWork _uow;

        public FavListService()
        {
            _uow = new UnitOfWork();
        }
        public void SaveAssignedUsers(int ownerId, int[] userIds)
        {
            int favorateId = _uow.TraderFavRepo.FindByOwner(ownerId);
            if(favorateId>0)
            _uow.FavorateListRepo.DeleteFavorateTraders(favorateId);
            else if(favorateId==0)
            {
                TraderFavorites tf = new TraderFavorites {FavOwnerId = ownerId};
                _uow.TraderFavRepo.Add(tf);
                _uow.SaveChanges();
                favorateId = tf.Id;
            }

            if (userIds != null)
            {
                foreach (var usr in userIds)
                {
                    _uow.FavorateListRepo.Add(new FavoriteList { TraderFavoriteId = favorateId, SuperTraderId = usr });
                }
            }
            else
            {
                _uow.TraderFavRepo.Remove(favorateId);
            }
            _uow.SaveChanges();
        }
    }
}
