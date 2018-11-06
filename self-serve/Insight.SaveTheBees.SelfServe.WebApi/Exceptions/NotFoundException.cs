using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Exceptions
{
    /// <summary>
    /// This exception class represents the exception that is thrown when an entity is
    /// not found in the database.
    /// </summary>
    public class NotFoundException : Exception
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="NotFoundException" />.
        /// </summary>
        public NotFoundException() : base()
        {
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="NotFoundException" /> and sets
        /// exception message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public NotFoundException(string message) : base(message)
        {
        }

        #endregion
    }
}