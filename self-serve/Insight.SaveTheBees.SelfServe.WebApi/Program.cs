using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Insight.SaveTheBees.SelfServe.WebApi
{
    /// <summary>
    /// The static class that contains the main method used to start up the application.
    /// </summary>
    public static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry method of the application.
        /// </summary>
        /// <param name="args">The list of arguments provided to the application.</param>
        public static void Main(string[] args) => BuildWebHost(args).Run();

        /// <summary>
        /// Builds the web host that is used to start the application.
        /// </summary>
        /// <param name="args">The arguments provided to the builder.</param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();

        #endregion
    }
}