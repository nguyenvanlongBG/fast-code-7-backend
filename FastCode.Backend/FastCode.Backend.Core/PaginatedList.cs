using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCode.Backend.Core
{
    public class PaginatedList<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int TotalRecord { get; private set; }
        public List<T> List { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            CurrentPage = pageIndex;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecord = count;
            List = items;
        }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPage;
            }
        }

        public static PaginatedList<T> Create(List<T> source, int count, int pageIndex, int pageSize)
        {
            //var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(source, count, pageIndex, pageSize);
        }
    }
}
