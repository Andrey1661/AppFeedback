using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppFeedBack.Commands.Interfaces;

namespace AppFeedBack.Commands.Interfaces
{
    public interface IFeedbackDeleteCommand : ICommand, IFileManager
    {
        void LoadData(Guid id, string path);
    }
}