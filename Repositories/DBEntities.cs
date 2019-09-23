using Base;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return createCollection<T>(collection.collectionName, collection.index).Result;
        }

        public async Task<IMongoCollection<T>> createCollection<T>(string collectionName, string index)
        {
            var collections = await db.ListCollectionsAsync(new ListCollectionsOptions
            {
                Filter = new BsonDocument("name", collectionName)
            });
            if (await collections.AnyAsync())
            {
                return db.GetCollection<T>(collectionName);
            }
            await db.CreateCollectionAsync(collectionName);
            var result = db.GetCollection<T>(collectionName);
            if (!Validator.isNullOrEmpty(index)) result.Indexes.CreateOneAsync(Builders<T>.IndexKeys.Ascending(index));
            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
