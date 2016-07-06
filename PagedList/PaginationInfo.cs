using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagedList
{
    public class PaginationInfo
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages
        {
            get { return TotalItems/PageSize; }
        }

        public PaginationInfo(int page, int pageSize, int totalItems)
        {
            if (page <= 0 || pageSize <= 0)
                throw new ArgumentException("Номер и рамер страницы не могут быть меньше ил равны нулю");
            if (totalItems < 0)
                throw new ArgumentException("Общее количество элементов не может быть отрицательным");

            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
        }
    }
}
