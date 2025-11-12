using ProductManagement.App.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.App.Interfaces
{
    public interface IAssignedProductService
    {
        List<AssignedProductResponse> GetAllAssignedProducts();
        AssignedProductResponse AssignProductToCustomer(AssignProductRequest? assignProductRequest);
        AssignedProductResponse GetAssignedProductById(Guid assignedProductId);
        AssignedProductResponse DeleteAssignedProduct(Guid assignedProductId);
    }
}
