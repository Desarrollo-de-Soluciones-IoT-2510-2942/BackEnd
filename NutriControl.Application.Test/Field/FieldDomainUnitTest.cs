

using AutoMapper;
using Domain;
using Moq;
using NutriControl.Domain.Fields.Models.Entities;
using Presentation.Request;
using Xunit;

namespace Application.Test
{
    public class FieldDomainUnitTest
    {
        [Fact]
        public async Task SaveFieldAsync_ValidInput_ReturnValidId()
        {
            // Arrange
            var mockRepository = new Mock<IFieldRepository>();
            var mockMapper = new Mock<IMapper>();

            var command = new CreateFieldCommand
            {
                UserId = 1, 
                Name = "Campo SCA",
                Location = "Santa Cruz",
                SoilType = "Arenoso",
                Elevation = 500
            };

            var fieldEntity = new Field
            {
                UserId = command.UserId,
                Name = command.Name,
                Location = command.Location,
                SoilType = command.SoilType,
                Elevation = command.Elevation
            };

            mockMapper.Setup(m => m.Map<CreateFieldCommand, Field>(command)).Returns(fieldEntity);
            mockRepository.Setup(repo => repo.GetFieldByIdAsync(It.IsAny<int>())).ReturnsAsync((Field)null);
            mockRepository.Setup(repo => repo.GetAllFieldsAsync()).ReturnsAsync(new List<Field>());
            mockRepository.Setup(repo => repo.SaveFieldAsync(It.IsAny<Field>())).ReturnsAsync(1);

            var commandService = new FieldCommandService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await commandService.Handle(command);

            // Assert
            Assert.Equal(1, result);
        }
    }
}