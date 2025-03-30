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
    }
}
