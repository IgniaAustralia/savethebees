using Insight.SaveTheBees.SelfServe.WebApi.Models.Application;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Identity;
using System.Threading.Tasks;

namespace Insight.SaveTheBees.SelfServe.WebApi.Services
{
    /// <summary>
    /// This interface contains the methods to manage users in both the identity and
    /// application databases.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Finds the user in the identity database by the username asynchronously.
        /// </summary>
        /// <param name="userName">The username to find by.</param>
        /// <returns>An identity user.</returns>
        Task<User> FindUserByNameAsync(string userName);

        /// <summary>
        /// Creates the user in both the identity and application database
        /// asynchronously.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        Task CreateUserAsync(UserDto user);
    }
}