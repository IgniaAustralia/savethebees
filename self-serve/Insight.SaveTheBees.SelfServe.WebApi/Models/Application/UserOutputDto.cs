using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Application
{
    /// <summary>
    /// This DTO class represents the user (output).
    /// </summary>
    public class UserOutputDto
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        public string UserName { get; set; }
    }
}
