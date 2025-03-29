

namespace GradeHub.MainClasses;

public class Student : Person
{
    private Dictionary<DateTime, Grade> _grades;
    private string _studentId;
    
    public Student(string firstName, string lastName, int age, string studentId) : base(firstName, lastName, age)
    {
        _studentId = studentId;
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
    
    public Dictionary<DateTime, Grade> GetGrades()
    {
        return _grades;
    }

    public void SetStudentId(string studentId)
    {
        _studentId = studentId;
    }

    public string GetStudentId()
    {
        return _studentId;
    }

}