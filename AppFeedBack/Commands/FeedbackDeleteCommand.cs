using System;
using System.IO;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Utils;

namespace AppFeedBack.Commands
{
    public class FeedbackDeleteCommand : FeedbackManageBase, IFeedbackDeleteCommand
    {
        private Guid _id;
        private string _path;

        /// <summary>
        /// Получает значения, необходимые для выполнения команды
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        public void LoadData(Guid id, string path)
        {
            _id = id;
            _path = path;
        }

        /// <summary>
        /// Удаляет отзыв с указанным id и все прикрепленные файлы
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task Execute()
        {
            ServerFileManager.DeleteAttachedFiles(Path.Combine(_path, _id.ToString()));
            await Repository.Delete(_id);
        }

        /// <summary>
        /// Инициализирует экземпляр FeedbackDeleteCommand
        /// </summary>
        /// <param name="repository">Репозиторий для доступа к хранящимся отзывам</param>
        /// <param name="manager">Файловый менеджер сервера</param>
        public FeedbackDeleteCommand(IFeedbackRepository repository, IServerFileManager manager) : base(repository, manager){}
    }
}