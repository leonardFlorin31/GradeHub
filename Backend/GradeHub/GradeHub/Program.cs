using GradeHub;
using GradeHub.Class;
using GradeHub.MainClasses;

public class MainClass
{
    public static void Main(string[] args)
    {
        var people = new List<Person>();

        var teacher = new Teacher("Leutu", "ProfuThau", 30, "Math101");
        var student1 = new Student("George", "Bossu", 20, "12340");
        var student2 = new Student("Raul", "The Horse", 21, "56789");
        var student3 = new Student("habar", "n am", 19, "98765");

        people.Add(teacher);
        people.Add(student1);
        people.Add(student2);
        people.Add(student3);

        var teacherCredentials = new UserCredentials("teacher", "password", "teacher@email.com", UserType.Teacher);
        teacher.SetUserCredentials(teacherCredentials);

        var student1Credentials = new UserCredentials("student1", "password1", "student1@email.com", UserType.Student);
        student1.SetUserCredentials(student1Credentials);

        var student2Credentials = new UserCredentials("student2", "password2", "student2@email.com", UserType.Student);
        student2.SetUserCredentials(student2Credentials);

        var student3Credentials = new UserCredentials("student3", "password3", "student3@email.com", UserType.Student);
        student3.SetUserCredentials(student3Credentials);

        // Create a new class
        var mathClass = new Class("Math 101", teacher);

        // Add students to the class
        mathClass.AddStudent(student1);
        mathClass.AddStudent(student2);
        mathClass.AddStudent(student3);

        // Simulate bulk grade upload
        var gradesToUpload = new List<(string studentId, int gradeValue)>
        {
            ("12340", 7), 
            ("56789", 9), 
            ("98765", -1) 
        };

        mathClass.BulkUploadGrades("Math101", gradesToUpload);

        Console.WriteLine($"Grades for students in {mathClass.ClassName}:");
        foreach (var student in mathClass.Students)
        {
            Console.WriteLine($"{student.GetName().firstName} {student.GetName().lastName} (ID: {student.GetStudentId()}):");
            foreach (var gradeEntry in student.GetGrades())
            {
                Console.WriteLine($"  Date: {gradeEntry.Key.ToShortDateString()}, Grade: {gradeEntry.Value.GradeValue}");
            }
        }
    }
}