using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartRetail.Core.Models;


namespace SmartRetail.Core.Models
{
    public class GenerateBillRequest
    {
        public List<CartItemViewModel> CartItems { get; set; }
    }

    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
