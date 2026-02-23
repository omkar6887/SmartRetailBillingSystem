using SmartRetail.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;  // Make sure to install EntityFramework via NuGet

namespace SmartRetail.Data.Context
{
    public class SmartRetailDbContext : DbContext
    {
        public SmartRetailDbContext() : base("name=SmartRetailConnection")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<InventoryLog> InventoryLogs { get; set; }

    }
}