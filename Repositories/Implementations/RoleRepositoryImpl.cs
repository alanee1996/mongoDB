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
    public class RoleRepositoryImpl : RoleRepository
    {
        private bool disposeStatus = false;
        private RolePermissionRepository rolePermission;
        public RoleRepositoryImpl(DBEntities db, RolePermissionRepository rolePermission) : base(db)
        {
            this.rolePermission = rolePermission;
        }

        public async override Task<bool> create(Role obj)
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

        public async override Task<IEnumerable<Role>> findAll()
        {
            var roles = await collections.Find(c => c.isActive).ToListAsync();
            foreach (var r in roles)
            {
                r.rolePermissions = await rolePermission.findByRoleId(r.id);
            }
            return roles;
        }

        public async override Task<PageResult<Role>> findAllInPage(int page)
        {
            var result = await findAll();
            return result.AsQueryable().GetPage(page, db.settings.pageSize);
        }

        public async override Task<Role> findById(ObjectId id)
        {
            return await collections.Find(c => c.isActive && c.id == id).FirstOrDefaultAsync();
        }

        public async override Task<bool> hardDelete(Role obj)
        {
            var result = await collections.DeleteOneAsync(c => c.id == obj.id);
            return result.DeletedCount > 0 ? true : false;
        }

        public async override Task<bool> softDelete(Role obj)
        {
            var result = await collections.UpdateOneAsync(c => c.id == obj.id, Builders<Role>.Update.Set(c => c.isActive, false));
            return result.ModifiedCount > 0 ? true : false;
        }

        public async override Task<bool> update(Role obj)
        {
            validation(obj);
            var result = await collections.ReplaceOneAsync(c => c.id == obj.id, obj);
            return result.ModifiedCount > 0 ? true : false;
        }
    }
}
