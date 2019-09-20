using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    [DBConfig.Collection("users")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime dob { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
