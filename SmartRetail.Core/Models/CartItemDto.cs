using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartRetail.Core.Models
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

        //using this because JavaScript Cart -> JSON -> C# Model Binding
        //without DTO post will Fail (null data issue)
    }
}