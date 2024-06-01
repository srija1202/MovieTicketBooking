using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;

namespace MovieTicketBooking.Service.Interface
{
    /// <summary>
    /// Interface for booking service to handle ticket booking operations.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Books a movie ticket for a user.
        /// </summary>
        /// <param name="ticket">The ticket details to be booked.</param>
        /// <param name="userId">The ID of the user booking the ticket.</param>
        /// <returns>A response indicating the success or failure of the booking operation.</returns>
        Task<CreateResponse> TicketBook(TicketDto ticket, string userId);

        /// <summary>
        /// Retrieves all tickets booked by a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose tickets are to be retrieved.</param>
        /// <returns>A list of tickets booked by the user.</returns>
        Task<List<Tickets>> ReteriveTicktes(string userId);

        /// <summary>
        /// Updates an existing ticket booking for a user.
        /// </summary>
        /// <param name="model">The updated ticket details.</param>
        /// <param name="userId">The ID of the user updating the ticket.</param>
        /// <returns>A response indicating the success or failure of the update operation.</returns>
        Task<CreateResponse> UpdateTicket(TicketDto model, string userId);
    }
}
