using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Duende.IdentityServer.EntityFramework.Options;

using EqDemo.Models;

namespace EqDemo.Data
{
    public class AppDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public AppDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

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

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
                .ToTable("Order_Details")
                .HasKey(od => new { od.OrderID, od.ProductID });
        }
    }
}
