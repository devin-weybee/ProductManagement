using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ProductManagement.App.DTO
{
    public class ProductUpdateRequest
    {
        public Guid ProductID { get; set; }
        public string? ProductName { get; set; }
        public double ProductRate { get; set; }

        public Product ToProduct()
        {
            return new Product
            {
                ProductID = this.ProductID,
                ProductName = this.ProductName,
                ProductRates = new List<double> { this.ProductRate }
            };
        }
    }
}
