using System;
using System.Collections.Generic;

namespace ProductManagement.Domain.Entities
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string? ProductName { get; set; }
        public List<Double>? ProductRates { get; set; }
    }
}
