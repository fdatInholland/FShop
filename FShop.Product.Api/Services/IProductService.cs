using FShop.Infrastructure.EventBus.Product;

namespace FShop.Product.Api.Services
{
    public interface IProductService
    {
        Task<ProductCreated> GetProductById(string ProductId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
