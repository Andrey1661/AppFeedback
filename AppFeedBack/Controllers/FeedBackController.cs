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
using AppFeedBack.ViewModels;

namespace AppFeedBack.Controllers
{
    public class FeedBackController : Controller
    {
        public FeedbackContext db = new FeedbackContext();

        [HttpGet]
        public async Task<ActionResult> CreateFeedback()
        {
            var model = new FeedbackViewModel
            {
                Categories = await GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFeedback(FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View("CreateFeedback", model);
            }

            string user = string.IsNullOrWhiteSpace(User.Identity.Name) ? "Default" : User.Identity.Name;
            string path = SaveUploadedFile(model.File);

            var feedback = new Feedback
            {
                Text = model.Text,
                UserName = user,
                PostDate = DateTime.Now,
                FilePath = path
            };

            using (var db = new FeedbackContext())
            {
                db.Feedbacks.Add(feedback);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }
            
            return View("Success");
        }

        private async Task<List<SelectListItem>> GetCategories()
        {
            var categories = await db.Categories.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToListAsync();

            categories.Add(new SelectListItem
            {
                Text = "Не выбрана",
                Value = "0"
            });

            return categories;
        }

        private string SaveUploadedFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0) return null;

            string name = string.Format("{0}_{1}", DateTime.Now.ToString("g").Replace(":", "."), Path.GetFileName(file.FileName).Replace("_", "-"));
            //string path = Server.MapPath(string.Format(@"\Files\{0}\", User.Identity.Name));
            string path = Server.MapPath(@"Files\Default\");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullName = path + name;

            file.SaveAs(fullName); 

            return fullName;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}