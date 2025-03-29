using Person = global.GradeHub.MainClasses.Person;

namespace global::GradeHub.MainClasses;

public class Teacher : Person
{
    public string ClassId { get; set; }
    
    public Teacher(string firstName, string lastName, int age, string classId) : base(firstName, lastName, age)
    {
        ClassId = classId;
    }
}