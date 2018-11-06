using Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Insight.SaveTheBees.SelfServe.WebApi.Controllers
{
    /// <summary>
    /// This abstract base class contains the base methods that controllers implementing
    /// this class will use.
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        #region Properties

        /// <summary>
        /// Gets and sets the database context.
        /// </summary>
        public ApplicationDbContext Context { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="BaseController" /> and sets the
        /// internal components.
        /// </summary>
        /// <param name="context">The database context.</param>
        public BaseController(ApplicationDbContext context)
        {
            Context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the user has permissions to access resources for specified user.
        /// </summary>
        /// <param name="userId">The user id to check permissions against.</param>
        /// <returns>True iif the user has permissions to access resources for user.</returns>
        public async Task<bool> ValidatePermission(Guid userId)
        {
            // Retrieve user
            var username = GetUserName();
            var user = await Context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null || user.UserName != username) return false;
            return true;
        }

        /// <summary>
        /// Creates a <see cref="ContentResult" /> object that produces a 500 Internal
        /// Server Error response with a message.
        /// </summary>
        /// <param name="controller">The controller to return internal server error
        /// response.</param>
        /// <param name="errorMessage">The error message in the response.</param>
        /// <returns>A 500 internal server error response as a
        /// <see cref="ContentResult" /></returns>
        public ContentResult InternalServerError(string errorMessage)
        {
            var result = Content(errorMessage);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            return result;
        }

        /// <summary>
        /// Gets the username from the controller user claim.
        /// </summary>
        /// <param name="controller">The controller to get the user name from.</param>
        /// <returns>A user name as a string.</returns>
        public string GetUserName()
        {
            if (User == null || User.Claims == null || !User.Claims.Any(x => x.Type == "username")) return string.Empty;
            return User.Claims.FirstOrDefault(x => x.Type == "username").Value;
        }

        #endregion
    }
}