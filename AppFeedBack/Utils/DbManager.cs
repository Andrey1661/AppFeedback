using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Domain;
using AppFeedBack.Domain.Entities;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Utils
{
    /// <summary>
    /// Класс, используемый для взаимодействия с базой данных
    /// </summary>
    public static class DbManager
    {
        /// <summary>
        /// Создает или изменяет отзыв пользователя в базе, используя готовую модель, и сохраняет файлы, переданные пользователем, по указанному пути на сервере
        /// </summary>
        /// <param name="model">Модель представления, хранящая данные для создания или изменения отзыва</param>
        /// <param name="path">Путь к корневому каталогу, в который будут сохранены файлы, прикрепленные пользователем</param>
        public static async Task<int> StoreFeedback(FeedbackCreateViewModel model, string path)
        {
            using (var db = new FeedbackContext())
            {
                bool alreadyExists = model.Id != null && model.Id != Guid.Empty;

                var feedback = await db.Feedbacks.FindAsync(model.Id) ?? new Feedback(Guid.NewGuid());
                feedback.UserName = model.UserName;
                feedback.Text = model.Text;
                feedback.PostDate = DateTime.Now;
                feedback.CategoryId = model.CategoryId;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    path = Path.Combine(path, feedback.Id.ToString());
                    var files = Utility.SaveFilesToServer(model.Files, path);

                    feedback.AttachedFiles = files.Select(file => new FeedBackFile
                    {
                        FilePath = file
                    }).ToList();
                }          

                if (!alreadyExists)
                {
                    db.Feedbacks.Add(feedback);
                }
                else
                {
                    db.Entry(feedback).State = EntityState.Modified;
                }

                return await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Возвращает модель для создания отзыва. 
        /// По умолчанию загружает в модель список категорий из базы
        /// </summary>
        /// <param name="loadCategories">Если true - загружает в модель спискок существующих категорий</param>
        /// <returns></returns>
        public static async Task<FeedbackCreateViewModel> GetFeedbackModel(bool loadCategories = true)
        {
            return await GetFeedbackModel(Guid.Empty, loadCategories);
        }

        /// <summary>
        /// Возвращает модель для редактирования отзыва на основе существующего. 
        /// По умолчанию загружает в модель список категорий из базы. 
        /// Вернет null, если в базе нет отзыва с указанным id
        /// </summary>
        /// <param name="id">id существующего отзыва</param>
        /// <param name="loadCategories">Если true - загружает в модель спискок существующих категорий</param>
        /// <returns></returns>
        public static async Task<FeedbackCreateViewModel> GetFeedbackModel(Guid id, bool loadCategories = true)
        {
            var model = new FeedbackCreateViewModel();

            if (loadCategories) model.Categories = await GetCategories("Не выбрана");

            if (id != Guid.Empty)
            {
                using (var db = new FeedbackContext())
                {
                    var feedback = await db.Feedbacks.FindAsync(id);

                    if (feedback != null)
                    {
                        model.Id = feedback.Id;
                        model.Text = feedback.Text;
                        model.CategoryId = feedback.CategoryId;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return model;
        } 

        /// <summary>
        /// Формирует список моделей представления для категорий из базы данных
        /// </summary>
        /// <param name="defaultName">Определяет необходимость добавления выбора по умолчанию и задает ему имя</param>
        /// <returns></returns>
        public static async Task<ICollection<CategoryViewModel>> GetCategories(string defaultName = "")
        {
            var categories = new List<CategoryViewModel>();

            if (!string.IsNullOrWhiteSpace(defaultName))
            {
                categories.Add(new CategoryViewModel { Id = Guid.Empty, Name = defaultName });
            }

            using (var db = new FeedbackContext())
            {
                categories.AddRange(
                    await db.Categories.Select(t => new CategoryViewModel { Id = t.Id, Name = t.Name }).ToListAsync());
            }

            return categories;
        }

        /// <summary>
        /// Возвращает коллекцию моделей представления для отзывов
        /// </summary>
        /// <param name="filter">Фильтр по автору и содержанию</param>
        /// <param name="category">Фильтр по категории</param>
        /// <returns></returns>
        public static async Task<IEnumerable<FeedbackDisplayViewModel>> GetFeedbacks(string filter, string category)
        {
            using (var db = new FeedbackContext())
            {
                IQueryable<Feedback> feedbacks = db.Feedbacks;

                if (!string.IsNullOrWhiteSpace(filter))
                    feedbacks = feedbacks.Where(t => t.UserName.Contains(filter));

                if (!string.IsNullOrWhiteSpace(category))
                    feedbacks = feedbacks.Where(t => t.Category.Name == category);

                var model = await feedbacks.OrderBy(t => t.PostDate).Select(t => new FeedbackDisplayViewModel
                {
                    Text = t.Text,
                    Author = t.UserName,
                    Category = t.Category.Name,
                    Id = t.Id,
                    PostDate = t.PostDate
                }).ToListAsync();

                return model;
            }
        }
    }
}