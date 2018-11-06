using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data
{
    /// <summary>
    /// This class model represents the hive stored in the application database.
    /// </summary>
    public class Hive
    {
        /// <summary>
        /// Gets and sets the hive id.
        /// </summary>
        [Key]
        public Guid HiveId { get; set; }

        /// <summary>
        /// Gets and sets the hive name.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the hive's user.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Gets and sets the hive's user id.
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets the hive's cluster.
        /// </summary>
        public HiveCluster Cluster { get; set; }

        /// <summary>
        /// Gets and sets the hive's cluster id.
        /// </summary>
        [ForeignKey("Cluster")]
        public Guid? ClusterId { get; set; }

        /// <summary>
        /// Gets and sets the row version.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
