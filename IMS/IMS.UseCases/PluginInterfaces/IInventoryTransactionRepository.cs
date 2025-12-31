using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        void PurchaseAsync(string poNumber, Inventory inventory, string doneBy, int quantity, double price);
        void ProduceAsync(string productionNumber, Inventory inventory,int consumedQuantity, string doneBy,double price);
        Task<IEnumerable<InventoryTransaction>> GetInventroyTransactions(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType);   
    }
}
