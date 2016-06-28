using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace AppFeedBack.Domain.Entities
{
   
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Название категории
        /// </summary>
        [Required]
        [Display(Name="Название категории")]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public Category()
        {
            Feedbacks = new List<Feedback>();
        }
    }
}