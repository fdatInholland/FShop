using FShop.Infrastructure.EventBus;
using FShop.Infrastructure.Mongo;

namespace FShop.Api.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add the MassTransit and RabbitMQ
            builder.Services.AddRabbitMQ(builder.Configuration);

            builder.Services.AddControllers();

            //builder.Services.AddScoped<IRepo, Repo>();
            //builder.Services.AddScoped<IService, Service>();

            //TODO
            //builder.Services.AddScoped<CreateProductHandler>();

            builder.Services.AddMongoDB(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
