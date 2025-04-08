

using GradeHub.Class;

namespace GradeHub.MainClasses;

public class Student : Person
{
    private List<GradeEntry> _gradeHistory;
    private string _studentId;

    public Student(string name, string studentId) : base(name)
    {
        _studentId = studentId;
        _gradeHistory = new List<GradeEntry>();
    }

    public void AddGrade(DateTime timestamp, Grade grade)
    {
        if (grade.GradeValue > 0 && grade.GradeValue <= 10)
        {
            _gradeHistory.Add(new GradeEntry(timestamp, grade)); // auto-ID
        }
        else
        {
            Console.WriteLine($"Invalid grade value: {grade}. Grade must be between 1 and 10.");
        }
    }

    public void AddGrade(GradeEntry entry)
    {
        _gradeHistory.Add(entry);
    }


    public void AddGradeToday(Grade grade)
    {
        if (grade.GradeValue > 0 && grade.GradeValue <= 10)
        {
            _gradeHistory.Add(new GradeEntry(DateTime.Now, grade)); // auto-ID
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

    public void RemoveFirstGradeByValue(int value)
    {
        var gradeToRemove = _gradeHistory.FirstOrDefault(g => g.Grade.GradeValue == value);
        if (gradeToRemove != null)
        {
            _gradeHistory.Remove(gradeToRemove);
        }
    }

    public bool RemoveGradeById(Guid gradeId)
    {
        var gradeToRemove = _gradeHistory.FirstOrDefault(g => g.Id == gradeId);
        if (gradeToRemove != null)
        {
            _gradeHistory.Remove(gradeToRemove);
            return true;
        }
        return false;
    }
}