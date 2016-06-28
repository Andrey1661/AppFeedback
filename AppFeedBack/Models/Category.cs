using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace AppFeedBack.Models
{
   [TableName("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

       public Category()
       {
           Feedbacks = new List<Feedback>();
       }
    }
}