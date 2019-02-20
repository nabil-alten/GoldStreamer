using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.DomainClasses;

namespace GoldStreamer.Models.ViewModels
{
    public class TradersComparisonViewModel
    {
        public int ID { get; set; }
        public List<Trader> TraderList { get; set; }
        public int Buy { get; set; }
        public int Sell { get; set; }



    } 
}