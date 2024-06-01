using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MovieTicketBooking.Data;
using MovieTicketBooking.Data.Models.Dto;
using MovieTicketBooking.Data.Models.Entities;
using MovieTicketBooking.Models.Entities;
using MovieTicketBooking.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketBooking.Business.Repository
{
    /// <summary>
    /// Repository class for managing theater data.
    /// </summary>
    public class TheaterRepository : ITheaterRepository
    {
        private readonly IMongoCollection<Theater> _theatre;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterRepository"/> class.
        /// </summary>
        /// <param name="settings">Database connection settings.</param>
        /// <param name="configuration">Application configuration settings.</param>
        public TheaterRepository(IDatabaseConnection settings, IConfiguration configuration)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            _theatre = database.GetCollection<Theater>("Theater");
        }

        /// <summary>
        /// Retrieves a list of all theaters.
        /// </summary>
        /// <returns>A list of theaters.</returns>
        public async Task<List<Theater>> GetTheater()
        {
            try
            {
                return await _theatre.Find(t => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving theaters: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves a theater by its ID.
        /// </summary>
        /// <param name="id">The ID of the theater.</param>
        /// <returns>The theater with the specified ID.</returns>
        public async Task<Theater> GetTheater(string id)
        {
            try
            {
                return await _theatre.Find(t => t.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the theater with ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new theater.
        /// </summary>
        /// <param name="data">The theater data to add.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public async Task<CreateResponse> AddTheater(Theater data)
        {
            CreateResponse response = new CreateResponse();
            try
            {
                await _theatre.InsertOneAsync(data);
                response.IsSuccess = true;
                response.Message = "Theater added successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred while adding the theater: {ex.Message}";
            }
            return response;
        }

        /// <summary>
        /// Deletes a theater by its ID.
        /// </summary>
        /// <param name="id">The ID of the theater to delete.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public async Task<CreateResponse> DeleteTheater(string id)
        {
            CreateResponse response = new CreateResponse();
            try
            {
                await _theatre.DeleteOneAsync(t => t.Id == id);
                response.IsSuccess = true;
                response.Message = "Theater deleted successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred while deleting the theater: {ex.Message}";
            }
            return response;
        }
    }
}
