using Grade = global.GradeHub.MainClasses.Grade;
using Person = global.GradeHub.MainClasses.Person;

namespace global::GradeHub.MainClasses;

public class Student : Person
{
    private Dictionary<DateTime, Grade> _grades;
    private string _studentId;
    private string _classId;
    
    public Student(string firstName, string lastName, int age, string studentId, string classId) : base(firstName, lastName, age)
    {
        _studentId = studentId;
        _classId = classId;
        _grades = new Dictionary<DateTime, Grade>();
    }
    
    public void AddGrade(DateTime date, Grade grade)
    {
        _grades.Add(date, grade);
    }
    
    public void AddGradeToday(Grade grade)
    {
        _grades.Add(DateTime.Now, grade);
    }

    public void SetStudentId(string studentId)
    {
        _studentId = studentId;
    }

    public string GetStudentId()
    {
        return _studentId;
    }

    public void SetClassId(string classId)
    {
        _classId = classId;
    }

    public string GetClassId()
    {
        return _classId;
    }
}