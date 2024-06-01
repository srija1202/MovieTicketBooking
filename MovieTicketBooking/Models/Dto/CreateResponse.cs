namespace MovieTicketBooking.Data.Models.Dto
{
    /// <summary>
    /// Represents the response for create operations such as ticket booking and updates.
    /// </summary>
    public class CreateResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the message providing details about the operation's success or failure.
        /// </summary>
        public string Message { get; set; }
    }
}
