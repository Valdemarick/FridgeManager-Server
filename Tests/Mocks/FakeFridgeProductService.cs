using Application.Common.Interfaces.Services;
using Moq;

namespace Tests.Mocks
{
    public class FakeFridgeProductService
    {
        public Mock<IFridgeProductService> Mock;
        public IFridgeProductService Service;

        public FakeFridgeProductService()
        {
            Mock = new Mock<IFridgeProductService>();
            Service = Mock.Object;
        }
    }
}