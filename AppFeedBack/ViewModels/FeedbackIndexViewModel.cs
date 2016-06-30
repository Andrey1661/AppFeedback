using System;
using System.Collections.Generic;
using PagedList;

namespace AppFeedBack.ViewModels
{
    /// <summary>
    /// Модель представления, содержащая данные отзывов пользователей, а также фильтрации и сортировки этих данных
    /// </summary>
    public class FeedbackIndexViewModel
    {
        /// <summary>
        /// Список моделей данных для отображения отзывов
        /// </summary>
        public IPagedList<FeedbackDisplayViewModel> Feedbacks { get; set; }

        /// <summary>
        /// Список названий категорий
        /// </summary>
        public IEnumerable<string> Categories { get; set; } 

        /// <summary>
        /// Строка фильтра по автору
        /// </summary>
        public string AuthorFilter { get; set; }

        /// <summary>
        /// Строка фильтра по текстовому содержанию
        /// </summary>
        public string ContentFilter { get; set; }

        /// <summary>
        /// Название выбранной для фильтрации категории
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Самая ранняя дата
        /// </summary>
        public DateTime? MinDate { get; set; }

        /// <summary>
        /// Сама поздняя дата
        /// </summary>
        public DateTime? MaxDate { get; set; }
    }
}