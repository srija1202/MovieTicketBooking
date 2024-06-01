using AutoMapper;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using MovieTicketBooking.Service.Interface;

namespace MovieTicketBooking.Business.Service
{
    /// <summary>
    /// Service for handling booking operations.
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _repository;

        /// <summary>
        /// Constructor for BookingService.
        /// </summary>
        /// <param name="mapper">Mapper for converting DTOs to entities.</param>
        /// <param name="repository">Repository for interacting with the database.</param>
        public BookingService(IMapper mapper, IBookingRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all tickets booked by a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose tickets are to be retrieved.</param>
        /// <returns>A list of tickets booked by the user.</returns>
        public async Task<List<Tickets>> ReteriveTicktes(string userId)
        {
            return await _repository.ReteriveTicktes(userId);
        }

        /// <summary>
        /// Books a movie ticket for a user.
        /// </summary>
        /// <param name="ticket">The ticket details to be booked.</param>
        /// <param name="userId">The ID of the user booking the ticket.</param>
        /// <returns>A response indicating the success or failure of the booking operation.</returns>
        public async Task<CreateResponse> TicketBook(TicketDto ticket, string userId)
        {
            Tickets ticketBook = _mapper.Map<Tickets>(ticket);
            ticketBook.UserId = userId;
            ticketBook.Created = DateTime.Now;
            ticketBook.Updated = DateTime.Now;

            return await _repository.TicketBook(ticketBook);
        }

        /// <summary>
        /// Updates an existing ticket booking for a user.
        /// </summary>
        /// <param name="ticket">The updated ticket details.</param>
        /// <param name="userId">The ID of the user updating the ticket.</param>
        /// <returns>A response indicating the success or failure of the update operation.</returns>
        public async Task<CreateResponse> UpdateTicket(TicketDto ticket, string userId)
        {
            try
            {
                // Find the existing ticket
                var existingTicket = await _repository.GetTicketById(ticket.TicketId);
                if (existingTicket == null || existingTicket.UserId != userId)
                {
                    return new CreateResponse
                    {
                        IsSuccess = false,
                        Message = "Ticket not found or unauthorized access"
                    };
                }

                // Retrieve the associated theater
                var theater = await _repository.GetTheaterById(ticket.TheaterId);
                if (theater == null)
                {
                    return new CreateResponse
                    {
                        IsSuccess = false,
                        Message = "Theater not found"
                    };
                }

                // Calculate the difference in seat count
                int seatDifference = ticket.TicketsCount - existingTicket.TotalCount;

                // Check if the theater has enough available seats
                if (seatDifference > theater.AvailableSeat)
                {
                    return new CreateResponse
                    {
                        IsSuccess = false,
                        Message = $"Only {theater.AvailableSeat} additional seats are available"
                    };
                }

                // Update ticket fields
                existingTicket.TotalCount = ticket.TicketsCount;
                existingTicket.TheaterId = ticket.TheaterId;
                existingTicket.MovieId = ticket.MovieId;
                existingTicket.Updated = DateTime.Now;

                // Update the available seat count in the theater
                theater.AvailableSeat -= seatDifference;
                theater.Updated = DateTime.Now;

                // Perform the update in the repository
                var updateTicketResponse = await _repository.UpdateTicket(existingTicket);
                if (!updateTicketResponse.IsSuccess)
                {
                    return updateTicketResponse;
                }

                // Update the theater seat count
                return await _repository.UpdateTheater(theater);
            }
            catch (Exception ex)
            {
                return new CreateResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
