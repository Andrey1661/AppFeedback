using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Utils;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Controllers
{
    public class FeedBackController : Controller
    {
        /// <summary>
        /// Возвращает физический путь к каталогу, в котором хранятся прикрепленные пользователями файлы
        /// </summary>
        private string PathToUploadedFiles
        {
            get { return Server.MapPath("~/Uploads"); }
        }

        /// <summary>
        /// Возвращает представление со списком отзывов пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ViewFeedbacks(string author, string category, int? page, FeedbackOrderBy orderBy = FeedbackOrderBy.Date)
        {
            var manager = new CommandManager();
            var model = await manager.CreateIndexViewModel(author, category, orderBy, page ?? 1);

            return View(model);
        }

        /// <summary>
        /// Предоставляет файл для скачивания клиентов
        /// </summary>
        /// <param name="path">Часть пути к файлу</param>
        /// <returns></returns>
        public ActionResult Download(string path)
        {
            path = Path.Combine(PathToUploadedFiles, path);
            var bytes = ServerFileManager.GetFile(path);

            if (bytes == null)
            {
                return View("Error");
            }

            string fileName = Path.GetFileName(path);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        /// <summary>
        /// Возвращает представление для создания и редактирования отзыва
        /// </summary>
        /// <param name="id">Id существующего отзыва для редактирования</param>
        /// <returns></returns>
        public async Task<ActionResult> StoreFeedback(Guid? id)
        {           
            var manager = new CommandManager();
            var model = await manager.GetFeedbackModel(id ?? Guid.Empty);

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        /// <summary>
        /// Получает и сохраняет в базу данные для создания или изменения отзыва
        /// </summary>
        /// <param name="model">Модлеь с данными из представления</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> StoreFeedback(FeedbackStoreViewModel model)
        {
            if (!model.EditMode)
            {
                model.UserName = User.Identity.Name;
            }

            var manager = new CommandManager();
            await manager.StoreFeedback(model, PathToUploadedFiles);
            return RedirectToAction("ViewFeedbacks");
        }

        /// <summary>
        /// Удаляет выбранный отзыв и перенаправляет на главную страницу
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteFeedback(Guid id)
        {
            var manager = new CommandManager();
            await manager.DeleteFeedback(id, PathToUploadedFiles);
            return RedirectToAction("ViewFeedbacks");
        }
    }
}