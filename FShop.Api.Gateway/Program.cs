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

          //  var prov = builder.Services.BuildServiceProvider();

            builder.Services.AddControllers();

            builder.Services.AddRabbitMQ(builder.Configuration);

            builder.Services.AddMongoDB(builder.Configuration);

          //  builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
