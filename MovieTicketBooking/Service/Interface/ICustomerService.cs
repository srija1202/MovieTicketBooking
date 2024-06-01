using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using System.Threading.Tasks;

namespace MovieTicketBooking.Service.Interface
{
    /// <summary>
    /// Interface for customer-related operations.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="data">The user data.</param>
        /// <param name="isAdmin">Flag indicating if the user is an admin.</param>
        /// <returns>A create response indicating the result of the operation.</returns>
        Task<CreateResponse> CreateUser(UserDto data, bool isAdmin);

        /// <summary>
        /// Updates user password.
        /// </summary>
        /// <param name="userPassword">The user password update data.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>A create response indicating the result of the operation.</returns>
        Task<CreateResponse> PasswordUpdate(UserPasswordUpdate userPassword, string username);

        /// <summary>
        /// Creates a JSON token for user authentication.
        /// </summary>
        /// <param name="user">The authentication request containing username and password.</param>
        /// <returns>A create response containing the token.</returns>
        Task<CreateResponse> CreateJSONToken(AuthenticationRequest user);

        /// <summary>
        /// Verifies a user's password.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="user">The user object containing the hashed password and salt.</param>
        /// <returns>True if the password is verified, false otherwise.</returns>
        Task<bool> VerifyUserPassword(string password, User? user);
    }
}
