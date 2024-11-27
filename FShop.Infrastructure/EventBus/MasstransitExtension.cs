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

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitmq.Connectionstring), hostcfg =>
                    {
                        hostcfg.Username(rabbitmq.Username);
                        hostcfg.Password(rabbitmq.Password);
                    });
                    cfg.ConfigureEndpoints(provider);
                }));
                x.AddRequestClient<GetProductById>();
            });
            return services;
        }
    }
}
