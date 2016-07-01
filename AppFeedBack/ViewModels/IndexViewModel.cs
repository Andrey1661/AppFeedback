using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace AppFeedBack.ViewModels
{
    public class IndexViewModel
    {
        public IPagedList<FeedbackDisplayViewModel> Feedbacks { get; set; }

        public string Filter { get; set; }

        public string Category { get; set; }

        public int? Page { get; set; }

        public IEnumerable<CategoryViewModel> CategoryList { get; set; }
    }
}