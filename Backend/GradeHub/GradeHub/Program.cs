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

// Initialize sample data
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

// Add some initial grades
student1.AddGrade(DateTime.Now.AddDays(-7), new Grade("Math101", 8));
student1.AddGradeToday(new Grade("Math101", 5));

// API Endpoints

// Students endpoints
app.MapGet("/api/students", () => people.OfType<Student>().Select(s => new {
    Id = s.GetStudentId(),
    Name = new { 
        FirstName = s.GetName().firstName, 
        LastName = s.GetName().lastName 
    },
    Age = s.GetAge()
}));

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

// Teachers endpoints
app.MapGet("/api/teachers", () => people.OfType<Teacher>().Select(t => new {
    Name = new { 
        FirstName = t.GetName().firstName, 
        LastName = t.GetName().lastName 
    },
    Age = t.GetAge(),
    ClassId = t.ClassId
}));

// Classes endpoints
app.MapGet("/api/classes", () => {
    var classDto = new {
        ClassName = mathClass.ClassName,
        Teacher = new {
            Name = new { 
                FirstName = mathClass.Teacher.GetName().firstName, 
                LastName = mathClass.Teacher.GetName().lastName 
            },
            ClassId = mathClass.Teacher.ClassId
        },
        Students = mathClass.Students.Select(s => new {
            Id = s.GetStudentId(),
            Name = new { 
                FirstName = s.GetName().firstName, 
                LastName = s.GetName().lastName 
            }
        }).ToList()
    };
    
    return new[] { classDto };
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