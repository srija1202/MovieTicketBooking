using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Service.Interface;
using System.Security.Claims;

namespace MovieTicketBooking.Controllers
{
    /// <summary>
    /// Controller for handling movie ticket bookings.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "CustomerOnly")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        /// <summary>
        /// Constructor of Ticket Controller
        /// </summary>
        /// <param name="service">Service for handling booking operations.</param>
        public BookingController(IBookingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Book a movie ticket for the user.
        /// </summary>
        /// <param name="model">Ticket booking details.</param>
        /// <returns>Response indicating the success or failure of the booking operation.</returns>
        [HttpPost]
        [Route("Book")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TicketBook([FromBody] TicketDto model)
        {
            try
            {
                string userId = User.FindFirstValue("Id");
                CreateResponse response = await _service.TicketBook(model, userId);

                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CreateResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieve all tickets booked by the user.
        /// </summary>
        /// <returns>List of tickets booked by the user.</returns>
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(typeof(List<Tickets>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RetrieveTickets()
        {
            try
            {
                string userId = User.FindFirstValue("Id");
                List<Tickets> response = await _service.ReteriveTicktes(userId);

                return response.Count > 0 ? Ok(response) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing ticket booking for the user.
        /// </summary>
        /// <param name="model">Updated ticket details.</param>
        /// <returns>Response indicating the success or failure of the update operation.</returns>
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTicket([FromBody] TicketDto model)
        {
            try
            {
                string userId = User.FindFirstValue("Id");
                CreateResponse response = await _service.UpdateTicket(model, userId);

                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CreateResponse { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
