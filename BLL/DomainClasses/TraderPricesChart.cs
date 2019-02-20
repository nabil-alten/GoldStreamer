using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DomainClasses
{
    public class TraderPricesChart
    {
        public int ID { get; set; }
     
        public int? TraderID { get; set; }
        public Trader Trader { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: hh:mm:ss tt}")]
        public DateTime Date { get; set; }
        public decimal? BuyAverage { get; set; }
        public decimal? SellAverage { get; set; }
        public decimal? BuyClose { get; set; }
        public decimal? SellClose { get; set; }
      
    }
}