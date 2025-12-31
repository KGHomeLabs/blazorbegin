using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Plugin.EFCoreSQL
{
    public class InventoryTransEFCoreRepo : IInventoryTransactionRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public InventoryTransEFCoreRepo(IDbContextFactory<IMSContext> contextFactory) 
        {
            this.contextFactory = contextFactory;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventroyTransactions(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            using var db = contextFactory.CreateDbContext();

            var query = from it in db.InventoryTransactions
                        join inv in db.Inventories on it.InventoryId equals inv.InventoryId
                        where (string.IsNullOrWhiteSpace(inventoryName) ||
                                inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                                &&
                                (!dateFrom.HasValue || it.TransDate >= dateFrom.Value.Date)
                                &&
                                (!dateTo.HasValue || it.TransDate <= dateTo.Value.Date)
                                &&
                                (!transactionType.HasValue || it.ActivityType == transactionType)
                        select it;

            return await query.Include(item=>item.Inventory).ToListAsync();
        }

        public void ProduceAsync(string productionNumber, Inventory inventory, int consumedQuantity, string doneBy, double price)
        {
            using var db = contextFactory.CreateDbContext();

            var newTransaction = new InventoryTransaction
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.ProduceProduct,
                QuantityAfter = inventory.Quantity - consumedQuantity,
                TransDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            };
            db.InventoryTransactions?.Add(newTransaction);
            db.SaveChanges();
            
        }

        public async void PurchaseAsync(string poNumber, Inventory inventory, string doneBy, int quantity, double price)
        {
            using var db = contextFactory.CreateDbContext();

            var newTransaction = new InventoryTransaction
            {
                PONumber = poNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            };

            db.InventoryTransactions?.Add(newTransaction); 
            db.SaveChangesAsync();

        }
    }
}
