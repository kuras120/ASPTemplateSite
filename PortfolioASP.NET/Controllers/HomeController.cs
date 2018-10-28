using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioASP.NET.BLL.UserUtilities;
using PortfolioASP.NET.BLL.UserUtilities.Interfaces;
using PortfolioASP.NET.DAL;
using PortfolioASP.NET.Models;

namespace PortfolioASP.NET.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Active = "Home";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Active = "About";
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Active = "Contact";
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult AcceptTerms()
        {
            ViewModelManager model = new ViewModelManager();
            model.RegisterModel = new RegisterViewModel();
            model.RegisterModel.AcceptTerms = true;
            return View("Register", model);
        }

        public ActionResult SubmitRegister(ViewModelManager model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }
            var register = (IRegister)UserManager.Instance;
            if (!register.Validate(model.RegisterModel.Login, model.RegisterModel.Password, model.RegisterModel.Email))
            {
                ModelState.AddModelError("", "User already exists");
                return View("Register");
            }

            register.AddUser(model.RegisterModel.Login, model.RegisterModel.Password, model.RegisterModel.Email);
            UserManager.Instance.CurrentUser = ((ILogIn)UserManager.Instance).GetUser(model.RegisterModel.Login);
            CreateCookieToken();
            return RedirectToAction("Account", "Profile");
        }

        public ActionResult SubmitLogin(ViewModelManager model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ShowModal = true;
                return View("Index", model);
            }

            var login = (ILogIn)UserManager.Instance;
            if(!login.Validate(model.LoginModel.Login, model.LoginModel.Password))
            {
                ModelState.AddModelError("", "Invalid username or password");
                ViewBag.ShowModal = true;
                return View("Index");
            }
            CreateCookieToken();
            return RedirectToAction("Account", "Profile");

        }

        private void CreateCookieToken()
        {
            var token = TokenGenerator.GenerateToken(
                UserManager.Instance.CurrentUser.Login,
                UserManager.Instance.CurrentUser.Password,
                ServerManager.GetIP(Request),
                Request.UserAgent,
                DateTime.UtcNow.Ticks,
                true
            );

            HttpCookie tokenCookie = new HttpCookie("Token")
            {
                Value = token,
                Expires = DateTime.UtcNow.AddHours(12)
            };
            Response.Cookies.Add(tokenCookie);

            Session["UserID"] = UserManager.Instance.CurrentUser.Id;
        }
    }
}