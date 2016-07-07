﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Utils;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Commands.Interfaces
{
    public interface IFeedbackStoreCommand : ICommand, IFileManager
    {
        IServerFileManager ServerFileManager { get; }

        void LoadData(FeedbackStoreViewModel model, string path);
    }
}