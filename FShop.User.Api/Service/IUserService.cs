using FShop.Infrastructure.Command.User;
using FShop.Infrastructure.Event.User;

namespace FShop.User.Api.Service
{
    public interface IUserService
    {
        Task<UserCreated> AddUser(CreateUser createUser);
        Task<UserCreated> GetUser(CreateUser createUser);
    }
}
