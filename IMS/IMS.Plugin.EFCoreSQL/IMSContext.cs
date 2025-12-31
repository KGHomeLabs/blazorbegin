using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Plugin.EFCoreSQL
{
    public class IMSContext :DbContext
    {
        public IMSContext(DbContextOptions<IMSContext> options):base(options)
        {
            
        }
        public DbSet<Inventory>? Inventories { get; set; }
        public DbSet<Product>? Productes { get; set; }
        
        public DbSet<ProductInventory>? ProductInventories { get; set; }
        public DbSet<InventoryTransaction>? InventoryTransactions { get; set; }
        public DbSet<ProductTransaction>? ProductTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInventory>()
                .HasKey(pk => new { pk.ProductId, pk.InventoryId });

            modelBuilder.Entity<ProductInventory>()
                .HasOne(pi => pi.Product)
                .WithMany(pi => pi.ProductInventories)
                .HasForeignKey(fk=>fk.ProductId);

            modelBuilder.Entity<ProductInventory>()
                .HasOne(pi=>pi.Inventory)
                .WithMany(pi=>pi.ProductInventories)
                .HasForeignKey(fk=>fk.InventoryId);

            modelBuilder.Entity<Inventory>().HasData(
                 new Inventory { InventoryId = 1, InventoryName = "Bike Seat", Price = 2.00, Quantity = 10 },
                new Inventory { InventoryId = 2, InventoryName = "Bike Body", Price = 15.00, Quantity = 10 },
                new Inventory { InventoryId = 3, InventoryName = "Bike Wheels", Price = 8.00, Quantity = 20 },
                new Inventory { InventoryId = 4, InventoryName = "Bike Pedals", Price = 1.00, Quantity = 20 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Bike", Price = 150.00, Quantity = 10 },
                new Product { Id = 2, Name = "Car", Price = 20000.00, Quantity = 10 }
                );

            modelBuilder.Entity<ProductInventory>().HasData(
                new ProductInventory { ProductId = 1, InventoryId = 1, InventoryQuantity = 1 },
                new ProductInventory { ProductId = 1, InventoryId = 2, InventoryQuantity = 1 },
                new ProductInventory { ProductId = 1, InventoryId = 3, InventoryQuantity = 2 },
                new ProductInventory { ProductId = 1, InventoryId = 4, InventoryQuantity = 2 }
                );
        }
    }
}
