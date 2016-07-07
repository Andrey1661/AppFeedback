using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;
using PagedList;

namespace AppFeedBack.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Get(Guid id);

        Task<IEnumerable<T>> GetList();

        Task<int> Insert(T item);

        Task<int> Update(T item);

        Task<int> Delete(Guid id);
            
        Task<int> Delete(T item);
    }
}
