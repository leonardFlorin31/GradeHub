using GradeHub.Class;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeHub.MainClasses
{
    /// <summary>
    /// Represents a student in the system. Inherits from <see cref="Person"/>.
    /// </summary>
    public class Student : Person
    {
        private List<GradeEntry> _gradeHistory;
        private string _studentId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class.
        /// </summary>
        /// <param name="name">The student's name.</param>
        /// <param name="studentId">The student's unique ID.</param>
        public Student(string name, string studentId) : base(name)
        {
            _studentId = studentId;
            _gradeHistory = new List<GradeEntry>();
        }

        /// <summary>
        /// Adds a grade for the student with a specific timestamp.
        /// </summary>
        /// <param name="timestamp">The date and time of the grade.</param>
        /// <param name="grade">The grade object.</param>
        public void AddGrade(DateTime timestamp, Grade grade)
        {
            if (grade.GradeValue > 0 && grade.GradeValue <= 10)
            {
                _gradeHistory.Add(new GradeEntry(timestamp, grade)); // auto-ID
            }
            else
            {
                Console.WriteLine($"Invalid grade value: {grade}. Grade must be between 1 and 10.");
            }
        }

        /// <summary>
        /// Adds a pre-constructed <see cref="GradeEntry"/> to the student's grade history.
        /// </summary>
        /// <param name="entry">The grade entry to add.</param>
        public void AddGrade(GradeEntry entry)
        {
            _gradeHistory.Add(entry);
        }

        /// <summary>
        /// Adds a grade using the current system time as the timestamp.
        /// </summary>
        /// <param name="grade">The grade object.</param>
        public void AddGradeToday(Grade grade)
        {
            if (grade.GradeValue > 0 && grade.GradeValue <= 10)
            {
                _gradeHistory.Add(new GradeEntry(DateTime.Now, grade)); // auto-ID
            }
            else
            {
                Console.WriteLine($"Invalid grade value: {grade}. Grade must be between 1 and 10.");
            }
        }

        /// <summary>
        /// Retrieves the student's full grade history.
        /// </summary>
        /// <returns>A list of <see cref="GradeEntry"/> objects.</returns>
        public List<GradeEntry> GetGradeHistory()
        {
            return _gradeHistory;
        }

        /// <summary>
        /// Retrieves the student's grades as a dictionary of timestamps and grade values.
        /// </summary>
        /// <returns>A dictionary mapping <see cref="DateTime"/> to <see cref="Grade"/>.</returns>
        public Dictionary<DateTime, Grade> GetGrades()
        {
            return _gradeHistory.ToDictionary(entry => entry.Timestamp, entry => entry.Grade);
        }

        /// <summary>
        /// Sets the student's ID.
        /// </summary>
        /// <param name="studentId">The new student ID.</param>
        public void SetStudentId(string studentId)
        {
            _studentId = studentId;
        }

        /// <summary>
        /// Gets the student's unique ID.
        /// </summary>
        /// <returns>The student ID.</returns>
        public string GetStudentId()
        {
            return _studentId;
        }

        /// <summary>
        /// Removes the first grade entry that matches the given value.
        /// </summary>
        /// <param name="value">The grade value to remove.</param>
        public void RemoveFirstGradeByValue(int value)
        {
            var gradeToRemove = _gradeHistory.FirstOrDefault(g => g.Grade.GradeValue == value);
            if (gradeToRemove != null)
            {
                _gradeHistory.Remove(gradeToRemove);
            }
        }

        /// <summary>
        /// Removes a grade entry by its unique identifier.
        /// </summary>
        /// <param name="gradeId">The GUID of the grade entry.</param>
        /// <returns>True if removed; false if not found.</returns>
        public bool RemoveGradeById(Guid gradeId)
        {
            var gradeToRemove = _gradeHistory.FirstOrDefault(g => g.Id == gradeId);
            if (gradeToRemove != null)
            {
                _gradeHistory.Remove(gradeToRemove);
                return true;
            }
            return false;
        }
    }
}
