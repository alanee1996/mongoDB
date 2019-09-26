using Base;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Base.Exceptions;
using System.Linq;

namespace Repositories.ARepositories
{
    public abstract class UserRepository : ARepository<User>
    {

        public UserRepository(DBEntities db) : base(db)
        {

        }

        public abstract Task<User> findUserByUsername(string username);

        public override void validation(User obj)
        {
            if (Validator.isNullOrEmpty(obj.name)) throw new InvalidDataException("User name cannot be null");
            if (Validator.isNullOrEmpty(obj.password)) throw new InvalidDataException("User password cannot be null");
            if (Validator.isNullOrEmpty(obj.email)) throw new InvalidDataException("User email cannot be null");
            if (Validator.isNullOrEmpty(obj.dob)) throw new InvalidDataException("User date of birth cannot be null");
        }
    }
}
