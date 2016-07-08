using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Controllers;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;
using AppFeedBack.Domain.Tests.Mocks;
using AppFeedBack.ViewModels;
using Ninject;
using NUnit.Framework;

namespace AppFeedBack.Domain.Tests.Tests
{
    [TestFixture]
    public class AdminControllerTest
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
            kernel.Bind<IFeedbackDeleteCommand>().ToMethod(t => kernel.Get<MockFeedbackDeleteCommand>().Object);
        }

        [Test]
        public void ViewFeedbacks_Test()
        {
            var controller = DependencyResolver.Current.GetService<AdminController>();
            var result = controller.ViewFeedbacks("", "", 1).Result;

            var model = ((ViewResult) result).Model;     

            Assert.IsInstanceOf<IndexViewModel>(model);
            var itemsCount = (model as IndexViewModel).Feedbacks.Count;

            Assert.That(itemsCount == 3);
        }

        [Test]
        public void ViewFeedbacks_AuthorFilterTest()
        {
            var controller = DependencyResolver.Current.GetService<AdminController>();
            var result = controller.ViewFeedbacks("user1", "", 1).Result;
            var model = ((ViewResult)result).Model;

            var values = (model as IndexViewModel).Feedbacks.Select(t => t.Author).Distinct();

            Assert.That(values.Count() == 1);
            Assert.That(values.First() == "user1");
        }
    }
}
