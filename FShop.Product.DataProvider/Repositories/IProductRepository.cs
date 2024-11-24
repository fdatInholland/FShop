using FShop.Infrastructure.EventBus.Product;

namespace FShop.Product.DataProvider
{
    public interface IProductRepository
    {
        Task<ProductCreated> AddProduct(CreateProduct product);
        Task<ProductCreated> GetProduct(string ProductId);
    }
}
