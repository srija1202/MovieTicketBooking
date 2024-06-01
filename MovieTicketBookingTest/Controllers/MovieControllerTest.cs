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
    /// Test class for <see cref="MovieController"/>.
    /// </summary>
    public class MovieControllerTests
    {
        private readonly Mock<IMovieService> _serviceMock;
        private readonly MovieController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieControllerTests"/> class.
        /// Sets up the mock service and the controller.
        /// </summary>
        public MovieControllerTests()
        {
            _serviceMock = new Mock<IMovieService>();
            _controller = new MovieController(_serviceMock.Object);
        }

        /// <summary>
        /// Tests the CreateMovie method with valid data.
        /// </summary>
        [Fact]
        public async Task CreateMovie_ValidData_ReturnsOk()
        {
            // Arrange
            var model = new MovieDto();
            var response = new CreateResponse { IsSuccess = true, Message = "Movie created" };
            _serviceMock.Setup(s => s.Create(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateMovie(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the CreateMovie method with invalid data.
        /// </summary>
        [Fact]
        public async Task CreateMovie_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.CreateMovie(new MovieDto()) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        /// <summary>
        /// Tests the GetMovies method.
        /// </summary>
        [Fact]
        public async Task GetMovies_ReturnsListOfMovies()
        {
            // Arrange
            var movies = new List<Movie>();
            _serviceMock.Setup(s => s.GetMovie()).ReturnsAsync(movies);

            // Act
            var result = await _controller.GetMovies() as OkObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(movies, result.Value);
        }

        /// <summary>
        /// Tests the GetMovieById method with valid ID.
        /// </summary>
        [Fact]
        public async Task GetMovieById_ValidId_ReturnsMovie()
        {
            // Arrange
            var id = "123";
            var movie = new Movie();
            _serviceMock.Setup(s => s.GetMovie(id)).ReturnsAsync(movie);

            // Act
            var result = await _controller.GetMovieById(id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(movie, result.Value);
        }

        /// <summary>
        /// Tests the GetMovieById method with invalid ID.
        /// </summary>
        [Fact]
        public async Task GetMovieById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = "invalid";
            _serviceMock.Setup(s => s.GetMovie(id)).ReturnsAsync((Movie)null);

            // Act
            var result = await _controller.GetMovieById(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        /// <summary>
        /// Tests the DeleteMovie method with valid ID.
        /// </summary>
        [Fact]
        public async Task DeleteMovie_ValidId_ReturnsOk()
        {
            // Arrange
            var id = "123";
            var response = new CreateResponse { IsSuccess = true, Message = "Movie deleted" };
            _serviceMock.Setup(s => s.DeleteMovie(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteMovie(id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the DeleteMovie method with invalid ID.
        /// </summary>
        [Fact]
        public async Task DeleteMovie_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = "invalid";
            _serviceMock.Setup(s => s.DeleteMovie(id)).ReturnsAsync(new CreateResponse { IsSuccess = false, Message = "Movie not found" });

            // Act
            var result = await _controller.DeleteMovie(id) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}
