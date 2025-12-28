using IMS.CoreBusiness;
using IMS.WebApp.ViewModelValidation;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class ProduceVM
    {
        [Required]
        public string ProductionNumber { get; set; } = string.Empty;
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "One Product item minimum")]
        public int ProductId { get; set; }

        [Produce_EnsureEnoughInventory]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Minimum Quantity 1")]
        public int QuantityToProduce { get; set; }

        public double Price { get; set; }

        public Product? Product { get; set; } = null;
    }
}
