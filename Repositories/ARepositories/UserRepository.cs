using Base;
using Repositories.Entities;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Base.Exceptions;
using System.Linq;

namespace Repositories.ARepositories
{
    public abstract class UserRepository : IRepository<User>
    {

        public abstract Task<bool> create(User obj);
        public abstract void Dispose();
        public abstract Task<IQueryable<User>> findAll();
        public abstract Task<PageResult<User>> findAllInPage(int page);
        public abstract Task<User> findById(int id);
        public abstract Task<bool> hardDelete(User obj);
        public abstract Task<bool> softDelete(User obj);
        public abstract Task<bool> update(User obj);

        public virtual void validation(User obj)
        {
            if (Validator.isNullOrEmpty(obj.name)) throw new InvalidDataException("User name cannot be null");
            if (Validator.isNullOrEmpty(obj.password)) throw new InvalidDataException("User password cannot be null");
            if (Validator.isNullOrEmpty(obj.email)) throw new InvalidDataException("User enail cannot be null");
            if (Validator.isNullOrEmpty(obj.Dob)) throw new InvalidDataException("User date of birth cannot be null");
        }
    }
}
