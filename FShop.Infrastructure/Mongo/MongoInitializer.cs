﻿using FShop.Infrastructure.EventBus.Product;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace FShop.Infrastructure.Mongo
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool initialized = true;
        private IMongoDatabase _database;


        public MongoInitializer(IMongoDatabase mongoDatabase)
        {
            _database = mongoDatabase;
            
            //TODO - remove
            var temp = _database.GetCollection<CreateProduct>("product");
        }

        public async Task InitializingAsync()
        {
            if (initialized)
                return;

            IConventionPack conventionPack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("FShop", conventionPack, c => true);
        }
    }
}
