using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppFeedBack.Utils
{
    public interface IServerFileManager
    {
        IEnumerable<string> SaveFilesToServer(IEnumerable<HttpPostedFileBase> files, string rootPath, string path);

        void DeleteAttachedFiles(string path);

        byte[] GetFile(string path);
    }
}
