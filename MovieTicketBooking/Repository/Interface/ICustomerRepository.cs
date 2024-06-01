using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repository.Interface
{
    /// <summary>
    /// Interface for customer repository.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="data">The user data.</param>
        /// <returns>The creation response.</returns>
        Task<CreateResponse> CreateUser(User data);

        /// <summary>
        /// Checks if a user exists by username.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the user exists, false otherwise.</returns>
        Task<bool> CheckUserExistsByUsername(string username);

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The user.</returns>
        Task<User> GetUserByUsername(string username);

        /// <summary>
        /// Verifies a user's password.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="user">The user object containing the hashed password and salt.</param>
        /// <returns>True if the password is verified, false otherwise.</returns>
        Task<bool> VerifyUserPassword(string password, User? user);

        /// <summary>
        /// Updates user password.
        /// </summary>
        /// <param name="userPassword">The new user password.</param>
        /// <param name="username">The username.</param>
        /// <returns>The update response.</returns>
        Task<CreateResponse> PasswordUpdate(UserPasswordUpdate userPassword, User username);

        /// <summary>
        /// Generates a token for a user with a specific role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The user role.</param>
        /// <returns>The generated token.</returns>
        string GenerateToken(User user, string role);
    }
}
