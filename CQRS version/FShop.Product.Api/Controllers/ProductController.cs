﻿using FShop.Infrastructure.EventBus.Product;
using FShop.Product.DataProvider;
using Microsoft.AspNetCore.Mvc;

namespace FShop.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _Service { get; set; }
        
        public ProductController(IProductService productService)
        {
                _Service = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var prod = await _Service.GetProduct(id);
            return Ok(prod);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateProduct product)
        {
            var addedprod = await _Service.AddProduct(product);
            return Ok(addedprod);
        }
    }
}
