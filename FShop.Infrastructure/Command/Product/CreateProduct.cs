using MongoDB.Bson.Serialization.Attributes;

namespace FShop.Infrastructure.Command.Product
{
    public class CreateProduct
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonId]
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
        public Guid CategoryId { get; set; }
    }
}
