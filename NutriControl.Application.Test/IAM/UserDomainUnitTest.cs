

using Application.IAM.CommandServices;
using AutoMapper;
using Moq;
using NutriControl.Domain.IAM.Models.Comands;
using NutriControl.Domain.IAM.Repositories;
using NutriControl.Domain.IAM.Services;
using Xunit;

namespace NutriControl.Application.Test.IAM
{
    public class UserDomainUnitTest
    {
        [Fact]
        public async Task RegisterUserAsync_ValidInput_ReturnValidId()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var mockEncryptService = new Mock<IEncryptService>();
            var mockTokenService = new Mock<ITokenService>();
            var mockMapper = new Mock<IMapper>();

            var command = new SingupCommand
            {
                Username = "JhonMendoza",
                DniOrRuc = "74385278",
                EmailAddress = "jhonbur@email.com",
                Phone = "987350322",
                Role = "Farmer",
                PasswordHashed = "Password123!",
                ConfirmPassword = "Password123!"
            };

            mockRepository.Setup(r => r.GetUserByUserNameAsync(command.Username)).ReturnsAsync((User)null);
            mockRepository.Setup(r => r.GetUserByDniOrRucAsync(command.DniOrRuc)).ReturnsAsync((User)null);
            mockEncryptService.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("hashedPassword");
            mockRepository.Setup(r => r.RegisterAsync(It.IsAny<User>())).ReturnsAsync(1);

            var service = new UserCommandService(
                mockRepository.Object,
                mockEncryptService.Object,
                mockTokenService.Object,
                mockMapper.Object
            );

            // Act
            var result = await service.Handle(command);

            // Assert
            Assert.Equal(1, result);
        }
    }
}