using FeederCapturer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.ServiceProcess;
using System.Timers;
using FeederCapturer.ForexFeederHelper;
using FeederCapturer.DbHelper;

namespace FeederCapturer
{
    public partial class GlobalPricesCapturerService : ServiceBase
    {
        private int _interval;
        private int _periods;
        private string _conStr;
        private int _feedRate;
        private string _symbol;
        private string _price;
        private Timer _aTimer;
        public GlobalPricesCapturerService()
        {
            InitializeComponent();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            GetFeed();
        }
        private void GetFeed()
        {
            ForexFeeder forexFeeder = new ForexFeeder(_symbol, _interval, _periods, _price);

            try
            {
                List<GlobalPrice> feed = forexFeeder.GetFeed();

                SqlServerSaverMgr saver = new SqlServerSaverMgr(_conStr);

                foreach (var gp in feed)
                {
                    gp.SaveToEntity();
                    saver.SaveElement(gp.Entity);
                    gp.Id = (int)saver.Outputs[0];
                }
                TraderPricesChart traderPricesChart = new TraderPricesChart
                {
                    GlobalPriceId = feed[0].Id
                };
                traderPricesChart.SaveToEntity();
                saver.UpdateTraderPricesChart(traderPricesChart.Entity);
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            _interval = Convert.ToInt32(ConfigurationManager.AppSettings["interval"]);
            _periods = Convert.ToInt32(ConfigurationManager.AppSettings["periods"]);
            _symbol = Convert.ToString(ConfigurationManager.AppSettings["symbol"]);
            _price = Convert.ToString(ConfigurationManager.AppSettings["price"]);

            _feedRate = Convert.ToInt32(ConfigurationManager.AppSettings["feedRate"]);
            _conStr = ConfigurationManager.ConnectionStrings["goldConStr"].ConnectionString;

            _aTimer = new Timer(_feedRate);

            _aTimer.Elapsed += OnTimedEvent;

            _aTimer.Interval = _feedRate;
            _aTimer.Enabled = true;
        }
        protected override void OnStop()
        {
            _aTimer.Enabled = false;
        }
        protected override void OnContinue()
        {
            _aTimer.Enabled = true;
        }
        protected override void OnPause()
        {
            _aTimer.Enabled = false;
        }
    }
}
