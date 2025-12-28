using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        void PurchaseAsync(string poNumber, Inventory inventory, string doneBy1, int quantity, string doneBy2, double price);
        void ProduceAsync(string productionNumber, Inventory inventory,int consumedQuantity, string doneBy,double price);
    }
}
