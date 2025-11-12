using System;
using System.Collections.Generic;
using ProductManagement.Domain.Entities;

namespace ProductManagement.App.DTO
{
    public class AssignedProductResponse
    {
        public Guid AssignedProductID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid ProductID { get; set; }
        public string? CustomerName { get; set; }
        public string? ProductName { get; set; }

    }

    public static class AssignedProductResponseExtensions
    {
        public static AssignedProductResponse ToAssignedProductResponse(this AssignedProduct assignedProduct,string customerName,string productName)
        {
            return new AssignedProductResponse
            {
                AssignedProductID = assignedProduct.AssignedProductID,
                CustomerID = assignedProduct.CustomerID,
                ProductID = assignedProduct.ProductID,
                CustomerName = customerName,
                ProductName = productName
            };
        }
    }
}