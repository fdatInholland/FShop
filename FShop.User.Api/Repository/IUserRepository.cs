using FShop.Infrastructure.Command.User;
using FShop.Infrastructure.Event.User;

namespace FShop.User.Api.Repository
{
    public interface IUserRepository
    {
        Task<UserCreated> AddUser(CreateUser createUser);
        Task<UserCreated> GetUser(CreateUser createUser);
    }
}
