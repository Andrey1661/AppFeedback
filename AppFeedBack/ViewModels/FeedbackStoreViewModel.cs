
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using AppFeedBack.Validation;

namespace AppFeedBack.ViewModels
{
    /// <summary>
    /// Модель представления, хранящая данные отзыва пользователя
    /// </summary>
    public class FeedbackStoreViewModel
    {
        public Guid? Id { get; set; }

        public bool EditMode { get; set; }

        /// <summary>
        /// Текст отзыва
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Необходимо заполнить поле")]
        [MaxLength(4000, ErrorMessage = "Максимальная длина сообщения - 4000 символов")]
        public string Text { get; set; }

        /// <summary>
        /// Категория, к которой относится отзыв
        /// </summary>
        [NotEmpty(ErrorMessage = "Категория не выбрана")]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Имя автора
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Прикрепленные пользователем файлы
        /// </summary>
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        /// <summary>
        /// Список категорий для представления
        /// </summary>
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}