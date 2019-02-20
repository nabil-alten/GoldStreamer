using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class PricerRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public PricerRepo(GoldStreamerContext dbc)
        {
            _context = dbc;
        }
        public IEnumerable<Prices> GetAllPrices(int traderId, bool fromFavScreen = false)
        {

            List<Trader> traderList;
            if (fromFavScreen == false)
            {
                traderList = _context.Set<Trader>().Where(x => x.TypeFlag == 1 && x.IsActive).OrderBy(x => x.SortOrder).ToList();
            }
            else
            {
                TraderFavorites traderFav = _context.Set<TraderFavorites>().FirstOrDefault(tf => tf.FavOwnerId == traderId);
                traderList = (_context.Traders.Join(_context.FavorateList, trd => trd.ID, fav => fav.SuperTraderId,
                    (trd, fav) => new {trd, fav})
                    .Where(@t => @t.fav.TraderFavoriteId == traderFav.Id && @t.trd.IsActive)
                    .Select(@t => @t.trd)).ToList();
            }

            return GetTraderAllPrice(traderId, traderList);
        }
        private IEnumerable<Prices> GetTraderAllPrice(int traderId, List<Trader> traderList)
        {
            List<TraderPrices> allWithMaxBuy = GetTradersWithMaxBuy(traderList);
            List<TraderPrices> allWithMinSell = GetTradersWithMinSell(traderList);

            return (from t in traderList
                    let inBasketId = CheckTraderInBasket(t.ID, traderId)
                    select new Prices
                    {
                        Name = t.Name,
                        CurrentBuy = GetCurrentBuy(t.ID),
                        CurrentSell = GetCurrentSell(t.ID),
                        OpenBuy = GetOpenBuy(t.ID),
                        OpenSell = GetOpenSell(t.ID),
                        MaxBuy = GetMaxBuyPerTrader(t.ID),
                        MinSell = GetMinSellPerTrader(t.ID),
                        TradersMaxBuy = allWithMaxBuy.Any(x => x.TraderID == t.ID),
                        TradersMinSell = allWithMinSell.Any(x => x.TraderID == t.ID),
                        date = GetLastPriceDate(t.ID),
                        BasketBuy = inBasketId != 0 ? GetBasketBuyPrice(inBasketId) : 0,
                        BasketSell = inBasketId != 0 ? GetBasketSellPrice(inBasketId) : 0,
                        InBasket = inBasketId,
                        TraderID = t.ID
                    }).ToList();
        }
        public int CheckTraderInBasket(int superTraderId, int traderId)
        {
            var baskets = _context.Set<Basket>()
                                  .Where(x => x.BasketOwner == superTraderId && x.IsDeleted == false).ToList();
            int inBasketId = 0;
            foreach(var basketTrader in baskets.Select(basket => _context.Set<BasketTraders>()
                                                                         .SingleOrDefault(
                                                                             x =>
                                                                                 x.TraderId == traderId &&
                                                                                 x.BasketId == basket.ID))
                                               .Where(basketTrader => basketTrader != null))
            {
                inBasketId = basketTrader.BasketId;
            }
            return inBasketId;

            //return baskets.Select(basket => _context.Set<BasketTraders>()
            //    .SingleOrDefault(x => x.TraderId == traderId)).Where(basketTrader => basketTrader != null)
            //    .Select(basketTrader => basketTrader.BasketId).FirstOrDefault();
        }
        public decimal GetOpenBuy(int traderId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var traderLastRecord = _context.Set<TraderPrices>()
                                               .ToList().FirstOrDefault(x => x.TraderID == traderId
                                                                             && x.priceDate >= today);

                res = traderLastRecord.BuyPrice.Value;
            }
            catch(Exception ex)
            {
                res = 0;
            }
            return res;
        }
        public decimal GetOpenSell(int traderId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var traderLastRecord = _context.Set<TraderPrices>()
                                               .ToList().FirstOrDefault(x => x.TraderID == traderId
                                                                             && x.priceDate >= today);

                res = traderLastRecord.SellPrice.Value;
            }
            catch(Exception ex)
            {
                res = 0;
            }
            return res;
        }
        public decimal GetCurrentBuy(int traderId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var traderLastRecord = _context.Set<TraderPrices>()
                                               .ToList().LastOrDefault(x => x.TraderID == traderId
                                                                            && x.priceDate >= today);

                res = traderLastRecord.BuyPrice.Value;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        public decimal GetCurrentSell(int traderId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var traderLastRecord = _context.Set<TraderPrices>()
                                               .ToList().LastOrDefault(x => x.TraderID == traderId
                                                                            && x.priceDate >= today);

                res = traderLastRecord.SellPrice.Value;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        public decimal GetMaxBuyPerTrader(int traderId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var maxBuyPerTrader = _context.Set<TraderPrices>()
                    .Where(x => x.TraderID == traderId
                                && x.priceDate >= today)
                    .Max(x => x.BuyPrice);

                res = maxBuyPerTrader.Value;
            }
            catch(Exception ex)
            {
                res = 0;
            }
            return res;
        }
        public decimal GetMinSellPerTrader(int traderId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var maxSellPerTrader = _context.Set<TraderPrices>()
                                               .Where(x => x.TraderID == traderId
                                                          && x.priceDate >= today).ToList()
                                               .Min(x => x.SellPrice);

                res = maxSellPerTrader.Value;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        public int GetTraderWithMaxBuy()
        {
            int res = 1;
            DateTime today = DateTime.Now.Date;
            try
            {
                var todayRecords = _context.Set<TraderPrices>()
                                           .Where(x => x.priceDate >= today).ToList();

                var _lastTodayTradersRecord =
                    (from record in todayRecords
                     group record by record.TraderID
                     into g
                     select new
                     {
                         Group = g,
                         Max = g.Max(kvp => kvp.ID)
                     }
                     into ag
                     from x in ag.Group
                     where x.ID == ag.Max
                     select x);

                var maxTodayBuy = _lastTodayTradersRecord.Max(x => x.BuyPrice);
                var traderWithMaxBuy = _context.Set<TraderPrices>()
                                               .Where(x => x.BuyPrice == maxTodayBuy).FirstOrDefault();

                res = traderWithMaxBuy.TraderID;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        public int GetTraderWithMinSell()
        {
            int res = 1;
            DateTime today = DateTime.Now.Date;
            try
            {
                var todayRecords = _context.Set<TraderPrices>()
                                           .Where(x => x.priceDate >= today).ToList();

                var _lastTodayTradersRecord =
                    (from record in todayRecords
                     group record by record.TraderID
                     into g
                     select new
                     {
                         Group = g,
                         Max = g.Max(kvp => kvp.ID)
                     }
                     into ag
                     from x in ag.Group
                     where x.ID == ag.Max
                     select x);

                var minTodaySell = _lastTodayTradersRecord.Min(x => x.SellPrice);
                var traderWithMinSell = _context.Set<TraderPrices>()
                                                .Where(x => x.SellPrice == minTodaySell).FirstOrDefault();

                res = traderWithMinSell.TraderID;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        public DateTime GetLastPriceDate(int traderId)
        {
            DateTime res = DateTime.Now;
            DateTime today = DateTime.Now.Date;
            try
            {
                var traderLastRecord = _context.Set<TraderPrices>()
                                               .Where(x => x.TraderID == traderId
                                                           && x.priceDate >= today)
                                               .ToList()
                                               .LastOrDefault();

                res = traderLastRecord.priceDate;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        public List<TraderPrices> GetTradersWithMaxBuy(List<Trader> traderList)
        {
            var res = new List<TraderPrices>();
            DateTime today = DateTime.Now.Date;
            try
            {
                var todayRecords = (from tp in _context.Set<TraderPrices>().ToList()
                                    join t in traderList on tp.TraderID equals t.ID
                                    where tp.priceDate >= today
                                    select tp).ToList();

                var lastTodayTradersRecord =
                    (from record in todayRecords
                     group record by record.TraderID
                     into g
                     select new
                     {
                         Group = g,
                         Max = g.Max(kvp => kvp.ID)
                     }
                     into ag
                     from x in ag.Group
                     where x.ID == ag.Max
                     select x);

                var maxTodayBuy = lastTodayTradersRecord.Max(x => x.BuyPrice);

                var tradersWithMaxBuy = lastTodayTradersRecord.Where(x => x.BuyPrice == maxTodayBuy).ToList();

                res = tradersWithMaxBuy;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        public List<TraderPrices> GetTradersWithMinSell(List<Trader> traderList)
        {
            var res = new List<TraderPrices>();
            DateTime today = DateTime.Now.Date;
            try
            {
                var todayRecords = (from tp in _context.Set<TraderPrices>().ToList()
                                    join t in traderList on tp.TraderID equals t.ID
                                    where tp.priceDate >= today
                                    select tp).ToList();

                var lastTodayTradersRecord =
                    (from record in todayRecords
                     group record by record.TraderID
                     into g
                     select new
                     {
                         Group = g,
                         Max = g.Max(kvp => kvp.ID)
                     }
                     into ag
                     from x in ag.Group
                     where x.ID == ag.Max
                     select x);

                var minTodaySell = lastTodayTradersRecord.Min(x => x.SellPrice);

                var tradersWithMinSell = lastTodayTradersRecord.Where(x => x.SellPrice == minTodaySell).ToList();

                res = tradersWithMinSell;
            }
            catch(Exception ex)
            {
            }
            return res;
        }
        private decimal GetBasketBuyPrice(int basketId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var current = _context.Set<BasketPrices>()
                                      .SingleOrDefault(x => x.BasketID == basketId
                                                            && x.IsCurrent
                                                            && x.PriceDate >= today);

                if(current != null && current.BuyPrice != null)
                    res = current.BuyPrice.Value;
            }
            catch(Exception)
            {
                res = 0;
            }
            return res;
        }
        private decimal GetBasketSellPrice(int basketId)
        {
            decimal res = decimal.Parse("0.0");
            DateTime today = DateTime.Now.Date;
            try
            {
                var current = _context.Set<BasketPrices>()
                                      .SingleOrDefault(x => x.BasketID == basketId
                                                            && x.IsCurrent
                                                            && x.PriceDate >= today);

                if(current != null && current.SellPrice != null)
                    res = current.SellPrice.Value;
            }
            catch(Exception)
            {
                res = 0;
            }
            return res;
        }
        //[Obsolete]
        //private static string GetArabicNum(decimal valueDecimal, string currentCulture)
        //{
        //    UsefulMethods helpers = new UsefulMethods();
        //    string strFormatted = string.Format("{0:0,0.00;(0:0,0.00);----}", valueDecimal);
        //    CultureInfo ci = new CultureInfo(currentCulture);
        //    return helpers.ConvertToEasternArabicNumbers(strFormatted.ToString(ci));
        //}
        public decimal GetCurrentBestSell()
        {
            decimal res;
            DateTime today = DateTime.Now.Date;
            try
            {
                var todayRecords = (from tp in _context.Set<TraderPrices>().ToList()
                                    where tp.priceDate >= today
                                    select tp).ToList();

                var lastTodayTradersRecord =
                    (from record in todayRecords
                     group record by record.TraderID
                         into g
                         select new
                         {
                             Group = g,
                             Max = g.Max(kvp => kvp.ID)
                         }
                             into ag
                             from x in ag.Group
                             where x.ID == ag.Max
                             select x);

                var minTodaySell = lastTodayTradersRecord.Min(x => x.SellPrice);

                res = minTodaySell.Value;
            }
            catch (Exception)
            {
                res = (decimal) 0.0;
            }
            return res;
        }
        public decimal GetCurrentBestBuy()
        {
            decimal res;
            DateTime today = DateTime.Now.Date;
            try
            {
                var todayRecords = (from tp in _context.Set<TraderPrices>().ToList()
                                    where tp.priceDate >= today
                                    select tp).ToList();

                var lastTodayTradersRecord =
                    (from record in todayRecords
                     group record by record.TraderID
                         into g
                         select new
                         {
                             Group = g,
                             Max = g.Max(kvp => kvp.ID)
                         }
                             into ag
                             from x in ag.Group
                             where x.ID == ag.Max
                             select x);

                var maxTodayBuy = lastTodayTradersRecord.Max(x => x.BuyPrice );

                res = maxTodayBuy.Value;
            }
            catch (Exception)
            {
                res = (decimal)0.0;
            }
            return res;
        }
    }
}