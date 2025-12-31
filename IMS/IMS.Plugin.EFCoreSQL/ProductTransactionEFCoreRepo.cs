using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Plugin.EFCoreSQL
{
    public class ProductTransactionEFCoreRepo :IProductTransactionRepo
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        private readonly IProductRepository productRepo;
        private readonly IInventoryTransactionRepository invTransactionRepo;
        private readonly IInventoryRepository invRepo;

        public ProductTransactionEFCoreRepo(
            IDbContextFactory<IMSContext> contextFactory,
            IProductRepository productRepo, 
            IInventoryTransactionRepository invTransactionRepo, 
            IInventoryRepository invRepo)
        {
            this.contextFactory = contextFactory;
            this.productRepo = productRepo;
            this.invTransactionRepo = invTransactionRepo;
            this.invRepo = invRepo;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactions(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            using var db = contextFactory.CreateDbContext();           

            var query = from it in db.ProductTransactions
                        join inv in db.Productes on it.ProductId equals inv.Id
                        where (string.IsNullOrWhiteSpace(productName)
                                ||
                                inv.Name.ToLower().IndexOf(productName.ToLower()) >= 0)
                                &&
                                (!dateFrom.HasValue || it.TransDate >= dateFrom.Value.Date)
                                &&
                                (!dateTo.HasValue || it.TransDate <= dateTo.Value.Date)
                                &&
                                (!transactionType.HasValue || it.ActivityType == transactionType)
                        select it;
            var result = await query.Include(item => item.Product).ToListAsync();
            return result;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            using var db = contextFactory.CreateDbContext();

            var prod = await this.productRepo.GetProductByIdAsync(product.Id);

            //add inventory transaction
            if (prod != null)
            {
                foreach (var pi in prod.ProductInventories)
                {
                    if (pi.Inventory != null)
                    {
                        this.invTransactionRepo.ProduceAsync(productionNumber,
                            pi.Inventory,
                            pi.InventoryQuantity * quantity,
                            doneBy,
                            -1);
                        var inv = await invRepo.GetInventoryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;
                        await invRepo.UpdateInventoryAsync(inv);

                    }

                }
            }
            //add product transaction
            var newProductTransaction = new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.Id,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity + quantity,
                TransDate = DateTime.Now,
                DoneBy = doneBy

            };
            db.ProductTransactions?.Add(newProductTransaction);
            await db.SaveChangesAsync();
        }

        public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            using var db = contextFactory.CreateDbContext();

            var newTransaction = new ProductTransaction();
            newTransaction.ActivityType = ProductTransactionType.SellProduct;
            newTransaction.SONumber = salesOrderNumber;
            newTransaction.ProductId = product.Id;
            newTransaction.QuantityBefore = product.Quantity;
            newTransaction.QuantityAfter = product.Quantity - quantity;
            newTransaction.TransDate = DateTime.Now;
            newTransaction.DoneBy = doneBy;
            newTransaction.UnitPrice = unitPrice;

            db.ProductTransactions?.Add(newTransaction);
            await db.SaveChangesAsync();     
        }
    }
}
