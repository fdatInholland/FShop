using FShop.Infrastructure.EventBus;
using FShop.Infrastructure.Mongo;
using FShop.Product.Api.Repositories;
using FShop.Product.Api.Services;
using MassTransit;

namespace FShop.Product.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
           
            builder.Services.AddMongoDB(configuration);
            
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();


            //see InitializeDB in userApi
            var provider = builder.Services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IDatabaseInitializer>();
                if (db is null)
                {
                    return;
                }
                db.InitializingAsync().Wait();     //--not to sure about the wait???
            }

          

            builder.Services.AddControllers();

            // Add MassTransit handler.
            builder.Services.AddScoped<CreateProductHandler>();

            // Add MassTransit and rabbitmq.

            var rabbitmqoptions = new RabbitMQOption();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitmqoptions);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateProductHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmqoptions.Connectionstring), hostconfig =>
                    {
                        hostconfig.Username(rabbitmqoptions.Username);
                        hostconfig.Password(rabbitmqoptions.Password);
                    });
                    cfg.ReceiveEndpoint("create_product", ep =>
                    {
                        //16 messages per time
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryconfig => { retryconfig.Interval(2, 100); });
                        //create_product is linked to CreateProductHandler
                        ep.ConfigureConsumer<CreateProductHandler>(provider);
                    });
                }));
            });

            var prov = builder.Services.BuildServiceProvider();
            using (var scope = prov.CreateScope())
            {
                var buscontrol = scope.ServiceProvider.GetService<IBusControl>();
                buscontrol.Start();
            }
            //hope this all works.... is the bus started?
            //builder.Services.Configure<MassTransitHostOptions>(options =>
            //{
            //    options.WaitUntilStarted = true;
            //    options.StartTimeout = TimeSpan.FromSeconds(30);
            //    options.StopTimeout = TimeSpan.FromMinutes(1);
            //});

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
