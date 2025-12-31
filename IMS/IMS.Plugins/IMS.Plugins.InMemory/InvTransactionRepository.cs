using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Plugins.InMemory
{
    public class InvTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IInventoryRepository invRepo;
        public List<InventoryTransaction> _transactions = new List<InventoryTransaction>();


        public InvTransactionRepository(IInventoryRepository invRepo)
        {
            this.invRepo = invRepo;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventroyTransactions(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            var inventories = (await invRepo.GetInventoriesByNameAsync(string.Empty)).ToList();

            var query = from it in _transactions
                        join inv in inventories on it.InventoryId equals inv.InventoryId
                        where (string.IsNullOrWhiteSpace(inventoryName) ||
                                inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                                &&
                                (!dateFrom.HasValue || it.TransDate >= dateFrom.Value.Date)
                                &&
                                (!dateTo.HasValue || it.TransDate <= dateTo.Value.Date)
                                &&
                                (!transactionType.HasValue || it.ActivityType == transactionType)
                        select new InventoryTransaction
                        {
                            Inventory = inv, 
                            InventoryTransactionId= it.InventoryTransactionId,
                            PONumber = it.PONumber,
                            InventoryId = it.InventoryId,
                            QuantityBefore = it.QuantityBefore,
                            ActivityType = it.ActivityType,
                            QuantityAfter = it.QuantityAfter,
                            TransDate = it.TransDate,
                            DoneBy = it.DoneBy,
                            UnitPrice = it.UnitPrice
                        };

            return query;
        }

     

        public void ProduceAsync(string productionNumber, Inventory inventory, int consumedQuantity, string doneBy, double price)
        {
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

            _transactions.Add(newTransaction);
        }

        public void PurchaseAsync(string poNumber, Inventory inventory, string doneBy, int quantity, double price)
        {
            var newTransaction = new InventoryTransaction
            {
               PONumber =poNumber,
               InventoryId = inventory.InventoryId,
               QuantityBefore=inventory.Quantity,
               ActivityType = InventoryTransactionType.PurchaseInventory,
               QuantityAfter= inventory.Quantity + quantity,
               TransDate = DateTime.Now,
               DoneBy = doneBy,
               UnitPrice = price
            };

            this._transactions.Add(newTransaction);
        }
    }
}
