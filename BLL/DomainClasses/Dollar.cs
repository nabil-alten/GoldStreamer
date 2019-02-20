using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainClasses
{
    public class Dollar
    {
        [Required]
        public int ID { get; set; }
        public decimal SellPrice { get; set; }

        public decimal BuyPrice { get; set; }

        public DateTime CaptureDate { get; set; }

    }
}
