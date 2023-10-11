using CeridianWebTest.Browsers;
using CeridianWebTest.Configuration;
using CeridianWebTest.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace CeridianWebTest
{
    public class PowerpayPageTests
    {
        private readonly ThreadLocal<IWebDriver> driver = new();

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
            if (driver.Value != null)
            {
                driver.Value.Quit();
                driver.Value.Dispose();
            }
        }

        private static IEnumerable<object[]> BrowserTypes => EnvironmentConfiguration.BrowserTypes;

        private static IEnumerable<TestCaseData> InvalidEmailData
        {
            get
            {
                string[] badEmails = { "a", "a@", "a@aa", "a@a.a", "@a.com", "a@.com", "a@a.com.", "a..a@a.com", "a@a..com", "a@a a.com", "a@a)a.com" };
                foreach (var browserType in BrowserTypes)
                {
                    foreach (var email in badEmails)
                    {
                        yield return new TestCaseData(browserType[0], email);
                    }
                }
            }
        }

        [TestCaseSource(nameof(InvalidEmailData))]
        [Parallelizable(ParallelScope.All)]
        [Test]
        public void ShouldShowErrorOnInvalidEmail(BrowserType browserType, string email)
        {
            driver.Value = WebDriverFactory.Create(browserType);
            PowerpayPage powerpayPage = new PowerpayPage(driver.Value).Load();
            powerpayPage.SubmitForm(email);
            Assert.That(powerpayPage.EmailErrorText, Is.EqualTo("Please enter a valid email address."));
        }
    }
}