using MongoDB.Driver;

namespace MongoWebApi.Models
{
    public interface IUserContext
    {
        IMongoCollection<User> Users { get; }
    }
}