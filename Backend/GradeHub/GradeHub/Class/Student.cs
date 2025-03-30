

using GradeHub.Class;

namespace GradeHub.MainClasses;

public class Student : Person
{
    private List<GradeEntry> _gradeHistory;
    private string _studentId;

    public Student(string firstName, string lastName, int age, string studentId) : base(firstName, lastName, age)
    {
        _studentId = studentId;
        _gradeHistory = new List<GradeEntry>();
    }

    public void AddGrade(DateTime timestamp, Grade grade)
    {
        if (grade.GradeValue > 0 && grade.GradeValue <= 10)
        {
            _gradeHistory.Add(new GradeEntry(timestamp, grade));
        }
        else
        {
            Console.WriteLine($"Invalid grade value: {grade}. Grade must be between 1 and 10.");
        }
    }

    public void AddGradeToday(Grade grade)
    {
        if (grade.GradeValue > 0 && grade.GradeValue <= 10)
        {
            _gradeHistory.Add(new GradeEntry(DateTime.Now, grade));
        }
        else
        {
            Console.WriteLine($"Invalid grade value: {grade}. Grade must be between 1 and 10.");
        }
    }

    public List<GradeEntry> GetGradeHistory()
    {
        return _gradeHistory;
    }

    // Kept for backward compatibility in the Main method's initial grade addition
    public Dictionary<DateTime, Grade> GetGrades()
    {
        return _gradeHistory.ToDictionary(entry => entry.Timestamp, entry => entry.Grade);
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