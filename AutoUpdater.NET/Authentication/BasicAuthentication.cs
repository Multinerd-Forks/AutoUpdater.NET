using System;
using System.Text;

namespace AutoUpdaterDotNET
{
    /// <summary>
    ///     Provides Basic Authentication header for web request.
    /// </summary>
    public class BasicAuthentication : IAuthentication
	{
        /// <summary>
        ///     Initializes credentials for Basic Authentication.
        /// </summary>
        /// <param name="username">Username to use for Basic Authentication</param>
        /// <param name="password">Password to use for Basic Authentication</param>
        public BasicAuthentication(string username, string password)
		{
			Username = username;
			Password = password;
		}

		private string Username { get; }

		private string Password { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
			return $"Basic {token}";
		}
	}
}
