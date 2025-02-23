using FShop.Infrastructure.Queries.Product;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FShop.Infrastructure.EventBus
{
    public static class MasstransitExtension
    {

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitmq = new RabbitMQOption();
            configuration.GetSection("rabbitmq").Bind(rabbitmq);

            
            services.AddMassTransit(x => {

              //TODO
           //   x.AddConsumer<GetProductByIdConsumer>();

                x.UsingRabbitMq((ctx, cfg) => {

                    cfg.Host("localhost", "/", c =>
                    {
                        c.Username(rabbitmq.Username);
                        c.Password(rabbitmq.Password);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
                x.AddRequestClient<GetProductById>();
            });

            return services;

        }
    }
}
