using GradeHub.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeHub.Class
{
    public class Class
    {
        public string ClassName { get; set; }
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; private set; }

        public Class(string className, Teacher teacher)
        {
            ClassName = className;
            Teacher = teacher;
            Students = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                Students.Add(student);
                Console.WriteLine($"{student.GetName().firstName} {student.GetName().lastName} added to {ClassName}.");
            }
            else
            {
                Console.WriteLine($"{student.GetName().firstName} {student.GetName().lastName} is already in {ClassName}.");
            }
        }

        public void RemoveStudent(Student student)
        {
            if (Students.Contains(student))
            {
                Students.Remove(student);
                Console.WriteLine($"{student.GetName().firstName} {student.GetName().lastName} removed from {ClassName}.");
            }
            else
            {
                Console.WriteLine($"{student.GetName().firstName} {student.GetName().lastName} is not in {ClassName}.");
            }
        }

        public void BulkUploadGrades(string classId, List<(string studentId, int gradeValue)> grades)
        {
            Console.WriteLine($"\nStarting bulk grade upload for class: {ClassName} ({classId})");
            foreach (var gradeData in grades)
            {
                var studentId = gradeData.studentId;
                var gradeValue = gradeData.gradeValue;

                // Grade Validation Logic
                if (gradeValue > 0 && gradeValue <= 10)
                {
                    var student = Students.FirstOrDefault(s => s.GetStudentId() == studentId);

                    if (student != null)
                    {
                        var grade = new Grade(classId, gradeValue);
                        student.AddGradeToday(grade);
                        Console.WriteLine($"  Uploaded grade {gradeValue} for student ID {studentId} ({student.GetName().firstName} {student.GetName().lastName}).");
                    }
                    else
                    {
                        Console.WriteLine($"  Error: Student with ID {studentId} not found in class {ClassName}.");
                    }
                }
                else
                {
                    Console.WriteLine($"  Warning: Invalid grade value '{gradeValue}' for student ID {studentId}. Grade not uploaded.");
                }
            }
            Console.WriteLine("Bulk grade upload complete.");
        }
    }
}
