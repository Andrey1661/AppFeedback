using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Commands.Interfaces;
using AppFeedBack.Domain.Entities;
using AppFeedBack.ViewModels;
using Moq;

namespace AppFeedBack.Domain.Tests.Mocks
{
    class MockFeedbackStoreCommand : Mock<IFeedbackStoreCommand>
    {
        private FeedbackStoreViewModel _model;

        internal MockFeedbackStoreCommand()
        {
            Setup(p => p.LoadData(It.IsAny<FeedbackStoreViewModel>(), It.IsAny<string>())).Callback((FeedbackStoreViewModel model, string path) =>
            {
                _model = model;
            });

            Setup(p => p.Execute()).Returns(() => Task.FromResult(Execute()));
        }

        private int Execute()
        {
            var feedback = new Feedback
            {
                Id = _model.Id ?? Guid.Empty,
                CategoryId = _model.CategoryId,
                AttachedFiles = _model.Files.Select(t => new FeedbackFile(t.FileName)).ToList(),
                PostDate = DateTime.Now,
                Text = _model.Text,
                UserName = _model.UserName
            };

            MockFeedbackRepository.Feedbacks.Add(feedback);
            return 0;
        }
    }
}
