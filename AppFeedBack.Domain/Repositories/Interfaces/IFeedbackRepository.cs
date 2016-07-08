using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;

namespace AppFeedBack.Domain.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<IPagedList<Feedback>> GetPagedList(string author, string category, FeedbackOrderBy order, int page, int pageSize);

        Task Insert(Guid id, Guid categoryId, string text, string userName = "", IEnumerable<string> files = null);

        Task Update(Guid id, string text);

        Task<Feedback> Get(Guid id);

        Task Delete(Guid id);
    }
}
