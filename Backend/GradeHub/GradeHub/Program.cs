using GradeHub;
using GradeHub.MainClasses;

public class MainClass
{
    public static void Main(string[] args)
    {
        var people = new List<Person>();

        var teacher = new Teacher("John", "Doe", 30, "1234");
        var student = new Student("Jane", "Doe", 20, "12340");

        people.Add(teacher);
        people.Add(student);

        var teacherCredentials = new UserCredentials("teacher", "password", "teacher@email.com", UserType.Teacher);
        teacher.SetUserCredentials(teacherCredentials);

        var studentCredentials = new UserCredentials("student1", "password1", "student1@email.com", UserType.Student);
        student.SetUserCredentials(studentCredentials);

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