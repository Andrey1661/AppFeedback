using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AppFeedBack.Domain.Entities
{
    /// <summary>
    /// Предоставляет способы сортировки отзывов
    /// </summary>
    public enum FeedbackOrderBy
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

    /// <summary>
    /// Модель отзыва пользователя
    /// </summary>
    public class Feedback
    {
        [Key]
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
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Категория, в которой оставлен отзыв
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Ссылка на прикрепленные файлы
        /// </summary>
        public virtual ICollection<FeedbackFile> AttachedFiles { get; set; }

        public Feedback()
        {
            AttachedFiles = new List<FeedbackFile>();
        }

        public Feedback(Guid id, Guid categoryId, string text, DateTime postDate, string userName = "") : this()
        {
            Id = id;
            CategoryId = categoryId;
            Text = text;
            PostDate = postDate;
            UserName = userName;
        }
    }
}