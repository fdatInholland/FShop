using FShop.Infrastructure.Command.User;
using FShop.Infrastructure.Event.User;
using FShop.User.Api.Repository;

namespace FShop.User.Api.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserCreated> AddUser(CreateUser createUser)
        {
            return await _userRepository.AddUser(createUser);
        }

        public async Task<UserCreated> GetUser(CreateUser createUser)
        {
            return await (_userRepository.GetUser(createUser));
        }
    }
}
