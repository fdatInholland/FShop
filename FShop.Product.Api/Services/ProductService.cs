using FShop.Infrastructure.EventBus.Product;
using FShop.Product.Api.Repositories;

namespace FShop.Product.Api.Services
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

        public async Task<ProductCreated> GetProduct(string ProductId)
        {
             return await _repository.GetProduct(ProductId); ;
        }
    }
}
