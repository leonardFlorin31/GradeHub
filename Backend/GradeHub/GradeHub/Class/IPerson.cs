namespace GradeHub.MainClasses
{
    /// <summary>
    /// Represents a basic contract for any person in the system (e.g., student, teacher).
    /// </summary>
    public interface IPerson
    {
        /// <summary>
        /// Sets the name of the person.
        /// </summary>
        /// <param name="name">The new name to assign.</param>
        void SetName(string name);

        /// <summary>
        /// Gets the name of the person.
        /// </summary>
        /// <returns>The current name of the person.</returns>
        string GetName();
    }
}
