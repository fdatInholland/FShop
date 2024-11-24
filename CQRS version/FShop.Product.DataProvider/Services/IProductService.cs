using FShop.Infrastructure.EventBus.Product;

namespace FShop.Product.DataProvider
{
    public interface IProductService
    {
        Task<ProductCreated> GetProduct(string ProductId);
        Task<ProductCreated> AddProduct(CreateProduct product);

    }
}
