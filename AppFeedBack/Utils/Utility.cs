using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AppFeedBack.Utils
{
    public class Utility
    {
        /// <summary>
        /// Сохраняет переданные файлы на сервер в указанный каталог
        /// </summary>
        /// <param name="files">Файлы для сохранения</param>
        /// <param name="rootPath">Каталог, где будут созданы подкаталоги для сохранения файлов</param>
        /// <param name="path">Каталог для сохранения</param>
        /// <returns>Список полных имен сохраненный файлов</returns>
        public static IEnumerable<string> SaveFilesToServer(IEnumerable<HttpPostedFileBase> files, string rootPath, string path)
        {
            if (files == null || !files.Any()) yield break;

            string directory = Path.Combine(rootPath, path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (var file in files.Where(file => file != null))
            {
                file.SaveAs(Path.Combine(directory, file.FileName));
                yield return Path.Combine(path, file.FileName);
            }
        }

        /// <summary>
        /// Удаляет каталог и все файлы в нем
        /// </summary>
        /// <param name="path">Путь к каталогу</param>
        public static void DeleteAttachedFiles(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }       
        }
    }
}