using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRetail.Data.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0.1, 9999990)]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string PaymentMode { get; set; }
        public int CreatedBy { get; set; }      // UserId (Cashier)

        //Navigation Property (One Order -> Many OrderItems)
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
