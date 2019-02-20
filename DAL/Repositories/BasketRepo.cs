using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class BasketRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public BasketRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }
        public List<Basket> GetAllBaskerts()
        {
            return _context.Set<Basket>().Where(b => b.IsDeleted == false).ToList();
        }
        public int CheckNameExists(string basketName, int bigTrader)
        {
            Basket name =
                _context.Set<Basket>()
                        .FirstOrDefault(
                            b => b.Name == basketName.Trim() && b.IsDeleted == false && b.BasketOwner == bigTrader);
            return name == null ? 0 : 1;
        }
        public int CheckNameExists(string basketName, int bigTrader, int basketId)
        {
            Basket name =
                _context.Set<Basket>()
                        .FirstOrDefault(
                            b => b.Name == basketName.Trim() && b.IsDeleted == false && b.BasketOwner == bigTrader);
            if(name == null)
                return 0;
            if(name.ID == basketId)
                return 0;

            return 1;
        }
        public List<Basket> GetByBigTraderId(int bigTraderId)
        {
            List<Basket> bigTraderBasket =
                _context.Set<Basket>().Where(b => b.BasketOwner == bigTraderId && b.IsDeleted == false).ToList();
            return bigTraderBasket;
        }
        public List<Basket> GetTraderBasketsPrice(int bigTraderId)
        {
            DateTime today = DateTime.Now.Date;

            var data = (from q in _context.Basket
                        where q.BasketOwner == bigTraderId && q.IsDeleted == false
                        select q).ToList();

            foreach(var v in data)
            {
                _context.Entry(v).Collection(b => b.BasketPrices).Query()
                        .Where(a => a.IsCurrent && a.PriceDate >= today)
                        .Load();
            }

            return data;
        }
        public void DeleteBasket(int basketId)
        {
            Basket bskt = _context.Set<Basket>().FirstOrDefault(b => b.ID == basketId);
            if(bskt != null)
            {
                bskt.IsDeleted = true;
                _context.Entry(bskt).State = EntityState.Modified;
            }
        }
    }
}