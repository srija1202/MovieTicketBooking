namespace MovieTicketBooking.Data.Models.Dto
{
    /// <summary>
    /// Represents a data transfer object for movie information.
    /// </summary>
    public class MovieDto
    {
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
    }
}
