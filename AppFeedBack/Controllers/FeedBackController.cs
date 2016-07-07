using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.ViewModels;

namespace AppFeedBack.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackStoreCommand _storeCommand;
        private readonly ICategoryListCreator _creator;
        private readonly IFeedbackRepository _repository;

        public FeedbackController(IFeedbackStoreCommand storeCommand, IFeedbackRepository repository, ICategoryListCreator creator)
        {
            _storeCommand = storeCommand;
            _creator = creator;
            _repository = repository;
        }

        /// <summary>
        /// Возвращает представление для создания и редактирования отзыва
        /// </summary>
        /// <param name="id">Id существующего отзыва для редактирования</param>
        /// <returns></returns>
        public async Task<ActionResult> StoreFeedback(Guid? id)
        {
            var model = new FeedbackStoreViewModel(_repository, _creator);
            await model.Initialize(id ?? Guid.Empty);

            return View(model);
        }

        /// <summary>
        /// Получает и сохраняет в базу данные для создания или изменения отзыва
        /// </summary>
        /// <param name="model">Модлеь с данными из представления</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> StoreFeedback(FeedbackStoreViewModel model)
        {
            if (!model.EditMode)
            {
                model.UserName = User.Identity.Name;
            }

            _storeCommand.LoadData(model, PathToUploadedFiles);
            await _storeCommand.Execute();
            return RedirectToAction("ViewFeedbacks", "Admin");
        }
    }
}