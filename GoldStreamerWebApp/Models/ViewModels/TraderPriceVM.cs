using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldStreamer.Models.ViewModels
{
    public class TraderPriceVM
    {
        public decimal MinBuy { get; set; }
        public decimal MaxSell { get; set; }
    }
}