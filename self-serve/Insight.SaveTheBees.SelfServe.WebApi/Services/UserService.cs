using AutoMapper;
using IdentityModel;
using Insight.SaveTheBees.SelfServe.WebApi.Exceptions;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Insight.SaveTheBees.SelfServe.WebApi.Services
{
    public class UserService : IUserService
    {
        #region Members

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="UserService" /> and set the
        /// internal components.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="applicationDbContext">The application DB context.</param>
        /// <param name="mapper">The mapper instance.</param>
        public UserService (UserManager<User> userManager, ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the user in the identity database by the username asynchronously.
        /// </summary>
        /// <param name="userName">The username to find by.</param>
        /// <returns>An identity user.</returns>
        public async Task<User> FindUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// Creates the user in both the identity and application database
        /// asynchronously.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        public async Task CreateUserAsync(UserDto user)
        {
            // Checks if user already exists in identity
            var dbUser = await FindUserByNameAsync(user.UserName);
            if (dbUser != null) throw new UserExistsException($"{user.UserName} already exists");

            // Create user in application
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            _applicationDbContext.Users.Add(applicationUser);
            await _applicationDbContext.SaveChangesAsync();

            // Create user in identity
            dbUser = _mapper.Map<User>(user);
            dbUser.ApplicationUserId = applicationUser.UserId;
            var result = await _userManager.CreateAsync(dbUser, user.Password);
            if (!result.Succeeded) throw new IdentityErrorsException(result.Errors.ToList());

            // Add the claims
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(user.EmailAddress)) claims.Add(new Claim(JwtClaimTypes.Email, user.EmailAddress));
            if (!string.IsNullOrEmpty(user.FirstName)) claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            if (!string.IsNullOrEmpty(user.LastName)) claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            if (!string.IsNullOrEmpty(user.FullName)) claims.Add(new Claim(JwtClaimTypes.Name, user.FullName));
            claims.Add(new Claim("username", user.UserName));
            claims.Add(new Claim("user_id", applicationUser.UserId.ToString()));
            result = await _userManager.AddClaimsAsync(dbUser, claims);
            if (!result.Succeeded) throw new IdentityErrorsException(result.Errors.ToList());
        }

        #endregion
    }
}
