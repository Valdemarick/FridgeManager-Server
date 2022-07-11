﻿using Application.Common.Interfaces.Managers;
using Moq;

namespace Tests.Mocks
{
    public class FakeUnitOfWork
    {
        public Mock<IUnitOfWork> Mock;
        public IUnitOfWork UnitOfWork;

        public FakeUnitOfWork()
        {
            Mock = new Mock<IUnitOfWork>();
            UnitOfWork = Mock.Object;
        }
    }
}