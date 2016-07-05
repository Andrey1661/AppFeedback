using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;

namespace AppFeedBack.Domain.Interfaces
{
    public interface IRepository
    {
        Task<int> InsertFeedback(Guid id, Guid categoryId, string text, string userName, IEnumerable<string> files);

        Task<int> UpdateFeedback(Guid id, string text);

        Task<Feedback> GetFeedback(Guid id);

        Task<IEnumerable<Feedback>> GetFeedbackList(string author, string category);

        Task<IEnumerable<Category>> GetCategories();

        Task<int> DeleteFeedback(Guid id);
    }
}
