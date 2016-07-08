using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;

namespace AppFeedBack.ViewModels
{
    public class IndexViewModel
    {
        private readonly IFeedbackRepository _repository;
        private readonly ICategoryListCreator _categoryCreator;
        private readonly int pageSize = 3;

        public IPagedList<FeedbackDisplayViewModel> Feedbacks { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Display(Name = "Сортировка")]
        public FeedbackOrderBy OrderBy { get; set; }

        public int? Page { get; set; }

        public IEnumerable<CategoryViewModel> CategoryList { get; set; }

        public IndexViewModel(IFeedbackRepository repository, ICategoryListCreator categoryCreator)
        {
            _repository = repository;
            _categoryCreator = categoryCreator;
        }

        /// <summary>
        /// Инициализирует модель представления данными, полученными из БД с использованием указанных фильтров
        /// </summary>
        /// <param name="author">Фильтр по автору</param>
        /// <param name="category">Фильтр по категории</param>
        /// <param name="orderBy">Порядок сортировки</param>
        /// <param name="page">Страница вывода</param>
        /// <returns></returns>
        public async Task Initialize(string author, string category, FeedbackOrderBy orderBy, int page)
        {
            Feedbacks = await GetFeedbackPagedList(author, category, orderBy, page);
            CategoryList = await _categoryCreator.GetCategories("Все категории");
            Author = author;
            Category = category;
            OrderBy = orderBy;
            Page = page;
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
        private async Task<PagedList<FeedbackDisplayViewModel>> GetFeedbackPagedList(string author, string category, FeedbackOrderBy order, int page)
        {
            var result = await _repository.GetPagedList(author, category, order, page, pageSize);

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
    }
}