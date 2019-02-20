using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DomainClasses
{
    [NotMapped]
    public class Prices
    {
        public int ID { get; set; }
        public int TraderID { get; set; }
        public string Name { get; set; }
        public decimal CurrentBuy { get; set; }
        public decimal CurrentSell { get; set; }
        public decimal OpenBuy { get; set; }
        public decimal OpenSell { get; set; }
        public decimal MinSell { get; set; }
        public decimal MaxBuy { get; set; }
        //from normal traders point of viewe, best sell is the min value.
        public bool TradersMinSell { get; set; }
        //from normal traders point of viewe, best buy is the max value.
        public bool TradersMaxBuy { get; set; }
        public DateTime date { get; set; }
        public bool IsDefault { get; set; }
        public decimal BasketBuy { get; set; }
        public decimal BasketSell { get; set; }
        public int InBasket { get; set; }
    }
}