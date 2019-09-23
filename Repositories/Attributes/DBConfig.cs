using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Attributes
{
    public class DBConfig
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class CollectionAttribute : Attribute
        {
            public string collectionName { get; set; }
            public string index { get; set; }
            public CollectionAttribute(string collectionName)
            {
                this.collectionName = collectionName;
            }

            public CollectionAttribute(string collectionName, string index)
            {
                this.collectionName = collectionName;
                this.index = index;
            }
        }
    }
}
