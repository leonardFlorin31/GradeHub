
namespace GradeHub.MainClasses;

public class Teacher : Person
{
    public string ClassId { get; set; }

    public Teacher(string name, string classId) : base(name)
    {
        ClassId = classId;
    }
}