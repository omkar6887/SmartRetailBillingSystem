using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartRetail.UI.Models
{
    public class InvoiceViewModel
    {
        public int BillId { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public decimal GrandTotal { get; set; }
        public List<InvoiceItemViewModel> Items { get; set; }
    }

    public class InvoiceItemViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}