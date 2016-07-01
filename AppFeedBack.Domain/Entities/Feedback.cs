using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppFeedBack.Domain.Entities
{
    /// <summary>
    /// Модель отзыва пользователя
    /// </summary>
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя автора отзыва
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Текст отзыва
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Дата и время отправки отзыва
        /// </summary>
        public DateTime PostDate { get; set; }

        [ForeignKey("Category")]
        public virtual Guid CategoryId { get; set; }

        /// <summary>
        /// Категория, в которой оставлен отзыв
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Сссылка на прикрепленные файлы
        /// </summary>
        public virtual ICollection<FeedBackFile> AttachedFiles { get; set; }

        public Feedback() : this(Guid.Empty) { }

        public Feedback(Guid id)
        {
            Id = id;
            AttachedFiles = new List<FeedBackFile>();
        }
    }
}