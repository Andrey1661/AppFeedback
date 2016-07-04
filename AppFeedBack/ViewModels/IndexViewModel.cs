using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace AppFeedBack.ViewModels
{
    public enum OrderBy
    {
        [Display(Name = "По автору")]
        Author = 1,

        [Display(Name = "По категории")]
        Category = 2,

        [Display(Name = "По дате")]
        Date = 4,

        [Display(Name = "По автору (в обратном порядке)")]
        AuthorDesc = 8,

        [Display(Name = "По категории (в обратном порядке)")]
        CategoryDesc = 16,

        [Display(Name = "По дате (в обратном порядке)")]
        DateDesc = 32
    }

    public class IndexViewModel
    {
        public IPagedList<FeedbackDisplayViewModel> Feedbacks { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Display(Name = "Сортировка")]
        public OrderBy OrderBy { get; set; }

        public int? Page { get; set; }

        public IEnumerable<CategoryViewModel> CategoryList { get; set; }

        public IndexViewModel()
        {
            OrderBy = OrderBy.Date;
        }
    }
}