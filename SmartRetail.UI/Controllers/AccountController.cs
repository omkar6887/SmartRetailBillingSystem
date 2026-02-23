using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRetail.Core.Models;
using SmartRetail.Core.Services;

namespace SmartRetail.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        
        public AccountController()
        {
            _authService = new AuthService();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            // Trim to avoid space issues
            string username = request.Username?.Trim();
            string password = request.Password;

            var user = _authService.ValidateUser(username, password);

            if (user != null)
            {
                // Store session values
                Session["UserId"] = user.UserId;
                Session["Username"] = user.Username;
                Session["Role"] = user.Role;

                // Redirect to Billing Dashboard (POS Screen)
                return RedirectToAction("Index", "Dashboard");

            }

            ViewBag.Error = "Invalid Username or Password";
            return View(request);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }


            


    }
}