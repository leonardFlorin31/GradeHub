namespace GradeHub.MainClasses
{
    /// <summary>
    /// Base class representing a person in the system.
    /// Implements <see cref="IPerson"/> interface.
    /// </summary>
    public class Person : IPerson
    {
        private string _name;
        private UserCredentials _userCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class with the given name.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        public Person(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Sets the credentials for the person.
        /// </summary>
        /// <param name="userCredentials">The credentials to assign.</param>
        public void SetUserCredentials(UserCredentials userCredentials)
        {
            _userCredentials = userCredentials;
        }

        /// <summary>
        /// Gets the credentials associated with the person.
        /// </summary>
        /// <returns>The <see cref="UserCredentials"/> object.</returns>
        public UserCredentials GetUserCredentials()
        {
            return _userCredentials;
        }

        /// <inheritdoc/>
        public void SetName(string name)
        {
            _name = name;
        }

        /// <inheritdoc/>
        public string GetName()
        {
            return _name;
        }
    }
}
