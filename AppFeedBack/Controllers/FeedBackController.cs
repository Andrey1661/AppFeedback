using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppFeedBack.Models;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Controllers
{
    public class FeedBackController : Controller
    {
        public FeedbackContext db = new FeedbackContext();

        [HttpGet]
        public ActionResult CreateFeedback()
        {
            var model = new FeedbackViewModel
            {
                Categories = GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFeedback(FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = GetCategories();
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

            db.Feedbacks.Add(feedback);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            
            return View("Success");
        }

        private List<SelectListItem> GetCategories(int idSelected = 0)
        {
            var categories = db.Categories.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString(),
                Selected =  t.Id == idSelected
            }).ToList();

            categories.Add(new SelectListItem
            {
                Text = "Не выбрана",
                Value = "0",
                Selected = idSelected <= 0
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