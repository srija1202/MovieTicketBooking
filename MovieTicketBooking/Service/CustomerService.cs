using AutoMapper;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using MovieTicketBooking.Service.Interface;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MovieTicketBooking.Business.Service
{
    /// <summary>
    /// Service handling customer operations.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService"/> class.
        /// </summary>
        /// <param name="repository">The customer repository.</param>
        /// <param name="mapper">The mapper.</param>
        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="data">The user data.</param>
        /// <param name="isAdmin">Indicates if the user is an admin.</param>
        /// <returns>The creation response.</returns>
        public async Task<CreateResponse> CreateUser(UserDto data, bool isAdmin)
        {
            try
            {
                User user = _mapper.Map<User>(data);
                user.IsAdmin = isAdmin;
                user.Created = DateTime.Now;
                user.Updated = DateTime.Now;

                CreatePasswordHash(data.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                return await _repository.CreateUser(user);
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                return new CreateResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Update user password.
        /// </summary>
        /// <param name="userPassword">The new user password.</param>
        /// <param name="username">The username.</param>
        /// <returns>The update response.</returns>
        public async Task<CreateResponse> PasswordUpdate(UserPasswordUpdate userPassword, string username)
        {
            try
            {
                User currentUser = await _repository.GetUserByUsername(username);

                if (currentUser != null)
                {
                    return await _repository.PasswordUpdate(userPassword, currentUser);
                }
                else
                {
                    return new CreateResponse
                    {
                        IsSuccess = false,
                        Message = "User not found."
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                return new CreateResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Verify password with hashed password.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="user">The user object containing the hashed password and salt.</param>
        /// <returns>True if password is verified, false otherwise.</returns>
        public Task<bool> VerifyUserPassword(string password, User? user)
        {
            return Task.FromResult(VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt));
        }

        /// <summary>
        /// Create a JSON token for user.
        /// </summary>
        /// <param name="user">The user authentication request.</param>
        /// <returns>The authentication response.</returns>
        public async Task<CreateResponse> CreateJSONToken(AuthenticationRequest user)
        {
            try
            {
                CreateResponse response = new CreateResponse();

                if (await _repository.CheckUserExistsByUsername(user.Username))
                {
                    User currentUser = await _repository.GetUserByUsername(user.Username);

                    if (await _repository.VerifyUserPassword(user.Password, currentUser))
                    {
                        string token = _repository.GenerateToken(currentUser, currentUser.IsAdmin ? "Admin" : "Customer");

                        response.IsSuccess = true;
                        response.Message = token;
                        return response;
                    }
                    response.IsSuccess = false;
                    response.Message = "Password not match";
                    return response;
                }
                response.IsSuccess = false;
                response.Message = "No user found";
                return response;
            }
            catch (Exception ex)
            {
                // Handle exception, log it, and return appropriate response
                return new CreateResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Create hash and salt for password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="passwordHash">The computed password hash.</param>
        /// <param name="passwordSalt">The password salt.</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verify password with hashed password using salt.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="passwordHash">The stored password hash.</param>
        /// <param name="passwordSalt">The stored password salt.</param>
        /// <returns>True if password is verified, false otherwise.</returns>
        private bool VerifyPasswordHash(string password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
