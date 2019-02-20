using BLL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DollarRepo<T> : BaseRepo<T> where T : class
    {
        GoldStreamerContext _context = null;
        public DollarRepo(GoldStreamerContext dbContext)
        {
            _context = dbContext;
        }

        public decimal GetDollarSellPrice()
        {
            Dollar dollarPrice = _context.Dollar.OrderByDescending(x => x.ID).FirstOrDefault();
            if (dollarPrice != null)
                return dollarPrice.SellPrice;
            return 0.0m;
        }

        public decimal GetDollarBuyPrice()
        {
            Dollar dollarPrice = _context.Dollar.OrderByDescending(x => x.ID).FirstOrDefault();
            if (dollarPrice != null)
                return dollarPrice.BuyPrice;
            return 0.0m;
        }

        public Dollar GetCurrentDollarPrice()
        {
            return _context.Dollar.OrderByDescending(x => x.ID).FirstOrDefault();
        }
    }
}
