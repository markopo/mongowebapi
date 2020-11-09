using MongoDB.Driver;
using MongoWebApi.Config;

namespace MongoWebApi.Models
{
    public class TodoContext : ITodoContext
    {
        private readonly IMongoDatabase _db;

        public IMongoCollection<Todo> Todos => _db.GetCollection<Todo>("Todos");

        public TodoContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);

            _db = client.GetDatabase(config.Database);
        }
    }
}