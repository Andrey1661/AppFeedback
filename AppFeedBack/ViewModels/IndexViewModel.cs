using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppFeedBack.Domain;
using AppFeedBack.Domain.Entities;

namespace AppFeedBack.ViewModels
{
    public class IndexViewModel
    {
        public PagedList<FeedbackDisplayViewModel> Feedbacks { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Display(Name = "Сортировка")]
        public FeedbackOrderBy OrderBy { get; set; }

        public int? Page { get; set; }

        public IEnumerable<CategoryViewModel> CategoryList { get; set; }

        public IndexViewModel()
        {
            OrderBy = FeedbackOrderBy.Date;
        }
    }
}