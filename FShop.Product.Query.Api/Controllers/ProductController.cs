using FShop.Product.DataProvider;
using Microsoft.AspNetCore.Mvc;

namespace FShop.Product.Query.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _productService.GetProductByID(id);
            return Ok(result);
        }
    }
}
