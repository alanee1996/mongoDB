using Base;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ARepositories
{
    public abstract class ARepository<T> : IDisposable where T : class
    {
        protected DBEntities db;
        protected IMongoCollection<T> collections;

        public ARepository(DBEntities db)
        {
            this.db = db;
            this.collections = db.getCollection<T>();
        }

        public abstract Task<bool> create(T obj);
        public abstract void Dispose();
        public abstract Task<IEnumerable<T>> findAll();
        public abstract Task<PageResult<T>> findAllInPage(int page);
        public abstract Task<T> findById(int id);
        public abstract Task<bool> hardDelete(T obj);
        public abstract Task<bool> softDelete(T obj);
        public abstract Task<bool> update(T obj);
        public abstract void validation(T obj);

    }

    public interface test
    {

    }
}
