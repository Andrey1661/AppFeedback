using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.ViewModels;
using Moq;

namespace AppFeedBack.Domain.Tests.Mocks
{
    public class MockCategoryListCreator : Mock<ICategoryListCreator>
    {
        public MockCategoryListCreator()
        {
            Setup(p => p.GetCategories(It.IsAny<string>()))
                .Returns((string t) => Task.FromResult(FakeCategoriesList(t)));
        }

        private IEnumerable<CategoryViewModel> FakeCategoriesList(string name)
        {
            return new List<CategoryViewModel>
            {
                new CategoryViewModel(Guid.Empty, name),
                new CategoryViewModel(Guid.Empty, "Кат1"),
                new CategoryViewModel(Guid.Empty, "Кат2")
            };
        } 
    }
}
