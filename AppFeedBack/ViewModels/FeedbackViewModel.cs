
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AppFeedBack.ViewModels
{
    public class FeedbackViewModel
    {
        [DataType(DataType.MultilineText)]
        [Required]
        public string Text { get; set; }

        public string Category { get; set; }

        public HttpPostedFileBase File { get; set; }

        public ICollection<SelectListItem> Categories { get; set; }
    }
}