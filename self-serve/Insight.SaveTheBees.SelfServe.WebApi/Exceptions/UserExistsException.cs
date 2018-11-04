using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Exceptions
{
    /// <summary>
    /// This exception class represents the exception that is thrown when the user exists
    /// in the identity database.
    /// </summary>
    public class UserExistsException : Exception
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="UserExistsException" />.
        /// </summary>
        public UserExistsException() : base()
        {
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="UserExistsException" /> and sets
        /// exception message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UserExistsException(string message) : base(message)
        {
        }

        #endregion
    }
}