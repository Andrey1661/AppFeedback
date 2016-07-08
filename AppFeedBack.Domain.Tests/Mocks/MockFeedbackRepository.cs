using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;
using Moq;

namespace AppFeedBack.Domain.Tests.Mocks
{
    class MockFeedbackRepository : Mock<IFeedbackRepository>
    {
        public MockFeedbackRepository()
        {
            Setup(p => p.Get(It.IsAny<Guid>())).Returns((Guid id) => Task.FromResult(GetFeedback(id)));
        }

        private Feedback GetFeedback(Guid id)
        {
            return new Feedback
            {
                Id = id
            };
        }
    }
}
