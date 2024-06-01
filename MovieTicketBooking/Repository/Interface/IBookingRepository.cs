using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repository.Interface
{
    /// <summary>
    /// Interface for booking repository to handle database operations related to ticket bookings.
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Books a movie ticket.
        /// </summary>
        /// <param name="ticket">The ticket details to be booked.</param>
        /// <returns>A response indicating the success or failure of the booking operation.</returns>
        Task<CreateResponse> TicketBook(Tickets ticket);

        /// <summary>
        /// Retrieves all tickets booked by a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose tickets are to be retrieved.</param>
        /// <returns>A list of tickets booked by the user.</returns>
        Task<List<Tickets>> ReteriveTicktes(string userId);

        /// <summary>
        /// Updates an existing ticket booking.
        /// </summary>
        /// <param name="ticket">The updated ticket details.</param>
        /// <returns>A response indicating the success or failure of the update operation.</returns>
        Task<CreateResponse> UpdateTicket(Tickets ticket);

        /// <summary>
        /// Retrieves a ticket by its ID.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to be retrieved.</param>
        /// <returns>The ticket details if found, otherwise null.</returns>
        Task<Tickets?> GetTicketById(string ticketId);

        /// <summary>
        /// Retrieves a theater by its ID.
        /// </summary>
        /// <param name="theaterId">The ID of the theater to be retrieved.</param>
        /// <returns>The theater details if found, otherwise null.</returns>
        Task<Theater?> GetTheaterById(string theaterId);

        /// <summary>
        /// Updates the details of a theater.
        /// </summary>
        /// <param name="theater">The updated theater details.</param>
        /// <returns>A response indicating the success or failure of the update operation.</returns>
        Task<CreateResponse> UpdateTheater(Theater theater);
    }
}
