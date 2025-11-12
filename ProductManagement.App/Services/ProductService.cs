using Microsoft.EntityFrameworkCore.Metadata;
using ProductManagement.App.DTO;
using ProductManagement.App.Interfaces;
using ProductManagement.Data;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.App.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext _dbContext;
        //private readonly List<Product>? _productList;
        public ProductService(ApplicationDBContext applicationDBContext)
        {
            _dbContext = applicationDBContext;
            //_productList = new List<Product>();
        }

        public List<ProductResponse> GetAllProducts()
        {
            return _dbContext.Products.Select(p => p.ToProductResponse()).ToList();
        }

        public ProductResponse AddProduct(ProductAddRequest? productAddRequest)
        {
            if (productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }
            var product = productAddRequest.ToProduct();

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return product.ToProductResponse();
        }

        public ProductResponse GetProductById(Guid productId)
        {
            var product = _dbContext.Products.Find(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            return product.ToProductResponse();
        }

        public ProductResponse UpdateProduct(ProductUpdateRequest? productUpdateRequest)
        {
            if (productUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(CustomerUpdateRequest));
            }

            var product = _dbContext.Products.Find(productUpdateRequest.ProductID);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            product.ProductName = productUpdateRequest.ProductName;

            product.ProductRates!.Add(productUpdateRequest.ProductRate);
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return product.ToProductResponse();
        }

        public ProductResponse DeleteProduct(Guid productId)
        {
            var product = _dbContext.Products.Find(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            var assignedProducts = _dbContext.AssignedProduct.Where(ap => ap.ProductID == productId).ToList();
            _dbContext.AssignedProduct.RemoveRange(assignedProducts);

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return product.ToProductResponse();
        }

        public double? GetLatestPrice(Guid? productId)
        {
            var product = _dbContext.Products.Find(productId);
            if (product == null || product.ProductRates == null || !product.ProductRates.Any())
            {
                return null;
            }

            return product.ProductRates.Last();
        }
    }
}
