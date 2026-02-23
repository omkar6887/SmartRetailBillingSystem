using SmartRetail.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRetail.Data.Models
{
    [Table("BillItems")]
    public class BillItem
    {
        [Key]
        public int BillItemId { get; set; }  // matches DB column

        [Required]
        public int BillId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Price { get; set; }

        [Column("Total", TypeName = "decimal")]
        public decimal Total { get; set; }   // matches DB column name

        // Navigation Properties
        [ForeignKey("BillId")]
        public virtual Bill Bill { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
