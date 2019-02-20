using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BLL.DomainClasses;
using Resources;

namespace GoldStreamer.Models.ViewModels
{
    public class TraderViewModel
    {

        [Required]
        public int ID { get; set; } // Primary Key 

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"TraderNameVal")]
        [RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "BasketNameLengthVal")]
        public string Name { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"TraderNameVal")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "BasketNameLengthVal")]
        [RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]
        public string Family { get; set; }

        public bool Gender { get; set; }

        [RegularExpression(@"^([1-9]{0,1}[0-9]{0,100})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "phoneVal")]
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "TraderNameVal")]
        [StringLength(11, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PhoneLengthVal")]
        public string Phone { get; set; }

        [RegularExpression(@"^([1-9]{0,1}[0-9]{0,100})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "phoneVal")]
        [StringLength(15, MinimumLength = 11, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MobileLengthVal")]
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"mobileReq")]
        public string Mobile { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"EmailRequired")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MailExpErr")]
        public string Email { get; set; }

        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MailExpErr")]
        [System.Web.Mvc.Compare("Email", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ConfirmMailVal")]
        public string ReEmail { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ShopNameReq")]
        //[RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ShopNameRegEx")]
        public string ShopName { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"GovVal")]
        public int? Governorate { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"CityVal")]
        public int? City { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"RegionVal")]
        public int? District { get; set; }
        public int TypeFlag { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] // "orderReq")]
        [RegularExpression(@"^(0|[1-9][0-9]*)$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "NumbersOnly")]
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
    }

    public class TraderPricesPrintVM:TraderPrices
    {
        public string TraderName { get; set; }
        public DateTime Date { get; set; }
        public bool print { get; set; }
    }

    public class BasketPricesPrintVM : BasketPrices
    {
        public string BasketName { get; set; }
        public DateTime Date { get; set; }
        public bool print { get; set; }
    }
}