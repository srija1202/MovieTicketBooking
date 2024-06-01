using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repository.Interface
{
    /// <summary>
    /// Interface for handling movie data operations.
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movie">The movie object to create.</param>
        /// <returns>The creation response.</returns>
        Task<CreateResponse> Create(Movie movie);

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
