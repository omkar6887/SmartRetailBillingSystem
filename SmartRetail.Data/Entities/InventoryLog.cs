using System;
using System.ComponentModel.DataAnnotations; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRetail.Data.Entities
{
    public class InventoryLog
    {
        [Key] //  PRIMARY KEY (FIXES 500 ERROR)
        public int LogId { get; set; }

        public int ProductId { get; set; }

        public string ChangeType { get; set; }
        // SALE, PURCHASE, ADJUSTMENT, RETURN

        public int QuantityChanged { get; set; }

        public int PreviousStock { get; set; }

        public int NewStock { get; set; }

        public int? ReferenceId { get; set; }
        // BillId reference

        public string Remarks { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public virtual Product Product { get; set; }
    }
}
