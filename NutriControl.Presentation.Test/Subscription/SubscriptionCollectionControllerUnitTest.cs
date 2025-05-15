

using _1_API.Response;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriControl.Domain.Subscriptions.Models.Queries;
using Presentation.Controllers;
using Xunit;

namespace NutriControl.Presentation.Test.Subscription
{
    public class SubscriptionCollectionControllerUnitTest
    {
        [Fact]
        public async Task GetAsync_ResultOk()
        {
            // Arrange
            var mockQueryService = new Mock<ISubscriptionQueryService>();
            var mockCommandService = new Mock<ISubscriptionCommandService>();
            var mockMapper = new Mock<IMapper>();
            var fakeList = new List<SubscriptionResponse> { new SubscriptionResponse() };

            mockQueryService.Setup(s => s.Handle(It.IsAny<GetAllSusbcriptionsQuery>())).ReturnsAsync(fakeList);

            var controller = new SubscriptionController(mockQueryService.Object, mockCommandService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAsync_ResultNotFound()
        {
            // Arrange
            var mockQueryService = new Mock<ISubscriptionQueryService>();
            var mockCommandService = new Mock<ISubscriptionCommandService>();
            var mockMapper = new Mock<IMapper>();
            var fakeList = new List<SubscriptionResponse>();

            mockQueryService.Setup(s => s.Handle(It.IsAny<GetAllSusbcriptionsQuery>())).ReturnsAsync(fakeList);

            var controller = new SubscriptionController(mockQueryService.Object, mockCommandService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}