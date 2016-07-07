using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;
using Moq;

namespace AppFeedBack.Tests.Mocks
{
    public class MockFeedbackRepository : Mock<IFeedbackRepository>
    {
        private List<Feedback> _feedbacks;

        public MockFeedbackRepository()
        {
            Setup(t => t.Get(It.Is<Guid>(id => id != Guid.Empty)))
                .Returns((Guid id) => Task.FromResult(_feedbacks.FirstOrDefault(t => t.Id == id)));
        }
    }
}
