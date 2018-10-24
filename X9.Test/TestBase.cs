using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System;
using X9.Test.Pages;

namespace X9.Test
{
    [TestClass]
    public class TestBase : PageBase
    {
        protected ChromeDriver chromeDriver;

        [TestInitialize]
        public void SyncDriver()
        {
            chromeDriver = OpenChrome();
        }

        [TestCleanup]
        public void AfterTests()
        {
            try
            {
                chromeDriver.Close();
                chromeDriver.Dispose();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
