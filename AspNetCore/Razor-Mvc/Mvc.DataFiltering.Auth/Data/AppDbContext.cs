using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using EqDemo.Models;

namespace EqDemo
{

    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        #region NWind
        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Shipper> Shippers { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        #endregion

        public DbSet<UserQuery> UserQueries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
                .ToTable("Order_Details")
                .HasKey(od => new { od.OrderID, od.ProductID });

            //different users can store their queries under the same ID (e.g. "LastQuery"),
            //so the primary key must include the owner as well
            modelBuilder.Entity<UserQuery>()
                .HasKey(q => new { q.Id, q.OwnerId });
        }
    }
}
