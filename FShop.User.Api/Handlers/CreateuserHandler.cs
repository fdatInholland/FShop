using FShop.Infrastructure.Command.User;
using FShop.User.DataProvider.Services;
using MassTransit;

namespace FShop.User.Api.Handlers
{
    public class CreateuserHandler : IConsumer<CreateUser>
    {
        private readonly IUserService _userService;

        public CreateuserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<CreateUser> context)
        {
            var createdUser = await _userService.AddUser(context.Message);
        }
    }
}
