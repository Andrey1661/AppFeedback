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
        public async Task<ActionResult> ViewFeedbacks()
        {
            string userName = string.IsNullOrWhiteSpace(User.Identity.Name) ? "Default" : User.Identity.Name;

            using (var db = new FeedbackContext())
            {
                var model = await db.Feedbacks.Where(t => t.UserName == userName).Select(t => new FeedbackDisplayViewModel
                {
                    Text = t.Text,
                    Author = t.UserName,
                    Category = t.Category.Name,
                    Id = t.Id
                }).ToListAsync();

                return View(model);
            }
        }

        public async Task<ActionResult> StoreFeedback(Guid? id)
        {
            var model = new FeedbackCreateViewModel
            {
                Categories = await GetCategories()
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

        [HttpPost]
        public async Task<ActionResult> StoreFeedback(FeedbackCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View(model);
            }

            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(model.Id) ?? new Feedback();
                //feedback.UserName = User.Identity.Name;
                feedback.UserName = "Default";
                feedback.Text = model.Text;
                feedback.PostDate = DateTime.Now;
                feedback.CategoryId = model.Category;

                var files = SaveUploadedFiles(model.Files);
                feedback.AttachedFiles = files.Select(file => new FeedBackFile
                {
                    FilePath = file
                }).ToList();

                if (feedback.Id == Guid.Empty)
                {
                    db.Feedbacks.Add(feedback);
                }
                else
                {
                    db.Entry(feedback).State = EntityState.Modified;
                }

                await db.SaveChangesAsync();
            }

            return RedirectToAction("ViewFeedbacks");
        }

        private async Task<List<CategoryViewModel>> GetCategories()
        {
            var categories = new List<CategoryViewModel>
            {
                new CategoryViewModel{Id = Guid.Empty, Name = "Не выбрана"}
            };

            using (var db = new FeedbackContext())
            {
                categories.AddRange(
                    await db.Categories.Select(t => new CategoryViewModel { Id = t.Id, Name = t.Name }).ToListAsync());
            }
            
            return categories;
        }

        private ICollection<string> SaveUploadedFiles(ICollection<HttpPostedFileBase> files)
        {
            if (files == null || !files.Any()) return new List<string>();

            //string user = User.Identity.Name;
            string user = "Default";
            var id = Guid.NewGuid().ToString();
            string path = Server.MapPath(string.Format("~/Uploads/{0}/{1}/", user, id));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileNames = new List<string>();

            foreach (var file in files)
            {
                fileNames.Add(Path.Combine(path, Path.GetFileName(file.FileName)));
                file.SaveAs(fileNames.Last());
            }

            return fileNames;
        }
    }
}