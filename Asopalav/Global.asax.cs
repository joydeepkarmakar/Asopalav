using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Asopalav
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("http://asopalavjewelers.com"))
            {
                string referrerUrl = Request.Url.ToString().ToLower().Replace("http://asopalavjewelers.com", "https://www.asopalavjewelers.com");
                HttpContext.Current.Response.RedirectPermanent(referrerUrl, true);
            }
            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("http://www.asopalavjewelers.com"))
            {
                string referrerUrl = Request.Url.ToString().ToLower().Replace("http://www.asopalavjewelers.com", "https://www.asopalavjewelers.com");
                HttpContext.Current.Response.RedirectPermanent(referrerUrl, true);
            }
            //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("https://asopalavjewelers.com"))
            //{
            //    string referrerUrl = Request.Url.ToString().ToLower().Replace("https://asopalavjewelers.com", "https://www.asopalavjewelers.com");
            //    HttpContext.Current.Response.RedirectPermanent(referrerUrl, true);
            //}
        }
    }
}
