using Application.Common.Interfaces.Services;
using Moq;

namespace Tests.Mocks
{
    public class FakeProductService
    {
        public Mock<IProductService> Mock;
        public IProductService Service;

        public FakeProductService()
        {
            Mock = new Mock<IProductService>();
            Service = Mock.Object;
        }
    }
}