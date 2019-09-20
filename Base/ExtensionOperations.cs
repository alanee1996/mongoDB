using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public static class ExtensionOperations
    {
        //this method can only used by entity framework
        public static PageResult<T> GetPage<T>(this IQueryable<T> data, int page, int pageSize) where T : class
        {
            var result = new PageResult<T>();
            result.currentPage = page;
            result.pageSize = pageSize;
            result.totalRow = data.Count();
            var skip = (page - 1) * pageSize;
            result.results = data.Skip(skip).Take(pageSize).ToList();
            return result;
        }

        //this method is for storeProcedure or raw sql query to use but must be Iqueryrable
        public static PageResult<T> toPageResult<T>(this IQueryable<T> data, int page, int pageSize, int rows) where T : class
        {
            var result = new PageResult<T>();
            result.currentPage = page;
            result.pageSize = pageSize;
            result.totalRow = rows;
            result.results = data.ToList();
            return result;
        }
    }
}
