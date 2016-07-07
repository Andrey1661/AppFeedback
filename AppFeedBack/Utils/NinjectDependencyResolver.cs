using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppFeedBack.Commands;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Repositories;
using AppFeedBack.Domain.Repositories.Interfaces;
using Ninject;

namespace AppFeedBack.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void AddBindings()
        {
            kernel.Bind<IFeedbackRepository>().To<FeedbackRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<IFeedbackDeleteCommand>().To<FeedbackDeleteCommand>();
            kernel.Bind<IFeedbackStoreCommand>().To<FeedbackStoreCommand>();
            kernel.Bind<IServerFileManager>().To<ServerFileManager>();
            kernel.Bind<ICategoryListCreator>().To<CategoryListCreator>();
        }
    }
}