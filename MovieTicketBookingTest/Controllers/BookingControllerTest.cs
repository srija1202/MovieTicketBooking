using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Service.Interface;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace MovieTicketBooking.Tests
{
    public class BookingControllerTests
    {
        private readonly Mock<IBookingService> _mockService;
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            _mockService = new Mock<IBookingService>();
            _controller = new BookingController(_mockService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", "test-user-id")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        /// <summary>
        /// Verifies that the TicketBook method returns an OkObjectResult when booking is successful.
        /// </summary>
        [Fact]
        public async Task TicketBook_Success_ReturnsOkResult()
        {
            // Arrange
            var model = new TicketDto();
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(s => s.TicketBook(model, "test-user-id")).ReturnsAsync(response);

            // Act
            var result = await _controller.TicketBook(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(okResult.Value);
            Assert.True(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that the TicketBook method returns a BadRequestObjectResult when booking fails.
        /// </summary>
        [Fact]
        public async Task TicketBook_Failure_ReturnsBadRequest()
        {
            // Arrange
            var model = new TicketDto();
            var response = new CreateResponse { IsSuccess = false };
            _mockService.Setup(s => s.TicketBook(model, "test-user-id")).ReturnsAsync(response);

            // Act
            var result = await _controller.TicketBook(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(badRequestResult.Value);
            Assert.False(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that the RetrieveTickets method returns an OkObjectResult when tickets are found.
        /// </summary>
        [Fact]
        public async Task RetrieveTickets_Success_ReturnsOkResult()
        {
            // Arrange
            var tickets = new List<Tickets> { new Tickets() };
            _mockService.Setup(s => s.ReteriveTicktes("test-user-id")).ReturnsAsync(tickets);

            // Act
            var result = await _controller.RetrieveTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Tickets>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Verifies that the RetrieveTickets method returns a NotFoundResult when no tickets are found.
        /// </summary>
        [Fact]
        public async Task RetrieveTickets_NotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var tickets = new List<Tickets>();
            _mockService.Setup(s => s.ReteriveTicktes("test-user-id")).ReturnsAsync(tickets);

            // Act
            var result = await _controller.RetrieveTickets();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Verifies that the UpdateTicket method returns an OkObjectResult when the update is successful.
        /// </summary>
        [Fact]
        public async Task UpdateTicket_Success_ReturnsOkResult()
        {
            // Arrange
            var model = new TicketDto();
            var response = new CreateResponse { IsSuccess = true };
            _mockService.Setup(s => s.UpdateTicket(model, "test-user-id")).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateTicket(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(okResult.Value);
            Assert.True(returnValue.IsSuccess);
        }

        /// <summary>
        /// Verifies that the UpdateTicket method returns a BadRequestObjectResult when the update fails.
        /// </summary>
        [Fact]
        public async Task UpdateTicket_Failure_ReturnsBadRequest()
        {
            // Arrange
            var model = new TicketDto();
            var response = new CreateResponse { IsSuccess = false };
            _mockService.Setup(s => s.UpdateTicket(model, "test-user-id")).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateTicket(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<CreateResponse>(badRequestResult.Value);
            Assert.False(returnValue.IsSuccess);
        }
    }
}
