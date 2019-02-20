using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class TraderFavRepo<T> : BaseRepo<T> where T : class
    {
         private readonly GoldStreamerContext _context;
         public TraderFavRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }

         public bool HasFavorite(int favoriteOwner)
         {
             var fav=_context.Set<TraderFavorites>().FirstOrDefault(tf => tf.FavOwnerId == favoriteOwner);
             return fav != null;
         }

        public int FindByOwner(int favoriteOwner)
        {
            var fav = _context.Set<TraderFavorites>().FirstOrDefault(tf => tf.FavOwnerId == favoriteOwner);
            return fav==null?0:fav.Id;
        }
    }
}
