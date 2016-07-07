using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AppFeedBack.Utils
{
    public class ServerFileManager : IServerFileManager
    {
        /// <summary>
        /// Сохраняет переданные файлы на сервер в указанный каталог
        /// </summary>
        /// <param name="files">Файлы для сохранения</param>
        /// <param name="rootPath">Каталог, где будут созданы подкаталоги для сохранения файлов</param>
        /// <param name="path">Каталог для сохранения</param>
        /// <returns>Список полных имен сохраненный файлов</returns>
        public IEnumerable<string> SaveFilesToServer(IEnumerable<HttpPostedFileBase> files, string rootPath, string path)
        {
            var listFiles = files != null ? files.ToList() : null;

            if (files == null || !listFiles.Any()) yield break;

            string directory = Path.Combine(rootPath, path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (var file in listFiles.Where(file => file != null))
            {
                file.SaveAs(Path.Combine(directory, file.FileName));
                yield return Path.Combine(path, file.FileName);
            }
        }

        /// <summary>
        /// Удаляет каталог и все файлы в нем
        /// </summary>
        /// <param name="path">Путь к каталогу</param>
        public void DeleteAttachedFiles(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }       
        }

        /// <summary>
        /// Вовращает файл по указанному пути в виде массива байт. Если файл не сущетсвует, вернет null
        /// </summary>
        /// <param name="path">Физический путь к файлу</param>
        /// <returns></returns>
        public byte[] GetFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }

            return null;
        }
    }
}