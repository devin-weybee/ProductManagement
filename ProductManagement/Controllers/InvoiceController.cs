using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagement.App.DTO;
using ProductManagement.App.Interfaces;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IAssignedProductService _assignedProductService;
        private readonly static List<Invoice> _invoiceList = new List<Invoice>();

        public InvoiceController(ICustomerService customerService, IProductService productService, IAssignedProductService assignedProductService)
        {
            _customerService = customerService;
            _productService = productService;
            _assignedProductService = assignedProductService;
        }

        [Route("/invoices")]
        public IActionResult Index()
        {
            ViewBag.Invoices = _invoiceList;
            List<CustomerResponse> customerResponses = _customerService.GetSearchedCustomer(null);
            var Customers = customerResponses.Select(customerResponse => new SelectListItem()
            {
                Value = customerResponse.CustomerID.ToString(),
                Text = customerResponse.CustomerName
            }).ToList();

            var Products = new List<SelectListItem>()
            {
                new SelectListItem { Value = "" , Text = "-- Select Item --"}
            };

            InvoiceViewModel invoiceModel = new InvoiceViewModel() { customerResponse = Customers, assignedProductResponse = Products };

            return View(invoiceModel);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAssignedProduct(Guid? customerId)
        {
            List<AssignedProductResponse> assignedProductResponses = new List<AssignedProductResponse>();
            if (customerId != null)
            {
                assignedProductResponses = _assignedProductService.GetAllAssignedProducts()
                    .Where(apr => apr.CustomerID == customerId).ToList();
            }
            var Products = assignedProductResponses.Select(assignedProductResponse => new SelectListItem()
            {
                Value = assignedProductResponse.ProductID.ToString(),
                Text = assignedProductResponse.ProductName
            }).ToList();
            return Json(Products);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetPriceOfProduct(Guid? productId)
        {
            double? latestPrice = _productService.GetLatestPrice(productId);
            return Json(latestPrice);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddInvoice(InvoiceViewModel invoiceModel)
        {
            if (invoiceModel.CustomerID == Guid.Empty || invoiceModel.ProductID == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var customer = _customerService.GetCustomerById(invoiceModel.CustomerID);
            var product = _productService.GetProductById(invoiceModel.ProductID);

            var newInvoice = new Invoice
            {
                CustomerName = customer?.CustomerName,
                ProductName = product?.ProductName,
                Price = product.ProductRate,
                Quantity = invoiceModel.Quantity,
            };

            _invoiceList.Add(newInvoice);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ViewInvoices()
        {
            return View(_invoiceList);
        }
    }
}
