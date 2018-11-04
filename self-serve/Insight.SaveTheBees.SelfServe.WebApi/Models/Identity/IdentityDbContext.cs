using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Identity
{
    /// <summary>
    /// The EF database context class that is used to connect with the identity database.
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<User, Role, Guid>
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the <see cref="IdentityDbContext" /> class and
        /// initialises the default identity database components.
        /// </summary>
        /// <param name="options">The context options.</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        #endregion
    }
}