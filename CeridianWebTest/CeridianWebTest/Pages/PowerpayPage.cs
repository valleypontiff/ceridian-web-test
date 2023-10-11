using CeridianWebTest.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CeridianWebTest.Pages
{
    /// <summary>
    /// Contains interactions with the Get Started Powerpay page
    /// </summary>
    internal class PowerpayPage : LoadableComponent<PowerpayPage>
    {
        private readonly Uri pageUrl;

        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private readonly By emailBoxBy = By.Id("Email");
        private IWebElement EmailBox => driver.FindElement(emailBoxBy);

        private readonly By emailErrorBy = By.Id("Email-error");
        private IWebElement EmailError => driver.FindElement(emailErrorBy);

        private readonly By cookiePreferencesButtonBy = By.Id("onetrust-pc-btn-handler");

        public PowerpayPage(IWebDriver driver)
        {
            EnvironmentConfiguration config = ConfigHelper.GetEnvironmentConfiguration();

#pragma warning disable CS8604 // Possible null reference argument (it's validated in ConfigHelper so shouldn't be possible for it to be null here).
            pageUrl = new Uri(config.BaseUrl, "ca/get-started/powerpay");
#pragma warning restore CS8604 // Possible null reference argument.

            this.driver = driver;
            wait = new WebDriverWait(driver, config.WaitTimeout);
        }

        public void SubmitForm(string email)
        {
            EmailBox.SendKeys(email + Keys.Tab);
            //Thread.Sleep(500);
        }

        public string EmailErrorText
        {
            get
            {
                try
                {
                    return EmailError.Text;
                }
                catch (NoSuchElementException)
                {
                    return string.Empty;
                }
            }
        }

        protected override bool EvaluateLoadedStatus()
        {
            return EmailBox.Displayed;
        }

        protected override void ExecuteLoad()
        {
            driver.Navigate().GoToUrl(pageUrl);

            // the cookie prompt always comes up, but it takes a little while; we'll wait for it
            // if we don't wait for it, the tests run fine on Chrome, but Firefox may show strange failures
            // if we're in an env where the cookie prompt isn't displayed, we'll need to make changes
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(cookiePreferencesButtonBy));
        }
    }
}
