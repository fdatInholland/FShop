using MassTransit;
using FShop.Infrastructure.EventBus.Product;
using FShop.Product.DataProvider;

namespace FShop.Product.Api
{
    public class CreateProductHandler : IConsumer<CreateProduct>
    {
        private IProductService _productService;

        public CreateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<CreateProduct> context)
        {
            await _productService.AddProduct(context.Message);
            await Task.CompletedTask;
        }
    }
}
