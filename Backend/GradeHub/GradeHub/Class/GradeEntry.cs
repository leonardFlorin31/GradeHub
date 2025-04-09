using GradeHub.MainClasses;
using System;

namespace GradeHub.Class
{
    /// <summary>
    /// Represents a grade entry for a student, including its unique identifier, timestamp, and grade details.
    /// </summary>
    public class GradeEntry
    {
        /// <summary>
        /// Gets the unique identifier for the grade entry.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets the timestamp when the grade was recorded.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Gets the <see cref="Grade"/> associated with this entry.
        /// </summary>
        public Grade Grade { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeEntry"/> class with a specified ID, timestamp, and grade.
        /// </summary>
        /// <param name="id">The unique identifier of the grade entry.</param>
        /// <param name="timestamp">The date and time the grade was recorded.</param>
        /// <param name="grade">The grade details.</param>
        public GradeEntry(Guid id, DateTime timestamp, Grade grade)
        {
            Id = id;
            Timestamp = timestamp;
            Grade = grade;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeEntry"/> class with a new ID, timestamp, and grade.
        /// </summary>
        /// <param name="timestamp">The date and time the grade was recorded.</param>
        /// <param name="grade">The grade details.</param>
        public GradeEntry(DateTime timestamp, Grade grade)
        {
            Id = Guid.NewGuid(); // new ID for new entries
            Timestamp = timestamp;
            Grade = grade;
        }
    }
}
