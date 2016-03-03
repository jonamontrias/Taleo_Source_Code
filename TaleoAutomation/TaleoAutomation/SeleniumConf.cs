using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TaleoAutomation
{
    enum Browsers
    {
        Chrome,
        Firefox,
        IE
    };

    static class SeleniumConf
    {
        public static IWebDriver Driver(Browsers request)
        {
            if (request.Equals(Browsers.Firefox))
                return new FirefoxDriver();
            else
                return null;
        }
    }
}
