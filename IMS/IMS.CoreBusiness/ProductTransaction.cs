using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.CoreBusiness
{
    public class ProductTransaction
    {
        public int ProductTransactionId { get; set; }
        public string ProductionNumber { get; set; }

        public string SONumber { get; set; }
        [Required]
        public int ProductId { get; set; }
        public ProductTransactionType ActivityType { get; set; }
        [Required]
        public int QuantityBefore { get; set; }
        [Required]
        public int QuantityAfter { get; set; }
        public double? UnitPrice { get; set; }
        public string DoneBy { get; set; } = string.Empty;
        public DateTime TransDate { get; set; }

        public Product? Product { get; set; }
    }
}
