using System;
using System.Collections.Generic;

namespace ProductManagement.App.DTO
{
    public class CustomerResponse
    {
        public Guid CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public List<Guid>? ProductIDs { get; set; }

        public CustomerUpdateRequest ToCustomerUpdateRequest()
        {
            return new CustomerUpdateRequest()
            {
                CustomerID = this.CustomerID,
                CustomerName = this.CustomerName,
                CustomerEmail = this.CustomerEmail
            };
        }
    }

    public static class CustomerResponseExtension
    {
        public static CustomerResponse ToCustomerResponse(this Domain.Entities.Customer customer)
        {
            return new CustomerResponse()
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                CustomerEmail = customer.CustomerEmail,
                ProductIDs = customer.ProductIDs
            };
        } 
    }
}
