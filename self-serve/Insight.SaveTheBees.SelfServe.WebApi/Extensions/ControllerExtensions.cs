using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Insight.SaveTheBees.SelfServe.WebApi.Extensions
{
    /// <summary>
    /// This static class contains extension methods for the controllers.
    /// </summary>
    public static class ControllerExtensions
    {
        public static ContentResult InternalServerError(this ControllerBase controller, string errorMessage)
        {
            var result = controller.Content(errorMessage);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            return result;
        }
    }
}