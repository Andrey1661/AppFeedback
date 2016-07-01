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
        public static IEnumerable<string> SaveFilesToServer(ICollection<HttpPostedFileBase> files, string path)
        {
            var filePaths = new List<string>();

            if (files == null || !files.Any()) return filePaths;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in files)
            {
                if (file != null)
                {
                    filePaths.Add(Path.Combine(path, file.FileName));
                    file.SaveAs(filePaths.Last());
                }          
            }

            return filePaths;
        }
    }
}