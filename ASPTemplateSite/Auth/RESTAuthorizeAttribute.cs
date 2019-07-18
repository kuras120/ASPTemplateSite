using ASPTemplateSite.BLL.UserUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace ASPTemplateSite.DAL
{
    public class RESTAuthorizeAttribute : AuthorizeAttribute
    {
        private const string _securityToken = "Token"; // Name of the url parameter.
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }
            HandleUnauthorizedRequest(filterContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(_securityToken, ""));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            UserManager.Instance.CurrentUser = null;
            filterContext.Result = new RedirectResult("~/Home/Index");
            //base.HandleUnauthorizedRequest(filterContext);
        }
        private bool Authorize(AuthorizationContext actionContext)
        {
            try
            {
                HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;
                string token = request.Cookies.Get(_securityToken).Value;
                return TokenGenerator.IsTokenValid(token, ServerManager.GetIP(request), request.UserAgent);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}