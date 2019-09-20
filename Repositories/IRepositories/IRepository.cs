using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task<bool> create(T obj);
        Task<bool> update(T obj);
        Task<bool> softDelete(T obj);
        Task<bool> hardDelete(T obj);
        Task<T> findById(int id);
        Task<IQueryable<T>> findAll();
        Task<PageResult<T>> findAllInPage(int page);
        void validation(T obj);
    }
}
