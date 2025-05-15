

using _1_API.Response;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriControl.Domain.Fields.Models.Queries;
using Presentation.Controllers;
using Xunit;

namespace NutriControl.Presentation.Test.Field
{
    public class FieldCollectionControllerUnitTest
    {
        [Fact]
        public async Task GetAsync_ResultOk()
        {
            // Arrange
            var mockQueryService = new Mock<IFieldQueryService>();
            var mockCommandService = new Mock<IFieldCommandService>();
            var mockMapper = new Mock<IMapper>();
            var fakeList = new List<FieldResponse> { new FieldResponse() };

            mockQueryService.Setup(s => s.Handle(It.IsAny<GetAllFieldsQuery>())).ReturnsAsync(fakeList);

            var controller = new FieldController(mockQueryService.Object, mockCommandService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAsync_ResultNotFound()
        {
            // Arrange
            var mockQueryService = new Mock<IFieldQueryService>();
            var mockCommandService = new Mock<IFieldCommandService>();
            var mockMapper = new Mock<IMapper>();
            var fakeList = new List<FieldResponse>();

            mockQueryService.Setup(s => s.Handle(It.IsAny<GetAllFieldsQuery>())).ReturnsAsync(fakeList);

            var controller = new FieldController(mockQueryService.Object, mockCommandService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}