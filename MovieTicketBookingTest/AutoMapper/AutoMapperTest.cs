using AutoMapper;
using MovieTicketBooking.Data.AutoMapper;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using Xunit;

namespace MovieTicketBooking.Tests
{
    /// <summary>
    /// Test class for verifying AutoMapper configuration.
    /// </summary>
    public class AutoMapperProfileTests
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfileTests"/> class.
        /// Configures the AutoMapper with the <see cref="AutoMapperProfile"/>.
        /// </summary>
        public AutoMapperProfileTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        /// <summary>
        /// Tests the mapping from <see cref="TheaterDto"/> to <see cref="Theater"/>.
        /// </summary>
        [Fact]
        public void Should_Map_TheaterDto_To_Theater()
        {
            var theaterDto = new TheaterDto
            {
                TheaterName = "Sample Theater",
                City = "Sample City"
            };
            var theater = _mapper.Map<Theater>(theaterDto);
            Assert.Equal(theaterDto.TheaterName, theater.TheaterName);
            Assert.Equal(theaterDto.City, theater.City);
        }

        /// <summary>
        /// Tests the mapping from <see cref="UserDto"/> to <see cref="User"/>.
        /// </summary>
        [Fact]
        public void Should_Map_UserDto_To_User()
        {
            var userDto = new UserDto
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                Username = "johndoe",
                ContactNumber = "1234567890"
            };
            var user = _mapper.Map<User>(userDto);
            Assert.Equal(userDto.FirstName, user.FirstName);
            Assert.Equal(userDto.LastName, user.LastName);
            Assert.Equal(userDto.EmailAddress, user.EmailAddress);
            Assert.Equal(userDto.Username, user.Username);
            Assert.Equal(userDto.ContactNumber, user.ContactNumber);
        }

        /// <summary>
        /// Tests the mapping from <see cref="MovieDto"/> to <see cref="Movie"/>.
        /// </summary>
        [Fact]
        public void Should_Map_MovieDto_To_Movie()
        {
            var movieDto = new MovieDto
            {
                MovieName = "Sample Movie",
                MoviePoster = "poster.jpg",
                Genre = "Action",
                Description = "Sample Description",
                Languages = "English"
            };
            var movie = _mapper.Map<Movie>(movieDto);
            Assert.Equal(movieDto.MovieName, movie.MovieName);
            Assert.Equal(movieDto.MoviePoster, movie.MoviePoster);
            Assert.Equal(movieDto.Genre, movie.Genre);
            Assert.Equal(movieDto.Description, movie.Description);
            Assert.Equal(movieDto.Languages, movie.Languages);
        }

        /// <summary>
        /// Tests the mapping from <see cref="TicketDto"/> to <see cref="Tickets"/>.
        /// </summary>
        [Fact]
        public void Should_Map_TicketDto_To_Tickets()
        {
            var ticketDto = new TicketDto
            {
                TicketsCount = 5,
                MovieId = "movie123",
                TheaterId = "theater123"
            };
            var tickets = _mapper.Map<Tickets>(ticketDto);
            Assert.Equal(ticketDto.TicketsCount, tickets.TotalCount);
            Assert.Equal(ticketDto.MovieId, tickets.MovieId);
            Assert.Equal(ticketDto.TheaterId, tickets.TheaterId);
        }
    }
}
