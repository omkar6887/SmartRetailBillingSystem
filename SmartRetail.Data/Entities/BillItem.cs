using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRetail.Data.Entities
{
    public class BillItem
    {
        [Key]
        public int BillItemId { get; set; }

        [ForeignKey("Bill")]
        public int BillId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal Total { get; set; }

        // Navigation Properties
        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }
    }
}
