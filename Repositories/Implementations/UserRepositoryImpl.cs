using Base;
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

        public override Task<bool> create(User obj)
        {
            throw new NotImplementedException();
        }

        public bool isDisposed() => disposeStatus;

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            disposeStatus = true;
        }

        public override Task<PageResult<User>> findAllInPage(int page)
        {
            throw new NotImplementedException();
        }

        public override Task<User> findById(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> hardDelete(User obj)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> softDelete(User obj)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> update(User obj)
        {
            throw new NotImplementedException();
        }

        public override Task<IQueryable<User>> findAll()
        {
            throw new NotImplementedException();
        }
    }
}
