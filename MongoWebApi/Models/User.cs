using System;

namespace MongoWebApi.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    
    public class User
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
    }
}