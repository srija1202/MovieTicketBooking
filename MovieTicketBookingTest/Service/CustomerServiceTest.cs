using AutoMapper;
using Moq;
using MovieTicketBooking.Business.Service;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using ZstdSharp.Unsafe;

namespace MovieTicketBooking.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CustomerService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateUser_SuccessfullyCreatesUser()
        {
            // Arrange
            var userDto = new UserDto { FirstName = "test1", LastName = "233", ContactNumber = "241234", Password = "S2872", Username = "adad", };
            var userEntity = new User();
            var createResponse = new CreateResponse { IsSuccess = true };

            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(userEntity);
            _mockRepository.Setup(r => r.CreateUser(userEntity)).ReturnsAsync(createResponse);

            // Act
            var result = await _service.CreateUser(userDto, isAdmin: false);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task PasswordUpdate_SuccessfullyUpdatesPassword()
        {
            // Arrange
            var userPasswordUpdate = new UserPasswordUpdate();
            var username = "username";
            var currentUser = new User();
            var updateResponse = new CreateResponse { IsSuccess = true };

            _mockRepository.Setup(r => r.GetUserByUsername(username)).ReturnsAsync(currentUser);
            _mockRepository.Setup(r => r.PasswordUpdate(userPasswordUpdate, currentUser)).ReturnsAsync(updateResponse);

            // Act
            var result = await _service.PasswordUpdate(userPasswordUpdate, username);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task VerifyUserPassword_SuccessfullyVerifiesPassword()
        {
            // Arrange
            var password = "password";
            var storedSalt = new byte[128]; // Sample salt
            var storedHash = ComputePasswordHash(password, storedSalt); // Sample hash

            var user = new User { PasswordHash = storedHash, PasswordSalt = storedSalt };

            // Act
            var result = await _service.VerifyUserPassword(password, user);

            // Assert
            Assert.True(result, "Password verification failed.");
        }

        [Fact]
        public async Task CreateJSONToken_SuccessfullyCreatesToken()
        {
            // Arrange
            var authenticationRequest = new AuthenticationRequest();
            var createResponse = new CreateResponse { IsSuccess = true };

            _mockRepository.Setup(r => r.CheckUserExistsByUsername(authenticationRequest.Username)).ReturnsAsync(true);
            _mockRepository.Setup(r => r.GetUserByUsername(authenticationRequest.Username)).ReturnsAsync(new User());
            _mockRepository.Setup(r => r.VerifyUserPassword(authenticationRequest.Password, It.IsAny<User>())).ReturnsAsync(true);

            // Act
            var result = await _service.CreateJSONToken(authenticationRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateUser_Failure_ReturnsFailureResponse()
        {
            // Arrange
            var userDto = new UserDto();
            var userEntity = new User();

            var errorMessage = "String reference not set to an instance of a String. (Parameter 's')";
            var createResponse = new CreateResponse { IsSuccess = false, Message = errorMessage };

            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(userEntity);
            _mockRepository.Setup(r => r.CreateUser(userEntity)).ReturnsAsync(createResponse);

            // Act
            var result = await _service.CreateUser(userDto, isAdmin: false);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task PasswordUpdate_Failure_ReturnsFailureResponse()
        {
            // Arrange
            var userPasswordUpdate = new UserPasswordUpdate();
            var username = "username";
            var errorMessage = "An error occurred while updating password.";
            var currentUser = new User();
            var updateResponse = new CreateResponse { IsSuccess = false, Message = errorMessage };

            _mockRepository.Setup(r => r.GetUserByUsername(username)).ReturnsAsync(currentUser);
            _mockRepository.Setup(r => r.PasswordUpdate(userPasswordUpdate, currentUser)).ReturnsAsync(updateResponse);

            // Act
            var result = await _service.PasswordUpdate(userPasswordUpdate, username);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task CreateJSONToken_Failure_ReturnsFailureResponse()
        {
            // Arrange
            var authenticationRequest = new AuthenticationRequest();
            var errorMessage = "No user found";
            var createResponse = new CreateResponse { IsSuccess = false, Message = errorMessage };

            _mockRepository.Setup(r => r.CheckUserExistsByUsername(authenticationRequest.Username)).ReturnsAsync(false);

            // Act
            var result = await _service.CreateJSONToken(authenticationRequest);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
        }

        private byte[] ComputePasswordHash(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
