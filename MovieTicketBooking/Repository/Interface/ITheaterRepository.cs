using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repository.Interface
{
    /// <summary>
    /// Interface for Theater Repository
    /// </summary>
    public interface ITheaterRepository
    {
        /// <summary>
        /// Retrieves a list of all theaters.
        /// </summary>
        /// <returns>A list of theaters.</returns>
        Task<List<Theater>> GetTheater();

        /// <summary>
        /// Retrieves a theater by its ID.
        /// </summary>
        /// <param name="id">The ID of the theater.</param>
        /// <returns>The theater with the specified ID.</returns>
        Task<Theater> GetTheater(string id);

        /// <summary>
        /// Adds a new theater.
        /// </summary>
        /// <param name="data">The theater data to add.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        Task<CreateResponse> AddTheater(Theater data);

        /// <summary>
        /// Deletes a theater by its ID.
        /// </summary>
        /// <param name="id">The ID of the theater to delete.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        Task<CreateResponse> DeleteTheater(string id);
    }
}
