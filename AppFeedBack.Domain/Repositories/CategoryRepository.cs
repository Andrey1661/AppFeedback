using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;

namespace AppFeedBack.Domain.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// Возвращает из базы категорию с указанным id
        /// </summary>
        /// <param name="id">id категории</param>
        /// <returns></returns>
        public async Task<Category> Get(Guid id)
        {
            using (var db = new FeedbackContext())
            {
                return await db.Categories.FindAsync(id);
            }
        }

        /// <summary>
        /// Возвращает список всех категорий
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetList()
        {
            using (var db = new FeedbackContext())
            {
                return await db.Categories.ToListAsync();
            }
        }

        /// <summary>
        /// Удаляет категорию с указанным id
        /// </summary>
        /// <param name="id">id категории</param>
        /// <returns></returns>
        public async Task<int> Delete(Guid id)
        {
            using (var db = new FeedbackContext())
            {
                var category = await db.Categories.FindAsync(id);
                if (category == null) return -1;
                db.Categories.Remove(category);

                return await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Возвращает первую категорию, содержащую переданный параментр в имени
        /// </summary>
        /// <param name="name">Имя или часть имени категории</param>
        /// <returns></returns>
        public async Task<Category> Get(string name)
        {
            using (var db = new FeedbackContext())
            {
                return await db.Categories.FirstOrDefaultAsync(t => t.Name.Contains(name));
            }
        }
        
        /// <summary>
        /// Возвращает список активных категорий
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetActive()
        {
            using (var db = new FeedbackContext())
            {
                return await db.Categories.Where(t => t.IsActive).ToListAsync();
            }
        }

        /// <summary>
        /// Добавляет в базу новую категорию с указанными данными
        /// </summary>
        /// <param name="name">Имя категории</param>
        /// <param name="isActive">Активность категории</param>
        /// <returns></returns>
        public async Task<int> Insert(string name, bool isActive)
        {
            using (var db = new FeedbackContext())
            {
                var category = new Category
                {
                    Name = name,
                    IsActive = isActive
                };

                db.Categories.Add(category);
                return await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Устанавливает значение активности категории
        /// </summary>
        /// <param name="id">id категории</param>
        /// <param name="active">Активность</param>
        /// <returns></returns>
        public async Task<int>SetActive(Guid id, bool active)
        {
            using (var db = new FeedbackContext())
            {
                var category = await db.Categories.FindAsync(id);
                if (category == null) return -1;
                category.IsActive = active;

                return await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удалить категорию, имеющую указанное имя
        /// </summary>
        /// <param name="name">Имя категории</param>
        /// <returns></returns>
        public async Task<int> Delete(string name)
        {
            using (var db = new FeedbackContext())
            {
                var category = await db.Categories.FirstOrDefaultAsync(t => t.Name == name);
                if (category == null) return -1;
                category.Name = name;

                return await db.SaveChangesAsync();
            }
        }
    }
}
