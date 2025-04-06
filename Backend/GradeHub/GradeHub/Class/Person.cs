
namespace GradeHub.MainClasses;

public class Person : IPerson
{
    private string _name;
    private UserCredentials _userCredentials;

    public Person(string name)
    {
        _name = name;
    }

    public void SetUserCredentials(UserCredentials userCredentials)
    {
        _userCredentials = userCredentials;
    }

    public UserCredentials GetUserCredentials()
    {
        return _userCredentials;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public string GetName()
    {
        return _name;
    }
}