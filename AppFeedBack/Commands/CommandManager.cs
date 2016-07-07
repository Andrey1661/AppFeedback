using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Domain;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Utils
{
    /// <summary>
    /// Класс, используемый для взаимодействия с базой данных
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Создает или изменяет отзыв пользователя в базе, используя готовую модель, и сохраняет файлы, переданные пользователем, по указанному пути на сервере
        /// </summary>
        /// <param name="model">Модель представления, хранящая данные для создания или изменения отзыва</param>
        /// <param name="path">Путь к корневому каталогу, в который будут сохранены файлы, прикрепленные пользователем</param>
        public async Task<int> StoreFeedback(FeedbackStoreViewModel model, string path)
        {
            var repository = new FeedbackRepository();
            var id = model.Id ?? Guid.NewGuid();

            if (model.EditMode)
            {
                return await repository.Update(id, model.Text);
            }

            var files = ServerFileManager.SaveFilesToServer(model.Files, path, id.ToString());

            return await repository.Insert(
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
            var repository = new FeedbackRepository();
            ServerFileManager.DeleteAttachedFiles(Path.Combine(path, id.ToString()));
            return await repository.Delete(id);
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
            var repository = new FeedbackRepository();
            var model = new FeedbackStoreViewModel();

            if (loadCategories)
            {
                model.Categories = await GetCategories("Не выбрана");
            }

            if (id == Guid.Empty) return model;

            var feedback = await repository.Get(id);

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
            var repository = new CategoryRepository();

            if (!string.IsNullOrWhiteSpace(defaultName))
            {
                categories.Add(new CategoryViewModel(Guid.Empty, defaultName));
            }

            var categoriesFromDb = await repository.GetActive();
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
            var repository = new FeedbackRepository();
            var feedback = await repository.Get(id);

            if (feedback == null) return null;

            return new FeedbackDisplayViewModel
            {
                Id = feedback.Id,
                Text = feedback.Text,
                Author = feedback.UserName,
                PostDate = feedback.PostDate,
                Category = feedback.Category.Name,
                Files = feedback.AttachedFiles.Select(f => f.FilePath)
            };
        }

        /// <summary>
        /// Возвращает список  моделей представления для отзывов
        /// </summary>
        /// <param name="author">Фильтр по автору и содержанию</param>
        /// <param name="category">Фильтр по категории</param>
        /// <param name="order">Определяет поле и порядок сортировки</param>
        /// <param name="page">Задает страницу выдачи</param>
        /// <param name="pageSize">Задает количество элементов на страницу</param>
        /// <returns></returns>
        public async Task<PagedList<FeedbackDisplayViewModel>> GetFeedbackPagedList(string author, string category, FeedbackOrderBy order, int page, int pageSize)
        {
            var repository = new FeedbackRepository();
            var result = await repository.GetPagedList(author, category, order, page, pageSize);

            return new PagedList<FeedbackDisplayViewModel>(
                result.Select(t => new FeedbackDisplayViewModel
                {
                    Id = t.Id,
                    Category = t.Category.Name,
                    Text = t.Text,
                    PostDate = t.PostDate,
                    Author = t.UserName,
                    Files = t.AttachedFiles.Select(f => f.FilePath)
                }),
                result.Page,
                result.PageSize,
                result.TotalItems
                );
        }

        /// <summary>
        /// Создает модель представления на основе данных, полученных из БД с использованием указанных фильтров
        /// </summary>
        /// <param name="author">Фильтр по автору</param>
        /// <param name="category">Фильтр по категории</param>
        /// <param name="orderBy">Порядок сортировки</param>
        /// <param name="page">Страница вывода</param>
        /// <param name="pageSize">Количетсов элементов на страницу</param>
        /// <returns></returns>
        public async Task<IndexViewModel> CreateIndexViewModel(string author, string category, FeedbackOrderBy orderBy, int page, int pageSize = 3)
        {
            return new IndexViewModel
            {
                Feedbacks = await GetFeedbackPagedList(author, category, orderBy, page, pageSize),
                CategoryList = await GetCategories("Все категории"),
                Author = author,
                Category = category,
                OrderBy = orderBy,
                Page = page
            };
        }
    }
}