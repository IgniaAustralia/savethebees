using Microsoft.AspNetCore.Identity;
using System;

namespace Insight.SaveTheBees.SelfServe.WebApi.Models.Identity
{
    /// <summary>
    /// This model class represents the roles stored in the identity database. 
    /// </summary>
    public class Role : IdentityRole<Guid>
    {
    }
}