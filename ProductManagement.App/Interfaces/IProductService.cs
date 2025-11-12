using ProductManagement.App.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.App.Interfaces
{
    public interface IProductService
    {
        List<ProductResponse> GetAllProducts();
        ProductResponse AddProduct(ProductAddRequest? productAddRequest);
        ProductResponse GetProductById(Guid productId);
        ProductResponse UpdateProduct(ProductUpdateRequest? productUpdateRequest);
        ProductResponse DeleteProduct(Guid productId);
        double? GetLatestPrice(Guid? productId);
    }
}
