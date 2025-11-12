using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.App.DTO
{
    public class ProductResponse
    {
        public Guid ProductID { get; set; }
        public string? ProductName { get; set; }
        public double ProductRate { get; set; }
        public List<double>? ProductRates { get; set; }
        public ProductUpdateRequest ToProductUpdateRequest()
        {
            return new ProductUpdateRequest()
            {
                ProductID = this.ProductID,
                ProductName = this.ProductName,
                ProductRate = this.ProductRate
            };
        }
    }

    public static class ProductResponseExtensions
    {
        public static ProductResponse ToProductResponse(this Domain.Entities.Product product)
        {
            return new ProductResponse
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductRate = product.ProductRates.Last(),
                ProductRates = product.ProductRates
            };
        }
    }
}
