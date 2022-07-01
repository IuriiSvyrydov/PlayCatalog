using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PlayCatalog.Application.Repositories;
using PlayCatalog.Application.Settings;
using PlayCatalog.Model;

namespace PlayCatalog.Application.Extensions;

public static class ApplicationExtensions
{
    //private static ServiceSettings _settings;

    public static IServiceCollection ConfigurationMongoSettigs(this IServiceCollection services,
        IConfiguration configuration)
    {

        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
           
        services.AddSingleton(serviceProvider =>
        {
            var _settings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(_settings.ServiceName);
        });
        return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)where T: IEntity
    {
        services.AddSingleton<IRepository<T>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(database, collectionName);

        });
        return services;
    }
}