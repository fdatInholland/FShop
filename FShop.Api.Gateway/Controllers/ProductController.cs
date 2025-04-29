using FShop.Infrastructure.EventBus.Product;
using FShop.Infrastructure.Queries.Product;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FShop.Api.Gateway.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IPublishEndpoint _publish;
        private IRequestClient<GetProductById> _requestClient;

        public ProductController(IPublishEndpoint endPoint, IRequestClient<GetProductById> requestClient)
        {
            _publish = endPoint;
           _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductByID(string ProductId)
        {
            if (ProductId == null)
            {
                return BadRequest("Product Id is required");
            }
            try
            {
                var p = new GetProductById { ProductId = ProductId };
                var response = await _requestClient.GetResponse<ProductCreated>(p, timeout: TimeSpan.FromSeconds(2));
                return Ok(response);
            }
            catch (RequestTimeoutException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateProduct product)
        {
            //TODO - remove hardcoded values
            //Try catch stuff
            CreateProduct p = new CreateProduct()
            {
                ProductId = "666123",
                ProductName = "Test",
                ProductDescription = "sample Product",
                CategoryId = Guid.NewGuid(),
            };

            //TODO - remove hardcoded values
            Uri uri = new Uri("rabbitmq://localhost/create_product");
            await _publish.Publish<CreateProduct>(p);

            return Accepted("Product created");
        }
    }
}
