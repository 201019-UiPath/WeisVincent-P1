using IceShopDB.Models;
using IceShopDB.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace IceShopDB
{
    public class IceShopContext : DbContext
    {

        public IceShopContext(DbContextOptions options) : base(options)
        {

        }

        public IceShopContext() { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<InventoryLineItem> InventoryLineItems { get; set; }
        public DbSet<OrderLineItem> OrderLineItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // This will get the current directory the program runs in
                string whereTheAppRuns = AppDomain.CurrentDomain.BaseDirectory;

                // This will get the parent directory.
                string projectDirectory = Directory.GetParent(whereTheAppRuns).Parent.Parent.Parent.FullName;

                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(projectDirectory) //Directory.GetCurrentDirectory()
                    .AddJsonFile("appsettings.json")
                    .Build();


                string connectionString = configuration.GetConnectionString("IceShopDB");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //TODO: Configure modelBuilder to implement model relationships
            /* Needs: Customer to Orders OTM
            Order to Customer OTO
            */


            #region Seed Data
            modelBuilder.Entity<Customer>().HasData(SampleData.GetSampleCustomers());
            modelBuilder.Entity<Location>().HasData(SampleData.GetSampleLocations());
            modelBuilder.Entity<Order>().HasData(SampleData.GetSampleOrders());
            modelBuilder.Entity<Manager>().HasData(SampleData.GetSampleManagers());
            modelBuilder.Entity<Product>().HasData(SampleData.GetSampleProducts());

            modelBuilder.Entity<InventoryLineItem>().HasData(SampleData.GetSampleInventoryLineItems());
            modelBuilder.Entity<OrderLineItem>().HasData(SampleData.GetSampleOrderLineItems());
            #endregion



            modelBuilder.Entity<Customer>().HasKey("Id");
            modelBuilder.Entity<Location>().HasKey("Id");
            modelBuilder.Entity<Manager>().HasKey("Id");
            modelBuilder.Entity<Order>().HasKey("Id");
            modelBuilder.Entity<Product>().HasKey("Id");

            #region Manual Model Relationship Mapping


            modelBuilder.Entity<Product>()
                .Property(p => p.TypeOfProduct)
                .HasConversion<int>();



            // Location - Manager One to Many Relationship
            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Location)
                .WithMany(l => l.Managers)
                .HasForeignKey(m => m.LocationId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Managers)
                .WithOne(m => m.Location)
                .HasForeignKey(l => l.LocationId);

            // Customer - Order One to Many Relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);


            #region Location - Product Many to Many Relationship (InventoryLineItem)
            modelBuilder.Entity<InventoryLineItem>()
                .HasKey(ili => new { ili.LocationId, ili.ProductId });
            modelBuilder.Entity<InventoryLineItem>()
                .HasOne(ili => ili.Location)
                .WithMany(o => o.InventoryLineItems)
                .HasForeignKey(ili => ili.LocationId);
            modelBuilder.Entity<InventoryLineItem>()
                .HasOne(oli => oli.Product)
                .WithMany(o => o.LocationsWithProduct)
                .HasForeignKey(oli => oli.ProductId);

            #endregion

            #region Order - Product Many to Many Relationship (OrderLineItem)
            modelBuilder.Entity<OrderLineItem>()
                .HasKey(oli => new { oli.OrderId, oli.ProductId });
            modelBuilder.Entity<OrderLineItem>()
                .HasOne(oli => oli.Order)
                .WithMany(o => o.OrderLineItems)
                .HasForeignKey(oli => oli.OrderId);
            modelBuilder.Entity<OrderLineItem>()
                .HasOne(oli => oli.Product)
                .WithMany(o => o.OrdersWithProduct)
                .HasForeignKey(oli => oli.ProductId);
            #endregion


            #endregion





        }

    }
}
