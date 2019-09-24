using Base;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.ARepositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UserRepositoryImpl : UserRepository
    {
        private bool disposeStatus = false;

        public UserRepositoryImpl(DBEntities db) : base(db)
        {
        }

        public async override Task<bool> create(User obj)
        {
            validation(obj);
            try
            {
                await collections.InsertOneAsync(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool isDisposed() => disposeStatus;

        public override void Dispose()
        {
            this.db.Dispose();
            GC.SuppressFinalize(this);
            disposeStatus = true;
        }

        public async override Task<PageResult<User>> findAllInPage(int page)
        {
            var result = await findAll();
            return result.AsQueryable().GetPage(page, db.settings.pageSize);
        }

        public async override Task<User> findById(ObjectId id)
        {
            return await collections.Find(c => c.id == id && c.isActive).FirstOrDefaultAsync();
        }

        public async override Task<bool> hardDelete(User obj)
        {
            var result = await collections.DeleteOneAsync(c => c.id == obj.id);
            return result.DeletedCount > 0 ? true : false;
        }

        public async override Task<bool> softDelete(User obj)
        {
            var result = await collections.UpdateOneAsync(c => c.id == obj.id, Builders<User>.Update.Set(c => c.isActive, false));
            return result.ModifiedCount > 0 ? true : false;
        }

        public async override Task<bool> update(User obj)
        {
            validation(obj);
            var result = await collections.ReplaceOneAsync(c => c.id == obj.id, obj);
            return result.ModifiedCount > 0 ? true : false;
        }

        public async override Task<IEnumerable<User>> findAll()
        {
            var result = await collections.Aggregate().Match(c => c.isActive).Lookup<Role, User>("roles", "roleId", "id", "roles").ToListAsync();
            return result;
        }
    }
}
