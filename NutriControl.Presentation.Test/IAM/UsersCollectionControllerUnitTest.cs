

using _1_API.Response;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriControl.Domain.IAM.Queries;
using NutriControl.Domain.IAM.Services;
using NutriControl.Presentation.IAM.Controllers;
using Xunit;

namespace NutriControl.Presentation.Test.IAM
{
    public class UserControllerUnitTest
    {
        [Fact]
        public async Task GetAsync_ResultOk()
        {
            // Arrange
            var mockQueryService = new Mock<IUserQueryService>();
            var mockMapper = new Mock<IMapper>();
            var fakeList = new List<UserResponse> { new UserResponse() };

            mockQueryService.Setup(s => s.Handle(It.IsAny<GetUserAllQuery>())).ReturnsAsync(fakeList);

            var controller = new UserController(null, mockQueryService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAsync_ResultNotFound()
        {
            // Arrange
            var mockQueryService = new Mock<IUserQueryService>();
            var mockMapper = new Mock<IMapper>();
            var fakeList = new List<UserResponse>();

            mockQueryService.Setup(s => s.Handle(It.IsAny<GetUserAllQuery>())).ReturnsAsync(fakeList);

            var controller = new UserController(null, mockQueryService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}