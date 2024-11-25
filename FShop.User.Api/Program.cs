using FShop.Infrastructure.Mongo;
using FShop.User.Api.Repository;
using FShop.User.Api.Service;

namespace FShop.User.Api
{
    public class Program
    {
        //WTAF!!
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddMongoDB(builder.Configuration);

            await InitializeDB(builder);

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
                if (db is null)
                {
                    await db.InitializingAsync();
                }
            }
        }
    }
}
