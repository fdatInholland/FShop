using FShop.Infrastructure.EventBus.Product;
using MongoDB.Driver;

namespace FShop.Product.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<CreateProduct> _productCollection;

        public ProductRepository(IMongoDatabase mongoDatabase, IMongoCollection<ProductCreated> productCollection)
        {
            _mongoDatabase = mongoDatabase;
            _productCollection = _mongoDatabase.GetCollection<CreateProduct>("product");
        }
        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            await _productCollection.InsertOneAsync(product);
            return new ProductCreated { ProductId = product.ProductId, ProductName = product.ProductName, CreatedAt = DateTime.Now };
        }

        public async Task<ProductCreated> GetProduct(string ProductId)
        {
            var product = _productCollection.AsQueryable().Where(x => x.ProductId == ProductId).FirstOrDefault();

            if (product is null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                await Task.CompletedTask;
                return new ProductCreated { ProductName = product.ProductName };
            }
        }
    }
}
