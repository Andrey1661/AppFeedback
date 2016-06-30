using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppFeedBack.Domain;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Utils
{
    /// <summary>
    /// Класс, предоставлящие методы общего пользования
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Сохраняет переданные файлы на сервер в указанный каталог
        /// </summary>
        /// <param name="files">Файлы для сохранения</param>
        /// <param name="path">Каталог для сохранения</param>
        /// <returns>Список полных имен сохраненный файлов</returns>
        public static ICollection<string> SaveFilesToServer(ICollection<HttpPostedFileBase> files, string path)
        {
            if (files == null || !files.Any()) return new List<string>();

            //string user = User.Identity.Name;
            string user = "Default";

            var id = Guid.NewGuid().ToString();
            path = Path.Combine(path, id);

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