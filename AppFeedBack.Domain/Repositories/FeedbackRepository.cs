using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;

namespace AppFeedBack.Domain.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        /// <summary>
        /// Возвращает из базы отзыв с соответствующим id
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <returns></returns>
        public async Task<Feedback> Get(Guid id)
        {
            using (var db = new FeedbackContext())
            {
                return
                    await
                        db.Feedbacks.Include(t => t.AttachedFiles)
                            .Include(t => t.Category)
                            .FirstOrDefaultAsync(t => t.Id == id);
            }      
        }

        /// <summary>
        /// Возвращает запрос на список отзывов, соответствующих указанным фильтрам
        /// </summary>
        /// <param name="author">Фильтр по автору отзыва</param>
        /// <param name="category">Фильтр по категории</param>
        /// <param name="order">Порядок сортировки</param>
        /// <param name="page">Текущая страница выдачи</param>
        /// <param name="pageSize">Количество элементов на страницу</param>
        /// <returns></returns>
        public async Task<IPagedList<Feedback>> GetPagedList(string author, string category, FeedbackOrderBy order, int page, int pageSize)
        {
            using (var db = new FeedbackContext())
            {
                var feedbacks = db.Feedbacks.Include(t => t.AttachedFiles).Include(x => x.Category);

                if (!string.IsNullOrWhiteSpace(author))
                    feedbacks = feedbacks.Where(t => t.UserName.Contains(author));

                if (!string.IsNullOrWhiteSpace(category))
                    feedbacks = feedbacks.Where(t => t.Category.Name == category);

                switch (order)
                {
                    case FeedbackOrderBy.Author:
                        feedbacks = feedbacks.OrderBy(t => t.UserName);
                        break;
                    case FeedbackOrderBy.Category:
                        feedbacks = feedbacks.OrderBy(t => t.Category.Name);
                        break;
                    case FeedbackOrderBy.AuthorDesc:
                        feedbacks = feedbacks.OrderByDescending(t => t.UserName);
                        break;
                    case FeedbackOrderBy.CategoryDesc:
                        feedbacks = feedbacks.OrderByDescending(t => t.Category.Name);
                        break;
                    case FeedbackOrderBy.DateDesc:
                        feedbacks = feedbacks.OrderByDescending(t => t.PostDate);
                        break;
                    default:
                        feedbacks = feedbacks.OrderBy(t => t.PostDate);
                        break;
                }

                var result = await feedbacks.Skip((page - 1)*pageSize).Take(pageSize).ToListAsync();
                return new PagedList<Feedback>(result, page, pageSize, feedbacks.Count());
            }
        }

        /// <summary>
        /// Удаляет отзыв с заданным id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(id);
                db.Feedbacks.Remove(feedback);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Добавляет в базу новый отзыв с указанными данными
        /// </summary>
        /// <param name="id">id нового отзыва</param>
        /// <param name="categoryId">id категории отзыва</param>
        /// <param name="text">Текст отзыва</param>
        /// <param name="userName">Имя автора</param>
        /// <param name="files">Список путей к прикрепленным файлам</param>
        /// <returns></returns>
        public async Task Insert(Guid id, Guid categoryId, string text, string userName = "", IEnumerable<string> files = null)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = new Feedback(id, categoryId, text, DateTime.Now, userName);

                if (files != null)
                {
                    feedback.AttachedFiles = files.Select(file => new FeedbackFile(file)).ToList();
                }

                db.Feedbacks.Add(feedback);
                await db.SaveChangesAsync();
            }    
        }

        /// <summary>
        /// Асинхронно изменяет текст заданного отзыва
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <param name="text">Новый текст</param>
        /// <returns></returns>
        public async Task Update(Guid id, string text)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(id);
                feedback.Text = text;
                db.Entry(feedback).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }  
        }
    }
}
