using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRetail.Data.Context; //  using because i've added reference here.

namespace SmartRetail.UI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var db = new SmartRetailDbContext())
            {
                var productCount = db.Products.Count();
                ViewBag.Count = productCount;
            }
            return View();
        }


        public ActionResult TestBillItems()
        {
            using (var db = new SmartRetailDbContext())
            {
                var count = db.BillItems.Count();
                ViewBag.Count = count;
                return View();
            }
        }
    }
}