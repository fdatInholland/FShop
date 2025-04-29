
using FShop.Infrastructure.EventBus.Product;

namespace FShop.Product.DataProvider
{
    public class ProductService : IProductService
    {
        private IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            product.ProductId = Guid.NewGuid().ToString();
            return await _repository.AddProduct(product);
        }

        public async Task<ProductCreated> GetProductByID(string ProductId)
        {
                return await _repository.GetProduct(ProductId.ToString());
        }
    }
}
