using AutoMapper;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using MovieTicketBooking.Service.Interface;

namespace MovieTicketBooking.Business.Service
{
    /// <summary>
    /// Service handling movie operations.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieService"/> class.
        /// </summary>
        /// <param name="repository">The movie repository.</param>
        /// <param name="mapper">The mapper.</param>
        public MovieService(IMovieRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movie">The movie data.</param>
        /// <returns>The creation response.</returns>
        public async Task<CreateResponse> Create(MovieDto movie)
        {
            try
            {
                Movie movieData = _mapper.Map<Movie>(movie);
                movieData.Created = DateTime.Now;
                movieData.Updated = DateTime.Now;

                return await _repository.Create(movieData);
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
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
                return await _repository.GetMovie();
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
                return await _repository.GetMovie(movieId);
            }
            catch (Exception)
            {
                // Handle exception, log it, and return appropriate response
                return null;
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
                return await _repository.DeleteMovie(movieId);
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                return new CreateResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
