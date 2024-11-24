using FShop.Infrastructure.EventBus.Product;
using FShop.Infrastructure.Queries.Product;
using FShop.Product.DataProvider;
using MassTransit;

namespace FShop.Product.Query.Api.Handlers
{
    public class GetProductByIdHandler : IConsumer<GetProductById>
    {
        private IProductService _productService;

        public GetProductByIdHandler(IProductService productService)
        {
             _productService = productService;
        }

        public async Task Consume(ConsumeContext<GetProductById> context)
        {
            var prd = await _productService.GetProduct(context.Message.ProductId);
            await context.RespondAsync<ProductCreated>(prd);
        }
    }
}
