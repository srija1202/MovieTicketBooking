namespace MovieTicketBooking.Data.Models.Dto
{
    /// <summary>
    /// Represents a data transfer object for updating user password.
    /// </summary>
    public class UserPasswordUpdate
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the old password of the user.
        /// </summary>
        public string? OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password of the user.
        /// </summary>
        public string? NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation of the new password.
        /// </summary>
        public string? ConfirmPassword { get; set; }
    }
}
