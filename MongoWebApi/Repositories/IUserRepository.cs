using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoWebApi.Models;

namespace MongoWebApi.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(User user);
        
    }
}