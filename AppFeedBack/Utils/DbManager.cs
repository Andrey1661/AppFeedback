using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Domain.Interfaces;
using AppFeedBack.Domain.Repositories;
using AppFeedBack.ViewModels;
using PagedList;

namespace AppFeedBack.Utils
{
    /// <summary>
    /// Класс, используемый для взаимодействия с базой данных
    /// </summary>
    public class DbManager
    {
        private readonly IRepository _repository;

        public DbManager()
        {
            _repository = new ContextRepository();
        }

        /// <summary>
        /// Создает или изменяет отзыв пользователя в базе, используя готовую модель, и сохраняет файлы, переданные пользователем, по указанному пути на сервере
        /// </summary>
        /// <param name="model">Модель представления, хранящая данные для создания или изменения отзыва</param>
        /// <param name="path">Путь к корневому каталогу, в который будут сохранены файлы, прикрепленные пользователем</param>
        public async Task<int> StoreFeedback(FeedbackStoreViewModel model, string path)
        {
            var id = model.Id ?? Guid.NewGuid();

            if (model.EditMode)
            {
                return await _repository.UpdateFeedback(id, model.Text);
            }

            var files = Utility.SaveFilesToServer(model.Files, path, model.Id.ToString());

            return await _repository.InsertFeedback(
                id,
                model.CategoryId,
                model.Text,
                model.UserName,
                files
                );
        }

        /// <summary>
        /// Удаляет отзыв с указанным id и все прикрепленные файлы
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <param name="path">Корневой каталог с прикрепленными файлами</param>
        /// <returns>Возвращает true, если удаление успешно</returns>
        public async Task<int> DeleteFeedback(Guid id, string path)
        {
            Utility.DeleteAttachedFiles(Path.Combine(path, id.ToString()));
            return await _repository.DeleteFeedback(id);
        }

        /// <summary>
        /// Возвращает модель для создания отзыва. 
        /// По умолчанию загружает в модель список категорий из базы
        /// </summary>
        /// <param name="loadCategories">Если true - загружает в модель спискок существующих категорий</param>
        /// <returns></returns>
        public async Task<FeedbackStoreViewModel> GetFeedbackModel(bool loadCategories = true)
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
        public async Task<FeedbackStoreViewModel> GetFeedbackModel(Guid id, bool loadCategories = true)
        {
            var model = new FeedbackStoreViewModel();

            if (loadCategories)
            {
                model.Categories = await GetCategories("Не выбрана");
            }

            if (id == Guid.Empty) return model;

            var feedback = await _repository.GetFeedback(id);

            if (feedback != null)
            {
                model.EditMode = true;
                model.Id = feedback.Id;
                model.Text = feedback.Text;
                model.CategoryId = feedback.CategoryId;
            }
            else
            {
                return null;
            }

            return model;
        } 

        /// <summary>
        /// Формирует список моделей представления для категорий из базы данных
        /// </summary>
        /// <param name="defaultName">Определяет необходимость добавления выбора по умолчанию и задает ему имя</param>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryViewModel>> GetCategories(string defaultName = "")
        {
            var categories = new List<CategoryViewModel>();

            if (!string.IsNullOrWhiteSpace(defaultName))
            {
                categories.Add(new CategoryViewModel(Guid.Empty, defaultName));
            }

            var categoriesFromDb = await _repository.GetCategories();
            categories.AddRange(categoriesFromDb.Select(t => new CategoryViewModel(t.Id, t.Name)));

            return categories;
        }

        /// <summary>
        /// Формирует модель данных для отзыва с указанным id
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <returns></returns>
        public async Task<FeedbackDisplayViewModel> GetFeedback(Guid id)
        {
            var feedback = await _repository.GetFeedback(id);

            if (feedback == null) return null;

            return new FeedbackDisplayViewModel
            {
                Id = feedback.Id,
                Text = feedback.Text,
                Author = feedback.UserName,
                PostDate = feedback.PostDate,
                Category = feedback.Category.Name
            };
        } 

        /// <summary>
        /// Возвращает коллекцию моделей представления для отзывов
        /// </summary>
        /// <param name="author">Фильтр по автору и содержанию</param>
        /// <param name="category">Фильтр по категории</param>
        /// <param name="order">Определяет поле и порядок сортировки</param>
        /// <returns></returns>
        public async Task<IPagedList<FeedbackDisplayViewModel>> GetFeedbackPagedList(string author, string category, OrderBy order, int page = 1, int pageSize = 10)
        {
            var feedbacks = await _repository.GetFeedbackList(author, category);

            switch (order)
            {
                case OrderBy.Author:
                    feedbacks = feedbacks.OrderBy(t => t.UserName);
                    break;
                case OrderBy.Category:
                    feedbacks = feedbacks.OrderBy(t => t.Category.Name);
                    break;
                case OrderBy.AuthorDesc:
                    feedbacks = feedbacks.OrderByDescending(t => t.UserName);
                    break;
                case OrderBy.CategoryDesc:
                    feedbacks = feedbacks.OrderByDescending(t => t.Category.Name);
                    break;
                case OrderBy.DateDesc:
                    feedbacks = feedbacks.OrderByDescending(t => t.PostDate);
                    break;
                default:
                    feedbacks = feedbacks.OrderBy(t => t.PostDate);
                    break;
            }

            return feedbacks.Select(t => new FeedbackDisplayViewModel
            {
                Text = t.Text,
                Author = t.UserName,
                Category = t.Category.Name,
                Id = t.Id,
                PostDate = t.PostDate,
                Files = t.AttachedFiles.Select(f => f.FilePath)
            }).ToPagedList(page, pageSize);
        }

        public async Task<IndexViewModel> CreateIndexViewModel(string author, string category, OrderBy orderBy = OrderBy.Date, int page = 1, int pageSize = 10)
        {
            return new IndexViewModel
            {
                Feedbacks = await GetFeedbackPagedList(author, category, orderBy),
                CategoryList = await GetCategories("Все категории"),
                Author = author,
                Category = category,
                OrderBy = orderBy,
                Page = page
            };
        }
    }
}