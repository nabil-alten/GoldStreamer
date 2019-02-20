using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using BLL.DomainClasses;
using DAL;
using GoldStreamer.Models.ViewModels;
using GoldStreamerWebApp.Models;

namespace GoldStreamerWebApp
{
    public class MvcApplication : HttpApplication
    {
        readonly GoldStreamerContext _context = new GoldStreamerContext();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new ValidateInputAttribute(false));
            AutoMapper.Mapper.CreateMap<Trader, TraderViewModel>();
            AutoMapper.Mapper.CreateMap<TraderRegisterViewModel, Trader>();
            Mapper.CreateMap<TraderPrices, TraderPricesPrintVM>();
            Mapper.CreateMap<BasketPrices, BasketPricesPrintVM>();
            if (_context.Database.Exists())
                SqlDependency.Start(ConfigurationManager.ConnectionStrings["GoldStreamerContext"].ConnectionString);
        }

        protected void Application_End()
        {
            if (_context.Database.Exists())
                SqlDependency.Stop(ConfigurationManager.ConnectionStrings["GoldStreamerContext"].ConnectionString);
        }
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            string culture = null;
            if (context.Request.UserLanguages != null && Request.UserLanguages.Length > 0)
            {
                culture = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
        }
    }
}
