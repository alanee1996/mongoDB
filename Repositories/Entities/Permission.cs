using MongoDB.Bson;
using Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Entities
{
    [DBConfig.Collection("permissions", "id", "slug")]
    public class Permission
    {
        public ObjectId id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public bool isActive { get; set; }
    }
}
