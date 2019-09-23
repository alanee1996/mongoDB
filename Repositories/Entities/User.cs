using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    [DBConfig.Collection("users", "id")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string roleId { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime dob { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime createdAt { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime updatedAt { get; set; }

        public User()
        {
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
        }
    }
}
