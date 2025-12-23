using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories;

        public InventoryRepository()
        {
            _inventories = new List<Inventory>()
            {
                new Inventory{ InventoryId = 1, InventoryName = "Bike Seat", Price = 2.00, Quantity = 10 },
                new Inventory{ InventoryId = 2, InventoryName = "Bike Body", Price = 15.00, Quantity = 10 },
                new Inventory{ InventoryId = 3, InventoryName = "Bike Wheels", Price = 8.00, Quantity = 20 },
                new Inventory{ InventoryId = 4, InventoryName = "Bike Pedals", Price = 1.00, Quantity = 20 },
            };
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_inventories);
            }
            return _inventories.Where(item=> item.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
