namespace AutoUpdaterDotNET
{
    /// <summary>
    ///     Provides Custom Authentication header for web request.
    /// </summary>
    public class CustomAuthentication : IAuthentication
	{
        /// <summary>
        ///     Initializes authorization header value for Custom Authentication
        /// </summary>
        /// <param name="httpRequestHeaderAuthorizationValue">Value to use as http request header authorization value</param>
        public CustomAuthentication(string httpRequestHeaderAuthorizationValue) { HttpRequestHeaderAuthorizationValue = httpRequestHeaderAuthorizationValue; }

		private string HttpRequestHeaderAuthorizationValue { get; }

		/// <inheritdoc />
		public override string ToString() { return HttpRequestHeaderAuthorizationValue; }
	}
}
