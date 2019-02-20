using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class FavoriteListRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public FavoriteListRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }

        public List<Trader> GetAssignedUnAssignedUsers(int favOwner)
        {
            var traderFav = _context.Set<TraderFavorites>().FirstOrDefault(tf => tf.FavOwnerId == favOwner);

            var res = (from t in _context.Traders
                       where t.TypeFlag == 1 && t.IsActive && t.ID!=favOwner
                       select t).ToList();
            if (traderFav != null)
            {
                foreach (var v in res)
                {
                    _context.Entry(v).Collection(fl => fl.FavoriteList).Query()
                        .Where(a => a.TraderFavoriteId == traderFav.Id)
                        .Load();
                }
            }

            return res;
        }

        public List<Trader> GetAssignedTraders(int favOwner)
        {
            var traderFav = _context.Set<TraderFavorites>().FirstOrDefault(tf => tf.FavOwnerId == favOwner);

            var favorates = from t in _context.Traders
                     join fl in _context.FavorateList on t.ID equals fl.SuperTraderId
                     where fl.TraderFavoriteId == traderFav.Id
                     select t;

            return favorates.ToList();
        }


        public void DeleteFavorateTraders(int favId)
        {
            List<FavoriteList> fl = _context.Set<FavoriteList>().Where(d => d.TraderFavoriteId == favId).ToList();
            _context.Set<FavoriteList>().RemoveRange(fl);

        }
    }
}
