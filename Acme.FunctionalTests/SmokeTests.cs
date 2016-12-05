using System;
using System.Threading;
using OpenQA.Selenium.Remote;
using Xunit;

namespace Acme.FunctionalTests
{
    public class SmokeTests
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static RemoteWebDriver _session;

        [Fact]
        public void Test1()
        {
            // Launch the calculator app
            var appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "de6da024-297c-4556-baa2-c03db12623a5_madhb0sxxv2c2!App");
            _session = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            _session.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            //_session.GetScreenshot().SaveAsFile("c:\\temp\\acme1.png",ImageFormat.Png);            
            _session.FindElementByName("Login").Click();
            Thread.Sleep(1000);
            _session.Dispose();
            _session = null;
        }
    }
}
