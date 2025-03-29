namespace GradeHub.MainClasses;

public interface IPerson
{
    public void SetName(string firstName, string lastName);
    public (string firstName, string lastName) GetName();
    public void SetAge(int age);
    public int GetAge();
}