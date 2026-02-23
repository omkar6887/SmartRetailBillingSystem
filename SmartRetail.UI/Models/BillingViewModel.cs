using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartRetail.Data.Models;
namespace SmartRetail.UI.Models
{
    public class BillingViewModel
    {
        public string Barcode { get; set; }

        public List<BillItem> CartItems { get; set; }

        public decimal GrandTotal { get; set; }
    }
}