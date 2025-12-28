using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.CoreBusiness.Validation
{
    public class Product_EnsurePriceGreaterInventoryCost :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var product = validationContext.ObjectInstance as Product;
            if (product != null)
            {
                if (!ValidatePrice(product))
                {
                    double totalCost = TotalInentoryCost(product);
                    ValidationResult vr = new ValidationResult(
                        $"The product price is less than cost {totalCost}",
                        new List<string>() { validationContext.MemberName}
                        );
                    return vr;
                }
            }
            return ValidationResult.Success;
        }

        private double TotalInentoryCost(Product product) {
            if (product == null ||
                product.ProductInventories == null) 
            {
                return 0;
            }
         
            return product.ProductInventories.Sum(item => item.Inventory?.Price * item.InventoryQuantity ?? 0);

        }

        private bool ValidatePrice(Product product) {
            if (product == null ||
                product.ProductInventories.Count <0) {
                return true;
            }
            if (TotalInentoryCost(product) > product.Price) return false;
            return true;

        }
    }
}
