using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class BasketPricesRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public BasketPricesRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }
        public BasketPrices FindByBasketId(int basketId)
        {
            var basketPrices = _context.Set<BasketPrices>()
                                       .SingleOrDefault(x => x.BasketID == basketId
                                                             && x.IsCurrent
                                                             && x.PriceDate.Year == DateTime.Now.Year
                                                             && x.PriceDate.Month == DateTime.Now.Month
                                                             && x.PriceDate.Day == DateTime.Now.Day);

            return basketPrices;
        }

        public List<BasketPrices> FindAllTodayBasketPrices(int basketId)
        {
            var basketPrices = _context.Set<BasketPrices>()
                                       .Where(x => x.BasketID == basketId
                                                   && x.PriceDate.Year == DateTime.Now.Year
                                                   && x.PriceDate.Month == DateTime.Now.Month
                                                   && x.PriceDate.Day == DateTime.Now.Day).ToList();

            return basketPrices;
        }
        public List<BasketPrices> FindAllBasketPricesByDate(int basketId, DateTime? date)
        {
            var basketPrices = _context.Set<BasketPrices>()
                                       .Where(x => x.BasketID == basketId
                                                   && x.PriceDate.Year == date.Value.Year
                                                   && x.PriceDate.Month == date.Value.Month
                                                   && x.PriceDate.Day == date.Value.Day).ToList();

            return basketPrices;
        }
        public List<BasketPrices> SearchByDate(int basketId, DateTime date)
        {
            var basketPrices = _context.Set<BasketPrices>()
                                       .Where(x => x.BasketID == basketId
                                                   && x.PriceDate.Year == date.Year
                                                   && x.PriceDate.Month == date.Month
                                                   && x.PriceDate.Day == date.Day).ToList();

            return basketPrices;
        }
        public decimal GetBasketMinBuyPrice(int basketId, DateTime date, int TraderType)
        {
            decimal res = decimal.Parse("0.0");
            try
            {
                if(TraderType == 1)
                {
                    var min = _context.Set<BasketPrices>()
                                      .Where(x => x.BasketID == basketId
                                                  && x.PriceDate.Year == date.Year
                                                  && x.PriceDate.Month == date.Month
                                                  && x.PriceDate.Day == date.Day).Min(x => x.BuyPrice);
                    if(min != null)
                        res = min.Value;
                }
                else if(TraderType == 2 || TraderType == 3)
                {
                    var min = _context.Set<BasketPrices>()
                                      .Where(x => x.BasketID == basketId
                                                  && x.PriceDate.Year == date.Year
                                                  && x.PriceDate.Month == date.Month
                                                  && x.PriceDate.Day == date.Day).Max(x => x.BuyPrice);
                    if(min != null)
                        res = min.Value;
                }
            }
            catch(Exception)
            {
                res = 0;
            }
            return res;
        }
        public decimal GetBasketMaxSellPrice(int basketId, DateTime date, int TraderType)
        {
            var res = decimal.Parse("0.0");
            try
            {
                if(TraderType == 1)
                {
                    var max = _context.Set<BasketPrices>()
                                      .Where(x => x.BasketID == basketId
                                                  && x.PriceDate.Year == date.Year
                                                  && x.PriceDate.Month == date.Month
                                                  && x.PriceDate.Day == date.Day).Max(x => x.SellPrice);
                    if(max != null)
                        res = max.Value;
                }
                else if(TraderType == 2 || TraderType == 3)
                {
                    var max = _context.Set<BasketPrices>()
                                      .Where(x => x.BasketID == basketId
                                                  && x.PriceDate.Year == date.Year
                                                  && x.PriceDate.Month == date.Month
                                                  && x.PriceDate.Day == date.Day).Min(x => x.SellPrice);
                    if(max != null)
                        res = max.Value;
                }
            }
            catch(Exception)
            {
                res = 0;
            }
            return res;
        }
        public int CheckPrices(string buyPrice, string sellPrice, BasketPrices objTPrice)
        {
            int state = 0;

            if(buyPrice == "")
            {
                var buy = GetLastBuySellPriceToday(objTPrice.BasketID, "b");
                objTPrice.BuyPrice = buy;
                state = buy == 0M ? 2 : 0;
            }
            if(sellPrice == "")
            {
                var sell = GetLastBuySellPriceToday(objTPrice.BasketID, "s");
                objTPrice.SellPrice = sell;
                state = sell == 0M ? 3 : 0;
            }
            return state;
        }
        public decimal GetLastBuySellPriceToday(int basketId, string buySell)
        {
            DateTime today = DateTime.Now.Date;
            decimal buyPrice = 0M, sellPrice = 0M;
            var res =
                _context.Set<BasketPrices>().ToList().LastOrDefault(p => p.BasketID == basketId && p.PriceDate >= today);
            if(res == null)
            {
                Basket basket = _context.Set<Basket>().Find(basketId);
                var newRes =
                    _context.Set<TraderPrices>()
                            .ToList()
                            .LastOrDefault(p => p.TraderID == basket.BasketOwner && p.priceDate >= today);
                if(newRes != null)
                {
                    buyPrice = Convert.ToDecimal(newRes.BuyPrice);
                    sellPrice = Convert.ToDecimal(newRes.SellPrice);
                }
            }
            else
            {
                buyPrice = Convert.ToDecimal(res.BuyPrice);
                sellPrice = Convert.ToDecimal(res.SellPrice);
            }

            return buySell == "b" ? buyPrice : sellPrice;
        }
        public void SetCurrentFlag(int basketId, int recordId)
        {
            List<BasketPrices> basketPricesList =
                _context.Set<BasketPrices>().Where(x => x.BasketID == basketId && x.ID != recordId).ToList();
            for(int index = 0; index < basketPricesList.Count; index++)
            {
                var price = basketPricesList[index];
                price.IsCurrent = false;
            }
            SaveChanges();
        }
    }
}