using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryRepository
    {
        Task AddInventoryAsync(Inventory inventory);
        Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);
        Task<Inventory?> GetInventoryByIdAsync(int inventoryId);
        Task RemoveAsync(int inventoryId);
        Task UpdateInventoryAsync(Inventory inventory);
    }
}
