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
        /// <param name="userName">Имя пользователя, написавшего отзыв</param>
        /// <param name="path">Путь к корневому каталогу, в который будут сохранены файлы, прикрепленные пользователем</param>
        public static async Task<int> StoreFeedback(FeedbackCreateViewModel model, string userName, string path = "")
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(model.Id) ?? new Feedback();
                feedback.UserName = "Default";
                feedback.Text = model.Text;
                feedback.PostDate = DateTime.Now;
                feedback.CategoryId = model.Category;

                if (!String.IsNullOrWhiteSpace(path))
                {
                    string fullPath = Path.Combine(path, userName);

                    var files = Utility.SaveFilesToServer(model.Files, fullPath);

                    feedback.AttachedFiles = files.Select(file => new FeedBackFile
                    {
                        FilePath = file
                    }).ToList();
                }          

                if (feedback.Id == Guid.Empty)
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
        /// Формирует список моделей представления для категорий из базы данных
        /// </summary>
        /// <returns>Коллекция моделей представления</returns>
        public static async Task<ICollection<CategoryViewModel>> GetCategories()
        {
            var categories = new List<CategoryViewModel>
            {
                new CategoryViewModel{Id = Guid.Empty, Name = "Не выбрана"}
            };

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
        /// <param name="author">Фильтр по автору</param>
        /// <param name="category">Фильтр по категории</param>
        /// <returns></returns>
        public static async Task<IEnumerable<FeedbackDisplayViewModel>> GetFeedbacks(string author, string category)
        {
            using (var db = new FeedbackContext())
            {
                IQueryable<Feedback> feedbacks = db.Feedbacks;

                if (!string.IsNullOrWhiteSpace(author))
                {
                    feedbacks = db.Feedbacks.Where(t => t.UserName == author);
                }
                else if (!string.IsNullOrWhiteSpace(category))
                {
                    feedbacks = db.Feedbacks.Where(t => t.Category.Name == category);
                }

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