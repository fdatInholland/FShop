using FShop.Infrastructure.EventBus;
using FShop.Product.Query.Api.Handlers;
using MassTransit;
using System.Configuration;

namespace FShop.Product.Query.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var rabbitmq = new RabbitMQOption();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitmq);

            builder.Services.AddControllers();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<GetProductByIdHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmq.Connectionstring), hostcfg =>
                    {
                        hostcfg.Username(rabbitmq.Username);
                        hostcfg.Password(rabbitmq.Password);
                    });
                    cfg.ConfigureEndpoints(provider);
                }));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
