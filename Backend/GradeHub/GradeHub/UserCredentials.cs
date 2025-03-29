namespace GradeHub;

public class UserCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }
    
    public UserCredentials(string username, string password, string email, UserType userType)
    {
        Username = username;
        Password = password;
        Email = email;
        UserType = userType;
    }
}

public enum UserType
{
    Student,
    Teacher
}