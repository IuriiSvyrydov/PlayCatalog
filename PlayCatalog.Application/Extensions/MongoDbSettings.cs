
using MongoDB.Driver.Core.Configuration;

namespace PlayCatalog.Application.Extensions
{
    public class MongoDbSettings
    {

        public string Host { get; set; }
        public int Port { get; set; }
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}
