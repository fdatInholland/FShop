
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
            //TODO
            //product.ProductId = Guid.NewGuid();
            return await _repository.AddProduct(product);
        }

       public async Task<ProductCreated> GetProduct(Guid ProductId)
        {
            //TODO
            //return await _repository.GetProduct(ProductId); ;
            return null;
        }
    }
}
