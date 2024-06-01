using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Business.Repository;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Service.Interface;
using System.Threading.Tasks;

namespace MovieTicketBooking.Controllers
{
    /// <summary>
    /// Controller for handling authentication-related operations.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomerService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="service">The customer service.</param>
        public AuthenticationController(ICustomerService service)
        {
            _service = service;
        }

        /// <summary>
        /// User login method.
        /// </summary>
        /// <param name="model">The authentication request model.</param>
        /// <returns>An action result indicating the success or failure of the login operation.</returns>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(AuthenticationRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data");
                }

                CreateResponse response = await _service.CreateJSONToken(model);

                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
