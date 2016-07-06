using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagedList
{
    public class PagedList<T> : List<T>, IOrderedEnumerable<T>
    {
        public PaginationInfo PaginationInfo { get; set; }

        internal PagedList(PaginationInfo info, IEnumerable<T> origin)
            : base(origin)
        {
            PaginationInfo = info;
        }

        public PagedList<T> ToPagedList()
        {
            return new PagedList<T>(PaginationInfo, this);
        }

        public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return this.OrderBy(keySelector, comparer);
        }
    }
}
