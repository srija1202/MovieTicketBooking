using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Business.Service;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Models.Entities;
using MovieTicketBooking.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Controllers
{
    /// <summary>
    /// Controller for managing theaters.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class TheaterController : ControllerBase
    {
        private readonly ITheaterService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterController"/> class.
        /// </summary>
        /// <param name="service">The theater service.</param>
        public TheaterController(ITheaterService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a new theater.
        /// </summary>
        /// <param name="model">The theater data.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTheater(TheaterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid Data");
                }

                CreateResponse response = await _service.AddTheater(model);

                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get a list of theaters.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [Route("Get/All")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTheater()
        {
            try
            {
                List<Theater> theaters = await _service.GetTheater();
                return theaters.Count > 0 ? Ok(theaters) : BadRequest(theaters);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get a theater by ID.
        /// </summary>
        /// <param name="id">The ID of the theater.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [Route("Get/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTheater([FromRoute] string id)
        {
            try
            {
                Theater theater = await _service.GetTheater(id);
                return theater != null ? Ok(theater) : BadRequest(theater);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete a theater by ID.
        /// </summary>
        /// <param name="id">The ID of the theater to delete.</param>
        /// <returns>The action result.</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTheater([FromRoute] string id)
        {
            try
            {
                CreateResponse response = await _service.DeleteTheater(id);
                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
