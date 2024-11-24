using FShop.Infrastructure.EventBus;

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

            //TODO
            //builder.Services.AddScoped<CreateProductHandler>();

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
