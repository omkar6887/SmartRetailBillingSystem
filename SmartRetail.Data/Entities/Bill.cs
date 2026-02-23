using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRetail.Data.Entities
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }

        [Required]
        [StringLength(50)]
        public string BillNumber { get; set; }

        public DateTime BillDate { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal TotalAmount { get; set; }

        // FK → Users table
        public int? CreatedBy { get; set; }

        // Navigation Property
        public virtual ICollection<BillItem> BillItems { get; set; }
    }
}
