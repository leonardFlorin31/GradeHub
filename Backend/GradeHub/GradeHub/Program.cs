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

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
{
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
    classes.Add(new Class("Math 101", teacher));

    // Add students to the class
    new Class("Math 101", teacher).AddStudent(student1);
    new Class("Math 101", teacher).AddStudent(student2);
    new Class("Math 101", teacher).AddStudent(student3);

    // Add some initial grades
    student1.AddGrade(DateTime.Now.AddDays(-7), new Grade("Math101", 8));
    student1.AddGradeToday(new Grade("Math101", 5));

    // Save initial data
    SaveAllData(people, classes);
}

// Get reference to the first class (for demo purposes)
var mathClass = classes.FirstOrDefault();

// API Endpoints

// Students endpoints
app.MapGet("/api/students", () => {
    var students = people.OfType<Student>().Select(s => new {
        Id = s.GetStudentId(),
        Name = new { 
            FirstName = s.GetName().firstName, 
            LastName = s.GetName().lastName 
        },
        Age = s.GetAge()
    });
    return students;
});

app.MapGet("/api/students/{id}", (string id) => {
    var student = people.OfType<Student>().FirstOrDefault(s => s.GetStudentId() == id);
    if (student == null)
        return Results.NotFound();
        
    return Results.Ok(new {
        Id = student.GetStudentId(),
        Name = new { 
            FirstName = student.GetName().firstName, 
            LastName = student.GetName().lastName 
        },
        Age = student.GetAge()
    });
});

app.MapPost("/api/students", ([FromBody] StudentDto studentDto) => {
    var student = new Student(studentDto.FirstName, studentDto.LastName, studentDto.Age, studentDto.Id);
    
    if (studentDto.Username != null && studentDto.Password != null && studentDto.Email != null)
    {
        var credentials = new UserCredentials(
            studentDto.Username, 
            studentDto.Password, 
            studentDto.Email, 
            UserType.Student
        );
        student.SetUserCredentials(credentials);
    }
    
    people.Add(student);
    
    // Save student data
    SaveStudentData(student);
    
    return Results.Created($"/api/students/{student.GetStudentId()}", new {
        Id = student.GetStudentId(),
        Name = new { 
            FirstName = student.GetName().firstName, 
            LastName = student.GetName().lastName 
        },
        Age = student.GetAge()
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
        Name = new { 
            FirstName = t.GetName().firstName, 
            LastName = t.GetName().lastName 
        },
        Age = t.GetAge(),
        ClassId = t.ClassId
    });
    return teachers;
});

app.MapPost("/api/teachers", ([FromBody] TeacherDto teacherDto) => {
    var teacher = new Teacher(
        teacherDto.FirstName, 
        teacherDto.LastName, 
        teacherDto.Age, 
        teacherDto.ClassId
    );
    
    if (teacherDto.Username != null && teacherDto.Password != null && teacherDto.Email != null)
    {
        var credentials = new UserCredentials(
            teacherDto.Username, 
            teacherDto.Password, 
            teacherDto.Email, 
            UserType.Teacher
        );
        teacher.SetUserCredentials(credentials);
    }
    
    people.Add(teacher);
    
    // Save teacher data
    SaveTeacherData(teacher);
    
    return Results.Created($"/api/teachers/{teacher.ClassId}", new {
        Name = new { 
            FirstName = teacher.GetName().firstName, 
            LastName = teacher.GetName().lastName 
        },
        Age = teacher.GetAge(),
        ClassId = teacher.ClassId
    });
});

// Classes endpoints
app.MapGet("/api/classes", () => {
    var classesDto = classes.Select(cls => new {
        ClassName = cls.ClassName,
        Teacher = new {
            Name = new { 
                FirstName = cls.Teacher.GetName().firstName, 
                LastName = cls.Teacher.GetName().lastName 
            },
            ClassId = cls.Teacher.ClassId
        },
        Students = cls.Students.Select(s => new {
            Id = s.GetStudentId(),
            Name = new { 
                FirstName = s.GetName().firstName, 
                LastName = s.GetName().lastName 
            }
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
    
    return Results.Created($"/api/classes/{newClass.ClassName}", new {
        ClassName = newClass.ClassName,
        Teacher = new {
            Name = new { 
                FirstName = newClass.Teacher.GetName().firstName, 
                LastName = newClass.Teacher.GetName().lastName 
            },
            ClassId = newClass.Teacher.ClassId
        },
        Students = newClass.Students.Select(s => new {
            Id = s.GetStudentId(),
            Name = new { 
                FirstName = s.GetName().firstName, 
                LastName = s.GetName().lastName 
            }
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
        p.GetUserCredentials()?.Username == login.Username && 
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
    
    return Results.Ok(new {
        Username = person.GetUserCredentials()?.Username,
        Email = person.GetUserCredentials()?.Email,
        Role = role,
        Id = id,
        Name = new { 
            FirstName = person.GetName().firstName, 
            LastName = person.GetName().lastName 
        }
    });
});

// Helper methods for file-based storage

// Save a student's data to a file
void SaveStudentData(Student student)
{
    var studentDto = new StoredStudentDto
    {
        Id = student.GetStudentId(),
        FirstName = student.GetName().firstName,
        LastName = student.GetName().lastName,
        Age = student.GetAge(),
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
        FirstName = teacher.GetName().firstName,
        LastName = teacher.GetName().lastName,
        Age = teacher.GetAge(),
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
    
    // Load teachers first
    var teacherFiles = Directory.GetFiles(teachersDirectory, "*.json");
    foreach (var file in teacherFiles)
    {
        try
        {
            var json = File.ReadAllText(file);
            var teacherDto = JsonSerializer.Deserialize<StoredTeacherDto>(json);
            
            if (teacherDto != null)
            {
                var teacher = new Teacher(
                    teacherDto.FirstName,
                    teacherDto.LastName,
                    teacherDto.Age,
                    teacherDto.ClassId
                );
                
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
                var student = new Student(
                    studentDto.FirstName,
                    studentDto.LastName,
                    studentDto.Age,
                    studentDto.Id
                );
                
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
                    
                    // Add students
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
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string? Username { get; set; }
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
    public string Username { get; set; }
    public string Password { get; set; }
}

public class TeacherDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string ClassId { get; set; }
    public string? Username { get; set; }
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
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public StoredCredentialsDto? Credentials { get; set; }
    public List<StoredGradeDto> Grades { get; set; } = new List<StoredGradeDto>();
}

public class StoredTeacherDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
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