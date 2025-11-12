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
    public class AssignedProductService : IAssignedProductService
    {
        //private readonly List<AssignedProduct> _assignedProductsList;
        private readonly ApplicationDBContext _dbContext;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public AssignedProductService(IProductService productService, ICustomerService customerService, ApplicationDBContext applicationDBContext)
        {
            //_assignedProductsList = new List<AssignedProduct>();
            _dbContext = applicationDBContext;
            _productService = productService;
            _customerService = customerService;
        }

        public List<AssignedProductResponse> GetAllAssignedProducts()
        {
            var assignedProducts = _dbContext.AssignedProduct.ToList(); // Fetch from DB first

            return assignedProducts.Select(temp => new AssignedProductResponse
            {
                AssignedProductID = temp.AssignedProductID,
                CustomerID = temp.CustomerID,
                ProductID = temp.ProductID,
                CustomerName = _customerService.GetCustomerById(temp.CustomerID)?.CustomerName,
                ProductName = _productService.GetProductById(temp.ProductID)?.ProductName
            }).ToList();
        }


        public AssignedProductResponse AssignProductToCustomer(AssignProductRequest? assignProductRequest)
        {
            if (assignProductRequest == null)
            {
                throw new ArgumentNullException(nameof(assignProductRequest));
            }
            var assignedProduct = assignProductRequest.ToAssignProductToCustomer();

            _dbContext.AssignedProduct.Add(assignedProduct);
            _dbContext.SaveChanges();

            var customerName = _customerService.GetCustomerById(assignedProduct.CustomerID)?.CustomerName;
            var productName = _productService.GetProductById(assignedProduct.ProductID)?.ProductName;
            return assignedProduct.ToAssignedProductResponse(customerName, productName);
        }

        public AssignedProductResponse DeleteAssignedProduct(Guid assignedProductId)
        {
            var assignedProduct = _dbContext.AssignedProduct.Find(assignedProductId);
            if (assignedProduct == null)
            {
                throw new KeyNotFoundException("Assigned product not found.");
            }
            _dbContext.AssignedProduct.Remove(assignedProduct);
            _dbContext.SaveChanges();

            var customerName = _customerService.GetCustomerById(assignedProduct.CustomerID)?.CustomerName;
            var productName = _productService.GetProductById(assignedProduct.ProductID)?.ProductName;
            return assignedProduct.ToAssignedProductResponse(customerName, productName);
        }

        public AssignedProductResponse GetAssignedProductById(Guid assignedProductId)
        {
            var assignedProduct = _dbContext.AssignedProduct.Find(assignedProductId);
            
            return assignedProduct.ToAssignedProductResponse(
                _customerService.GetCustomerById(assignedProduct.CustomerID)?.CustomerName,
                _productService.GetProductById(assignedProduct.ProductID)?.ProductName);
        }
    }
}
