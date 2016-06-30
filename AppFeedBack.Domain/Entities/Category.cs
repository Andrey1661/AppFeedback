using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace AppFeedBack.Domain.Entities
{
    /// <summary>
    /// Модель данных категорий
    /// </summary>
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Название категории
        /// </summary>
        [Required]
        [Display(Name="Название категории")]
        public string Name { get; set; }

        /// <summary>
        /// Показывает, активна ли категория
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Сссылка на отзывы, относящиеся к данной категории
        /// </summary>
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public Category()
        {
            Feedbacks = new List<Feedback>();
        }
    }
}