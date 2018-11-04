using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Insight.SaveTheBees.SelfServe.WebApi.Exceptions
{
    /// <summary>
    /// This exception class represents the list of identity errors that is returned from
    /// an identity action.
    /// </summary>
    public class IdentityErrorsException : Exception
    {
        #region Properties

        /// <summary>
        /// Gets and sets the list of identity errors.
        /// </summary>
        public IList<IdentityError> IdentityErrors { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="IdentityErrorsException" /> and
        /// sets list of identity errors.
        /// </summary>
        /// <param name="errors">The list of identity errors.</param>
        public IdentityErrorsException(IList<IdentityError> errors) : base()
        {
            IdentityErrors = errors;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message => ConvertErrorsToMessage();

        private string ConvertErrorsToMessage()
        {
            return string.Join("\n", IdentityErrors.Select(x => $"{x.Code}: {x.Description}"));
        }

        #endregion
    }
}
