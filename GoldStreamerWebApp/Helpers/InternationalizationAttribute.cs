using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace localization.Helpers
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string language = (string)filterContext.RouteData.Values["language"] ?? "ar";
            string culture = (string)filterContext.RouteData.Values["culture"] ?? "eg";

            string CurrentUrl = filterContext.HttpContext.Request.Url.OriginalString;

            if (!CurrentUrl.Contains("en-us") && !CurrentUrl.Contains("ar-eg"))
            {
                language = "ar";
                culture = "eg";
            }
            else if (CurrentUrl.Contains("en-us"))
            {
                language = "en";
                culture = "us";
            }
            else
            {
                language = "ar";
                culture = "eg";
            }



            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "ar", "eg" ));
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "ar", "eg" ));

        }

    

    }
}