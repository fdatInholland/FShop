using FShop.Infrastructure.EventBus.Product;

namespace FShop.Product.Api.Repositories
{
    public interface IProductRepository
    {
        Task<ProductCreated> AddProduct(CreateProduct product);
        Task<ProductCreated> GetProductById(string ProductId);
    }
}
