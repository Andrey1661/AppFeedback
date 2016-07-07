using System.Web.Mvc;

namespace AppFeedBack.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Возвращает физический путь к каталогу, в котором хранятся прикрепленные пользователями файлы
        /// </summary>
        protected string PathToUploadedFiles
        {
            get { return Server.MapPath("~/Uploads"); }
        }
    }
}