using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.CoreBusiness
{
    public class InventoryTransaction
    {
        public int InventoryTransactionId { get; set; }
        public string PONumber { get; set; }
        public string ProductionNumber { get; set; }
        [Required]
        public int InventoryId { get; set; }
        public InventoryTransactionType ActivityType { get; set; }
        [Required]
        public int QuantityBefore { get; set; }
        [Required]
        public int QuantityAfter { get; set; }
        public double UnitPrice { get; set; }
        public string DoneBy { get; set; } = string.Empty;
        public DateTime TransDate { get; set; }

        public Inventory? Inventory { get; set; }    
    }
}
