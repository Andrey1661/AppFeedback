using System;
using System.Web.Mvc;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Controllers;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Domain.Tests.Mocks;
using AppFeedBack.ViewModels;
using Ninject;
using NUnit.Framework;

namespace AppFeedBack.Domain.Tests.Tests
{
    [TestFixture]
    class FeedbackControllerTest
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
            kernel.Bind<ICategoryListCreator>().ToMethod(t => kernel.Get<MockCategoryListCreator>().Object);
            kernel.Bind<IFeedbackStoreCommand>().ToMethod(t => kernel.Get<MockFeedbackStoreCommand>().Object);
        }

        [Test]
        public void StoreFeedbackTest()
        {
            var controller = DependencyResolver.Current.GetService<FeedbackController>();
            var guid = Guid.NewGuid();
            var result = controller.StoreFeedback(guid).Result;

            var model = ((ViewResult) result).Model;
            Assert.IsInstanceOf<FeedbackStoreViewModel>(model);
            Assert.AreEqual(guid, (model as FeedbackStoreViewModel).Id);
        }
    }
}
