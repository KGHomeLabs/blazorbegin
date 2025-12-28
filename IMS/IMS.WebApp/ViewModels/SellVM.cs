using IMS.CoreBusiness;
using IMS.WebApp.ViewModelValidation;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class SellVM
    {
        [Required]
        public string SalesOrderNumber { get; set; } = string.Empty;

        public int ProductId { get; set; }
        [Sell_EnsureEnoughProducts]
        public int QuantityToSell { get; set; }
        public double UnitPrice { get; set; }
        public Product? Product { get; set; }
    }
}
