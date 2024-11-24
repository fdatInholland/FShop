using FShop.Infrastructure.EventBus.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Xml;

namespace FShop.Infrastructure.Mongo
{
    public static partial class MongoConfigExtension
    {
        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            var configSection = configuration.GetSection("mongo");

            var mongoconfig = new MongoConfig();

            configSection.Bind(mongoconfig);

            services.AddSingleton<IMongoClient>(client =>
            {
                return new MongoClient(mongoconfig.ConnectionString);
            });

            services.AddSingleton<IMongoDatabase>(client =>
            {
                MongoClient mongoClient = new MongoClient(mongoconfig.ConnectionString);
                return mongoClient.GetDatabase(mongoconfig.DatabaseName);
            });

            services.AddSingleton<IMongoCollection<ProductCreated>>(client =>
            {
                MongoClient mongoClient = new MongoClient(mongoconfig.ConnectionString);
                var database = mongoClient.GetDatabase(mongoconfig.DatabaseName);
                return database.GetCollection<ProductCreated>("Product");
            });

            services.AddSingleton<IDatabaseInitializer, MongoInitializer>();
        }
    }
}
