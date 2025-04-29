using FShop.Infrastructure.EventBus;
using FShop.Infrastructure.Mongo;
using FShop.Product.DataProvider;
using FShop.Product.Query.Api.Handlers;
using MassTransit;

namespace FShop.Product.Query.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Services.AddControllers();

            builder.Services.AddMongoDB(configuration);
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<GetProductByIdHandler>();

            var rabbitmq = new RabbitMQOption();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitmq);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<GetProductByIdHandler>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitmq.Connectionstring), hostcfg =>
                    {
                        hostcfg.Username(rabbitmq.Username);
                        hostcfg.Password(rabbitmq.Password);
                    });

                    //get_product is the queue in RabbitMQ
                    cfg.ReceiveEndpoint("get_product", ep =>
                    {
                        //16 messages per time - twice retry and 100ms interval
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryconfig => { retryconfig.Interval(2, 100); });

                        //get product is linked to GetProductByIdHandler
                        ep.ConfigureConsumer<GetProductByIdHandler>(context);
                    });
                });
            });

            //configure masstransit options
            builder.Services.Configure<MassTransitHostOptions>(options =>
            {
                options.WaitUntilStarted = true;
                options.StartTimeout = TimeSpan.FromSeconds(30);
                options.StopTimeout = TimeSpan.FromMinutes(1);
            });

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IDatabaseInitializer>();
                await db.InitializingAsync();
            }
           
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}
