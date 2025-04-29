using FShop.Infrastructure.EventBus;
using FShop.Infrastructure.Mongo;
using MassTransit;

namespace FShop.Api.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Services.AddControllers();

            //builder.Services.AddMongoDB(configuration);

            builder.Services.AddRabbitMQ(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
