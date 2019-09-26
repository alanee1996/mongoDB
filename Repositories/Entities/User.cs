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
        public ObjectId id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public ObjectId roleId { get; set; }
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
        //one to one relationship
        [BsonIgnoreIfNull]
        public IEnumerable<Role> roles { get; set; }

        public User()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
            roles = new List<Role>();
        }
    }
}
