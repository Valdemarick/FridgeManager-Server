using AutoMapper;
using Moq;

namespace Tests.Mocks
{
    public class FakeMapper
    {
        public Mock<IMapper> Mock;
        public IMapper Mapper;

        public FakeMapper()
        {
            Mock = new Mock<IMapper>();
            Mapper = Mock.Object;
        }
    }
}