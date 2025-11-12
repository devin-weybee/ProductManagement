using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.App.DTO;
using ProductManagement.App.Interfaces;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("/products")]
        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddProduct(ProductAddRequest? productAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var productResponse = _productService.AddProduct(productAddRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult EditProduct(Guid productId)
        {
            var product = _productService.GetProductById(productId);
            return View(product.ToProductUpdateRequest());
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult EditProduct(ProductUpdateRequest? productUpdateRequest)
        {
            if (productUpdateRequest == null)
            {
                return BadRequest();
            }
            var productResponse = _productService.UpdateProduct(productUpdateRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{productId}")]
        public IActionResult DeleteProduct(Guid productId)
        {
            var product = _productService.GetProductById(productId);
            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [Route("[action]/{productId}")]
        public IActionResult DeleteProductConfirmed(Guid productId)
        {
            _productService.DeleteProduct(productId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ViewRateHistory(Guid productId)                                                                                                    
        {
            var product = _productService.GetProductById(productId);
            return View(product);
        }
    }
}
