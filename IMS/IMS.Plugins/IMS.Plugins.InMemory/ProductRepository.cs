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
        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product{ Id = 1, Name = "Bike", Price = 150.00, Quantity = 10 },
                new Product{ Id = 2, Name = "Car", Price = 20000.00, Quantity = 10 }
                
            };
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
            return await Task.FromResult(foundProduct);
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
            }
            return Task.CompletedTask;
        }
    }
}
