using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Plugins.InMemory
{
    public class InvTransactionRepository : IInventoryTransactionRepository
    {
        public List<InventoryTransaction> _transactions = new List<InventoryTransaction>();

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
        }

        public void PurchaseAsync(string poNumber, Inventory inventory, string doneBy, int quantity, string doneBy2, double price)
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
