using Application.Common.Interfaces.Services;
using Moq;

namespace Tests.Mocks
{
    public class FakeFridgeModelService
    {
        public Mock<IFridgeModelService> Mock;
        public IFridgeModelService Service;

        public FakeFridgeModelService()
        {
            Mock = new Mock<IFridgeModelService>();
            Service = Mock.Object;
        }
    }
}