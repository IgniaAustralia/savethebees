using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data
{
    /// <summary>
    /// This class model represents the hive clusters stored in the application database.
    /// </summary>
    public class HiveCluster
    {
        /// <summary>
        /// Gets and sets the cluster id.
        /// </summary>
        [Key]
        public Guid ClusterId { get; set; }

        /// <summary>
        /// Gets and sets the cluster name.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the cluster's user.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Gets and sets the cluster's user id.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the row version.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
