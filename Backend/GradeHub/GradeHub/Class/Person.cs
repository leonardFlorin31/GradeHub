
namespace GradeHub.MainClasses;

public class Person : IPerson
{
    private string _firstName;
    private string _lastName;
    private int _age;
    private UserCredentials _userCredentials;

    public Person(string firstName, string lastName, int age)
    {
        _firstName = firstName;
        _lastName = lastName;
        _age = age;
    }
    
    public void SetUserCredentials(UserCredentials userCredentials)
    {
        _userCredentials = userCredentials;
    }
    
    public UserCredentials GetUserCredentials()
    {
        return _userCredentials;
    }

    public void SetName(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
    }

    public (string firstName, string lastName) GetName()
    {
        return (_firstName, _lastName);
    }

    public void SetAge(int age)
    {
        _age = age;
    }

    public int GetAge()
    {
        return _age;
    }
}