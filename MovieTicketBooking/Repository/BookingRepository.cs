using MovieTicketBooking.Data.Models.Dto;
using System;
using MovieTicketBooking.Data;
using MongoDB.Driver;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;

namespace MovieTicketBooking.Business.Repository
{
    /// <summary>
    /// Repository for handling ticket bookings.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly IDatabaseConnection _settings;
        private readonly IMongoCollection<Tickets> _ticket;
        private readonly IMongoCollection<Theater> _theater;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Movie> _movie;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="settings">The database connection settings.</param>
        public BookingRepository(IDatabaseConnection settings)
        {
            _settings = settings;
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            _ticket = database.GetCollection<Tickets>("Ticket");
            _theater = database.GetCollection<Theater>("Theater");
            _user = database.GetCollection<User>("User");
            _movie = database.GetCollection<Movie>("Movies");
        }

        /// <summary>
        /// Retrieves all tickets booked by a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose tickets are to be retrieved.</param>
        /// <returns>A list of tickets booked by the user.</returns>
        public async Task<List<Tickets>> ReteriveTicktes(string userId)
        {
            try
            {
                List<Tickets> tickets = await _ticket.Find(t => t.UserId == userId).Sort("{Created:-1}").ToListAsync();

                foreach (var ticket in tickets)
                {
                    ticket.User = await _user.Find(u => u.Id == userId).FirstOrDefaultAsync();
                    ticket.Theater = await _theater.Find(u => u.Id == ticket.TheaterId).FirstOrDefaultAsync();
                    ticket.Movie = await _movie.Find(u => u.Id == ticket.MovieId).FirstOrDefaultAsync();
                }

                return tickets;
            }
            catch (Exception)
            {
                // Log the exception if logging is implemented
                return new List<Tickets>();
            }
        }

        /// <summary>
        /// Books a movie ticket.
        /// </summary>
        /// <param name="ticket">The ticket details to be booked.</param>
        /// <returns>A response indicating the success or failure of the booking operation.</returns>
        public async Task<CreateResponse> TicketBook(Tickets ticket)
        {
            CreateResponse response = new CreateResponse();
            try
            {
                Theater theater = await _theater.Find(t => t.Id == ticket.TheaterId).FirstOrDefaultAsync();
                if (ticket.TotalCount <= theater.AvailableSeat)
                {
                    await _ticket.InsertOneAsync(ticket);

                    theater.AvailableSeat -= ticket.TotalCount;
                    theater.Updated = DateTime.Now;

                    await _theater.ReplaceOneAsync(t => t.Id == ticket.TheaterId, theater);

                    response.IsSuccess = true;
                    response.Message = $"{ticket.TotalCount} Ticket Booked";
                    return response;
                }

                response.IsSuccess = false;
                response.Message = $"Only {theater.AvailableSeat} seats are available";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// Retrieves a ticket by its ID.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to be retrieved.</param>
        /// <returns>The ticket details if found, otherwise null.</returns>
        public async Task<Tickets?> GetTicketById(string ticketId)
        {
            try
            {
                return await _ticket.Find(t => t.TicketId == ticketId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                // Log the exception if logging is implemented
                return null;
            }
        }

        /// <summary>
        /// Retrieves a theater by its ID.
        /// </summary>
        /// <param name="theaterId">The ID of the theater to be retrieved.</param>
        /// <returns>The theater details if found, otherwise null.</returns>
        public async Task<Theater?> GetTheaterById(string theaterId)
        {
            try
            {
                return await _theater.Find(t => t.Id == theaterId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                // Log the exception if logging is implemented
                return null;
            }
        }

        /// <summary>
        /// Updates an existing ticket booking.
        /// </summary>
        /// <param name="ticket">The updated ticket details.</param>
        /// <returns>A response indicating the success or failure of the update operation.</returns>
        public async Task<CreateResponse> UpdateTicket(Tickets ticket)
        {
            var response = new CreateResponse();
            try
            {
                var result = await _ticket.ReplaceOneAsync(t => t.TicketId == ticket.TicketId, ticket);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Ticket updated successfully";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Ticket update failed";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Updates the details of a theater.
        /// </summary>
        /// <param name="theater">The updated theater details.</param>
        /// <returns>A response indicating the success or failure of the update operation.</returns>
        public async Task<CreateResponse> UpdateTheater(Theater theater)
        {
            var response = new CreateResponse();
            try
            {
                var result = await _theater.ReplaceOneAsync(t => t.Id == theater.Id, theater);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Theater updated successfully";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Theater update failed";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
