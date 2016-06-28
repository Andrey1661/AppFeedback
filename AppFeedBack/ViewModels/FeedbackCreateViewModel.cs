
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AppFeedBack.ViewModels
{
    public class FeedbackCreateViewModel
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// Text field of feedback
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Required]
        public string Text { get; set; }

        public Guid Category { get; set; }

        public ICollection<HttpPostedFileBase> Files { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; }
    }

    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}