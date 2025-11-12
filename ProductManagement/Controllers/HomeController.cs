using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.App.DTO;
using ProductManagement.App.Interfaces;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;
        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index(string? searchTerm)
        {
            var customers = _customerService.GetAllCustomers();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                customers = customers
                    .Where(c => c.CustomerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(customers);
        }


        [HttpGet]
        [Route("[action]")]
        [Route("/add/customer")]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        [Route("/add/customer")]
        public IActionResult AddCustomer(CustomerAddRequest? customerAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(customerAddRequest);
            }
            var customerResponse = _customerService.AddCustomer(customerAddRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{customerId}")]
        public IActionResult EditCustomer(Guid customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);

            return View(customer.ToCustomerUpdateRequest());
        }

        [HttpPost]
        [Route("[action]/{customerId}")]
        public IActionResult EditCustomer(CustomerUpdateRequest? customerUpdateRequest)
        {
            if (customerUpdateRequest == null)
            {
                return BadRequest();
            }
            var customerResponse = _customerService.UpdateCustomer(customerUpdateRequest);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{customerId}")]
        public IActionResult DeleteCustomer(Guid customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [Route("[action]/{customerId}")]
        public IActionResult DeleteCustomerConfirmed(Guid customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            var customerResponse = _customerService.DeleteCustomer(customerId);

            return RedirectToAction("Index");
        }
    }
}
