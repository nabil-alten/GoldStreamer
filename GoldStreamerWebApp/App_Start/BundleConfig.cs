using System.Web;
using System.Web.Optimization;

namespace GoldStreamerWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //        "~/Scripts/jquery.validate*",
            //        "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/toastr.css"
                      ,
                      "~/Content/site.css"
                    
                      ));

            bundles.Add(new StyleBundle("~/bundles/toast").Include(              
                      "~/Scripts/toastr*",                      
                      "~/Scripts/GeneralJS.js"));

            bundles.Add(new StyleBundle("~/bundles/jqwidgets").Include(
                     "~/Scripts/jqwidgets/jqxcore.js",
                     "~/Scripts/jqwidgets/jqxdropdownbutton.js",
                     "~/Scripts/jqwidgets/jqxscrollbar.js",
                     "~/Scripts/jqwidgets/jqxbuttons.js",
                     "~/Scripts/jqwidgets/jqxtree.js",
                     "~/Scripts/jqwidgets/jqxpanel.js",
                     "~/Scripts/jqwidgets/jqxdatetimeinput.js",
                     "~/Scripts/jqwidgets/jqxcalendar.js",
                     "~/Scripts/jqwidgets/jqxtooltip.js",
                     "~/Content/jqwidgets/globalization/globalize.js"
                   ));

            bundles.Add(new StyleBundle("~/bundles/jqwidgetscss").Include(
                      "~/styles/jqx.base.css"
                      ));
     

        }
    }
}
