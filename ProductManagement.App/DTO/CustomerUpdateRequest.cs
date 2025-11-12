using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.App.DTO
{
    public class CustomerUpdateRequest
    {
        public Guid CustomerID { get; set; }
        public string? CustomerName { get; set; }
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
