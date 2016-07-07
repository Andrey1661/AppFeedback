
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Commands.Interfaces
{
    public interface ICategoryListCreator
    {
        Task<IEnumerable<CategoryViewModel>> GetCategories(string defaultName = "");
    }
}