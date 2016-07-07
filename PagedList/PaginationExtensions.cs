using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagedList
{
    public static class PaginationExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> list, int page, int pageSize)
        {
            var info = new PaginationInfo(page, pageSize, list.Count());
            var resultList = list.Skip((page - 1)*pageSize).Take(pageSize).ToList();

            return new PagedList<T>(info, resultList);
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> list, int page, int pageSize)
        {
            var info = new PaginationInfo(page, pageSize, list.Count());
            var resultList = await list.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(info, resultList);
        } 
    }
}
