using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppFeedBack.Domain;
using AppFeedBack.Domain.Entities;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Utils
{
    /// <summary>
    /// Класс, используемый для взаимодействия с базой данных
    /// </summary>
    public static class DbManager
    {
        /// <summary>
        /// Создает или изменяет отзыв пользователя в базе, используя готовую модель, и сохраняет файлы, переданные пользователем, по указанному пути на сервере
        /// </summary>
        /// <param name="model">Модель представления, хранящая данные для создания или изменения отзыва</param>
        /// <param name="userName">Имя пользователя, написавшего отзыв</param>
        /// <param name="path">Путь к корневому каталогу, в который будут сохранены файлы, прикрепленные пользователем</param>
        public static async Task<int> StoreFeedback(FeedbackCreateViewModel model, string userName, string path = "")
        {
            using (var db = new FeedbackContext())
            {
                var feedback = await db.Feedbacks.FindAsync(model.Id) ?? new Feedback();
                feedback.UserName = "Default";
                feedback.Text = model.Text;
                feedback.PostDate = DateTime.Now;
                feedback.CategoryId = model.Category;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    string fullPath = Path.Combine(path, userName);

                    var files = Utility.SaveFilesToServer(model.Files, fullPath);

                    feedback.AttachedFiles = files.Select(file => new FeedBackFile
                    {
                        FilePath = file
                    }).ToList();
                }          

                if (feedback.Id == Guid.Empty)
                {
                    db.Feedbacks.Add(feedback);
                }
                else
                {
                    db.Entry(feedback).State = EntityState.Modified;
                }

                return await db.SaveChangesAsync();
            }
        }
    }
}