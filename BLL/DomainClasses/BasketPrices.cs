using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DomainClasses
{
    public class BasketPrices
    {
        public int ID { get; set; }
        public int BasketID { get; set; }
        public decimal? BuyPrice { get; set; }
        public decimal? SellPrice { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: hh:mm:ss tt}")]
        public DateTime PriceDate { get; set; }
        public bool IsCurrent { get; set; }
    }
}