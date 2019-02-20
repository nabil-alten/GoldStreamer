using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DomainClasses
{
    public class TraderPrices
    {
        public int ID { get; set; }
        [Required]
        public int TraderID { get; set; }
        //[RegularExpression(@"^\s*(?:\d+|\d*\.\d+)?\s*$")]
        //[Range(0, 999999.99,ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PriceRangeVal")]
        //[Remote("IsUserNameExist", "User", HttpMethod = "POST", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "DataExistBefore")]
        public decimal? BuyPrice { get; set; }
        ////[RegularExpression(@"^\d+.\d{0,2}$")]
        //[Range(0, 999999999.99, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PriceRangeVal")]
        public decimal? SellPrice { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: hh:mm:ss tt}")]
        public DateTime priceDate { get; set; }
    }
}