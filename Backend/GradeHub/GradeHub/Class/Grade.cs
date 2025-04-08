namespace GradeHub.MainClasses;

public class Grade
{
    
    public string ClassId { get; set; }
    public int GradeValue { get; set; }
    
    public Grade(string classId, int gradeValue)
    {
        ClassId = classId;
        GradeValue = gradeValue;
    }
}