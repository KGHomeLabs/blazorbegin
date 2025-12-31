using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugin.EFCoreSQL
{
    public class InventoryEFCoreRepo : IInventoryRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFacatory;

        public InventoryEFCoreRepo(IDbContextFactory<IMSContext> contextFacatory)
        {
            this.contextFacatory = contextFacatory;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            using var db = contextFacatory.CreateDbContext();
            db.Inventories?.Add(inventory);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            using var context = contextFacatory.CreateDbContext();
            return await context.Inventories.Where(item => item.InventoryName.Contains(name)).ToListAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(int inventoryId)
        {
            using var db = contextFacatory.CreateDbContext();
            var inventory = await db.Inventories.FindAsync(inventoryId);
            return inventory;
        }

        public async Task RemoveAsync(int inventoryId)
        {
            using var db = contextFacatory?.CreateDbContext();
            var foundInventory= db.Inventories?.Find(inventoryId);
            if (foundInventory == null) return;
            db.Inventories?.Remove(foundInventory);
            await db.SaveChangesAsync();
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            using var db = contextFacatory.CreateDbContext();
            var itemToUpdate = await db.Inventories.FindAsync(inventory.InventoryId);
            if(itemToUpdate != null)
            {
                itemToUpdate.InventoryName = inventory.InventoryName;
                itemToUpdate.Price = inventory.Price;
                itemToUpdate.Quantity = inventory.Quantity;
                await db.SaveChangesAsync();
            }
        }
    }
}
