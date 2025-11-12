using ProductManagement.App.DTO;
using ProductManagement.App.DTO;
using ProductManagement.Domain.Entities;

namespace ProductManagement.App.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerResponse> GetAllCustomers();
        CustomerResponse AddCustomer(CustomerAddRequest? customerAddRequest);
        CustomerResponse GetCustomerById(Guid customerId);
        CustomerResponse UpdateCustomer(CustomerUpdateRequest? customerUpdateRequest);
        CustomerResponse DeleteCustomer(Guid customerId);
        List<CustomerResponse> GetSearchedCustomer(object value);
    }
}
