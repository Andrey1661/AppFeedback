using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppFeedBack.Commands;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Domain.Tests.Mocks;
using AppFeedBack.Utils;
using Ninject;
using NUnit.Framework;

namespace AppFeedBack.Domain.Tests.Tests
{
    [TestFixture]
    public class FeedbackDeleteCommandTest
    {
        [SetUp]
        public void SetUp()
        {
            var kernel = new StandardKernel();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            Initialize(kernel);
        }

        private void Initialize(IKernel kernel)
        {
            kernel.Bind<IFeedbackRepository>().ToMethod(t => kernel.Get<MockFeedbackRepository>().Object);
            kernel.Bind<IServerFileManager>().ToMethod(t => kernel.Get<MockServerFileManager>().Object);
        }

        [Test]
        public void Execute_Test()
        {
            var command = DependencyResolver.Current.GetService<FeedbackDeleteCommand>();
            var feedback = MockFeedbackRepository.Feedbacks.First();

            command.LoadData(feedback.Id, string.Empty);
            command.Execute();
            var first = MockFeedbackRepository.Feedbacks.First();

            Assert.AreEqual(4, MockFeedbackRepository.Feedbacks.Count);
            Assert.AreNotEqual(feedback, first);
        }
    }
}
