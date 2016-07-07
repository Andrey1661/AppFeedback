using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppFeedBack.ViewModels
{
    /// <summary>
    /// Модель представления, хранящая данные отображения отзыва пользователя
    /// </summary>
    public class FeedbackDisplayViewModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Текст отзыва
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата и врема создания отзыва
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Дата создания")]
        public DateTime PostDate { get; set; }

        /// <summary>
        /// Название категории
        /// </summary>
        [Display(Name = "Категория")]
        public string Category { get; set; }

        /// <summary>
        /// Имя автора отзыва
        /// </summary>
        [Display(Name = "Автор")]
        public string Author { get; set; }

        /// <summary>
        /// Пути к прикрепленным файлам
        /// </summary>
        [Display(Name = "Прикрепленные файлы")]
        public IEnumerable<string> Files { get; set; } 
    }
}