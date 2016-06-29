using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppFeedBack.Domain;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Utils;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Controllers
{
    public class FeedBackController : Controller
    {
        /// <summary>
        /// Возвращает представление со списком отзывов пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ViewFeedbacks()
        {
            string userName = string.IsNullOrWhiteSpace(User.Identity.Name) ? "Default" : User.Identity.Name;

            using (var db = new FeedbackContext())
            {
                var model =
                    await db.Feedbacks.Where(t => t.UserName == userName).Select(t => new FeedbackDisplayViewModel
                    {
                        Text = t.Text,
                        Author = t.UserName,
                        Category = t.Category.Name,
                        Id = t.Id
                    }).ToListAsync();

                return View(model);
            }
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
                Categories = await Utility.GetCategories()
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> StoreFeedback(FeedbackCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await Utility.GetCategories();
                return View(model);
            }

            string userName = string.IsNullOrWhiteSpace(User.Identity.Name) ? "Default" : User.Identity.Name;
            string path = Server.MapPath("~/Uploads/");
            await DbManager.StoreFeedback(model, userName, path);

            return RedirectToAction("ViewFeedbacks");
        }
    }
}