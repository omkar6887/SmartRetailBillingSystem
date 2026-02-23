using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace SmartRetail.Data.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; }  // NOT "Name"

        [Required]
        [StringLength(50)]
        public string Barcode { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }   // VERY IMPORTANT (exists in DB)

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Category Category { get; set; }
    }
}
