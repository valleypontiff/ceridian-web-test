using CeridianWebTest.Browsers;

namespace CeridianWebTest.Configuration
{
    internal class EnvironmentConfiguration
    {
        /// <summary>
        /// The URL of the website.
        /// </summary>
        /// <example>https://www.irissoftware.com</example>
        public Uri? BaseUrl { get; init; }

        /// <summary>
        /// Maximum time to wait for web page elements to load/change.
        /// </summary>
        /// <example>00:00:30</example>
        public TimeSpan WaitTimeout { get; init; }

        /// <summary>
        /// The browsers to test against.
        /// </summary>
        /// <example>Chrome, Firefox</example>
        public BrowserType[]? Browsers { get; init; }

        public static IEnumerable<object[]> BrowserTypes
        {
            get
            {
                EnvironmentConfiguration config = ConfigHelper.GetEnvironmentConfiguration();
                if (null == config.Browsers || config.Browsers.Length == 0)
                {
                    throw new Exception("No browsers provided in configuration");
                }

                foreach (var browserType in config.Browsers)
                {
                    yield return new object[] { browserType };
                }
            }
        }
    }
}
