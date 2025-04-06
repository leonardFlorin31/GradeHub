using GradeHub;
using GradeHub.Class;
using GradeHub.MainClasses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// 🔹 CORS Setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add a root URL redirect to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// Initialize data directories
var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
var studentsDirectory = Path.Combine(dataDirectory, "Students");
var teachersDirectory = Path.Combine(dataDirectory, "Teachers");
var classesDirectory = Path.Combine(dataDirectory, "Classes");

// Ensure directories exist
Directory.CreateDirectory(dataDirectory);
Directory.CreateDirectory(studentsDirectory);
Directory.CreateDirectory(teachersDirectory);
Directory.CreateDirectory(classesDirectory);

// Initialize sample data
var people = new List<Person>();
var classes = new List<Class>();

// Load existing data if available
LoadData(out people, out classes);

// If no data exists, create sample data
if (people.Count == 0 || classes.Count == 0)
//if (true)
{
    // Teachers
    var teacher1 = new Teacher("Leutu ProfuThau", "Math101");
    var teacher2 = new Teacher("Anna Biolog", "Bio202");
    var teacher3 = new Teacher("Marius Informat", "CS303");

    people.AddRange(new[] { teacher1, teacher2, teacher3 });

    teacher1.SetUserCredentials(new UserCredentials("null", "password", "math@email.com", UserType.Teacher));
    teacher2.SetUserCredentials(new UserCredentials("null", "password", "bio@email.com", UserType.Teacher));
    teacher3.SetUserCredentials(new UserCredentials("null", "password", "cs@email.com", UserType.Teacher));

    // Students
    var student1 = new Student("George Bossu", "12340");
    var student2 = new Student("Raul The Horse", "56789");
    var student3 = new Student("Habar N-am", "98765");
    var student4 = new Student("Elena Smart", "11223");
    var student5 = new Student("Ionut Coder", "44556");
    var student6 = new Student("Maria Green", "77889");

    people.AddRange(new[] { student1, student2, student3, student4, student5, student6 });

    student1.SetUserCredentials(new UserCredentials("null", "password1", "george@email.com", UserType.Student));
    student2.SetUserCredentials(new UserCredentials("null", "password2", "raul@email.com", UserType.Student));
    student3.SetUserCredentials(new UserCredentials("null", "password3", "habar@email.com", UserType.Student));
    student4.SetUserCredentials(new UserCredentials("null", "password4", "elena@email.com", UserType.Student));
    student5.SetUserCredentials(new UserCredentials("null", "password5", "ionut@email.com", UserType.Student));
    student6.SetUserCredentials(new UserCredentials("null", "password6", "maria@email.com", UserType.Student));

    // Classes
    var class1 = new Class("Math 101", teacher1);
    var class2 = new Class("Biology 202", teacher2);
    var class3 = new Class("Computer Science 303", teacher3);

    class1.AddStudent(student1);
    class1.AddStudent(student2);

    class2.AddStudent(student3);
    class2.AddStudent(student4);
    class2.AddStudent(student2);

    class3.AddStudent(student5);
    class3.AddStudent(student6);
    class3.AddStudent(student2);

    classes.AddRange(new[] { class1, class2, class3 });

    // Sample grades
    student1.AddGrade(DateTime.Now.AddDays(-3), new Grade("Math101", 6));
    student2.AddGrade(DateTime.Now.AddDays(-2), new Grade("Math101", 7));
    student2.AddGradeToday(new Grade("Math101", 9));
    student2.AddGradeToday(new Grade("CS303", 10));
    student2.AddGrade(DateTime.Now.AddDays(-1), new Grade("Bio202", 8));
    student2.AddGradeToday(new Grade("Bio202", 9));


    student3.AddGrade(DateTime.Now.AddDays(-5), new Grade("Bio202", 8));
    student4.AddGrade(DateTime.Now.AddDays(-1), new Grade("Bio202", 6));
    student4.AddGradeToday(new Grade("Bio202", 7));

    student5.AddGrade(DateTime.Now.AddDays(-2), new Grade("CS303", 9));
    student5.AddGradeToday(new Grade("CS303", 10));
    student6.AddGradeToday(new Grade("CS303", 8));
    student6.AddGrade(DateTime.Now.AddDays(-3), new Grade("CS303", 7));

    // Save data
    SaveAllData(people, classes);
}

// Get reference to the first class (for demo purposes)
var mathClass = classes.FirstOrDefault();

// API Endpoints

app.MapPost("/api/auth/reset-password", ([FromBody] ResetPasswordDto dto) =>
{
    var person = people.FirstOrDefault(p =>
        p.GetUserCredentials()?.Email == dto.Email);

    if (person == null)
        return Results.NotFound("User with this email not found");

    var credentials = person.GetUserCredentials();
    if (credentials == null)
        return Results.BadRequest("User does not have credentials");

    // Update password
    credentials.Password = dto.NewPassword;

    // Save user
    if (person is Student student)
        SaveStudentData(student);
    else if (person is Teacher teacher)
        SaveTeacherData(teacher);

    return Results.Ok("Password reset successfully");
});


// Students endpoints
app.MapGet("/api/students", () => {
    var students = people.OfType<Student>().Select(s => new {
        Id = s.GetStudentId(),
        Name = s.GetName()
    });
    return students;
});


app.MapGet("/api/students/{id}", (string id) => {
    var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == id);
    if (student == null)
        return Results.NotFound();

    return Results.Ok(new
    {
        Id = student.GetStudentId(),
        Name = student.GetName()
    });
});


app.MapPost("/api/students", ([FromBody] StudentDto studentDto) => {
    // Generate unique student ID (can be GUID or a custom short ID)
    var generatedId = Guid.NewGuid().ToString();

    var student = new Student(studentDto.Name, generatedId);

    if (!string.IsNullOrEmpty(studentDto.Email) && !string.IsNullOrEmpty(studentDto.Password))
    {
        var credentials = new UserCredentials(
            username: null!, // not used
            password: studentDto.Password,
            email: studentDto.Email,
            userType: UserType.Student
        );
        student.SetUserCredentials(credentials);
    }

    people.Add(student);

    // Save student data
    SaveStudentData(student);

    return Results.Created($"/api/students/{student.GetStudentId()}", new
    {
        Id = student.GetStudentId(),
        Name = student.GetName()
    });
});



app.MapDelete("/api/students/{id}", (string id) => {
    var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == id);
    if (student == null)
        return Results.NotFound();
    
    people.Remove(student);
    
    // Remove from classes
    foreach (var cls in classes)
    {
        if (cls.Students.Contains(student))
        {
            cls.Students.Remove(student);
            SaveClassData(cls);
        }
    }
    
    // Delete student file
    DeleteStudentData(id);
    
    return Results.NoContent();
});

// Teachers endpoints
app.MapGet("/api/teachers", () => {
    var teachers = people.OfType<Teacher>().Select(t => new {
        Name = t.GetName(),
        ClassId = t.ClassId
    });
    return teachers;
});


app.MapPost("/api/teachers", ([FromBody] TeacherDto teacherDto) => {
    // Create the teacher
    var teacher = new Teacher(
        teacherDto.Name,
        teacherDto.ClassId // this acts as the teacher's ID and links the class
    );

    if (!string.IsNullOrEmpty(teacherDto.Email) && !string.IsNullOrEmpty(teacherDto.Password))
    {
        var credentials = new UserCredentials(
            username: null!, // no username used
            password: teacherDto.Password,
            email: teacherDto.Email,
            userType: UserType.Teacher
        );
        teacher.SetUserCredentials(credentials);
    }

    // Add teacher to list
    people.Add(teacher);
    SaveTeacherData(teacher);

    // Create the class automatically and assign teacher
    var newClass = new Class(teacherDto.ClassName ?? teacherDto.ClassId, teacher);
    classes.Add(newClass);
    SaveClassData(newClass);

    return Results.Created($"/api/teachers/{teacher.ClassId}", new
    {
        Name = teacher.GetName(),
        ClassId = teacher.ClassId,
        ClassName = newClass.ClassName
    });
});



// Classes endpoints
app.MapGet("/api/classes", () => {
    var classesDto = classes.Select(cls => new {
        ClassName = cls.ClassName,
        Teacher = new
        {
            Name = cls.Teacher.GetName(),
            ClassId = cls.Teacher.ClassId
        },
        Students = cls.Students.Select(s => new {
            Id = s.GetStudentId(),
            Name = s.GetName()
        }).ToList()
    });

    return classesDto;
});


app.MapPost("/api/classes", ([FromBody] ClassDto classDto) => {
    // Find the teacher
    var teacher = people.OfType<Teacher>().FirstOrDefault(t => t.ClassId == classDto.TeacherId);
    if (teacher == null)
        return Results.BadRequest("Teacher not found");

    // Create the class
    var newClass = new Class(classDto.ClassName, teacher);

    // Add students if provided
    if (classDto.StudentIds != null)
    {
        foreach (var studentId in classDto.StudentIds)
        {
            var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == studentId);
            if (student != null)
                newClass.AddStudent(student);
        }
    }

    // Add to classes list
    classes.Add(newClass);

    // Save class data
    SaveClassData(newClass);

    return Results.Created($"/api/classes/{newClass.ClassName}", new
    {
        ClassName = newClass.ClassName,
        Teacher = new
        {
            Name = newClass.Teacher.GetName(),
            ClassId = newClass.Teacher.ClassId
        },
        Students = newClass.Students.Select(s => new {
            Id = s.GetStudentId(),
            Name = s.GetName()
        }).ToList()
    });
});


app.MapPost("/api/classes/{className}/students/{studentId}", (string className, string studentId) => {
    var classObj = classes.FirstOrDefault(c => c.ClassName == className);
    if (classObj == null)
        return Results.NotFound("Class not found");
    
    var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == studentId);
    if (student == null)
        return Results.NotFound("Student not found");
    
    if (classObj.Students.Contains(student))
        return Results.BadRequest("Student already in class");
    
    classObj.AddStudent(student);
    
    // Save class data
    SaveClassData(classObj);
    
    return Results.NoContent();
});

// Grades endpoints
app.MapGet("/api/students/{id}/grades", (string id) => {
    var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == id);
    if (student == null)
        return Results.NotFound();
    
    var grades = student.GetGradeHistory().Select(g => new {
        ClassId = g.Grade.ClassId,
        Value = g.Grade.GradeValue,
        Date = g.Timestamp
    });
    
    return Results.Ok(grades);
});

app.MapPost("/api/students/{id}/grades", (string id, [FromBody] GradeDto gradeDto) => {
    var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == id);
    if (student == null)
        return Results.NotFound("Student not found");
    
    var grade = new Grade(gradeDto.ClassId, gradeDto.Value);
    
    if (gradeDto.Value < 1 || gradeDto.Value > 10)
        return Results.BadRequest("Grade must be between 1 and 10");
    
    student.AddGradeToday(grade);
    
    // Save student data
    SaveStudentData(student);
    
    return Results.Created($"/api/students/{id}/grades", new {
        ClassId = grade.ClassId,
        Value = grade.GradeValue,
        Date = DateTime.Now
    });
});

// Bulk grades upload
app.MapPost("/api/classes/{classId}/bulk-grades", (string classId, [FromBody] BulkGradeUploadDto bulkUpload) => {
    var classObj = classes.FirstOrDefault(c => c.Teacher.ClassId == classId);
    if (classObj == null)
        return Results.NotFound("Class not found");
    
    var gradesToUpload = bulkUpload.Grades.Select(g => (g.StudentId, g.Value)).ToList();
    classObj.BulkUploadGrades(classId, gradesToUpload);
    
    // Save student data for each student that received a grade
    foreach (var gradeInfo in bulkUpload.Grades)
    {
        var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == gradeInfo.StudentId);
        if (student != null)
        {
            SaveStudentData(student);
        }
    }
    
    return Results.Ok("Grades uploaded successfully");
});

// Authentication endpoint
app.MapPost("/api/auth/login", ([FromBody] LoginDto login) => {
    var person = people.FirstOrDefault(p =>
        p.GetUserCredentials()?.Email == login.Email &&
        p.GetUserCredentials()?.Password == login.Password);

    if (person == null)
        return Results.Unauthorized();

    var userType = person.GetUserCredentials()?.UserType;
    string role = userType == UserType.Teacher ? "Teacher" : "Student";

    string id = "";
    if (person is Student student)
        id = student.GetStudentId();
    else if (person is Teacher teacher)
        id = teacher.ClassId;

    return Results.Ok(new
    {
        Email = person.GetUserCredentials()?.Email,
        Role = role,
        Id = id,
        Name = person.GetName()
    });
});


// Helper methods for file-based storage

// Save a student's data to a file
void SaveStudentData(Student student)
{
    var studentDto = new StoredStudentDto
    {
        Id = student.GetStudentId(),
        Name = student.GetName(),
        Credentials = student.GetUserCredentials() != null ? new StoredCredentialsDto
        {
            Username = student.GetUserCredentials().Username,
            Password = student.GetUserCredentials().Password,
            Email = student.GetUserCredentials().Email,
            UserType = student.GetUserCredentials().UserType.ToString()
        } : null,
        Grades = student.GetGradeHistory().Select(g => new StoredGradeDto
        {
            ClassId = g.Grade.ClassId,
            Value = g.Grade.GradeValue,
            Timestamp = g.Timestamp
        }).ToList()
    };

    var filePath = Path.Combine(studentsDirectory, $"{student.GetStudentId()}.json");
    var json = JsonSerializer.Serialize(studentDto, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, json);
}


// Save a teacher's data to a file
void SaveTeacherData(Teacher teacher)
{
    var teacherDto = new StoredTeacherDto
    {
        Name = teacher.GetName(),
        ClassId = teacher.ClassId,
        Credentials = teacher.GetUserCredentials() != null ? new StoredCredentialsDto
        {
            Username = teacher.GetUserCredentials().Username,
            Password = teacher.GetUserCredentials().Password,
            Email = teacher.GetUserCredentials().Email,
            UserType = teacher.GetUserCredentials().UserType.ToString()
        } : null
    };

    var filePath = Path.Combine(teachersDirectory, $"{teacher.ClassId}.json");
    var json = JsonSerializer.Serialize(teacherDto, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, json);
}


// Save a class's data to a file
void SaveClassData(Class classObj)
{
    var classDto = new StoredClassDto
    {
        ClassName = classObj.ClassName,
        TeacherId = classObj.Teacher.ClassId,
        StudentIds = classObj.Students.Select(s => s.GetStudentId()).ToList()
    };
    
    var filePath = Path.Combine(classesDirectory, $"{classObj.ClassName.Replace(" ", "_")}.json");
    var json = JsonSerializer.Serialize(classDto, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, json);
}

// Delete a student's data file
void DeleteStudentData(string studentId)
{
    var filePath = Path.Combine(studentsDirectory, $"{studentId}.json");
    if (File.Exists(filePath))
    {
        File.Delete(filePath);
    }
}

// Save all data
void SaveAllData(List<Person> people, List<Class> classes)
{
    // Save students
    foreach (var student in people.OfType<Student>())
    {
        SaveStudentData(student);
    }
    
    // Save teachers
    foreach (var teacher in people.OfType<Teacher>())
    {
        SaveTeacherData(teacher);
    }
    
    // Save classes
    foreach (var classObj in classes)
    {
        SaveClassData(classObj);
    }
}

// Load data from files
void LoadData(out List<Person> people, out List<Class> classes)
{
    people = new List<Person>();
    classes = new List<Class>();

    // Load teachers
    var teacherFiles = Directory.GetFiles(teachersDirectory, "*.json");
    foreach (var file in teacherFiles)
    {
        try
        {
            var json = File.ReadAllText(file);
            var teacherDto = JsonSerializer.Deserialize<StoredTeacherDto>(json);

            if (teacherDto != null)
            {
                var teacher = new Teacher(teacherDto.Name, teacherDto.ClassId);

                if (teacherDto.Credentials != null)
                {
                    var userType = Enum.Parse<UserType>(teacherDto.Credentials.UserType);
                    var credentials = new UserCredentials(
                        teacherDto.Credentials.Username,
                        teacherDto.Credentials.Password,
                        teacherDto.Credentials.Email,
                        userType
                    );
                    teacher.SetUserCredentials(credentials);
                }

                people.Add(teacher);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading teacher from {file}: {ex.Message}");
        }
    }

    // Load students
    var studentFiles = Directory.GetFiles(studentsDirectory, "*.json");
    foreach (var file in studentFiles)
    {
        try
        {
            var json = File.ReadAllText(file);
            var studentDto = JsonSerializer.Deserialize<StoredStudentDto>(json);

            if (studentDto != null)
            {
                var student = new Student(studentDto.Name, studentDto.Id);

                if (studentDto.Credentials != null)
                {
                    var userType = Enum.Parse<UserType>(studentDto.Credentials.UserType);
                    var credentials = new UserCredentials(
                        studentDto.Credentials.Username,
                        studentDto.Credentials.Password,
                        studentDto.Credentials.Email,
                        userType
                    );
                    student.SetUserCredentials(credentials);
                }

                // Load grades
                if (studentDto.Grades != null)
                {
                    foreach (var gradeDto in studentDto.Grades)
                    {
                        var grade = new Grade(gradeDto.ClassId, gradeDto.Value);
                        student.AddGrade(gradeDto.Timestamp, grade);
                    }
                }

                people.Add(student);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading student from {file}: {ex.Message}");
        }
    }

    // Load classes
    var classFiles = Directory.GetFiles(classesDirectory, "*.json");
    foreach (var file in classFiles)
    {
        try
        {
            var json = File.ReadAllText(file);
            var classDto = JsonSerializer.Deserialize<StoredClassDto>(json);

            if (classDto != null)
            {
                var teacher = people.OfType<Teacher>().FirstOrDefault(t => t.ClassId == classDto.TeacherId);
                if (teacher != null)
                {
                    var classObj = new Class(classDto.ClassName, teacher);

                    if (classDto.StudentIds != null)
                    {
                        foreach (var studentId in classDto.StudentIds)
                        {
                            var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == studentId);
                            if (student != null)
                            {
                                classObj.AddStudent(student);
                            }
                        }
                    }

                    classes.Add(classObj);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading class from {file}: {ex.Message}");
        }
    }
}


app.Run();

// DTOs for API request handling (keep at bottom)
public class StudentDto
{
    public string Name { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
}


public class GradeDto
{
    public string ClassId { get; set; }
    public int Value { get; set; }
}

public class BulkGradeUploadDto
{
    public List<GradeItem> Grades { get; set; }
    
    public class GradeItem
    {
        public string StudentId { get; set; }
        public int Value { get; set; }
    }
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}


public class TeacherDto
{
    public string Name { get; set; }
    public string ClassId { get; set; }        // backend-friendly unique ID
    public string? ClassName { get; set; }     // user-facing display name (optional)
    public string? Password { get; set; }
    public string? Email { get; set; }
}


public class ClassDto
{
    public string ClassName { get; set; }
    public string TeacherId { get; set; }
    public List<string>? StudentIds { get; set; }
}

// DTOs for file storage
public class StoredStudentDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public StoredCredentialsDto? Credentials { get; set; }
    public List<StoredGradeDto> Grades { get; set; } = new List<StoredGradeDto>();
}


public class StoredTeacherDto
{
    public string Name { get; set; }
    public string ClassId { get; set; }
    public StoredCredentialsDto? Credentials { get; set; }
}


public class StoredCredentialsDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string UserType { get; set; }
}

public class StoredGradeDto
{
    public string ClassId { get; set; }
    public int Value { get; set; }
    public DateTime Timestamp { get; set; }
}

public class StoredClassDto
{
    public string ClassName { get; set; }
    public string TeacherId { get; set; }
    public List<string> StudentIds { get; set; } = new List<string>();
}

public class ResetPasswordDto
{
    public string Email { get; set; }
    public string NewPassword { get; set; }
}
