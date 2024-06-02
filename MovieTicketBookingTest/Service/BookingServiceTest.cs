using AutoMapper;
using Moq;
using MovieTicketBooking.Business.Service;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using Xunit;

namespace MovieTicketBooking.Tests
{
    public class BookingServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBookingRepository> _mockRepository;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IBookingRepository>();
            _service = new BookingService(_mockMapper.Object, _mockRepository.Object);
        }

        /// <summary>
        /// Verifies that ReteriveTicktes returns a list of tickets booked by a user.
        /// </summary>
        [Fact]
        public async Task ReteriveTicktes_ReturnsListOfTickets()
        {
            // Arrange
            var userId = "user1";
            var expectedTickets = new List<Tickets> { new Tickets { UserId = userId } };
            _mockRepository.Setup(r => r.ReteriveTicktes(userId)).ReturnsAsync(expectedTickets);

            // Act
            var result = await _service.ReteriveTicktes(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Tickets>>(result);
            Assert.Equal(expectedTickets, result);
        }

        /// <summary>
        /// Verifies that TicketBook successfully books a movie ticket for a user.
        /// </summary>
        [Fact]
        public async Task TicketBook_SuccessfullyBooksTicket()
        {
            // Arrange
            var userId = "user1";
            var ticketDto = new TicketDto();
            var ticketEntity = new Tickets();
            var createResponse = new CreateResponse { IsSuccess = true };

            _mockMapper.Setup(m => m.Map<Tickets>(ticketDto)).Returns(ticketEntity);
            _mockRepository.Setup(r => r.TicketBook(ticketEntity)).ReturnsAsync(createResponse);

            // Act
            var result = await _service.TicketBook(ticketDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Verifies that UpdateTicket successfully updates an existing ticket booking for a user.
        /// </summary>
        [Fact]
        public async Task UpdateTicket_SuccessfullyUpdatesTicket()
        {
            // Arrange
            var userId = "user1";
            var ticketDto = new TicketDto();
            var ticketEntity = new Tickets();
            var existingTicket = new Tickets { UserId = userId };
            var theater = new Theater();
            var updateResponse = new CreateResponse { IsSuccess = true };

            _mockRepository.Setup(r => r.GetTicketById(ticketDto.TicketId)).ReturnsAsync(existingTicket);
            _mockRepository.Setup(r => r.GetTheaterById(ticketDto.TheaterId)).ReturnsAsync(theater);
            _mockRepository.Setup(r => r.UpdateTicket(existingTicket)).ReturnsAsync(updateResponse);
            _mockRepository.Setup(r => r.UpdateTheater(theater)).ReturnsAsync(updateResponse);

            // Act
            var result = await _service.UpdateTicket(ticketDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
