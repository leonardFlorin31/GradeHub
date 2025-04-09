using GradeHub.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeHub.Class
{
    /// <summary>
    /// Represents a school class with a name, a teacher, and a list of assigned students.
    /// </summary>
    public class Class
    {
        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the teacher assigned to this class.
        /// </summary>
        public Teacher Teacher { get; set; }

        /// <summary>
        /// Gets the list of students assigned to this class.
        /// </summary>
        public List<Student> Students { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <param name="teacher">The teacher assigned to the class.</param>
        public Class(string className, Teacher teacher)
        {
            ClassName = className;
            Teacher = teacher;
            Students = new List<Student>();
        }

        /// <summary>
        /// Adds a student to the class if they are not already assigned.
        /// </summary>
        /// <param name="student">The student to add.</param>
        public void AddStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                Students.Add(student);
                Console.WriteLine($"{student.GetName()} added to {ClassName}.");
            }
            else
            {
                Console.WriteLine($"{student.GetName()} is already in {ClassName}.");
            }
        }

        /// <summary>
        /// Removes a student from the class if they are currently assigned.
        /// </summary>
        /// <param name="student">The student to remove.</param>
        public void RemoveStudent(Student student)
        {
            if (Students.Contains(student))
            {
                Students.Remove(student);
                Console.WriteLine($"{student.GetName()} removed from {ClassName}.");
            }
            else
            {
                Console.WriteLine($"{student.GetName()} is not in {ClassName}.");
            }
        }

        /// <summary>
        /// Uploads multiple grades at once for students in this class.
        /// </summary>
        /// <param name="classId">The ID of the class these grades belong to.</param>
        /// <param name="grades">A list of tuples containing student IDs and grade values.</param>
        public void BulkUploadGrades(string classId, List<(string studentId, int gradeValue)> grades)
        {
            Console.WriteLine($"\nStarting bulk grade upload for class: {ClassName} ({classId})");

            foreach (var gradeData in grades)
            {
                var studentId = gradeData.studentId;
                var gradeValue = gradeData.gradeValue;

                if (gradeValue > 0 && gradeValue <= 10)
                {
                    var student = Students.FirstOrDefault(s => s.GetStudentId() == studentId);

                    if (student != null)
                    {
                        var grade = new Grade(classId, gradeValue);
                        student.AddGradeToday(grade);
                        Console.WriteLine($"  Uploaded grade {gradeValue} for student ID {studentId} ({student.GetName()}).");
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
