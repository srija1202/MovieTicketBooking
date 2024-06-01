namespace MovieTicketBooking.Models.Entities
{
    /// <summary>
    /// Represents a response indicating the preparation status.
    /// </summary>
    public class PrepareResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the preparation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets a message providing details about the preparation status.
        /// </summary>
        public string? Message { get; set; }
    }
}
