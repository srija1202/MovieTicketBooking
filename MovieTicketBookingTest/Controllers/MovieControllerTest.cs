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
    public class MovieControllerTests
    {
        private readonly Mock<IMovieService> _mockService;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _mockService = new Mock<IMovieService>();
            _controller = new MovieController(_mockService.Object);
        }

        /// <summary>
        /// Verifies that CreateMovie returns OkObjectResult when movie creation is successful.
        /// </summary>
        [Fact]
        public async Task CreateMovie_Success_ReturnsOkResult()
        {
            // Arrange
            var model = new MovieDto();
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(s => s.Create(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateMovie(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(okResult.Value);
            Assert.True(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that CreateMovie returns BadRequestObjectResult when movie creation fails.
        /// </summary>
        [Fact]
        public async Task CreateMovie_Failure_ReturnsBadRequest()
        {
            // Arrange
            var model = new MovieDto();
            var response = new CreateResponse { IsSuccess = false };
            _mockService.Setup(s => s.Create(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateMovie(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(badRequestResult.Value);
            Assert.False(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that GetMovies returns OkObjectResult when movies are found.
        /// </summary>
        [Fact]
        public async Task GetMovies_Success_ReturnsOkResult()
        {
            // Arrange
            var movies = new List<Movie> { new Movie() };
            _mockService.Setup(s => s.GetMovie()).ReturnsAsync(movies);

            // Act
            var result = await _controller.GetMovies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Movie>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Verifies that GetMovies returns BadRequestObjectResult when no movies are found.
        /// </summary>
        [Fact]
        public async Task GetMovies_NotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var movies = new List<Movie>();
            _mockService.Setup(s => s.GetMovie()).ReturnsAsync(movies);

            // Act
            var result = await _controller.GetMovies();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Verifies that GetMovieById returns OkObjectResult when the movie is found.
        /// </summary>
        [Fact]
        public async Task GetMovieById_Success_ReturnsOkResult()
        {
            // Arrange
            var movie = new Movie();
            _mockService.Setup(s => s.GetMovie("1")).ReturnsAsync(movie);

            // Act
            var result = await _controller.GetMovieById("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Movie>(okResult.Value);
            Assert.NotNull(returnValue);
        }

        /// <summary>
        /// Verifies that GetMovieById returns BadRequestObjectResult when the movie is not found.
        /// </summary>
        [Fact]
        public async Task GetMovieById_NotFound_ReturnsBadRequestResult()
        {
            // Arrange
            Movie movie = null;
            _mockService.Setup(s => s.GetMovie("1")).ReturnsAsync(movie);

            // Act
            var result = await _controller.GetMovieById("1");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Verifies that DeleteMovie returns OkObjectResult when movie deletion is successful.
        /// </summary>
        [Fact]
        public async Task DeleteMovie_Success_ReturnsOkResult()
        {
            // Arrange
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(s => s.DeleteMovie("1")).ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteMovie("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(okResult.Value);
            Assert.True(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that DeleteMovie returns BadRequestObjectResult when movie deletion fails.
        /// </summary>
        [Fact]
        public async Task DeleteMovie_Failure_ReturnsBadRequest()
        {
            // Arrange
            var response = new CreateResponse { IsSuccess = false };
            _mockService.Setup(s => s.DeleteMovie("1")).ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteMovie("1");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(badRequestResult.Value);
            Assert.False(returnValue.IsSuccess);
        }
    }
}
