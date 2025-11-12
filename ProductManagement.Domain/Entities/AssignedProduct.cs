using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Entities
{
    public class AssignedProduct
    {
        public Guid AssignedProductID { get; set; }
        public Guid ProductID { get; set; }
        public Guid CustomerID { get; set; }
    }
}
