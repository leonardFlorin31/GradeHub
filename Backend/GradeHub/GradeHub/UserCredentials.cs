using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using GradeHub.Utils;

namespace GradeHub
{
    /// <summary>
    /// Represents the authentication credentials for a user.
    /// Stores the username, hashed password, email, and user type (Student or Teacher).
    /// </summary>
    public class UserCredentials
    {
        /// <summary>
        /// Gets or sets the username. (Optional and unused in some contexts.)
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the hashed password using SHA256.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's role (Student or Teacher).
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCredentials"/> class.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's plain text password to be hashed.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="userType">The type of user (Student or Teacher).</param>
        public UserCredentials(string username, string password, string email, UserType userType)
        {
            Username = username;
            PasswordHash = HashPassword(password);
            Email = email;
            UserType = userType;
        }
    }

    /// <summary>
    /// Enumeration for distinguishing user types within the application.
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// A user who is a student.
        /// </summary>
        Student,

        /// <summary>
        /// A user who is a teacher.
        /// </summary>
        Teacher
    }
}
