using ASPTemplateSite.BLL.UserUtilities;
using ASPTemplateSite.DAL;
using ASPTemplateSite.DAL.Interfaces;
using ASPTemplateSite.DAL.Services;
using ASPTemplateSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPTemplateSite.Controllers
{
    [RESTAuthorize]
    public class ProfileController : Controller
    {
        public ActionResult Account()
        {
            return View(UserManager.Instance.CurrentUser);
        }
        public ActionResult Projects()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Response.Cookies.Add(new HttpCookie("Token", ""));

            UserManager.Instance.CurrentUser = null;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Management()
        {
            return View();
        }
    }
}