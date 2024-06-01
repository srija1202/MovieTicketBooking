using MongoDB.Driver;
using MovieTicketBooking.Data;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Business.Repository
{
    /// <summary>
    /// Repository for handling movie data operations.
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _movie;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieRepository"/> class.
        /// </summary>
        /// <param name="settings">The database connection settings.</param>
        public MovieRepository(IDatabaseConnection settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            _movie = database.GetCollection<Movie>("Movies");
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movie">The movie object to create.</param>
        /// <returns>The creation response.</returns>
        public async Task<CreateResponse> Create(Movie movie)
        {
            try
            {
                await _movie.InsertOneAsync(movie);
                return new CreateResponse { IsSuccess = true, Message = "Movie created" };
            }
            catch (Exception ex)
            {
                return new CreateResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Deletes a movie by its ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie to delete.</param>
        /// <returns>The deletion response.</returns>
        public async Task<CreateResponse> DeleteMovie(string movieId)
        {
            try
            {
                await _movie.DeleteOneAsync(m => m.Id == movieId);
                return new CreateResponse { IsSuccess = true, Message = "Movie deleted" };
            }
            catch (Exception ex)
            {
                return new CreateResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>The list of movies.</returns>
        public async Task<List<Movie>> GetMovie()
        {
            try
            {
                return await _movie.Find(m => true).Sort("{Created:-1}").ToListAsync();
            }
            catch (Exception)
            {
                // Handle exception, log it, and return appropriate response
                // For brevity, returning an empty list if there's an error
                return new List<Movie>();
            }
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie to retrieve.</param>
        /// <returns>The movie object if found, otherwise null.</returns>
        public async Task<Movie?> GetMovie(string movieId)
        {
            try
            {
                return await _movie.Find(m => m.Id == movieId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                // Handle exception, log it, and return appropriate response
                return null;
            }
        }
    }
}
