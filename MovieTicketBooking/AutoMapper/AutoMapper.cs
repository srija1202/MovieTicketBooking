using AutoMapper;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;

namespace MovieTicketBooking.Data.AutoMapper
{
    /// <summary>
    /// AutoMapper profile class for mapping DTOs to Entities and vice versa.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
        /// Configures the mappings between DTOs and Entities.
        /// </summary>
        public AutoMapperProfile()
        {
            // Mapping configuration for TheaterDto and Theater
            CreateMap<TheaterDto, Theater>()
                .ForMember(dest => dest.TheaterName, opt => opt.MapFrom(src => src.TheaterName))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ReverseMap();

            // Mapping configuration for UserDto and User
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ContactNumber, opt => opt.MapFrom(src => src.ContactNumber))
                .ReverseMap();

            // Mapping configuration for MovieDto and Movie
            CreateMap<MovieDto, Movie>()
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.MovieName))
                .ForMember(dest => dest.MoviePoster, opt => opt.MapFrom(src => src.MoviePoster))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
                .ReverseMap();

            // Mapping configuration for TicketDto and Tickets
            CreateMap<TicketDto, Tickets>()
                .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TicketsCount))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.TheaterId, opt => opt.MapFrom(src => src.TheaterId))
                .ReverseMap();
        }
    }
}
