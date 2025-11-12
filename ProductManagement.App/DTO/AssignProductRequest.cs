using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.App.DTO
{
    public class AssignProductRequest
    {
        public Guid AssignedProductID { get; set; }
        public Guid ProductID { get; set; }
        public Guid CustomerID { get; set; }

        public AssignedProduct ToAssignProductToCustomer()
        {
            return new AssignedProduct
            {
                AssignedProductID = Guid.NewGuid(),
                ProductID = this.ProductID,
                CustomerID = this.CustomerID
            };
        }
    }
}
