using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    public abstract class Pagination
    {
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int totalRow { get; set; }
        public int totalPage => (int)Math.Ceiling((double)totalRow / pageSize);
        public int pageLeft => totalPage - currentPage;


    }

    public class PageResult<T> : Pagination where T : class
    {
        public IList<T> results { get; set; }
        public PageResult()
        {
            results = new List<T>();
        }
    }

}
