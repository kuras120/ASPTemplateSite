using PortfolioASP.NET.Models;
using PortfolioASP.NET.DAL;
using PortfolioASP.NET.Logger;
using PortfolioASP.NET.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PortfolioASP.NET
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DataManager m = new DataManager();
#if DEBUG
            m.DeleteAll();
            m.CreateDebug(10);
#else
            m.DeleteAll();
            m.CreateRelease();
#endif
        }
    }
}
