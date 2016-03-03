using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Data;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace TaleoAutomation
{
    class TaleoBot
    {
        private List<Action> currentList = null;
        private Property myProperty = new Property();
        
        private Action previous = null;

        //TextBox mainconsole = null;

        public bool Automate()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities = DesiredCapabilities.Firefox();

            RemoteWebDriver driver = new RemoteWebDriver(new Uri("http://10.0.0.198:4444/wd/hub"), capabilities);

            //RemoteWebDriver driver = new RemoteWebDriver(new Uri("http://10.251.120.164:1111/wd/hub"), capabilities);
            //IWebDriver driver = SeleniumConf.Driver(Browsers.Firefox);
            if (LogIn(driver, "john.lester.f.lucena", "Accenture01"))
            {
				driver.Url = "https://ptraccenture.taleo.net/smartorg/index.jsf";
                driver.Navigate();
                currentList = myProperty.PageList["main"];
                //string excelFile = "C:\\Jenkins\\workspace\\Create_Release_in_Release_Repo\\Configurations.xlsx";
                //string excelFile = "C:\\Jenkins\\workspace\\run\\Configurations.xlsx";
                string excelFile = "//var//jenkins_home//jobs//02_Test//workspace//config.txt";
                //string excelFile = "C:\\Users\\kristen.r.t.limuaco\\Desktop\\TaleoReleaseCopy\\release\\Configurations.xlsx";

                Console.WriteLine("File loaded! (" + excelFile + "Configurations.xlsx).");

                //List<string[]> dt = ExcelReader.Open(excelFile);
                //List<string[]> dt = ExcelReader.Open(excelFile);
                TxtToDataTable _TxtToDataTable = new TxtToDataTable();
                List<string[]> dt = _TxtToDataTable.returnConvertedList(excelFile);
                Configurations configs = DTtoConfigTable(dt);

                Console.WriteLine("--------Running Scripts-----------");
                //int i = 1;
                foreach (Configuration conf in configs)
                {
                    //mainconsole.AppendText("Configuration " + i.ToString() + " . . . .");
                    //Console.WriteLine("Configuration " + i.ToString() + " . . . .");
                    currentList = myProperty.PageList["main"];
                    Console.Write("\nConfiguration: ");
                    foreach (string s in conf)
                    {
                        Console.Write(">" + s.ToString() + "");
                        if (SearchAndDo(driver, currentList, s))
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("   Failed!");
                            break;
                        }
                    }
                    driver.Url = "https://ptraccenture.taleo.net/smartorg/index.jsf";
                    driver.Navigate();
                    //i++;
                }
                Console.WriteLine("Update Success!");
                driver.Close();
				driver.Quit();
            }
            else
            {
                Console.WriteLine("Login Failed, the program will be Terminated!");
                driver.Close();
				driver.Quit();
            }
            return true;
        }
		
		private bool IsElementPresent(IWebDriver driver, By by)
		{
			try
			{
				driver.FindElement(by);
				return true;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}
        
                    
        private bool LogIn(IWebDriver driver, string username, string password)
        {
            driver.Url = "https://ptraccenture.taleo.net/smartorg/iam/accessmanagement/login.jsf?redirectionURI=https%3A%2F%2Fptraccenture.taleo.net%2Fsmartorg%2FTaleoHomePage.jss&TARGET=https%3A%2F%2Fptraccenture.taleo.net%2Fsmartorg%2FTaleoHomePage.jss";
            driver.Navigate();
            try
            {
				if (IsElementPresent(driver, By.Id("dialogTemplate-dialogForm-content-legalAgreement-acceptCmd")))
				driver.FindElement(By.Id("dialogTemplate-dialogForm-content-legalAgreement-acceptCmd")).Click();
                Console.WriteLine("Logging in. . .");
				Thread.Sleep(5000);
                driver.FindElement(By.Id("dialogTemplate-dialogForm-content-login-name1")).Clear();
                driver.FindElement(By.Id("dialogTemplate-dialogForm-content-login-name1")).SendKeys(username);
                driver.FindElement(By.Id("dialogTemplate-dialogForm-content-login-password")).Clear();
                driver.FindElement(By.Id("dialogTemplate-dialogForm-content-login-password")).SendKeys(password);
                driver.FindElement(By.CssSelector("span.nav-btn5")).Click();
                Console.WriteLine("Login Successful!!!");
                Thread.Sleep(5000);
                driver.FindElement(By.Id("menuTemplate-menuForm-globalHeader-pageRibbonSubView-j_id_jsp_1251634767_29pc12-3-ribbonItemCommandLink")).Click();						  
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        } 

        private bool SearchAndDo(IWebDriver driver, List<Action> actions, string key)
        {
            try
            {
                if (previous == null)
                {
                    foreach (Action item in actions)
                        if (key.ToLower() == item.name.ToLower())
                        {
                            if (item.type.Equals("link") || item.type.Equals("checkbox"))
                            {
                                try
                                {
                                    driver.FindElement(By.Id(item.id)).Click();
                                    Thread.Sleep(10000);
                                    if (item.value != "null")
                                        currentList = myProperty.PageList[item.value];
                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;                     
                                }                            
                            }
                            else
                            {
                                previous = item;
                                break;
                            }
                        }
                }
                else
                {
                    if (previous.type.Equals("textbox"))
                    {
                        try
                        {
                            driver.FindElement(By.Id(previous.id)).Clear();
                            driver.FindElement(By.Id(previous.id)).SendKeys(key);
                            Thread.Sleep(10000);
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                        
                    }
                    else if (previous.type.Equals("dropdown") || previous.type.Equals("select"))
                    {
                        try
                        {
                            new SelectElement(driver.FindElement(By.Id(previous.id))).SelectByText(key);
                            Thread.Sleep(10000);
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                       
                    }
                    else if (previous.type.Equals("radio"))
                    {
                        foreach (Action item in actions)
                        {
                            if (previous.name.ToLower() == item.name.ToLower())
                            {
                                if (item.value.ToLower() == key.ToLower())
                                {
                                    try
                                    {
                                        driver.FindElement(By.Id(item.id)).Click();
                                        Thread.Sleep(10000);
                                        break;
                                    }
                                    catch (Exception)
                                    {

                                        return false;
                                    }
                                  
                                }
                            }
                        }
                    }
                    previous = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        private Configurations DTtoConfigTable(List<string[]> dt)
        {
            Configurations configs = new Configurations();
            foreach(string[] row in dt)
            {
                Configuration conf = new Configuration();
                foreach (string s in row)
                    if (s.ToString() == "")
                        break;
                    else
                        conf.Add(s);
                configs.Add(conf);
            }
            return configs;
        }  
    }
}