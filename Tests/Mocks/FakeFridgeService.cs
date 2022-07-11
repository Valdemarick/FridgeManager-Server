using Application.Common.Interfaces.Services;
using Moq;

namespace Tests.Mocks
{
    public class FakeFridgeService
    {
        public Mock<IFridgeService> Mock;
        public IFridgeService Service;

        public FakeFridgeService()
        {
            Mock = new Mock<IFridgeService>();
            Service = Mock.Object;
        }
    }
}