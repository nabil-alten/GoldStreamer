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
    public class PricesListVM
    {
        IEnumerable<Prices> priceList
        {
            get;
            set;
        }

        bool isFavourited { get; set; }
    }
}