using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Controllers
{
    /// <summary>
    /// Controller for managing movie-related operations.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController"/> class.
        /// </summary>
        /// <param name="service">The movie service.</param>
        public MovieController(IMovieService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="model">The movie data.</param>
        /// <returns>The creation response.</returns>
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMovie(MovieDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid Data");
                }

                CreateResponse response = await _service.Create(model);

                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>The list of movies.</returns>
        [HttpGet]
        [Route("Retrieve/All")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMovies()
        {
            try
            {
                List<Movie> movies = await _service.GetMovie();
                return movies.Count > 0 ? Ok(movies) : BadRequest(movies);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie object if found, otherwise a bad request response.</returns>
        [HttpGet]
        [Route("Retrieve/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMovieById([FromRoute] string id)
        {
            try
            {
                Movie movie = await _service.GetMovie(id);
                return movie != null ? Ok(movie) : BadRequest(movie);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>The deletion response.</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMovie([FromRoute] string id)
        {
            try
            {
                CreateResponse response = await _service.DeleteMovie(id);
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
