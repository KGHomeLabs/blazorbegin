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
    }
}
