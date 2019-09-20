using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime Dob { get; set; }
        public string password { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
