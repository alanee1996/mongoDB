using Base;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.ARepositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class RolePermissionRepositoryImpl : RolePermissionRepository
    {
        public RolePermissionRepositoryImpl(DBEntities db) : base(db)
        {

        }

        public override Task<bool> create(RolePermission obj)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async override Task<IEnumerable<RolePermission>> findAll()
        {
            var result = await collections
                .Aggregate().Lookup<Permission, RolePermission>("permissions", "permissionId", "id", "permissions")
                .Lookup<Role, RolePermission>("roles", "roleId", "id", "roles")
                .Lookup<User, RolePermission>("users", "userId", "id", "users")
                .ToListAsync();

            return result;
        }

        public override Task<PageResult<RolePermission>> findAllInPage(int page)
        {
            throw new NotImplementedException();
        }

        public override Task<RolePermission> findById(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public async override Task<IEnumerable<RolePermission>> findByRoleId(ObjectId roleId)
        {
            var result = await collections.Aggregate().Match(c => c.roleId == roleId)
                .Lookup<Permission, RolePermission>("permissions", "permissionId", "id", "permissions")
                .Lookup<Role, RolePermission>("roles", "roleId", "id", "roles")
                .Lookup<User, RolePermission>("users", "userId", "id", "users")
                .ToListAsync();

            return result;
        }

        public override Task<bool> hardDelete(RolePermission obj)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> softDelete(RolePermission obj)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> update(RolePermission obj)
        {
            throw new NotImplementedException();
        }

        public override void validation(RolePermission obj)
        {
            throw new NotImplementedException();
        }
    }
}
