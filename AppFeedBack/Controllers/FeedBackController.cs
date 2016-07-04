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
        /// Возвращает представление со списком отзывов пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ViewFeedbacks(string filter, string category, int? page, OrderBy orderBy = OrderBy.Date)
        {
            page = page ?? 1;

            var model = new IndexViewModel
            {
                Feedbacks = (await DbManager.GetFeedbacks(filter, category, orderBy)).ToPagedList((int) page, 3),
                CategoryList = await DbManager.GetCategories("Все категории"),
                Filter = filter,
                Category = category,
                OrderBy = orderBy,
                Page = page
            };
                
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
                path = Path.Combine(Server.MapPath("~/Uploads/"), path);

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
            id = id ?? Guid.Empty;
            var model = await DbManager.GetFeedbackModel((Guid) id);

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
        public async Task<ActionResult> StoreFeedback(FeedbackCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await DbManager.GetFeedbackModel();
                return View(model);
            }

            string path = Server.MapPath("~/Uploads/");
            await DbManager.StoreFeedback(model, path);

            return RedirectToAction("ViewFeedbacks");
        }
    }
}