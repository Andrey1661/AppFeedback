using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppFeedBack.Commands;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Domain.Tests.Mocks;
using AppFeedBack.Utils;
using AppFeedBack.ViewModels;
using Moq;
using Ninject;
using NUnit.Framework;

namespace AppFeedBack.Domain.Tests.Tests
{
    [TestFixture]
    public class FeedbackStoreCommandTest
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
            var command = DependencyResolver.Current.GetService<FeedbackStoreCommand>();

            Guid id = Guid.NewGuid();
            string user = "test";
            string text = "test-text";

            var model = new FeedbackStoreViewModel
            {
                Id = id,
                CategoryId = Guid.Empty,
                UserName = user,
                Text = text
            };

            command.LoadData(model, string.Empty);
            command.Execute().Wait();

            var lastAdded = MockFeedbackRepository.Feedbacks.Last();

            Assert.AreEqual(id, lastAdded.Id);
            Assert.AreEqual(user, lastAdded.UserName);
            Assert.AreEqual(text, lastAdded.Text);
        }
    }
}
