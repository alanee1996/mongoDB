
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    [DBConfig.Collection("rolepermissions", "id")]
    public class RolePermission
    {
        [BsonId]
        public ObjectId id { get; set; }
        public ObjectId roleId { get; set; }
        public ObjectId userId { get; set; }
        public ObjectId permissionId { get; set; }
        public IEnumerable<Role> roles { get; set; }
        public IEnumerable<User> users { get; set; }
        public IEnumerable<Permission> permissions { get; set; }
    }
}
