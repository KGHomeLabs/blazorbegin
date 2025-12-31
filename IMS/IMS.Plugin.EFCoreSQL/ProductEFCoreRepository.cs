using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Plugin.EFCoreSQL
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> dbfacotry;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> dbfacotry)
        {
            this.dbfacotry = dbfacotry;
        }

        public async Task AddProductAsync(Product product)
        {
            using var db = dbfacotry.CreateDbContext();
            db.Productes.Add(product);
            FlagInventoryUnchanged(product, db);
            await db.SaveChangesAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            using var db = dbfacotry.CreateDbContext();
            var product = await db.Productes.FindAsync(productId);

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var db = dbfacotry.CreateDbContext();
            return await db.Productes.Where(item => item.Name.Contains(name)).ToListAsync();
        }

        public async Task RemoveAsync(int productId)
        {
            using var db = dbfacotry.CreateDbContext();
            var removable = db.Productes?.FindAsync(productId);
            if (removable == null) return;
            db.Remove(removable);
            await db.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            using var db = dbfacotry.CreateDbContext();
            var changeProd = await db.Productes.FindAsync(product.Id);
            if (changeProd != null)
            {
                changeProd.Name = product.Name;
                changeProd.Price = product.Price;
                changeProd.Quantity = product.Quantity;
                changeProd.ProductInventories = product.ProductInventories;
                FlagInventoryUnchanged(product, db);
                await db.SaveChangesAsync();
            }
        }

        private void FlagInventoryUnchanged(Product product, IMSContext db)
        {
            if(product.ProductInventories != null &&
                product.ProductInventories.Count() > 0)
            {
                foreach(var prodInv in product.ProductInventories)
                {
                    if(prodInv!= null)
                    {
                        db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                    }
                }
            }
        }
    }
}
