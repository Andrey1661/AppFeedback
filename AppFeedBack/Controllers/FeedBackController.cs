using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppFeedBack.Domain;
using AppFeedBack.Utils;
using AppFeedBack.ViewModels;
using PagedList;

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
        public async Task<ActionResult> ViewFeedbacks(string author, string category, int? page, OrderBy orderBy = OrderBy.Date)
        {
            var manager = new DbManager();
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
            try
            {
                path = Path.Combine(PathToUploadedFiles, path);

                var bytes = System.IO.File.ReadAllBytes(path);
                string fileName = Path.GetFileName(path);
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                var model = new HandleErrorInfo(ex, "FeedBack", "Download");
                return View("Error", model);
            }
        }

        /// <summary>
        /// Возвращает представление для создания и редактирования отзыва
        /// </summary>
        /// <param name="id">Id существующего отзыва для редактирования</param>
        /// <returns></returns>
        public async Task<ActionResult> StoreFeedback(Guid? id)
        {           
            var manager = new DbManager();
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

            var manager = new DbManager();
            await manager.StoreFeedback(model, PathToUploadedFiles);
            return RedirectToAction("ViewFeedbacks");
        }

        /// <summary>
        /// Выводит информацию об отзыве для подтверждения удаления
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteFeedback(Guid id)
        {
            var manager = new DbManager();
            var model = await manager.GetFeedback(id);
            if (model == null) return HttpNotFound();

            return View("ConfirmDelete", model);
        } 

        /// <summary>
        /// Удаляет выбранный отзыв и перенаправляет на главную страницу
        /// </summary>
        /// <param name="id">id отзыва</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var manager = new DbManager();
            await manager.DeleteFeedback(id, PathToUploadedFiles);
            return RedirectToAction("ViewFeedbacks");
        }
    }
}