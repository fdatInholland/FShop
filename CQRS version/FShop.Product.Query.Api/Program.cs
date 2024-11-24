using FShop.Infrastructure.EventBus;
using FShop.Infrastructure.Mongo;
using FShop.Product.DataProvider;
using FShop.Product.Query.Api.Handlers;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace FShop.Product.Query.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddMongoDB(builder.Configuration);

            //added 3-3-2024 - its a HACK!!!
            var provider = builder.Services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IDatabaseInitializer>();
                db.InitializingAsync().Wait();     //--not to sure about the wait???
            }

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<GetProductByIdHandler>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<GetProductByIdHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var rabbitmq = new RabbitMQOption();
                    builder.Configuration.GetSection("rabbitmq").Bind(rabbitmq);
                    
                    cfg.Host(new Uri(rabbitmq.Connectionstring), hostcfg =>
                    {
                        hostcfg.Username(rabbitmq.Username);
                        hostcfg.Password(rabbitmq.Password);
                    });
                }));
            });

            //newer version of masstransit
            //builder.Services.AddMassTransit(x =>
            //{
            //    x.AddConsumer<GetProductByIdHandler>();

            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        var rabbitmq = new RabbitMQOption();
            //        builder.Configuration.GetSection("rabbitmq").Bind(rabbitmq);

            //        cfg.Host(new Uri(rabbitmq.Connectionstring), hostcfg =>
            //        {
            //            hostcfg.Username(rabbitmq.Username);
            //            hostcfg.Password(rabbitmq.Password);
            //        });

            //        cfg.ReceiveEndpoint("get-product-by-id-queue", endpoint =>
            //        {
            //            endpoint.ConfigureConsumer<GetProductByIdHandler>(context);
            //        });
            //    });
            //});




            var prov = builder.Services.BuildServiceProvider();
            using (var scope = prov.CreateScope())
            {
                var buscontrol = scope.ServiceProvider.GetService<IBusControl>();
                buscontrol.Start();
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
