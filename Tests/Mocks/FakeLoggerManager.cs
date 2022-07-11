using Application.Common.Interfaces.Managers;
using Moq;

namespace Tests.Mocks
{
    public class FakeLoggerManager
    {
        public Mock<ILoggerManager> Mock;
        public ILoggerManager LoggerManager;

        public FakeLoggerManager()
        {
            Mock = new Mock<ILoggerManager>();
            LoggerManager = Mock.Object;
        }
    }
}