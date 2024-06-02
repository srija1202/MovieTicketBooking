using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Service.Interface;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests.Controllers
{
    /// <summary>
    /// Test class for AuthenticationController.
    /// </summary>
    public class AuthenticationControllerTests
    {
        private readonly Mock<ICustomerService> _mockService;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new AuthenticationController(_mockService.Object);
        }

        /// <summary>
        /// Tests the Login method with valid data.
        /// </summary>
        [Fact]
        public async Task Login_ValidData_ReturnsOk()
        {
            // Arrange
            var authRequest = new AuthenticationRequest { /* Initialize with valid data */ };
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(service => service.CreateJSONToken(authRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Login(authRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the Login method with invalid data.
        /// </summary>
        [Fact]
        public async Task Login_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Required");
            var authRequest = new AuthenticationRequest { /* Initialize with invalid data */ };

            // Act
            var result = await _controller.Login(authRequest) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Invalid data", result.Value);
        }

        /// <summary>
        /// Tests the Login method when an exception is thrown.
        /// </summary>
        [Fact]
        public async Task Login_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var authRequest = new AuthenticationRequest { /* Initialize with valid data */ };
            _mockService.Setup(service => service.CreateJSONToken(authRequest)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Login(authRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.Equal("Test exception", result.Value);
        }
    }
}
