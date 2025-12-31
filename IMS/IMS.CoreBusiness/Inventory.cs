using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        [Required(ErrorMessage ="Heisa Hopsassa")]
        [StringLength(120)]
        public string InventoryName { get; set; } = string.Empty;
        [Range(0, int.MaxValue,ErrorMessage = "Name must be there bruh")]
        public int Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage ="Must be larger than 0")]
        public double Price { get; set; }

        public List<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();
    }
}
