using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.App.DTO
{
    public class ProductAddRequest
    {
        [Required(ErrorMessage = "Product name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Product name cannot contain special characters.")]
        [StringLength(100, ErrorMessage = "Product name must be under 100 characters.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Initial price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double ProductRate { get; set; }
        public Product ToProduct()
        {
            return new Product
            {
                ProductID = Guid.NewGuid(),
                ProductName = ProductName,
                ProductRates = new List<double> { ProductRate }
            };
        }
    }
}
