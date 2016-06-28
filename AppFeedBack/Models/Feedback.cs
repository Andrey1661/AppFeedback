using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace AppFeedBack.Models
{
    [TableName("Feedbacks")]
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FilePath { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime PostDate { get; set; }

        public virtual Category Category { get; set; }
    }
}