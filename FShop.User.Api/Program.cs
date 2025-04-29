using FShop.Infrastructure.EventBus;
using FShop.Infrastructure.Mongo;
using FShop.User.Api.Handlers;
using FShop.User.DataProvider.Repositories;
using FShop.User.DataProvider.Services;
using MassTransit;

namespace FShop.User.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddMongoDB(builder.Configuration);
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<CreateuserHandler>();

            var rabbitmqoptions = new RabbitMQOption();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitmqoptions);


            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateuserHandler>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitmqoptions.Connectionstring), hostconfig =>
                    {
                        hostconfig.Username(rabbitmqoptions.Username);
                        hostconfig.Password(rabbitmqoptions.Password);
                    });

                    cfg.ReceiveEndpoint("add_user", ep =>
                    {
                        //16 messages per time - twice retry and 100ms interval
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryconfig => { retryconfig.Interval(2, 100); });
                        ep.ConfigureConsumer<CreateuserHandler>(context);
                    });
                });
            });

            await InitializeDB(builder);

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static async Task InitializeDB(WebApplicationBuilder builder)
        {
            var provider = builder.Services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IDatabaseInitializer>();
                if (db is not null)
                {
                    await db.InitializingAsync();
                }
            }
        }
    }
}
