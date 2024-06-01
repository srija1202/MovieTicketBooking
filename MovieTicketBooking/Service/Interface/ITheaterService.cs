using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Service.Interface
{
    /// <summary>
    /// Interface for managing theaters.
    /// </summary>
    public interface ITheaterService
    {
        /// <summary>
        /// Get a list of theaters.
        /// </summary>
        /// <returns>The list of theaters.</returns>
        Task<List<Theater>> GetTheater();

        /// <summary>
        /// Get a theater by ID.
        /// </summary>
        /// <param name="id">The ID of the theater.</param>
        /// <returns>The theater.</returns>
        Task<Theater> GetTheater(string id);

        /// <summary>
        /// Add a new theater.
        /// </summary>
        /// <param name="data">The data of the theater to add.</param>
        /// <returns>The response containing the result of the operation.</returns>
        Task<CreateResponse> AddTheater(TheaterDto data);

        /// <summary>
        /// Delete a theater by ID.
        /// </summary>
        /// <param name="id">The ID of the theater to delete.</param>
        /// <returns>The response containing the result of the operation.</returns>
        Task<CreateResponse> DeleteTheater(string id);
    }
}
