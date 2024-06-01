using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Service.Interface
{
    /// <summary>
    /// Interface for movie-related operations.
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movie">The movie data.</param>
        /// <returns>The creation response.</returns>
        Task<CreateResponse> Create(MovieDto movie);

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>The list of movies.</returns>
        Task<List<Movie>> GetMovie();

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie to retrieve.</param>
        /// <returns>The movie object if found, otherwise null.</returns>
        Task<Movie?> GetMovie(string movieId);

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie to delete.</param>
        /// <returns>The deletion response.</returns>
        Task<CreateResponse> DeleteMovie(string movieId);
    }
}
