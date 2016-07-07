using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;

namespace AppFeedBack.Domain.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> Get(string name);

        Task<IEnumerable<Category>> GetActive();

        Task<int> Insert(string name, bool isActive);

        Task<int> SetActive(Guid id, bool active);

        Task<int> Delete(string name);

        Task<Category> Get(Guid id);

        Task<IEnumerable<Category>> GetList();

        Task<int> Delete(Guid id);
    }
}
