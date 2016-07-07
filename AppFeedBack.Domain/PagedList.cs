using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFeedBack.Domain
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int TotalPages
        {
            get { return (int) Math.Ceiling((decimal) TotalItems/PageSize); }
        }

        public PagedList(IEnumerable<T> list, int page, int pageSize, int totalItems) : base(list)
        {
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
        } 
    }
}
