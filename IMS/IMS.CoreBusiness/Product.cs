using IMS.CoreBusiness.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Heisa Hopsassa")]
        [StringLength(120)]
        public string Name { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "Name must be there bruh")]
        public int Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Must be larger than 0")]
        public double Price { get; set; }
        [Product_EnsurePriceGreaterInventoryCost]
        public List<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();

        public void AddInventory(Inventory inventory)
        {

            bool containsName = this.ProductInventories
                                       .Any(item => item.Inventory != null &&
                                                item.Inventory.InventoryName.Equals(inventory.InventoryName));

            if (!containsName)
            {
                this.ProductInventories.Add(
                    new ProductInventory
                    {
                        InventoryId = inventory.InventoryId,
                        Inventory = inventory,
                        InventoryQuantity = 1,
                        ProductId = this.Id,
                        Product = this
                    }
                    );
            }
        }
        public void RemoveInventory(ProductInventory pInventory)
        {
            this.ProductInventories?.Remove(pInventory);
        }
    }
}
