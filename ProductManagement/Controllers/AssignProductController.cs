using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagement.App.DTO;
using ProductManagement.App.Interfaces;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class AssignProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IAssignedProductService _assignedProductService;
        
        public AssignProductController(IProductService productService, ICustomerService customerService, IAssignedProductService assignedProductService)
        {
            _productService = productService;
            _customerService = customerService;
            _assignedProductService = assignedProductService;
        }

        [Route("/assignproducts")]
        public IActionResult Index()
        {
            var assignedProducts = _assignedProductService.GetAllAssignedProducts();
            return View(assignedProducts);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult AssignProductToCustomer()
        {
            var products = _productService.GetAllProducts();
            ViewBag.Products = products.Select(p => new SelectListItem { 
                Value = p.ProductID.ToString(), 
                Text = p.ProductName 
            }).ToList();

            var customers = _customerService.GetAllCustomers();
            ViewBag.Customers = customers.Select(c => new SelectListItem { 
                Value = c.CustomerID.ToString(), 
                Text = c.CustomerName 
            }).ToList();

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AssignProductToCustomer(AssignProductRequest assignProductRequest)
        {
            if (assignProductRequest == null)
            {
                return BadRequest();
            }
            var assignedProductResponse = _assignedProductService.AssignProductToCustomer(assignProductRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult DeleteAssignedProduct(Guid assignedProductId)
        {
            var assignedProduct = _assignedProductService.GetAssignedProductById(assignedProductId);
            return View(assignedProduct);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DeleteAssignedProductConfirmed(Guid assignedProductId)
        {
            var deletedAssignedProduct = _assignedProductService.DeleteAssignedProduct(assignedProductId);
            return RedirectToAction("Index");
        }
    }
}