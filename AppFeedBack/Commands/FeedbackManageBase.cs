using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Utils;

namespace AppFeedBack.Commands
{
    public abstract class FeedbackManageBase
    {
        private readonly IServerFileManager _manager;
        private readonly IFeedbackRepository _repository;

        public IFeedbackRepository Repository { get { return _repository; } }

        public IServerFileManager ServerFileManager { get { return _manager; } }

        /// <summary>
        /// Инициализирует экземпляр FeedbackManageBase
        /// </summary>
        /// <param name="repository">Репозиторий для доступа к хранящимся отзывам</param>
        /// <param name="manager">Файловый менеджер сервера</param>
        protected FeedbackManageBase(IFeedbackRepository repository, IServerFileManager manager)
        {
            _repository = repository;
            _manager = manager;
        }
    }
}