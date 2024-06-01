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
    /// <summary>
    /// Test class for <see cref="TheaterController"/>.
    /// </summary>
    public class TheaterControllerTests
    {
        private readonly Mock<ITheaterService> _serviceMock;
        private readonly TheaterController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterControllerTests"/> class.
        /// Sets up the mock service and the controller.
        /// </summary>
        public TheaterControllerTests()
        {
            _serviceMock = new Mock<ITheaterService>();
            _controller = new TheaterController(_serviceMock.Object);
        }

        /// <summary>
        /// Tests the CreateTheater method with valid data.
        /// </summary>
        [Fact]
        public async Task CreateTheater_ValidData_ReturnsOk()
        {
            // Arrange
            var model = new TheaterDto();
            var response = new CreateResponse { IsSuccess = true, Message = "Theater created" };
            _serviceMock.Setup(s => s.AddTheater(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateTheater(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the CreateTheater method with invalid data.
        /// </summary>
        [Fact]
        public async Task CreateTheater_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.CreateTheater(new TheaterDto()) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        /// <summary>
        /// Tests the GetTheater method.
        /// </summary>
        [Fact]
        public async Task GetTheater_ReturnsListOfTheaters()
        {
            // Arrange
            var theaters = new List<Theater>();
            _serviceMock.Setup(s => s.GetTheater()).ReturnsAsync(theaters);

            // Act
            var result = await _controller.GetTheater() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(theaters, result.Value);
        }

        /// <summary>
        /// Tests the GetTheater method with valid ID.
        /// </summary>
        [Fact]
        public async Task GetTheater_ValidId_ReturnsTheater()
        {
            // Arrange
            var id = "123";
            var theater = new Theater();
            _serviceMock.Setup(s => s.GetTheater(id)).ReturnsAsync(theater);

            // Act
            var result = await _controller.GetTheater(id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(theater, result.Value);
        }

        /// <summary>
        /// Tests the GetTheater method with invalid ID.
        /// </summary>
        [Fact]
        public async Task GetTheater_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = "invalid";
            _serviceMock.Setup(s => s.GetTheater(id)).ReturnsAsync((Theater)null);

            // Act
            var result = await _controller.GetTheater(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        /// <summary>
        /// Tests the DeleteTheater method with valid ID.
        /// </summary>
        [Fact]
        public async Task DeleteTheater_ValidId_ReturnsOk()
        {
            // Arrange
            var id = "123";
            var response = new CreateResponse { IsSuccess = true, Message = "Theater deleted" };
            _serviceMock.Setup(s => s.DeleteTheater(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteTheater(id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the DeleteTheater method with invalid ID.
        /// </summary>
        [Fact]
        public async Task DeleteTheater_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = "invalid";
            _serviceMock.Setup(s => s.DeleteTheater(id)).ReturnsAsync(new CreateResponse { IsSuccess = false, Message = "Theater not found" });

            // Act
            var result = await _controller.DeleteTheater(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}
