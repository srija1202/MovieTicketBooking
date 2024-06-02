using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Service.Interface;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests.Controllers
{
    /// <summary>
    /// Test class for CustomerController.
    /// </summary>
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockService.Object);
        }

        /// <summary>
        /// Tests the CreateUser method with valid data.
        /// </summary>
        [Fact]
        public async Task CreateUser_ValidData_ReturnsOk()
        {
            // Arrange
            var userDto = new UserDto { /* Initialize with valid data */ };
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(service => service.CreateUser(userDto, It.IsAny<bool>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateUser(userDto, false) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the CreateUser method with invalid data.
        /// </summary>
        [Fact]
        public async Task CreateUser_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");
            var userDto = new UserDto { /* Initialize with invalid data */ };

            // Act
            var result = await _controller.CreateUser(userDto, false) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Provide valid data", result.Value);
        }

        /// <summary>
        /// Tests the UpdatePassword method with valid data.
        /// </summary>
        [Fact]
        public async Task UpdatePassword_ValidData_ReturnsOk()
        {
            // Arrange
            var userPasswordUpdate = new UserPasswordUpdate { Username = "testuser" /* Initialize with valid data */ };
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(service => service.PasswordUpdate(userPasswordUpdate, It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdatePassword(userPasswordUpdate) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the UpdatePassword method with invalid data.
        /// </summary>
        [Fact]
        public async Task UpdatePassword_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Password", "Required");
            var userPasswordUpdate = new UserPasswordUpdate { Username = "testuser" /* Initialize with invalid data */ };

            // Act
            var result = await _controller.UpdatePassword(userPasswordUpdate) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("No user found", result.Value);
        }
    }
}
