using Insight.SaveTheBees.SelfServe.WebApi.Exceptions;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application;
using Insight.SaveTheBees.SelfServe.WebApi.Models.Application.Data;
using Insight.SaveTheBees.SelfServe.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insight.SaveTheBees.SelfServe.WebApi.Controllers
{
    /// <summary>
    /// This controller class contains all the end points to manage users in both the
    /// identity and application databases.
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        #region Members

        private readonly IUserService _userService;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of <see cref="UsersController" /> and initialises
        /// all internal components.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="context">The application DB context.</param>
        public UsersController(IUserService userService, ApplicationDbContext context) : base(context)
        {
            _userService = userService;
        }

        #endregion

        #region End Points
        
        /// <summary>
        /// Creates the new user in both the identity and application databases.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        /// <returns>Action result for the end point.</returns>
        [HttpPut]
        public async Task<IActionResult> CreateUser([FromBody]UserDto user)
        {
            if (user == null) return BadRequest();
            if (!ValidateUserDto(user, out var errorMessage)) return BadRequest(errorMessage);

            // Find the user in identity database
            var dbUser = await _userService.FindUserByNameAsync(user.UserName);
            if (dbUser != null) return BadRequest($"{user.UserName} already exists");

            // Create user in identity database
            try
            {
                await _userService.CreateUserAsync(user);
            }
            catch (UserExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            return NoContent();
        }        

        #endregion

        #region Methods

        private bool ValidateUserDto(UserDto user, out string errorMessage)
        {
            errorMessage = null;
            var errors = new List<string>();
            if (string.IsNullOrEmpty(user.UserName)) errors.Add("Missing username");
            if (string.IsNullOrEmpty(user.Password)) errors.Add("Missing password");
            if (string.IsNullOrEmpty(user.EmailAddress)) errors.Add("Missing email address");
            if (string.IsNullOrEmpty(user.FullName)) errors.Add("Missing full name");
            if (!errors.Any()) return true;

            errorMessage = string.Join("\n", errors);
            return false;
        }

        //private async Task<User> CreateIdentityUser(UserDto user)
        //{
        //    var dbUser = _mapper.Map<User>(user);

        //    // Create user
        //    var result = await _userManager.CreateAsync(dbUser, user.Password);
        //    if (!result.Succeeded)
        //    {
        //        throw new IdentityErrorsException(result.Errors.ToList());
        //    }

        //    // Set user claims
        //    var claims = new List<Claim>();
        //    result = await _userManager.AddClaimsAsync()

        //    return dbUser;
        //}

        #endregion
    }
}