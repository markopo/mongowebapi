using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
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

        public async Task<IEnumerable<User>> GetUsers(bool isActive)
        {
            var filter = Builders<User>.Filter.Eq(m => m.IsActive, isActive);
            return await _context.Users.Find(filter).ToListAsync();
        }

        public async Task<Tuple<string, bool>> Authenticate(string userName, string passWord)
        {
            var foundUser = await GetUser(userName);
            var hasUser = foundUser != null;
            var isLoggedIn = hasUser && BCrypt.Net.BCrypt.Verify(passWord, foundUser.Password);
            return new Tuple<string, bool>(hasUser ? foundUser.UserName : "", isLoggedIn);
        }

        public async Task<long> GetNextId()
        {
            return await _context.Users.CountDocumentsAsync(new BsonDocument()) + 1;
        }
    }
}