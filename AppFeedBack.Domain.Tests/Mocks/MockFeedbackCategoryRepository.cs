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
    class MockCategoryRepository : Mock<ICategoryRepository>
    {
        public static List<Category> Categories { get; set; }

        static MockCategoryRepository()
        {
            CreateCategories();
        }

        public MockCategoryRepository()
        {
            Setup(p => p.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Task.FromResult(Categories.FirstOrDefault(t => t.Id == id)));
            Setup(p => p.GetActive()).Returns(() => Task.FromResult(Categories.Where(t => t.IsActive)));
            Setup(p => p.GetList()).Returns(() => Task.FromResult(Categories.AsEnumerable()));
        }

        internal Category GetCategory(Guid id)
        {
            return Categories.FirstOrDefault(t => t.Id == id);
        }

        private static void CreateCategories()
        {
            Categories = new List<Category>
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Name = "кат1"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Name = "кат2"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Name = "кат3"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    IsActive = false,
                    Name = "кат4"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    IsActive = false,
                    Name = "кат5"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Name = "кат6"
                },
            };
        }
    }
}
