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
        
        Task<User> GetUser(int userId);

        Task<IEnumerable<User>> GetUsers(bool isActive);

        Task<Tuple<string, bool>> Authenticate(string userName, string passWord);
        
        Task<long> GetNextId();
    }
}