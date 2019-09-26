using MongoDB.Bson;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ARepositories
{
    public abstract class RolePermissionRepository : ARepository<RolePermission>
    {
        public RolePermissionRepository(DBEntities db) : base(db)
        {

        }

        public abstract Task<IEnumerable<RolePermission>> findByRoleId(ObjectId roleId);
    }
}
