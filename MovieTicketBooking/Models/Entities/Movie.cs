using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace MovieTicketBooking.Data.Models.Entities
{
    /// <summary>
    /// Represents a movie entity.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the unique identifier of the movie.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie.
        /// </summary>
        public string? MovieName { get; set; }

        /// <summary>
        /// Gets or sets the URL of the movie poster.
        /// </summary>
        public string? MoviePoster { get; set; }

        /// <summary>
        /// Gets or sets the genre of the movie.
        /// </summary>
        public string? Genre { get; set; }

        /// <summary>
        /// Gets or sets the description of the movie.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the languages in which the movie is available.
        /// </summary>
        public string? Languages { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the movie was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the movie was last updated.
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
