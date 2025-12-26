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

        public Task AddInventoryAsync(Inventory inventory)
        {
            if(_inventories.Any(item=>item.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }
            var maxId = _inventories.Max(item => item.InventoryId);
            inventory.InventoryId = maxId + 1;
            _inventories.Add(inventory);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_inventories);
            }
            return _inventories.Where(item=> item.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Inventory?> GetInventoryByIdAsync(int inventoryId)
        {
            var foundInventory = _inventories.FirstOrDefault(item => item.InventoryId == inventoryId);
            return await Task.FromResult(foundInventory);

            
        }

        public Task RemoveAsync(int inventoryId)
        {
          var foundInventory = _inventories.FirstOrDefault(item=>  item.InventoryId == inventoryId);
          if(foundInventory is not null)
            {
                _inventories.Remove(foundInventory);
            }
          return Task.CompletedTask;
        }

        public Task UpdateInventoryAsync(Inventory inventory)
        {
            var differentIDWithSameName = _inventories.Any(item => item.InventoryId != inventory.InventoryId
                                                    && item.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase));
           if(differentIDWithSameName)
                return Task.CompletedTask;  


            var item =_inventories.FirstOrDefault(item => item.InventoryId == inventory.InventoryId);

            if(item!=null)
            {
                item.InventoryId = inventory.InventoryId;
                item.InventoryName = inventory.InventoryName;
                item.Price = inventory.Price;
                item.Quantity = inventory.Quantity;
            }
            return Task.CompletedTask;
        }
    }
}
