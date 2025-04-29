using FShop.Infrastructure.EventBus;
using FShop.Infrastructure.Mongo;
using FShop.Product.DataProvider;
using MassTransit;

namespace FShop.Product.Api
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
            builder.Services.AddScoped<CreateProductHandler>();

            // Load MassTransit and rabbitmq.
            var rabbitmqoptions = new RabbitMQOption();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitmqoptions);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateProductHandler>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitmqoptions.Connectionstring), hostconfig =>
                    {
                        hostconfig.Username(rabbitmqoptions.Username);
                        hostconfig.Password(rabbitmqoptions.Password);
                    });

                    //create_product is the queue in RabbitMQ
                    cfg.ReceiveEndpoint("create_product", ep =>
                    {
                        //16 messages per time - twice retry and 100ms interval
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryconfig => { retryconfig.Interval(2, 100); });

                        //create_product is linked to CreateProductHandler
                        ep.ConfigureConsumer<CreateProductHandler>(context);
                    });
                });
            });

            ///configure masstransit options
            builder.Services.Configure<MassTransitHostOptions>(options =>
            {
                options.WaitUntilStarted = true;
                options.StartTimeout = TimeSpan.FromSeconds(30);
                options.StopTimeout = TimeSpan.FromMinutes(1);
            });

            builder.Services.AddEndpointsApiExplorer();

            //Initialize DB
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
