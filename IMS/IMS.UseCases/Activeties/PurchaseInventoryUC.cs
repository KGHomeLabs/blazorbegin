using IMS.CoreBusiness;
using IMS.UseCases.Activeties.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Activeties
{
    public class PurchaseInventoryUC : IPurchaseInventoryUC
    {
        private readonly IInventoryTransactionRepository transactionRepo;
        private readonly IInventoryRepository inventoryRepo;

        public PurchaseInventoryUC(IInventoryTransactionRepository transactionRepo,
                                    IInventoryRepository inventoryRepo)
        {
            this.transactionRepo = transactionRepo;
            this.inventoryRepo = inventoryRepo;
        }

        public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
        {
            //insert a record in the transaction table
            transactionRepo.PurchaseAsync(poNumber, inventory, doneBy, quantity, doneBy, inventory.Price);
            //increase quaantity 
            inventory.Quantity += quantity;
            await inventoryRepo.UpdateInventoryAsync(inventory);
        }
    }
}
