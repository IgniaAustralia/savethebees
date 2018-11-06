using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Application
{
    /// <summary>
    /// This DTO class represents the hive cluster (output).
    /// </summary>
    public class ClusterOutputDto : ClusterDto
    {
        /// <summary>
        /// Gets and sets the cluster id.
        /// </summary>
        public Guid ClusterId { get; set; }

        /// <summary>
        /// Gets and sets the user id.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
