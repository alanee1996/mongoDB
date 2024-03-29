﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Repositories.Attributes;

namespace Repositories.Entities
{
    [DBConfig.Collection("roles", "id")]
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public bool isActive { get; set; }
        public IEnumerable<RolePermission> rolePermissions { get; set; }

        public Role()
        {
            rolePermissions = new List<RolePermission>();
        }
    }
}
