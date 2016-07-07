using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;

namespace AppFeedBack.Domain.Interfaces
{
    interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> Get(string name);

        Task<IEnumerable<Category>> GetActive();

        Task<int> Insert(string name, bool isActive);

        Task<int> SetActive(Guid id, bool active);

        Task<int> Delete(string name);
    }
}
