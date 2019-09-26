using Base;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.Attributes;
using Repositories.Entities;
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
            settings = options.Value;
            var client = new MongoClient(settings.connectionString);
            db = client.GetDatabase(settings.dbname);
        }

        public IMongoCollection<T> getCollection<T>()
        {
            Type t = typeof(T);
            DBConfig.CollectionAttribute collection = t.GetCustomAttributes(true).FirstOrDefault(c => c.GetType() == typeof(DBConfig.CollectionAttribute)) as DBConfig.CollectionAttribute;

            if (collection == null) throw new Exception("Cannot find the collection from entity model");
            return createCollection<T>(collection.collectionName, collection.indexes).Result;
        }

        public async Task<IMongoCollection<T>> createCollection<T>(string collectionName, string[] indexes)
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
            if (!Validator.isNullOrEmpty(indexes))
            {
                var indexDefinition = Builders<T>.IndexKeys.Combine(indexes.Select(c => Builders<T>.IndexKeys.Ascending(c)));
                string s = await result.Indexes.CreateOneAsync(indexDefinition);
            }

            return result;
        }

        public async Task RunSeeder<T>(List<T> data)
        {
            Type t = typeof(T);
            DBConfig.CollectionAttribute collection = t.GetCustomAttributes(true).FirstOrDefault(c => c.GetType() == typeof(DBConfig.CollectionAttribute)) as DBConfig.CollectionAttribute;
            var collections = await db.ListCollectionsAsync(new ListCollectionsOptions
            {
                Filter = new BsonDocument("name", collection.collectionName)
            });
            if (!await collections.AnyAsync())
            {
                await db.CreateCollectionAsync(collection.collectionName);
                var result = db.GetCollection<T>(collection.collectionName);
                if (collection.indexes.Length > 0)
                {
                    var indexDefinition = Builders<T>.IndexKeys.Combine(collection.indexes.Select(c => Builders<T>.IndexKeys.Ascending(c)));
                    string s = await result.Indexes.CreateOneAsync(indexDefinition);
                }
                await result.InsertManyAsync(data);
            }
            else
            {
                var result = db.GetCollection<T>(collection.collectionName);
                await result.InsertManyAsync(data);
            }

        }






        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
