using System.Security.Cryptography;
using System.Text;

namespace GradeHub.Utils
{
    /// <summary>
    /// Provides utility methods for password hashing using SHA256.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes the given plain text password using SHA256.
        /// </summary>
        /// <param name="password">The plain text password to be hashed.</param>
        /// <returns>The hashed password as a Base64-encoded string.</returns>
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
