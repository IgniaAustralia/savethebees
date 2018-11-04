namespace Insight.SaveTheBees.SelfServe.WebApi
{
    /// <summary>
    /// Represents the app setting keys for the application.
    /// </summary>
    public struct Settings
    {
        /// <summary>
        /// Represents the app setting keys for the authentication.
        /// </summary>
        public struct Authentication
        {
            /// <summary>
            /// The base address of the token issuer.
            /// </summary>
            public const string TokenIssuer = "Authentication:TokenIssuer";

            /// <summary>
            /// The audience of the token.
            /// </summary>
            public const string Audience = "Authentication:Audience";
        }

        /// <summary>
        /// Represents the app setting keys for the connection strings.
        /// </summary>
        public struct ConnectionStrings
        {
            /// <summary>
            /// The application database connection string.
            /// </summary>
            public const string Application = "ApplicationDatabase";

            /// <summary>
            /// The identity database connection string.
            /// </summary>
            public const string Identity = "IdentityDatabase";
        }
    }
}
