using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;


namespace BLL.DomainClasses
{
    public class Trader
    {
        [Required]
        public int ID { get; set; } // Primary Key 

        public string Name { get; set; }

        public string Family { get; set; }

        public bool Gender { get; set; }


        public string Phone { get; set; }

        public string Mobile { get; set; }
 
        public string Email { get; set; }

       
        public string ShopName { get; set; }

       
        public int? Governorate { get; set; }

       
        public int? City { get; set; }

       
        public int? District { get; set; }
        public int TypeFlag { get; set; }

        
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public virtual List<TraderPrices> TraderPrices { get; set; }
        public virtual List<Prices> Prices { get; set; }
        [ForeignKey("BasketOwner")]
        public List<Basket> Basket { get; set; }
        public List<BasketTraders> BasketTraders { get; set; }

        [ForeignKey("FavOwnerId")]
        public List<TraderFavorites> TraderFavorites { get; set; }
        [ForeignKey("SuperTraderId")]
        public List<FavoriteList> FavoriteList { get; set; }
        public List<Users> Users { get; set; }

        public bool IsPublicProfile { get; set; }
        public bool IsRegistered { get; set; }

    }
}