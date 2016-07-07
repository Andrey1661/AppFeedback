using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Commands.Interfaces
{
    public interface ICategoryListCreator
    {
        Task<IEnumerable<CategoryViewModel>> GetCategories(string defaultName = "");
    }
}