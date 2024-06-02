using AutoMapper;
using Moq;
using MovieTicketBooking.Business.Service;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using MovieTicketBooking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests
{
    /// <summary>
    /// Unit tests for the TheaterService class.
    /// </summary>
    public class TheaterServiceTests
    {
        private readonly Mock<ITheaterRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ITheaterService _theaterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterServiceTests"/> class.
        /// </summary>
        public TheaterServiceTests()
        {
            _repositoryMock = new Mock<ITheaterRepository>();
            _mapperMock = new Mock<IMapper>();
            _theaterService = new TheaterService(_repositoryMock.Object, _mapperMock.Object);
        }

        /// <summary>
        /// Test for GetTheater method to ensure it returns a list of theaters.
        /// </summary>
        [Fact]
        public async Task GetTheater_ShouldReturnListOfTheaters()
        {
            // Arrange
            var theaters = new List<Theater>
            {
                new Theater { Id = "1", TheaterName = "Theater 1" },
                new Theater { Id = "2", TheaterName = "Theater 2" }
            };

            _repositoryMock.Setup(repo => repo.GetTheater()).ReturnsAsync(theaters);

            // Act
            var result = await _theaterService.GetTheater();

            // Assert
            Assert.Equal(theaters, result);
            _repositoryMock.Verify(repo => repo.GetTheater(), Times.Once);
        }

        /// <summary>
        /// Test for GetTheater method to ensure it returns a theater by ID.
        /// </summary>
        [Fact]
        public async Task GetTheater_ById_ShouldReturnTheater()
        {
            // Arrange
            var theater = new Theater { Id = "1", TheaterName = "Theater 1" };
            _repositoryMock.Setup(repo => repo.GetTheater("1")).ReturnsAsync(theater);

            // Act
            var result = await _theaterService.GetTheater("1");

            // Assert
            Assert.Equal(theater, result);
            _repositoryMock.Verify(repo => repo.GetTheater("1"), Times.Once);
        }

        /// <summary>
        /// Test for AddTheater method to ensure it adds a new theater.
        /// </summary>
        [Fact]
        public async Task AddTheater_ShouldAddNewTheater()
        {
            // Arrange
            var theaterDto = new TheaterDto { TheaterName = "New Theater" };
            var theater = new Theater { Id = "1", TheaterName = "New Theater" };
            var response = new CreateResponse { IsSuccess = true, Message = "Theater added successfully." };

            _mapperMock.Setup(mapper => mapper.Map<Theater>(theaterDto)).Returns(theater);
            _repositoryMock.Setup(repo => repo.AddTheater(theater)).ReturnsAsync(response);

            // Act
            var result = await _theaterService.AddTheater(theaterDto);

            // Assert
            Assert.Equal(response, result);
            _mapperMock.Verify(mapper => mapper.Map<Theater>(theaterDto), Times.Once);
            _repositoryMock.Verify(repo => repo.AddTheater(theater), Times.Once);
        }

        /// <summary>
        /// Test for DeleteTheater method to ensure it deletes a theater by ID.
        /// </summary>
        [Fact]
        public async Task DeleteTheater_ShouldDeleteTheater()
        {
            // Arrange
            var response = new CreateResponse { IsSuccess = true, Message = "Theater deleted successfully." };
            _repositoryMock.Setup(repo => repo.DeleteTheater("1")).ReturnsAsync(response);

            // Act
            var result = await _theaterService.DeleteTheater("1");

            // Assert
            Assert.Equal(response, result);
            _repositoryMock.Verify(repo => repo.DeleteTheater("1"), Times.Once);
        }
    }
}
