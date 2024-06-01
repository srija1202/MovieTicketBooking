using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MovieTicketBooking.Data;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Business.Repository
{
    /// <summary>
    /// Repository for handling customer-related data operations.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<User> _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="settings">The database connection settings.</param>
        /// <param name="configuration">The configuration.</param>
        public CustomerRepository(IDatabaseConnection settings, IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>("User");
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="data">The user data.</param>
        /// <returns>The creation response.</returns>
        public async Task<CreateResponse> CreateUser(User data)
        {
            CreateResponse response = new CreateResponse();
            if (await _user.Find(u => u.Username == data.Username).AnyAsync())
            {
                response.IsSuccess = false;
                response.Message = "Username already exists";
                return response;
            }
            try
            {
                await _user.InsertOneAsync(data);
                response.IsSuccess = true;
                response.Message = "Data inserted";
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
        /// Checks if a user with the specified username exists.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the user exists, false otherwise.</returns>
        public async Task<bool> CheckUserExistsByUsername(string username)
        {
            try
            {
                return await _user.Find(u => u.Username == username).AnyAsync();
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                throw new Exception("Error checking user existence: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The user object if found, null otherwise.</returns>
        public async Task<User> GetUserByUsername(string username)
        {
            try
            {
                return await _user.Find(u => u.Username == username).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                throw new Exception("Error retrieving user by username: " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the password of a user.
        /// </summary>
        /// <param name="userPassword">The new user password.</param>
        /// <param name="user">The user whose password is to be updated.</param>
        /// <returns>The update response.</returns>
        public async Task<CreateResponse> PasswordUpdate(UserPasswordUpdate userPassword, User user)
        {
            CreateResponse response = new CreateResponse();

            if (await VerifyUserPassword(userPassword.OldPassword, user))
            {
                if (userPassword.NewPassword == userPassword.ConfirmPassword)
                {
                    CreatePasswordHash(userPassword.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    await _user.ReplaceOneAsync(u => u.Id == user.Id, user);

                    response.IsSuccess = true;
                    response.Message = "Password updated";
                    return response;
                }
                response.IsSuccess = false;
                response.Message = "New password and confirm password do not match";
                return response;

            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Old password does not match";
                return response;
            }
        }

        /// <summary>
        /// Verifies a user's password.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="user">The user object containing the hashed password and salt.</param>
        /// <returns>True if the password is verified, false otherwise.</returns>
        public async Task<bool> VerifyUserPassword(string password, User? user)
        {
            try
            {
                return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                throw new Exception("Error verifying user password: " + ex.Message);
            }
        }

        // Other methods...

        // Summary and try-catch added for brevity

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>The JWT token.</returns>
        public string GenerateToken(User user, string role)
        {
            try
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("AccessLevel", role)
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return null token or throw exception based on requirements
                throw new Exception("Error generating token: " + ex.Message);
            }
        }

        /// <summary>
        /// Creates hash and salt for password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="passwordHash">The computed password hash.</param>
        /// <param name="passwordSalt">The password salt.</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            try
            {
                using (HMACSHA512 hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                }
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                throw new Exception("Error creating password hash: " + ex.Message);
            }
        }

        /// <summary>
        /// Verifies password with hashed password using salt.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="passwordHash">The stored password hash.</param>
        /// <param name="passwordSalt">The stored password salt.</param>
        /// <returns>True if the password is verified, false otherwise.</returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            try
            {
                using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return computedHash.SequenceEqual(passwordHash);
                }
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                throw new Exception("Error verifying password hash: " + ex.Message);
            }
        }
    }
}

