using Base;
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

        public override Task<bool> create(User obj)
        {
            throw new NotImplementedException();
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

        public async override Task<IEnumerable<User>> findAll()
        {
            var result = await collections.FindAsync(c => c.isActive);
            return result.ToEnumerable();
        }
    }
}
