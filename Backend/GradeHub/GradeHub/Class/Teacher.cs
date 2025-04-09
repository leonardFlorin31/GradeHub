namespace GradeHub.MainClasses
{
    /// <summary>
    /// Represents a teacher in the system.
    /// Inherits from <see cref="Person"/>.
    /// </summary>
    public class Teacher : Person
    {
        /// <summary>
        /// Gets or sets the ID of the class this teacher is assigned to.
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Teacher"/> class.
        /// </summary>
        /// <param name="name">The name of the teacher.</param>
        /// <param name="classId">The unique class ID managed by the teacher.</param>
        public Teacher(string name, string classId) : base(name)
        {
            ClassId = classId;
        }
    }
}
