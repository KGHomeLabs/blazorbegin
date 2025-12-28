using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class PurchaseVM
    {
        [Required]
        public string PONumber { get; set; } = string.Empty;
        [Range(minimum:1, maximum:int.MaxValue,ErrorMessage ="One Inventory item minimum")]
        public int InventoryId { get; set; }
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Minimum Quantity 1")]
        public int QuantityToPurchose { get; set; }

        public double InventoryPrice { get; set; }
    }
}
