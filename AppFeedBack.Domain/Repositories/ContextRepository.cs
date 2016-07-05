using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Interfaces;

namespace AppFeedBack.Domain.Repositories
{
    public class ContextRepository : IRepository
    {
        /// <summary>
        /// Асинхронно возвращает из базы отзыв с соответствующим id
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <returns></returns>
        public async Task<Feedback> GetFeedback(Guid id)
        {
            using (var db = new FeedbackContext())
            {
                return await db.Feedbacks.FindAsync(id);
            }      
        }

        /// <summary>
        /// Возвращает запрос на список отзывов, соответствующих указанным фильтрам
        /// </summary>
        /// <param name="author">Фильтр по автору отзыва</param>
        /// <param name="category">Фильтр по категории</param>
        /// <returns></returns>
        public async Task<IEnumerable<Feedback>> GetFeedbackList(string author, string category)
        {
            using (var db = new FeedbackContext())
            {
                var feedbacks = db.Feedbacks.Include(t => t.AttachedFiles);

                if (!string.IsNullOrWhiteSpace(author))
                    feedbacks = feedbacks.Where(t => t.UserName.Contains(author));

                if (!string.IsNullOrWhiteSpace(category))
                    feedbacks = feedbacks.Where(t => t.Category.Name == category);

                return await feedbacks.ToListAsync();
            }
        }

        /// <summary>
        /// Возвращает запрос на список всех категорий в базе
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetCategories()
        {
            using (var db = new FeedbackContext())
            {
                return await db.Categories.ToListAsync();
            }
        }

        /// <summary>
        /// Асинхронно удаляет отзыв с заданным id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteFeedback(Guid id)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(id);
                db.Feedbacks.Remove(feedback);
                return await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Асинхронно добавляет в базу новый отзыв с указанными данными
        /// </summary>
        /// <param name="id">id нового отзыва</param>
        /// <param name="categoryId">id категории отзыва</param>
        /// <param name="text">Текст отзыва</param>
        /// <param name="userName">Имя автора</param>
        /// <param name="files">Список путей к прикрепленным файлам</param>
        /// <returns></returns>
        public async Task<int> InsertFeedback(Guid id, Guid categoryId, string text, string userName = "", IEnumerable<string> files = null)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = new Feedback(id, categoryId, text, DateTime.Now, userName);

                if (files != null)
                {
                    feedback.AttachedFiles = files.Select(file => new FeedbackFile(file)).ToList();
                }

                db.Feedbacks.Add(feedback);
                return await db.SaveChangesAsync();
            }    
        }

        /// <summary>
        /// Асинхронно изменяет текст заданного отзыва
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <param name="text">Новый текст</param>
        /// <returns></returns>
        public async Task<int> UpdateFeedback(Guid id, string text)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(id);
                feedback.Text = text;
                db.Entry(feedback).State = EntityState.Modified;

                return await db.SaveChangesAsync();
            }  
        }
    }
}
