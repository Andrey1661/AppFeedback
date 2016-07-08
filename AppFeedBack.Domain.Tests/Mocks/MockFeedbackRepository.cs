using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Domain.Entities;
using AppFeedBack.Domain.Repositories.Interfaces;
using Moq;

namespace AppFeedBack.Domain.Tests.Mocks
{
    class MockFeedbackRepository : Mock<IFeedbackRepository>
    {
        public static List<Feedback> Feedbacks { get; set; }

        static MockFeedbackRepository()
        {
            CreateFeedbacks();
        }

        public MockFeedbackRepository()
        {
            CreateFeedbacks();

            Setup(p => p.Get(It.IsAny<Guid>())).Returns((Guid id) => Task.FromResult(GetFeedback(id)));

            Setup(p => p.Delete(It.IsAny<Guid>())).Callback((Guid id) => DeleteFeedback(id));

            Setup(
                p =>
                    p.GetPagedList(It.IsAny<string>(), It.Is<string>(t => t == string.Empty), It.IsAny<FeedbackOrderBy>(), It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(
                    (string user, string b, FeedbackOrderBy o, int page, int pageSize) =>
                        Task.FromResult(GetPagedList(Feedbacks.Where(t => t.UserName.Contains(user)), page, pageSize)));

            Setup(
                p =>
                    p.GetPagedList(It.Is<string>(t => t == string.Empty), It.IsAny<string>(), It.IsAny<FeedbackOrderBy>(), It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(
                    (string a, string category, FeedbackOrderBy o, int page, int pageSize) =>
                        Task.FromResult(GetPagedList(Feedbacks.Where(t => t.Category.Name == category), page, pageSize)));

            Setup(p => p.Insert(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), null))
                .Callback(
                    (Guid id, Guid categoryid, string text, string name, IEnumerable<string> file) =>
                        Insert(id, categoryid, text, name));
        }

        private static void Insert(Guid id, Guid categoryid, string text, string name)
        {
            Feedbacks.Add(new Feedback(id, categoryid, text, DateTime.Now, name));
        }

        private Feedback GetFeedback(Guid id)
        {
            return new Feedback {Id = id};
        }

        private static void DeleteFeedback(Guid id)
        {
            Feedbacks.Remove(Feedbacks.FirstOrDefault(t => t.Id == id));
        }

        private IPagedList<Feedback> GetPagedList(IEnumerable<Feedback> superset , int page, int pageSize)
        {
            superset = Feedbacks.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedList<Feedback>(superset, 1, 1, Feedbacks.Count);
        }

        private static void CreateFeedbacks()
        {
            Feedbacks = new List<Feedback>
            {
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    Text = "text1",
                    PostDate = DateTime.Now,
                    UserName = "user4",
                    Category = new Category{Name = "кат1"}
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    Text = "text2",
                    PostDate = DateTime.Now,
                    UserName = "user1",
                    Category = new Category{Name = "кат2"}
                },new Feedback
                {
                    Id = Guid.NewGuid(),
                    Text = "text3",
                    PostDate = DateTime.Now,
                    UserName = "user3",
                    Category = new Category{Name = "кат3"}
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    Text = "text4",
                    PostDate = DateTime.Now,
                    UserName = "user2",
                    Category = new Category{Name = "кат4"}
                },new Feedback
                {
                    Id = Guid.NewGuid(),
                    Text = "text5",
                    PostDate = DateTime.Now,
                    UserName = "user2",
                    Category = new Category{Name = "кат5"}    
                },
            };
        }
    }
}
