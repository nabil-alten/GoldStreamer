using BLL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Repositories
{
    public class GlobalPriceRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public GlobalPriceRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }
      


        public decimal GetGoldSellPrice()
        {
            GlobalPrice globalPrice = _context.GlobalPrices.OrderByDescending(gp => gp.Id).FirstOrDefault();
            if (globalPrice != null)
                return Math.Round(globalPrice.Ask / 31.1035m, 2);
            return 0.0m;
        }

        public decimal GetGoldBuyPrice()
        {
            GlobalPrice globalPrice = _context.GlobalPrices.OrderByDescending(gp => gp.Id).FirstOrDefault();
            if (globalPrice != null)
                return Math.Round(globalPrice.Bid / 31.1035m, 2);
            return 0.0m;
        }
    }
}
