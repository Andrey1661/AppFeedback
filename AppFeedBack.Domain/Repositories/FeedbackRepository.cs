using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Interfaces;
using PagedList;
using PagedList.EntityFramework;

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
        /// Возвращает список всех отзывов
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Feedback>> GetList()
        {
            using (var db = new FeedbackContext())
            {
                return await db.Feedbacks.ToListAsync();
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
        public async Task<PagedList<Feedback>> GetPagedList(string author, string category, FeedbackOrderBy order, int page, int pageSize)
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
        /// Удаляет отзыв с заданным id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(Guid id)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(id);
                db.Feedbacks.Remove(feedback);
                return await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаляет переданный отзыв
        /// </summary>
        /// <param name="item">Отзыв для удаления</param>
        /// <returns></returns>
        public async Task<int> Delete(Feedback item)
        {
            using (var db = new FeedbackContext())
            {
                if (db.Feedbacks.Contains(item))
                {
                    db.Entry(item).State = EntityState.Deleted;
                    return await db.SaveChangesAsync();
                }

                return -1;
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
        public async Task<int> Insert(Guid id, Guid categoryId, string text, string userName = "", IEnumerable<string> files = null)
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
        public async Task<int> Update(Guid id, string text)
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(id);
                feedback.Text = text;
                db.Entry(feedback).State = EntityState.Modified;

                return await db.SaveChangesAsync();
            }  
        }

        /// <summary>
        /// Добавляет в базу готовый отзыв
        /// </summary>
        /// <param name="item">Отзыв для добавления</param>
        /// <returns></returns>
        public async Task<int> Insert(Feedback item)
        {
            using (var db = new FeedbackContext())
            {
                db.Feedbacks.Add(item);
                return await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Обновляет переданный отзыв в базе
        /// </summary>
        /// <param name="item">Отзыв для обновления</param>
        /// <returns></returns>
        public async Task<int> Update(Feedback item)
        {
            using (var db = new FeedbackContext())
            {
                db.Entry(item).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
        }
    }
}
