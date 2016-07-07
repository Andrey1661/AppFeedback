using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppFeedBack.Utils;

namespace AppFeedBack.Commands.Interfaces
{
    public interface IFileManager
    {
        IServerFileManager ServerFileManager { get; }
    }
}