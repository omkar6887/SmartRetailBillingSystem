using System;
using System.Linq;
using SmartRetail.Data.Context;
using SmartRetail.Data.Entities;
using SmartRetail.Core.Models;


namespace SmartRetail.Core.Services
{
    public class BillingService
    {
        private readonly SmartRetailDbContext db;

        public BillingService()
        {
            db = new SmartRetailDbContext();
        }

        public object GenerateBill(GenerateBillRequest request, int userId)
        {
            try
            {
                var cartItems = request.CartItems;

                if (cartItems == null || !cartItems.Any())
                {
                    return new { success = false, message = "Cart is empty" };
                }

                decimal grandTotal = cartItems.Sum(x => x.Total);

                var bill = new Bill
                {
                    BillNumber = "BILL-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    BillDate = DateTime.Now,
                    TotalAmount = grandTotal,
                    CreatedBy = userId
                };

                db.Bills.Add(bill);
                db.SaveChanges(); // Get BillId

                foreach (var item in cartItems)
                {
                    var product = db.Products.Find(item.ProductId);

                    if (product == null)
                    {
                        return new { success = false, message = "Product not found" };
                    }

                    if (product.Stock < item.Quantity)
                    {
                        return new
                        {
                            success = false,
                            message = "Insufficient stock for " + product.ProductName
                        };
                    }

                    int previousStock = product.Stock;
                    product.Stock -= item.Quantity;
                    int newStock = product.Stock;

                    var billItem = new BillItem
                    {
                        BillId = bill.BillId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = Math.Round(item.Total, 2)
                    };

                    db.BillItems.Add(billItem);

                    var inventoryLog = new InventoryLog
                    {
                        ProductId = product.ProductId,
                        ChangeType = "SALE",
                        QuantityChanged = item.Quantity,
                        PreviousStock = previousStock,
                        NewStock = newStock,
                        ReferenceId = bill.BillId,
                        Remarks = "Stock reduced after billing",
                        CreatedAt = DateTime.Now
                    };

                    db.InventoryLogs.Add(inventoryLog);
                }

                db.SaveChanges();

                return new
                {
                    success = true,
                    billId = bill.BillId,
                    message = "Bill Generated Successfully!"
                };
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message ?? ex.Message;

                return new
                {
                    success = false,
                    message = inner
                };
            }
        }
    }
}
