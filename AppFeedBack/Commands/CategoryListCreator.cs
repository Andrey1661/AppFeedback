using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Commands
{
    public class CategoryListCreator : ICategoryListCreator
    {
        private readonly ICategoryRepository _repository;

        /// <summary>
        /// Инициализирует новый экземпляр CategoryListCreator
        /// </summary>
        /// <param name="repository">Репозиторий для доствупа к сохраненным категориям</param>
        public CategoryListCreator(ICategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Формирует список категорий на основе базы данных
        /// </summary>
        /// <param name="defaultName">Добавляет в список значение по умолчанию с заданным текстом</param>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryViewModel>> GetCategories(string defaultName = "")
        {
            var categories = new List<CategoryViewModel>();

            if (!string.IsNullOrWhiteSpace(defaultName))
            {
                categories.Add(new CategoryViewModel(Guid.Empty, defaultName));
            }

            var categoriesFromDb = await _repository.GetActive();
            categories.AddRange(categoriesFromDb.Select(t => new CategoryViewModel(t.Id, t.Name)));

            return categories;
        }
    }
}