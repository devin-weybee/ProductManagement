using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.App.DTO
{
    public class CustomerAddRequest
    {
        [Required(ErrorMessage = "Customer name is required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Customer name must only contain letters and spaces (no numbers or special characters).")]
        [StringLength(100, ErrorMessage = "Customer name must be less than 100 characters.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Customer email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? CustomerEmail { get; set; }

        public Customer ToCustomer()
        {
            return new Customer
            {
                CustomerID = Guid.NewGuid(),
                CustomerName = CustomerName,
                CustomerEmail = CustomerEmail,
                ProductIDs = new List<Guid>()
            };
        }
    }
}