using System;
using System.Data.Entity;
using System.Linq;
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
        public async Task<ActionResult> ViewFeedbacks(FeedbackIndexViewModel model)
        {
            model = model ?? new FeedbackIndexViewModel();

            int pageNum = model.Page ?? 1;
            int pageSize = 3;

            model.Categories = (await DbManager.GetCategories()).Select(t => t.Name);
            model.Feedbacks = (await DbManager.GetFeedbacks(model.AuthorFilter, model.Category)).ToPagedList(pageNum, pageSize);

            return View(model);
        }

        /// <summary>
        /// Возвращает представление для создания и редактирования отзыва
        /// </summary>
        /// <param name="id">Id существующего отзыва для редактирования</param>
        /// <returns></returns>
        public async Task<ActionResult> StoreFeedback(Guid? id)
        {
            var model = new FeedbackCreateViewModel
            {
                Categories = await DbManager.GetCategories()
            };

            if (id != null)
            {
                using (var db = new FeedbackContext())
                {
                    var feedback = await db.Feedbacks.FirstOrDefaultAsync(t => t.Id == (Guid) id);
                    model.Id = id;
                    model.Text = feedback.Text;
                    model.Category = feedback.Category.Id;
                }
            }

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
                model.Categories = await DbManager.GetCategories();
                return View(model);
            }

            string userName = string.IsNullOrWhiteSpace(User.Identity.Name) ? "Default" : User.Identity.Name;
            string path = Server.MapPath("~/Uploads/");
            await DbManager.StoreFeedback(model, userName, path);

            return RedirectToAction("ViewFeedbacks");
        }
    }
}