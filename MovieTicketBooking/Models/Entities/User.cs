using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieTicketBooking.Data.Models.Entities
{
    /// <summary>
    /// Represents a user entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Required]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password hash of the user.
        /// </summary>
        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the password salt of the user.
        /// </summary>
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the contact number of the user.
        /// </summary>
        public string? ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an admin.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user was last updated.
        /// </summary>
        public DateTime Updated { get; set; }
    }

    /// <summary>
    /// Represents a user data transfer object (DTO).
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the contact number of the user.
        /// </summary>
        public string? ContactNumber { get; set; }
    }
}
