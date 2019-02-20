using System.Collections.Generic;
using System.Linq;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class BasketTradersRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public BasketTradersRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }
        public void DeleteBasketUsers(int basketId)
        {
            _context.Set<BasketTraders>().RemoveRange(GetBasketUsers(basketId));
        }
        public List<BasketTraders> GetBasketUsers(int basketId)
        {
            return _context.Set<BasketTraders>().Where(bt => bt.BasketId == basketId).Select(bt => bt).ToList();
        }
        public List<Trader> GetAssignedUnAssignedUsers(int basketId)
        {
            int basketOwner = _context.Set<Basket>().FirstOrDefault(b => b.ID == basketId).BasketOwner;


            var res = (from t in _context.Traders.Include("BasketTraders")
                       where t.TypeFlag == 2 && t.IsActive
                       select t).ToList();

            for(int i = 0; i < res.Count; i++)
            {
                for(int j = 0; j < res[i].BasketTraders.Count; j++)
                {
                    var b = res[i].BasketTraders[j];
                    int x = _context.Set<Basket>().FirstOrDefault(bs => bs.ID == b.BasketId).BasketOwner;
                    if(x == basketOwner && b.BasketId != basketId)
                    {
                        res.Remove(res[i]);
                        i--;
                        break;
                    }
                    if(b.BasketId != basketId)
                    {
                        res[i].BasketTraders.Remove(b);
                        j--;
                    }
                }
            }
            return res.ToList();
        }
    }
}