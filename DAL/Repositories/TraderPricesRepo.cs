using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class TraderPricesRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext context;
        public TraderPricesRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            context = dbc;
        }
        public List<TraderPrices> FindByTrader(int traderId)
        {
            DateTime today = DateTime.Now.Date;
            return
                context.Set<TraderPrices>()
                       .Where(p => p.TraderID == traderId && p.priceDate >= today)
                       .Select(p => p)
                       .ToList();
        }
        public int CheckPrices(string buyPrice, string sellPrice, TraderPrices objTPrice)
        {
            int res = 0;
            decimal buy = 0M, sell = 0M;
            if(buyPrice == "")
            {
                buy = GetLastBuySellPriceToday(objTPrice.TraderID, "b");
                objTPrice.BuyPrice = buy;
                res = buy == 0M ? 2 : 0;
            }
            if(sellPrice == "")
            {
                sell = GetLastBuySellPriceToday(objTPrice.TraderID, "s");
                objTPrice.SellPrice = sell;
                res = sell == 0M ? 3 : 0;
            }
            return res;
        }
        public decimal GetLastBuySellPriceToday(int traderId, string buySell)
        {
            DateTime today = DateTime.Now.Date;
            decimal buyPrice = 0M, sellPrice = 0M;
            var res =
                context.Set<TraderPrices>().ToList().LastOrDefault(p => p.TraderID == traderId && p.priceDate >= today);
            buyPrice = res == null ? 0M : Convert.ToDecimal(res.BuyPrice);
            sellPrice = res == null ? 0M : Convert.ToDecimal(res.SellPrice);

            return buySell == "b" ? buyPrice : sellPrice;
        }
        public decimal GetTraderMinBuyPrice(int traderId)
        {
            DateTime today = DateTime.Now.Date;
            var res =
                context.Set<TraderPrices>()
                       .Where(p => p.TraderID == traderId && p.priceDate >= today)
                       .Select(p => p)
                       .Min(p => p.BuyPrice);
            return Convert.ToDecimal(res);
        }
        public decimal GetTraderMaxSellPrice(int traderId)
        {
            DateTime today = DateTime.Now.Date;
            var res =
                context.Set<TraderPrices>()
                       .Where(p => p.TraderID == traderId && p.priceDate >= today)
                       .Select(p => p)
                       .Max(p => p.SellPrice);
            return Convert.ToDecimal(res);
        }
        public List<TraderPrices> GetDayPrices(DateTime date, int traderId)
        {
            var traderPrices = context.Set<TraderPrices>()
                                      .Where(x => x.TraderID == traderId
                                                  && x.priceDate.Year == date.Year
                                                  && x.priceDate.Month == date.Month
                                                  && x.priceDate.Day == date.Day).ToList();

            return traderPrices;
        }
        public decimal GetBasketMaxBuyPrice(int traderId, DateTime date)
        {
            decimal res = decimal.Parse("0.0");
            try
            {
                var min = context.Set<TraderPrices>()
                                 .Where(x => x.TraderID == traderId
                                             && x.priceDate.Year == date.Year
                                             && x.priceDate.Month == date.Month
                                             && x.priceDate.Day == date.Day).Max(x => x.BuyPrice);
                if(min != null)
                    res = min.Value;
            }
            catch(Exception)
            {
                res = 0;
            }
            return res;
        }
        public decimal GetBasketMinSellPrice(int traderId, DateTime date)
        {
            var res = decimal.Parse("0.0");
            try
            {
                var max = context.Set<TraderPrices>()
                                 .Where(x => x.TraderID == traderId
                                             && x.priceDate.Year == date.Year
                                             && x.priceDate.Month == date.Month
                                             && x.priceDate.Day == date.Day).Min(x => x.SellPrice);
                if(max != null)
                    res = max.Value;
            }
            catch(Exception)
            {
                res = 0;
            }
            return res;
        }
    }
}