using BakeryShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BakeryShop.Data
{
    public class BakeryShopDbContext: DbContext
    {
        public BakeryShopDbContext(DbContextOptions<BakeryShopDbContext> options):base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BakeryItem> BakeryItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BakeryShopDbContext).Assembly);
        }
    }
}
