using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoWebApi.Models;

namespace MongoWebApi.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(User user);

        Task<User> GetUser(string userName);

        Task<IEnumerable<User>> GetUsers(bool isActive);

        Task<bool> Authenticate(User user);
        
        Task<long> GetNextId();
    }
}