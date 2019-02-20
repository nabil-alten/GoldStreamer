using System.Web;

namespace GoldStreamer.Helpers
{
    public static class CultureHelper
    {
        public static string GetBaseURL()
        {
            string domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
            string culture = "";
            string primaryCulture = GetPrimaryCulture();
            string secondaryCulture = GetSecondaryCulture();

            if (HttpContext.Current.Request.Url.OriginalString.ToLower().Contains(primaryCulture.ToLower()))
            {
                culture = primaryCulture;
            }
            else
            {
                culture = secondaryCulture;
            }
            string domainWithCulture = domain + "/";// +culture;
            return domainWithCulture;
        }

        public static string GetCurrentCulture()
        {
            string culture = "";
            string primaryCulture = GetPrimaryCulture();
            string secondaryCulture = GetSecondaryCulture();

            if (HttpContext.Current.Request.Url.OriginalString.ToLower().Contains(primaryCulture.ToLower()))
            {
                culture = primaryCulture;
            }
            else
            {
                culture = secondaryCulture;
            }
            return culture;
        }

        public static string GetPrimaryCulture()
        {
            string primaryCulture = System.Configuration.ConfigurationManager.AppSettings["PrimaryCulture"] != null ? System.Configuration.ConfigurationManager.AppSettings["PrimaryCulture"].ToString() : "";
            return primaryCulture;
        }

        public static string GetSecondaryCulture()
        {
            string secondaryCulture = System.Configuration.ConfigurationManager.AppSettings["SecondaryCulture"] != null ? System.Configuration.ConfigurationManager.AppSettings["SecondaryCulture"].ToString() : "";
            return secondaryCulture;
        }

        public static string GetBaseUrlWithoutAreaAndCulture()
        {
            string domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
            string culture = "";
            string primaryCulture = GetPrimaryCulture();
            string secondaryCulture = GetSecondaryCulture();


            if (HttpContext.Current.Request.Url.OriginalString.ToLower().Contains(primaryCulture.ToLower()))
            {
                culture = primaryCulture;
            }
            else
            {
                culture = secondaryCulture;
            }
            string domainWithCulture = domain;
            return domainWithCulture;
        }
    }
}