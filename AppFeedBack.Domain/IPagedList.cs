using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFeedBack.Domain
{
    public interface IPagedList<T> : IList<T>
    {
        int TotalItems { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        int TotalPages { get; }
    }
}
