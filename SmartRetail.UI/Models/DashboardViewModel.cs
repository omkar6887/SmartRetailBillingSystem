using SmartRetail.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartRetail.UI.Models
{
    public class DashboardViewModel
    {
        public decimal TodaySales { get; set; }
        public int TotalBills { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public List<Product> LowStockProducts { get; set; }
    }
}