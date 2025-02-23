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
        public async Task<IActionResult> GetProductByID(Guid ProductId)
        { 
            var p = new GetProductById { ProductId = ProductId};
            var product = await _requestClient.GetResponse<ProductCreated>(p);
            return Accepted(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateProduct product)
        {
            CreateProduct p = new CreateProduct()
            {
                ProductId = "12345",
                ProductName = "Test",
                ProductDescription = "sample Product",
                CategoryId = Guid.NewGuid(),
            };

            Uri uri = new Uri("rabbitmq://localhost/create_product");
            //var endpoint = await _publishEndpoint.Publish(p);
            //    _bus.GetSendEndpoint(uri);
            //await endpoint.Send(p);
            _publish.Publish<CreateProduct>(p);

            return Accepted("Product created");
        }
    }
}
