using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppFeedBack.ViewModels
{
    public class FeedbackDisplayViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }
    }
}