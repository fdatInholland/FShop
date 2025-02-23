using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FShop.Infrastructure.EventBus.Product
{
    public class CreateProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid CategoryId { get; set; }
    }
}
