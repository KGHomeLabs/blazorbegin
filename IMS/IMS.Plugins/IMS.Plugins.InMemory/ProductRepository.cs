using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        private readonly IInventoryRepository inventoryRepo;

        public ProductRepository(IInventoryRepository inventoryRepo)
        {
            _products = new List<Product>()
            {
                new Product{ Id = 1, Name = "Bike", Price = 150.00, Quantity = 10 },
                new Product{ Id = 2, Name = "Car", Price = 20000.00, Quantity = 10 }
                
            };
            this.inventoryRepo = inventoryRepo;
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(item => item.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }
            var maxId = _products.Max(item => item.Id);
            product.Id = maxId + 1;
            _products.Add(product);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_products);
            }
            return _products.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var foundProduct = _products.FirstOrDefault(item => item.Id == productId);
            Product? newProd = null;
            if(foundProduct != null)
            {
                newProd = new Product();
                newProd.Id = productId;
                newProd.Name = foundProduct.Name;
                newProd.Price = foundProduct.Price;
                newProd.Quantity = foundProduct.Quantity;
                newProd.ProductInventories = new List<ProductInventory>();
                if(foundProduct.ProductInventories != null &&
                   foundProduct.ProductInventories.Count >0 )
                {
                    foreach(var inv in foundProduct.ProductInventories)
                    {
                        var newInv = new ProductInventory
                        {
                            InventoryId = inv.InventoryId,
                            ProductId = inv.ProductId,
                            Product = foundProduct,
                            Inventory = new Inventory(),
                            InventoryQuantity =inv.InventoryQuantity
                            
                        };
                        
                        if(inv.Inventory != null)
                        {
                            var currentInventory = await inventoryRepo.GetInventoryByIdAsync(inv.Inventory.InventoryId);
                            if (currentInventory != null)
                            {
                                newInv.Inventory.InventoryId = currentInventory.InventoryId;
                                newInv.Inventory.InventoryName = currentInventory.InventoryName;
                                newInv.Inventory.Price = currentInventory.Price;
                                newInv.Inventory.Quantity = currentInventory.Quantity;
                            }
                        }

                        newProd.ProductInventories.Add(newInv);
                    }
                }

            }
            return await Task.FromResult(newProd);
        }

        public Task RemoveAsync(int productId)
        {
            var foundProduct = _products.FirstOrDefault(item => item.Id == productId);
            if (foundProduct is not null)
            {
                _products.Remove(foundProduct);
            }
            return Task.CompletedTask;
        }

        public Task UpdateProductAsync(Product product)
        {
            var differentIDWithSameName = _products.Any(item => item.Id != product.Id
                                                    && item.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase));
            if (differentIDWithSameName)
                return Task.CompletedTask;


            var item = _products.FirstOrDefault(item => item.Id == product.Id);

            if (item != null)
            {
                item.Id = product.Id;
                item.Name = product.Name;
                item.Price = product.Price;
                item.Quantity = product.Quantity;
                item.ProductInventories = product.ProductInventories;
            }
            return Task.CompletedTask;
        }
    }
}
