using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Service.Interface;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests
{
    /// <summary>
    /// Test class for <see cref="AuthenticationController"/>.
    /// </summary>
    public class AuthenticationControllerTests
    {
        private readonly Mock<ICustomerService> _serviceMock;
        private readonly AuthenticationController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationControllerTests"/> class.
        /// Sets up the mock service and the controller.
        /// </summary>
        public AuthenticationControllerTests()
        {
            _serviceMock = new Mock<ICustomerService>();
            _controller = new AuthenticationController(_serviceMock.Object);
        }

        /// <summary>
        /// Tests the login method with valid data.
        /// </summary>
        [Fact]
        public async Task Login_ValidData_ReturnsOk()
        {
            // Arrange
            var model = new AuthenticationRequest
            {
                Username = "testuser",
                Password = "password"
            };
            var response = new CreateResponse
            {
                IsSuccess = true,
                Message = "Login successful"
            };
            _serviceMock.Setup(s => s.CreateJSONToken(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.Login(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the login method with invalid data.
        /// </summary>
        [Fact]
        public async Task Login_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = await _controller.Login(new AuthenticationRequest()) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Invalid data", result.Value);
        }

        /// <summary>
        /// Tests the login method when the service returns a failure response.
        /// </summary>
        [Fact]
        public async Task Login_ServiceFailure_ReturnsBadRequest()
        {
            // Arrange
            var model = new AuthenticationRequest
            {
                Username = "testuser",
                Password = "password"
            };
            var response = new CreateResponse
            {
                IsSuccess = false,
                Message = "Login failed"
            };
            _serviceMock.Setup(s => s.CreateJSONToken(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.Login(model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the login method when an exception is thrown.
        /// </summary>
        [Fact]
        public async Task Login_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var model = new AuthenticationRequest
            {
                Username = "testuser",
                Password = "password"
            };
            _serviceMock.Setup(s => s.CreateJSONToken(model)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Login(model) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.Equal("Test exception", result.Value);
        }
    }
}
