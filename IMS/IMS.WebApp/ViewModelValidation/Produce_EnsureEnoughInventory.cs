using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelValidation
{
    public class Produce_EnsureEnoughInventory : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ProduceVM produceVM= validationContext.ObjectInstance as ProduceVM;
            if (produceVM != null)
            {
                if (produceVM.Product != null && produceVM.Product.ProductInventories != null)
                {
                    foreach(var pi in produceVM.Product.ProductInventories)
                    {
                        if(pi.Inventory != null &&
                            pi.InventoryQuantity *produceVM.QuantityToProduce > pi.Inventory.Quantity)
                        {
                            return new ValidationResult($"{pi.Inventory.InventoryName} has too little quantiy for {produceVM.QuantityToProduce} products",
                                new[] {validationContext.MemberName});
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
