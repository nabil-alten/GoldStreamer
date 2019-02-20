using BLL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TraderPricesChartRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public TraderPricesChartRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }
        public List<TraderPricesChart> getALLPrices()
        {
           return _context.Set<TraderPricesChart>().Include("Trader").ToList();
        }
        public TraderPricesChart GetDayPrices(DateTime date, int? traderId)
        {
            var traderPricesChart = _context.Set<TraderPricesChart>()
                                      .Where(x => x.TraderID == traderId
                                                  && x.Date.Year == date.Year
                                                  && x.Date.Month == date.Month
                                                  && x.Date.Day == date.Day).ToList();
            if(traderPricesChart.Count>0)
            {
                return traderPricesChart.FirstOrDefault();
            }
          
            else
            {
                return null;
            } 
            
        }
        public decimal GetDayAverageBuyPrices(DateTime date)
        {
            decimal averageBuy = 0;
            List<GlobalPrice> PricesChart = _context.Set<GlobalPrice>().Where(x => x.CaptureTime.Year == date.Year
                                                  && x.CaptureTime.Month == date.Month
                                                  && x.CaptureTime.Day == date.Day).ToList();
            if (PricesChart.Count > 0)
            {
              
                averageBuy = PricesChart.Average(c=>c.Bid);
            }

            return averageBuy;
        }
        public decimal GetDayAverageSellPrices(DateTime date)
        {
            decimal averageSell = 0;
            List<GlobalPrice> PricesChart = _context.Set<GlobalPrice>().Where(x => x.CaptureTime.Year == date.Year
                                                  && x.CaptureTime.Month == date.Month
                                                  && x.CaptureTime.Day == date.Day).ToList();
            if (PricesChart.Count > 0)
            {
                averageSell = PricesChart.Average(c => c.Ask);
            }

            return averageSell;
        }
        public decimal GetDayCloseBuyPrices(DateTime date)
        {
            decimal CloseBuy = 0;
            List<GlobalPrice> PricesChart = _context.Set<GlobalPrice>().Where(x => x.CaptureTime.Year == date.Year
                                                  && x.CaptureTime.Month == date.Month
                                                  && x.CaptureTime.Day == date.Day).OrderBy(y => y.CaptureTime).ToList();
            if (PricesChart.Count > 0)
            {
                CloseBuy = PricesChart.FirstOrDefault(c => c.CaptureTime == PricesChart.Max(x => x.CaptureTime)).Bid;

            }

            return CloseBuy;
        }
        public decimal GetDayCloseSellPrices(DateTime date)
        {
            decimal CloseSell = 0;
            List<GlobalPrice> PricesChart = _context.Set<GlobalPrice>().Where(x => x.CaptureTime.Year == date.Year
                                                  && x.CaptureTime.Month == date.Month
                                                  && x.CaptureTime.Day == date.Day).OrderBy(y => y.CaptureTime).ToList();
            if (PricesChart.Count > 0)
            {
                CloseSell = PricesChart.FirstOrDefault(c => c.CaptureTime == PricesChart.Max(x => x.CaptureTime)).Ask;

            }

            return CloseSell;
        }
        public void GetAvgPricesInWeek(DateTime weekStartDay, DateTime weebEndDay,int? traderid, out decimal avgBuy,out decimal avgSell)
        {
            decimal sumbuy = 0;
            decimal sumsell = 0;
            int countbuy = 0;
            int countsell = 0;
            weekStartDay = new DateTime(weekStartDay.Year, weekStartDay.Month, weekStartDay.Day);
            weebEndDay = new DateTime(weebEndDay.Year, weebEndDay.Month, weebEndDay.Day).AddHours(23).AddMinutes(59).AddSeconds(59);
             List<TraderPricesChart> lst = _context.Set<TraderPricesChart>()
              .Where(x => x.TraderID == traderid && x.Date >= weekStartDay && x.Date<= weebEndDay).ToList();
            if (lst.Count > 0)
            {
                avgBuy = (decimal)_context.Set<TraderPricesChart>()
              .Where(x => x.TraderID == traderid && x.Date >= weekStartDay && x.Date <= weebEndDay && x.BuyAverage!=0 && x.BuyAverage!=null).Average(c => c.BuyAverage);
                avgSell = (decimal)_context.Set<TraderPricesChart>()
              .Where(x => x.TraderID == traderid && x.Date >= weekStartDay && x.Date <= weebEndDay && x.SellAverage != 0 && x.SellAverage != null).Average(c => c.SellAverage);
                //foreach (TraderPricesChart obj in lst)
                //{
                //    if (obj.BuyAverage != 0 && obj.BuyAverage != null)
                //    {
                //        countbuy += 1;
                //        sumbuy += decimal.Parse(obj.BuyAverage.ToString());
                //    }
                //    if (obj.SellAverage != 0 && obj.SellAverage != null)
                //    {
                //        countsell += 1;
                //        sumsell += decimal.Parse(obj.SellAverage.ToString());
                //    }


                //}
                //avgBuy = sumbuy / countbuy;
                //avgSell = sumsell / countsell;
            }
            else
            {
                avgBuy = 0;
                avgSell = 0;

            }

        }
        public void GetClosePricesInWeek(DateTime weekStartDay, DateTime weebEndDay,int? traderId, out decimal closeBuy,out decimal closeSell)
        {

            List<TraderPricesChart> lst = _context.Set<TraderPricesChart>()
                .Where(x => x.TraderID == traderId && x.Date >= weekStartDay && x.Date <= weebEndDay).ToList();
            weekStartDay = new DateTime(weekStartDay.Year, weekStartDay.Month, weekStartDay.Day);
            weebEndDay = new DateTime(weebEndDay.Year, weebEndDay.Month, weebEndDay.Day).AddHours(23).AddMinutes(59).AddSeconds(59);
            if (lst.Count > 0)
            {
                closeBuy = (decimal)_context.Set<TraderPricesChart>()
              .Where(x => x.TraderID == traderId && x.Date >= weekStartDay && x.Date <= weebEndDay && x.BuyClose != 0 && x.BuyClose != null).Average(c => c.BuyClose);
                closeSell = (decimal)_context.Set<TraderPricesChart>()
              .Where(x => x.TraderID == traderId && x.Date >= weekStartDay && x.Date <= weebEndDay && x.SellClose != 0 && x.SellClose != null).Average(c => c.SellClose);
            }
            else
            {
                closeBuy = 0;
                closeSell = 0;

            }

        }

       
    }
}
