using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests
{
    /// <summary>
    /// Test class for <see cref="BookingController"/>.
    /// </summary>
    public class BookingControllerTests
    {
        private readonly Mock<IBookingService> _serviceMock;
        private readonly BookingController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingControllerTests"/> class.
        /// Sets up the mock service and the controller.
        /// </summary>
        public BookingControllerTests()
        {
            _serviceMock = new Mock<IBookingService>();
            _controller = new BookingController(_serviceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", "test-user-id")
            }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        /// <summary>
        /// Tests the TicketBook method with valid data.
        /// </summary>
        [Fact]
        public async Task TicketBook_ValidData_ReturnsOk()
        {
            // Arrange
            var model = new TicketDto { TicketsCount = 2, MovieId = "movie-id", TheaterId = "theater-id" };
            var response = new CreateResponse { IsSuccess = true, Message = "Booking successful" };
            _serviceMock.Setup(s => s.TicketBook(model, "test-user-id")).ReturnsAsync(response);

            // Act
            var result = await _controller.TicketBook(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the TicketBook method with invalid data.
        /// </summary>
        [Fact]
        public async Task TicketBook_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("TicketsCount", "Required");

            // Act
            var result = await _controller.TicketBook(new TicketDto()) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        /// <summary>
        /// Tests the RetrieveTickets method when tickets are found.
        /// </summary>
        [Fact]
        public async Task RetrieveTickets_TicketsFound_ReturnsOk()
        {
            // Arrange
            var tickets = new List<Tickets> { new Tickets(), new Tickets() };
            _serviceMock.Setup(s => s.ReteriveTicktes("test-user-id")).ReturnsAsync(tickets);

            // Act
            var result = await _controller.RetrieveTickets() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(tickets, result.Value);
        }

        /// <summary>
        /// Tests the RetrieveTickets method when no tickets are found.
        /// </summary>
        [Fact]
        public async Task RetrieveTickets_NoTicketsFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.ReteriveTicktes("test-user-id")).ReturnsAsync(new List<Tickets>());

            // Act
            var result = await _controller.RetrieveTickets() as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        /// <summary>
        /// Tests the UpdateTicket method with valid data.
        /// </summary>
        [Fact]
        public async Task UpdateTicket_ValidData_ReturnsOk()
        {
            // Arrange
            var model = new TicketDto { TicketsCount = 2, MovieId = "movie-id", TheaterId = "theater-id" };
            var response = new CreateResponse { IsSuccess = true, Message = "Update successful" };
            _serviceMock.Setup(s => s.UpdateTicket(model, "test-user-id")).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateTicket(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        /// <summary>
        /// Tests the UpdateTicket method with invalid data.
        /// </summary>
        [Fact]
        public async Task UpdateTicket_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("TicketsCount", "Required");

            // Act
            var result = await _controller.UpdateTicket(new TicketDto()) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}
