using FShop.Infrastructure.EventBus.Product;

namespace FShop.Product.DataProvider
{
    public interface IProductService
    {
        Task<ProductCreated> GetProduct(Guid ProductId);
        Task<ProductCreated> AddProduct(CreateProduct product);

    }
}
