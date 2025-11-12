using ProductManagement.App.DTO;
using ProductManagement.App.Interfaces;
using ProductManagement.Data;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ProductManagement.App.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDBContext _dbContext;
        //private readonly List<Customer> _customerList;

        public CustomerService(ApplicationDBContext applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }

        public List<CustomerResponse> GetAllCustomers()
        {
            return _dbContext.sp_GetAllCustomers().Select(c => c.ToCustomerResponse()).ToList();
        }
        public CustomerResponse AddCustomer(CustomerAddRequest? customerAddRequest)
        {
            if (customerAddRequest == null)
            {
                throw new ArgumentNullException(nameof(customerAddRequest));
            }

            var customer = customerAddRequest.ToCustomer();
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();
            //_customerList.Add(customer);
            return customer.ToCustomerResponse();
        }

        public CustomerResponse GetCustomerById(Guid customerId)
        {
            var customer = _dbContext.Customers.Find(customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }
            return customer.ToCustomerResponse();
        }

        public CustomerResponse UpdateCustomer(CustomerUpdateRequest? customerUpdateRequest)
        {
            if (customerUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(customerUpdateRequest));
            }
            var customer = _dbContext.Customers.Find(customerUpdateRequest.CustomerID);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }
            customer.CustomerName = customerUpdateRequest.CustomerName;
            customer.CustomerEmail = customerUpdateRequest.CustomerEmail;
            return customer.ToCustomerResponse();
        }

        public CustomerResponse DeleteCustomer(Guid customerId)
        {
            var customer = _dbContext.Customers.Find(customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            var assignedProducts = _dbContext.AssignedProduct.Where(ap => ap.CustomerID == customerId).ToList();
            _dbContext.AssignedProduct.RemoveRange(assignedProducts);

            _dbContext.Customers.Remove(customer);
            _dbContext.SaveChanges();
            return customer.ToCustomerResponse();
        }

        public List<CustomerResponse> GetSearchedCustomer(object value)
        {
            // For demonstration, returning all customers as search functionality is not implemented
            return GetAllCustomers();
        }
    }
}
