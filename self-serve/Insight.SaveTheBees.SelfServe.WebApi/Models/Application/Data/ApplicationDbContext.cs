using Microsoft.EntityFrameworkCore;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data
{
    /// <summary>
    /// The EF database context class that is used to connect with the application
    /// database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Gets and sets the application users.
        /// </summary>
        public DbSet<ApplicationUser> Users { get; set; }

        /// <summary>
        /// Gets and sets the hive clusters.
        /// </summary>
        public DbSet<HiveCluster> Clusters { get; set; }

        /// <summary>
        /// Gets and sets the hives.
        /// </summary>
        public DbSet<Hive> Hives { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the <see cref="ApplicationDbContext" /> class
        /// and initialises the default identity database components.
        /// </summary>
        /// <param name="options">The context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #endregion
    }
}
