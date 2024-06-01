using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace MovieTicketBooking.Data.Models.Entities
{
    /// <summary>
    /// Represents a ticket entity.
    /// </summary>
    public class Tickets
    {
        /// <summary>
        /// Gets or sets the unique identifier of the ticket.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the total count of tickets.
        /// </summary>
        [BsonElement("TicketsCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the theater associated with the ticket.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? TheaterId { get; set; }

        /// <summary>
        /// Gets or sets the theater associated with the ticket.
        /// </summary>
        [BsonIgnore]
        public Theater? Theater { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the movie associated with the ticket.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? MovieId { get; set; }

        /// <summary>
        /// Gets or sets the movie associated with the ticket.
        /// </summary>
        [BsonIgnore]
        public Movie? Movie { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who booked the ticket.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who booked the ticket.
        /// </summary>
        [BsonIgnore]
        public User? User { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the ticket was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the ticket was last updated.
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
