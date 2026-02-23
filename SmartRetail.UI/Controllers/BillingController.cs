using SmartRetail.Data.Context;
using SmartRetail.Data.Entities;
using SmartRetail.UI.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using SmartRetail.Core.Services;
using SmartRetail.Core.Models;


namespace SmartRetail.UI.Controllers
{

    
    public class BillingController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] == null)
            {
                filterContext.Result = RedirectToAction("Login", "Account");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        private readonly SmartRetailDbContext db = new SmartRetailDbContext();
        private readonly BillingService billingService = new BillingService();

        // Load POS Screen
        public ActionResult Index()
        {
            return View();
        }

        // Barcode Scan -> Fetch Product (READ ONLY)
        [HttpGet]
        public JsonResult GetProductByBarcode(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var product = db.Products
                .Where(p => p.Barcode == barcode && p.Stock > 0)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.Price,
                    p.Stock
                })
                .FirstOrDefault();

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        // Generate Bill + Save BillItems + Reduce Stock
        //[HttpPost]
        //public JsonResult GenerateBill(GenerateBillRequest request)
        //{
        //    try
        //    {
        //        var cartItems = request.CartItems;

        //        if (cartItems == null || !cartItems.Any())
        //        {
        //            return Json(new { success = false, message = "Cart is empty" });
        //        }

        //        decimal grandTotal = cartItems.Sum(x => x.Total);

        //        var bill = new Bill
        //        {
        //            BillNumber = "BILL-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
        //            BillDate = DateTime.Now,
        //            TotalAmount = grandTotal,
        //            CreatedBy = 1 // after inserting admin user
        //        };

        //        db.Bills.Add(bill);
        //        db.SaveChanges(); // FIRST SAVE (gets BillId)

        //        foreach (var item in cartItems)
        //        {
        //            var product = db.Products.Find(item.ProductId);

        //            if (product == null)
        //            {
        //                return Json(new { success = false, message = "Product not found" });
        //            }

        //            if (product.Stock < item.Quantity)
        //            {
        //                return Json(new
        //                {
        //                    success = false,
        //                    message = "Insufficient stock for " + product.ProductName
        //                });
        //            }

        //            // STORE PREVIOUS STOCK (for logging)
        //            int previousStock = product.Stock;

        //            // REDUCE STOCK
        //            product.Stock -= item.Quantity;

        //            // NEW STOCK AFTER SALE
        //            int newStock = product.Stock;

        //            // SAVE BILL ITEM (existing logic)
        //            var billItem = new BillItem
        //            {
        //                BillId = bill.BillId,
        //                ProductId = item.ProductId,
        //                Quantity = item.Quantity,
        //                Price = item.Price,
        //                Total = Math.Round(item.Total, 2)
        //            };

        //            db.BillItems.Add(billItem);

        //            var inventoryLog = new InventoryLog
        //            {
        //                ProductId = product.ProductId,
        //                ChangeType = "SALE", // because stock reduced due to billing
        //                QuantityChanged = item.Quantity,
        //                PreviousStock = previousStock,
        //                NewStock = newStock,
        //                ReferenceId = bill.BillId, // links log to this bill
        //                Remarks = "Stock reduced after billing",
        //                CreatedAt = DateTime.Now
        //            };

        //            db.InventoryLogs.Add(inventoryLog);
        //        }

        //        db.SaveChanges(); // FINAL SAVE (BillItems + Stock + InventoryLogs)

        //        return Json(new
        //        {
        //            success = true,
        //            billId = bill.BillId,
        //            message = "Bill Generated Successfully with Inventory Log!"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        var inner = ex.InnerException?.InnerException?.Message ?? ex.Message;

        //        return Json(new
        //        {
        //            success = false,
        //            message = inner
        //        });
        //    }
        //}

        [HttpPost]
        public JsonResult GenerateBill(GenerateBillRequest request)
        {
            if (request == null || request.CartItems == null || !request.CartItems.Any())
            {
                return Json(new { success = false, message = "Cart is empty!" });
            }

            try
            {
                // Get Logged-in UserId from Session
                int userId = Convert.ToInt32(Session["UserId"]);

                // Calculate Total
                decimal totalAmount = request.CartItems.Sum(x => x.Total);

                // Generate Bill Number (POS Style)
                string billNumber = "BILL-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                // STEP 1: Create Bill
                var bill = new Bill
                {
                    BillNumber = billNumber,
                    BillDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    CreatedBy = userId
                };

                db.Bills.Add(bill);
                db.SaveChanges(); // Generates BillId

                // STEP 2: Save BillItems + Reduce Stock
                foreach (var item in request.CartItems)
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);

                    if (product == null)
                    {
                        return Json(new { success = false, message = "Product not found" });
                    }

                    if (product.Stock < item.Quantity)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Insufficient stock for " + product.ProductName
                        });
                    }

                    // Save Bill Item
                    var billItem = new BillItem
                    {
                        BillId = bill.BillId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Total
                    };

                    db.BillItems.Add(billItem);

                    // Reduce Stock (Matches your Products table: Stock INT NOT NULL)
                    product.Stock = product.Stock - item.Quantity;
                }

                db.SaveChanges();

                return Json(new
                {
                    success = true,
                    billId = bill.BillId,
                    message = "Bill Generated Successfully!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }





        public ActionResult Invoice(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index"); // safe fallback
            }
            var bill = db.Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null)
            {
                return HttpNotFound("Bill not found");
            }

            var items = (from bi in db.BillItems
                         join p in db.Products on bi.ProductId equals p.ProductId
                         where bi.BillId == id
                         select new InvoiceItemViewModel
                         {
                             ProductName = p.ProductName,
                             Quantity = bi.Quantity,
                             Price = bi.Price,
                             Total = bi.Total
                         }).ToList();

            var viewModel = new InvoiceViewModel
            {
                BillId = bill.BillId,
                BillNumber = bill.BillNumber,
                BillDate = bill.BillDate,
                GrandTotal = bill.TotalAmount,
                Items = items
            };

            return View(viewModel);
        }

    }
}
