using System;
using System.ComponentModel.DataAnnotations;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data
{
    /// <summary>
    /// This model class represents the user stored in the application database.
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the row version.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
