using System;
using System.Data.Entity;
using System.Linq;
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
        public async Task<ActionResult> ViewFeedbacks(string filter, string category, int? page)
        {
            page = page ?? 1;

            var model = new IndexViewModel
            {
                Feedbacks = (await DbManager.GetFeedbacks(filter, category)).ToPagedList((int) page, 3),
                CategoryList = await DbManager.GetCategories("Все категории"),
                Filter = filter,
                Category = category,
                Page = page
            };
                
            return View(model);
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