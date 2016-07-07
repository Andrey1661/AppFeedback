using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IFeedbackDeleteCommand _deleteCommand;
        private readonly IFeedbackRepository _repository;
        private readonly ICategoryListCreator _creator;

        public AdminController(IFeedbackDeleteCommand deleteCommand, IFeedbackRepository repository, ICategoryListCreator creator)
        {
            _deleteCommand = deleteCommand;
            _repository = repository;
            _creator = creator;
        }

        /// <summary>
        /// Возвращает представление со списком отзывов пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ViewFeedbacks(string author, string category, int? page, FeedbackOrderBy orderBy = FeedbackOrderBy.Date)
        {
            var model = new IndexViewModel(_repository, _creator);
            await model.Initialize(author, category, orderBy, page ?? 1);

            return View(model);
        }

        /// <summary>
        /// Удаляет выбранный отзыв и перенаправляет на главную страницу
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteFeedback(Guid id)
        {
            _deleteCommand.LoadData(id, PathToUploadedFiles);
            await _deleteCommand.Execute();
            return RedirectToAction("ViewFeedbacks");
        }

        /// <summary>
        /// Предоставляет файл для скачивания клиентов
        /// </summary>
        /// <param name="path">Часть пути к файлу</param>
        /// <returns></returns>
        public ActionResult Download(string path)
        {
            path = Path.Combine(PathToUploadedFiles, path);
            var bytes = _deleteCommand.ServerFileManager.GetFile(path);

            if (bytes == null)
            {
                return View("Error");
            }

            string fileName = Path.GetFileName(path);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}