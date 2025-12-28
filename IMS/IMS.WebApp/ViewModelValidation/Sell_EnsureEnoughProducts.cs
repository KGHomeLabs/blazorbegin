using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelValidation
{
    public class Sell_EnsureEnoughProducts :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var sellVM= validationContext.ObjectInstance as SellVM;
            if (sellVM != null)
            {
                if(sellVM.Product?.Quantity < sellVM.QuantityToSell)
                {
                    return new ValidationResult($"Only {sellVM.Product.Quantity} left!",
                        new[] { validationContext.MemberName });
                }
            }
            return ValidationResult.Success;
        }
    }
}
