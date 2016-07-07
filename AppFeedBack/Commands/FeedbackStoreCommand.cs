using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Utils;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Commands
{
    public class FeedbackStoreCommand : FeedbackManageBase, IFeedbackStoreCommand
    {
        private FeedbackStoreViewModel _model;
        private string _path;

        /// <summary>
        /// Создает или изменяет отзыв пользователя в базе, используя готовую модель, и сохраняет файлы, переданные пользователем, по указанному пути на сервере
        /// </summary>
        public async Task Execute()
        {
            var id = _model.Id ?? Guid.NewGuid();

            if (_model.EditMode)
            {
                await Repository.Update(id, _model.Text);
                return;
            }

            var files = ServerFileManager.SaveFilesToServer(_model.Files, _path, id.ToString());

            await Repository.Insert(
                id,
                _model.CategoryId,
                _model.Text,
                _model.UserName,
                files
                );
        }

        /// <summary>
        /// Инициализирует экземпляр FeedbackStoreCommand
        /// </summary>
        /// <param name="repository">Репозиторий для доступа к хранящимся отзывам</param>
        /// <param name="manager">Файловый менеджер сервера</param>
        public FeedbackStoreCommand(IFeedbackRepository repository, IServerFileManager manager) : base(repository, manager) { }

        public void LoadData(FeedbackStoreViewModel model, string path)
        {
            _model = model;
            _path = path;
        }
    }
}