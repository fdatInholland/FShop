using MongoDB.Bson.Serialization.Attributes;

namespace FShop.Infrastructure.EventBus.Product
{
    public class CreateProduct
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ProductId { get; set; }

        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float Price { get; set; }
        public Guid categoryId { get; set; }
    }
}
