using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests
{
    public class TheaterControllerTests
    {
        private readonly Mock<ITheaterService> _mockService;
        private readonly TheaterController _controller;

        public TheaterControllerTests()
        {
            _mockService = new Mock<ITheaterService>();
            _controller = new TheaterController(_mockService.Object);
        }

        /// <summary>
        /// Verifies that CreateTheater returns OkObjectResult when theater creation is successful.
        /// </summary>
        [Fact]
        public async Task CreateTheater_Success_ReturnsOkResult()
        {
            // Arrange
            var model = new TheaterDto();
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(s => s.AddTheater(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateTheater(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(okResult.Value);
            Assert.True(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that CreateTheater returns BadRequestObjectResult when theater creation fails.
        /// </summary>
        [Fact]
        public async Task CreateTheater_Failure_ReturnsBadRequest()
        {
            // Arrange
            var model = new TheaterDto();
            var response = new CreateResponse { IsSuccess = false };
            _mockService.Setup(s => s.AddTheater(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateTheater(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(badRequestResult.Value);
            Assert.False(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that GetTheater returns OkObjectResult when theaters are found.
        /// </summary>
        [Fact]
        public async Task GetTheater_Success_ReturnsOkResult()
        {
            // Arrange
            var theaters = new List<Theater> { new Theater() };
            _mockService.Setup(s => s.GetTheater()).ReturnsAsync(theaters);

            // Act
            var result = await _controller.GetTheater();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Theater>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Verifies that GetTheater returns BadRequestObjectResult when no theaters are found.
        /// </summary>
        [Fact]
        public async Task GetTheater_NotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var theaters = new List<Theater>();
            _mockService.Setup(s => s.GetTheater()).ReturnsAsync(theaters);

            // Act
            var result = await _controller.GetTheater();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Verifies that GetTheater returns OkObjectResult when the theater is found by ID.
        /// </summary>
        [Fact]
        public async Task GetTheaterById_Success_ReturnsOkResult()
        {
            // Arrange
            var theater = new Theater();
            _mockService.Setup(s => s.GetTheater("1")).ReturnsAsync(theater);

            // Act
            var result = await _controller.GetTheater("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Theater>(okResult.Value);
            Assert.NotNull(returnValue);
        }

        /// <summary>
        /// Verifies that GetTheater returns BadRequestObjectResult when the theater is not found by ID.
        /// </summary>
        [Fact]
        public async Task GetTheaterById_NotFound_ReturnsBadRequestResult()
        {
            // Arrange
            Theater theater = null;
            _mockService.Setup(s => s.GetTheater("1")).ReturnsAsync(theater);

            // Act
            var result = await _controller.GetTheater("1");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Verifies that DeleteTheater returns OkObjectResult when theater deletion is successful.
        /// </summary>
        [Fact]
        public async Task DeleteTheater_Success_ReturnsOkResult()
        {
            // Arrange
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(s => s.DeleteTheater("1")).ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteTheater("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(okResult.Value);
            Assert.True(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that DeleteTheater returns BadRequestObjectResult when theater deletion fails.
        /// </summary>
        [Fact]
        public async Task DeleteTheater_Failure_ReturnsBadRequest()
        {
            // Arrange
            var response = new CreateResponse { IsSuccess = false };
            _mockService.Setup(s => s.DeleteTheater("1")).ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteTheater("1");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(badRequestResult.Value);
            Assert.False(returnValue.IsSuccess);
        }
    }
}
