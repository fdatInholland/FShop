using FShop.Infrastructure.Command.User;
using FShop.Infrastructure.Event.User;
using MongoDB.Driver;

namespace FShop.User.Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private IMongoDatabase _database;

        private IMongoCollection<CreateUser> _collection => _database.GetCollection<CreateUser>("user", null);

        public UserRepository(IMongoDatabase mongoDatabase)
        {
            _database = mongoDatabase;
        }

        public async Task<UserCreated> AddUser(CreateUser createUser)
        {
            await _collection.InsertOneAsync(createUser);
            return new UserCreated
            {
                ContactNo = createUser.ContactNo,
                Email = createUser.Email,
                Name = createUser.Name,
                Password = createUser.Password
            };
        }

        public async Task<UserCreated> GetUser(CreateUser createUser)
        {
            CreateUser userResult = await _collection.Find(usr => usr.Name == createUser.Name).FirstOrDefaultAsync();

            if (userResult is null)
            {
                return null;
            }

            return new UserCreated
            {
                Name = userResult.Name,
                Email = userResult.Email,
                Password = userResult.Password,
                UserId = userResult.UserId,
                ContactNo = userResult.ContactNo,
            };
        }
    }
}
