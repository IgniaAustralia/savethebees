using Microsoft.AspNetCore.Identity;
using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Identity
{
    /// <summary>
    /// This model class represents the users stored in the identity database.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        #region Properties

        /// <summary>
        /// Gets and sets the application user id.
        /// </summary>
        /// <remarks>Used to associate the identity user with the application user.</remarks>
        public Guid? ApplicationUserId { get; set; }

        #endregion
    }
}