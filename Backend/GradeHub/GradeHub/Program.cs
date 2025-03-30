using GradeHub;
using GradeHub.Class;
using GradeHub.MainClasses;

public class MainClass
{
    public static void Main(string[] args)
    {
        var people = new List<Person>();

        var teacher = new Teacher("Leutu", "Thau", 30, "Math101");
        var student1 = new Student("George", "Bossu", 20, "12340");
        var student2 = new Student("Raul", "The Horse", 21, "56789");

        people.Add(teacher);
        people.Add(student1);
        people.Add(student2);

        var teacherCredentials = new UserCredentials("teacher", "password", "teacher@email.com", UserType.Teacher);
        teacher.SetUserCredentials(teacherCredentials);

        var student1Credentials = new UserCredentials("student1", "password1", "student1@email.com", UserType.Student);
        student1.SetUserCredentials(student1Credentials);

        var student2Credentials = new UserCredentials("student2", "password2", "student2@email.com", UserType.Student);
        student2.SetUserCredentials(student2Credentials);

        // Create a new class
        var mathClass = new Class("Math 101", teacher);

        // Add students to the class
        mathClass.AddStudent(student1);
        mathClass.AddStudent(student2);

        // Remove a student from the class
        mathClass.RemoveStudent(student1);

        Console.WriteLine($"Students in {mathClass.ClassName}:");
        foreach (var student in mathClass.Students)
        {
            Console.WriteLine($"{student.GetName().firstName} {student.GetName().lastName}");
        }

        foreach (var person in people)
        {
            var studentPerson = person as Student;
            if (studentPerson != null)
            {
                var grades = studentPerson.GetGrades();
                foreach (var grade in grades)
                {
                    Console.WriteLine($"Grade: {grade.Value} Date: {grade.Key}");
                }
            }
        }
    }
}