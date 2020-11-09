using System.Threading.Tasks;
using MongoDB.Driver;
using MongoWebApi.Config;

namespace MongoWebApi.Models
{
    public class DatabaseContext : ITodoContext, IUserContext
    {
        private readonly IMongoDatabase _db;

        public IMongoCollection<Todo> Todos => _db.GetCollection<Todo>("Todos");

        public IMongoCollection<User> Users => _db.GetCollection<User>("Users");

        public DatabaseContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);

            _db = client.GetDatabase(config.Database);

            Task.Run(HandleUserIndex).Wait();
        }

        private async Task HandleUserIndex()
        {
            var options = new CreateIndexOptions() { Unique = true };
            var field = new StringFieldDefinition<User>("UserName");
            var indexDefinition = new IndexKeysDefinitionBuilder<User>().Ascending(field);

            var indexModel = new CreateIndexModel<User>(indexDefinition,options);
            await _db.GetCollection<User>("Users").Indexes.CreateOneAsync(indexModel);
        }

        
    }
}