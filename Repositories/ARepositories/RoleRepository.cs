using Base;
using Base.Exceptions;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ARepositories
{
    public abstract class RoleRepository : ARepository<Role>
    {
        public RoleRepository(DBEntities db) : base(db)
        {

        }

        public override void validation(Role obj)
        {
            if (Validator.isNullOrEmpty(obj.name)) throw new InvalidDataException("Role name cannot be null");
            if (Validator.isNullOrEmpty(obj.slug)) throw new InvalidDataException("Role slug cannot be null");
        }

        public abstract Task<Role> findBySlug(string slug);

    }
}
