using Base;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class DBEntities : IDisposable
    {
        public AppSetting settings { get; set; }
        private readonly IMongoDatabase db;

        public DBEntities(IOptions<AppSetting> options)
        {
            this.settings = options.Value;
            var client = new MongoClient(settings.connectionString);
            db = client.GetDatabase(settings.dbname);
        }

        public IMongoCollection<T> getCollection<T>()
        {
            Type t = typeof(T);
            DBConfig.CollectionAttribute collection = t.GetCustomAttributes(true).FirstOrDefault(c => c.GetType() == typeof(DBConfig.CollectionAttribute)) as DBConfig.CollectionAttribute;
            if (collection == null) throw new Exception("Cannot find the collection from entity model");
            return db.GetCollection<T>(collection.collectionName);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
