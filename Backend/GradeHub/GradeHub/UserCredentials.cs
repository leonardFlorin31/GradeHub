using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace GradeHub;

public class UserCredentials
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }

    public UserCredentials(string username, string password, string email, UserType userType)
    {
        Username = username;
        PasswordHash = HashPassword(password);
        Email = email;
        UserType = userType;
    }

    static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}



public enum UserType
{
    Student,
    Teacher
}
