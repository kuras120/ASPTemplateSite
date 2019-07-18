using ASPTemplateSite.Models;
using ASPTemplateSite.DAL;
using ASPTemplateSite.Logger;
using ASPTemplateSite.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ASPTemplateSite.Auth;

namespace ASPTemplateSite
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
