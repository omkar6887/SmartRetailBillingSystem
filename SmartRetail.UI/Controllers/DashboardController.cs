using System;
using System.Linq;
using System.Web.Mvc;
using SmartRetail.Data.Context;
using SmartRetail.UI.Models;

namespace SmartRetail.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly SmartRetailDbContext db = new SmartRetailDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] == null)
            {
                filterContext.Result = RedirectToAction("Login", "Account");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            var today = DateTime.Today;

            var model = new DashboardViewModel
            {
                TodaySales = db.Bills
                    .Where(b => b.BillDate >= today)
                    .Sum(b => (decimal?)b.TotalAmount) ?? 0,

                TotalBills = db.Bills.Count(),

                TotalProducts = db.Products.Count(),

                LowStockCount = db.Products.Count(p => p.Stock <= 10),

                LowStockProducts = db.Products
                    .Where(p => p.Stock <= 10)
                    .OrderBy(p => p.Stock)
                    .Take(5)
                    .ToList()
            };

            return View(model);
        }
    }
}
