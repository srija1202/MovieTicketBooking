using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Service.Interface;
using Xunit;

namespace MovieTicketBooking.Tests
{
    /// <summary>
    /// Test class for <see cref="CustomerController"/>.
    /// </summary>
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _serviceMock;
        private readonly CustomerController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerControllerTests"/> class.
        /// Sets up the mock service and the controller.
        /// </summary>
        public CustomerControllerTests()
        {
            _serviceMock = new Mock<ICustomerService>();
            _controller = new CustomerController(_serviceMock.Object);
        }

        /// <summary>
        /// Tests the CreateUser method with valid data.
        /// </summary>
        [Fact]
        public async Task CreateUser_ValidData_ReturnsOk()
        {
            // Arrange
            var model = new UserDto();
            var response = new CreateResponse { IsSuccess = true, Message = "User created" };
            _serviceMock.Setup(s => s.CreateUser(model, false)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateUser(model, false) as OkObjectResult;

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
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = await _controller.CreateUser(new UserDto(), false) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        /// <summary>
        /// Tests the UpdatePassword method with valid data.
        /// </summary>
        [Fact]
        public async Task UpdatePassword_ValidData_ReturnsOk()
        {
            // Arrange
            var userPassword = new UserPasswordUpdate { Username = "testuser", OldPassword = "oldpass", NewPassword = "newpass" };
            var response = new CreateResponse { IsSuccess = true, Message = "Password updated" };
            _serviceMock.Setup(s => s.PasswordUpdate(userPassword, userPassword.Username)).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdatePassword(userPassword) as OkObjectResult;

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
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = await _controller.UpdatePassword(new UserPasswordUpdate()) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}
