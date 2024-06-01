namespace MovieTicketBooking.Data.Models.Dto
{
    /// <summary>
    /// Represents the request for user authentication.
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string? Password { get; set; }
    }

    /// <summary>
    /// Represents the response for user authentication.
    /// </summary>
    public class AuthenticationResponse
    {
        /// <summary>
        /// Gets or sets the access token generated upon successful authentication.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the ID of the authenticated user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the authenticated user is an administrator.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
