using AutoMapper;
using Moq;
using MovieTicketBooking.Business.Service;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests
{
    /// <summary>
    /// Unit tests for the MovieService class.
    /// </summary>
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _repositoryMock = new Mock<IMovieRepository>();
            _mapperMock = new Mock<IMapper>();
            _movieService = new MovieService(_repositoryMock.Object, _mapperMock.Object);
        }

        /// <summary>
        /// Tests the Create method with valid movie data.
        /// </summary>
        [Fact]
        public async Task Create_ValidMovie_ReturnsCreateResponse()
        {
            // Arrange
            var movieDto = new MovieDto { MovieName = "Test Movie" };
            var movie = new Movie { MovieName = "Test Movie" };
            var createResponse = new CreateResponse { IsSuccess = true };

            _mapperMock.Setup(m => m.Map<Movie>(movieDto)).Returns(movie);
            _repositoryMock.Setup(r => r.Create(It.IsAny<Movie>())).ReturnsAsync(createResponse);

            // Act
            var result = await _movieService.Create(movieDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.Create(It.Is<Movie>(m => m.MovieName == "Test Movie")), Times.Once);
        }

        /// <summary>
        /// Tests the GetMovie method to retrieve all movies.
        /// </summary>
        [Fact]
        public async Task GetMovie_ReturnsListOfMovies()
        {
            // Arrange
            var movies = new List<Movie> { new Movie { MovieName = "Movie 1" }, new Movie { MovieName = "Movie 2" } };
            _repositoryMock.Setup(r => r.GetMovie()).ReturnsAsync(movies);

            // Act
            var result = await _movieService.GetMovie();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _repositoryMock.Verify(r => r.GetMovie(), Times.Once);
        }

        /// <summary>
        /// Tests the GetMovie method to retrieve a movie by its ID.
        /// </summary>
        [Fact]
        public async Task GetMovie_ValidId_ReturnsMovie()
        {
            // Arrange
            var movieId = "1";
            var movie = new Movie { Id = "1", MovieName = "Movie 1" };
            _repositoryMock.Setup(r => r.GetMovie(movieId)).ReturnsAsync(movie);

            // Act
            var result = await _movieService.GetMovie(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(movieId, result?.Id);
            _repositoryMock.Verify(r => r.GetMovie(movieId), Times.Once);
        }

        /// <summary>
        /// Tests the DeleteMovie method with a valid movie ID.
        /// </summary>
        [Fact]
        public async Task DeleteMovie_ValidId_ReturnsCreateResponse()
        {
            // Arrange
            var movieId = "1";
            var deleteResponse = new CreateResponse { IsSuccess = true };
            _repositoryMock.Setup(r => r.DeleteMovie(movieId)).ReturnsAsync(deleteResponse);

            // Act
            var result = await _movieService.DeleteMovie(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.DeleteMovie(movieId), Times.Once);
        }
    }
}
