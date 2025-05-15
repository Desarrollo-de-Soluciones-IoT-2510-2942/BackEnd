

using Application;
using AutoMapper;
using Domain;
using Moq;
using Presentation.Request;
using Xunit;

namespace NutriControl.Application.Test.Subscription
{
    public class SubscriptionDomainUnitTest
    {
        [Fact]
        public async Task SaveSubscriptionAsync_ValidInput_ReturnValidId()
        {
            // Arrange
            var mockRepository = new Mock<ISubscriptionRepository>();
            var mockMapper = new Mock<IMapper>();

            var command = new CreateSubscriptionCommand
            {
                UserId = 1, 
                PlanType = PlanType.Basic,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };

            var subscriptionEntity = new global::Domain.Subscription
            {
                UserId = command.UserId,
                PlanType = command.PlanType,
                StartDate = command.StartDate,
                EndDate = command.EndDate
            };

            mockMapper.Setup(m => m.Map<CreateSubscriptionCommand, global::Domain.Subscription>(command)).Returns(subscriptionEntity);
            mockRepository.Setup(repo => repo.GetSubscriptionByIdAsync(It.IsAny<int>())).ReturnsAsync((global::Domain.Subscription)null);
            mockRepository.Setup(repo => repo.GetAllSubscriptionAsync()).ReturnsAsync(new List<global::Domain.Subscription>());
            mockRepository.Setup(repo => repo.SaveSubscriptionAsync(It.IsAny<global::Domain.Subscription>())).ReturnsAsync(1);

            var commandService = new SubscriptionCommandService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await commandService.Handle(command);

            // Assert
            Assert.Equal(1, result);
        }
    }
}