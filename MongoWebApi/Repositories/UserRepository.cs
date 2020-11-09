using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoWebApi.Models;

namespace MongoWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserContext _context;

        public UserRepository(IUserContext context)
        {
            _context = context;
        }
        
        public async Task CreateUser(User user)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            user.IsActive = true;
            await _context.Users.InsertOneAsync(user);
        }

        public async Task<User> GetUser(string userName)
        {
            var filter = Builders<User>.Filter.Eq(m => m.UserName, userName);
            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> Authenticate(User user)
        {
            var foundUser = await GetUser(user.UserName);
            return foundUser != null && BCrypt.Net.BCrypt.Verify(foundUser.Password, user.Password);
        }
    }
}