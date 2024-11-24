using FShop.Infrastructure.EventBus.Product;
using FShop.Infrastructure.Queries.Product;
using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Mvc;

namespace FShop.Api.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IBusControl _busControl;

        // interface representing a client component responsible for making requests to a server or another component in a system
        private IRequestClient<GetProductById> _requestClient;

        public ProductController(IBusControl busControl, RequestClient<GetProductById> request)
        {
            _requestClient = request;
            _busControl = busControl;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid ProductId)
        {
            var p = new GetProductById { ProductId = ProductId };
            var product = await _requestClient.GetResponse<ProductCreated>(p);

            return Accepted(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateProduct product)
        {
            Uri uri = new Uri("rabbitmq://localhost/create_product");
            var endpoint = await _busControl.GetSendEndpoint(uri);
            await endpoint.Send(product);

            return Accepted("Product created");
        }
    }
}
