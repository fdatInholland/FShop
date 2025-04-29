using FShop.Infrastructure.Command.User;
using FShop.Infrastructure.Queries.Product;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FShop.Api.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IPublishEndpoint _publish;
        private IRequestClient<GetProductById> _requestClient;

        public UsersController(IPublishEndpoint publish, IRequestClient<GetProductById> requestClient)
        {
            _publish = publish;
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateUser user)
        {
            Uri uri = new Uri("rabbitmq://localhost/add_user");
            await _publish.Publish<CreateUser>(user);

            return Accepted("User added");
        }
    }
}
