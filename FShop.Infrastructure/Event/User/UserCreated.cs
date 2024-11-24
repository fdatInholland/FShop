using MongoDB.Bson.Serialization.Attributes;

namespace FShop.Infrastructure.Event.User
{
    public class UserCreated
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
