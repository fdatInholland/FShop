using FShop.Infrastructure.EventBus.Product;
using FShop.Product.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop.Product.Api.Controllers
{
    [Route("api/products)")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _Service { get; set; }
        
        public ProductsController(IProductService productService)
        {
                _Service = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct(string productid = "P12345")
        {
            ProductCreated product = await _Service.GetProductById(productid);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateProduct product)
        {
            CreateProduct p = new CreateProduct()
            {
                ProductId = "1",
                ProductName = "Test",
                ProductDescription = "sample Product",
                CategoryId = Guid.NewGuid(),
            };
            var addedprod = await _Service.AddProduct(p);
            return Ok(addedprod);
        }
    }
}
