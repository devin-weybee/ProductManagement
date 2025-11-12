using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Entities
{
    public class Customer
    {
        public Guid CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public List<Guid>? ProductIDs { get; set; }
    }
}
