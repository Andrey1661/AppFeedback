using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFeedBack.Commands.Interfaces;
using Moq;

namespace AppFeedBack.Tests.Mocks
{
    public class MockCategoryListCreator : Mock<ICategoryListCreator>
    {
        public MockCategoryListCreator()
        {
            
        }
    }
}
