﻿using System.Configuration;

using Microsoft.EntityFrameworkCore;

namespace EqDemo.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext()
            : base()
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conenctionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ToString();
            optionsBuilder.UseSqlServer(conenctionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
               .ToTable("Order_Details")
               .HasKey(od => new { od.OrderID, od.ProductID });
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }

}