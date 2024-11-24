using MongoDB.Bson.Serialization.Attributes;

namespace FShop.Infrastructure.Command.User
{
    public class CreateUser
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonId]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNo { get; set; }
    }
}
