using System;
using System.Collections;
using System.Collections.Generic;
using forexfeed.net;
using FeederCapturer.Models;

namespace FeederCapturer.ForexFeederHelper
{
    public class ForexFeeder
    {
        private const string AccessKey = "143487764999755";
        private readonly feedapi _api;

        public ForexFeeder(string symbol, int interval, int periods, string price)
        {
            _api = new feedapi(AccessKey, symbol, interval, periods, price);
        }

        public List<GlobalPrice> GetFeed()
        {
            ArrayList quotes = _api.getData();
            List<GlobalPrice> feed = new List<GlobalPrice>();
            if (_api.getStatus().Equals("OK"))
            {
                IEnumerator itr = quotes.GetEnumerator();
                while (itr.MoveNext())
                {
                    Hashtable quote = (Hashtable)(itr.Current);
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(Convert.ToDouble(quote["time"])).ToLocalTime();
                    feed.Add(new GlobalPrice
                                       {
                                           Ask = Convert.ToDecimal(quote["ask"]),
                                           Bid = Convert.ToDecimal(quote["bid"]),
                                           Open = Convert.ToDecimal(quote["open"]),
                                           High = Convert.ToDecimal(quote["high"]),
                                           Low = Convert.ToDecimal(quote["low"]),
                                           Close = Convert.ToDecimal(quote["close"]),
                                           CaptureTime = dateTime,
                                       });
                }
            }
            else
            {
                throw new Exception(_api.getErrorMessage());
            }

            return feed;
        }
    }
}
