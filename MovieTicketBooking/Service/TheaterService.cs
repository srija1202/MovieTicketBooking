using AutoMapper;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using MovieTicketBooking.Service.Interface;

namespace MovieTicketBooking.Business.Service
{
    /// <summary>
    /// Service for managing theater-related operations.
    /// </summary>
    public class TheaterService : ITheaterService
    {
        private readonly ITheaterRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterService"/> class.
        /// </summary>
        /// <param name="repository">The repository for theater data.</param>
        /// <param name="mapper">The mapper for mapping data transfer objects.</param>
        public TheaterService(ITheaterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of theaters.
        /// </summary>
        /// <returns>A list of theaters.</returns>
        public async Task<List<Theater>> GetTheater()
        {
            try
            {
                return await _repository.GetTheater();
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                throw new Exception("An error occurred while retrieving theaters.", ex);
            }
        }

        /// <summary>
        /// Gets a theater by its ID.
        /// </summary>
        /// <param name="id">The ID of the theater.</param>
        /// <returns>The theater.</returns>
        public async Task<Theater> GetTheater(string id)
        {
            try
            {
                return await _repository.GetTheater(id);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                throw new Exception($"An error occurred while retrieving the theater with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Adds a new theater.
        /// </summary>
        /// <param name="data">The data of the theater to add.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public async Task<CreateResponse> AddTheater(TheaterDto data)
        {
            try
            {
                Theater theater = _mapper.Map<Theater>(data);
                theater.Created = DateTime.Now;
                theater.Updated = DateTime.Now;

                return await _repository.AddTheater(theater);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                return new CreateResponse { IsSuccess = false, Message = $"An error occurred while adding the theater. {ex.Message}" };
            }
        }

        /// <summary>
        /// Deletes a theater by its ID.
        /// </summary>
        /// <param name="id">The ID of the theater to delete.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public async Task<CreateResponse> DeleteTheater(string id)
        {
            try
            {
                return await _repository.DeleteTheater(id);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                return new CreateResponse { IsSuccess = false, Message = $"An error occurred while deleting the theater with ID {id}. {ex.Message}" };
            }
        }
    }
}
