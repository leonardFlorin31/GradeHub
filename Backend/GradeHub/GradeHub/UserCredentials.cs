using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using GradeHub.Utils;

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
}



public enum UserType
{
    Student,
    Teacher
}
