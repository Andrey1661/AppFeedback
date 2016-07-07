using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppFeedBack.Commands.Interfaces
{
    public interface IFeedbackDeleteCommand : ICommand, IFileManager
    {
        void LoadData(Guid id, string path);
    }
}