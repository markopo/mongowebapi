using System;

namespace MongoWebApi.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    
    public class Todo
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
        public User CreatedBy { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
    }
}