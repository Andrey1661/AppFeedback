using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AppFeedBack.Utils;
using Moq;

namespace AppFeedBack.Domain.Tests.Mocks
{
    class MockServerFileManager : Mock<IServerFileManager>
    {
        internal MockServerFileManager()
        {
            Setup(
                p =>
                    p.SaveFilesToServer(It.IsAny<IEnumerable<HttpPostedFileBase>>(), It.IsAny<string>(),
                        It.IsAny<string>())).Returns(new List<string>());
            Setup(p => p.DeleteAttachedFiles(It.IsAny<string>()));
            Setup(p => p.DeleteAttachedFiles(It.IsAny<string>()));
        }
    }
}
