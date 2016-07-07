using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;

namespace AppFeedBack.Domain.Interfaces
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<PagedList<Feedback>> GetPagedList(string author, string category, FeedbackOrderBy order, int page, int pageSize);

        Task<int> Insert(Guid id, Guid categoryId, string text, string userName = "", IEnumerable<string> files = null);

        Task<int> Update(Guid id, string text);
    }
}
