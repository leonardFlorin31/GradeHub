namespace GradeHub.MainClasses
{
    /// <summary>
    /// Represents a grade assigned to a student for a specific class.
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Gets or sets the ID of the class the grade is associated with.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Gets or sets the numeric value of the grade.
        /// </summary>
        public int GradeValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grade"/> class.
        /// </summary>
        /// <param name="classId">The ID of the class for which the grade is given.</param>
        /// <param name="gradeValue">The value of the grade (typically between 1 and 10).</param>
        public Grade(string classId, int gradeValue)
        {
            ClassId = classId;
            GradeValue = gradeValue;
        }
    }
}
