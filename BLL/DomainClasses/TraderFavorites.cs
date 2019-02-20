using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainClasses
{
     public class TraderFavorites
    {
        public int Id { get; set; }
        public int FavOwnerId { get; set; }
        //public int FavListId { get; set; }

         [ForeignKey("TraderFavoriteId")]
        public List<FavoriteList> FavoriteList { get; set; }
    }
}
