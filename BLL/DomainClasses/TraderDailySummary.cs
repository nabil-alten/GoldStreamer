using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainClasses
{
    public class TraderDailySummary
    {
        public int ID { get; set; }
        public int TraderID { get; set; }
        public decimal OpenBuy { get; set; }
        public decimal OpenSell { get; set; }
        public decimal MaxSell { get; set; }
        public decimal MinBuy { get; set; }
        public bool TradersMaxSell { get; set; }
        public bool TradersMinBuy { get; set; }
        public DateTime date { get; set; }
        public bool IsDefault { get; set; }

    }
}
