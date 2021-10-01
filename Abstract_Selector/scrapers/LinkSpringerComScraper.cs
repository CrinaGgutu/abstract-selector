//Abstract + Refs using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Abstract_Selector
{
    public class LinkSpringerComScraper
    {
        private string responseBody;

        public async Task InitAsync(string targetUrl) 
        {
            
        }

        public string GetAbstract(string doi)
        {
            try
            {
                IWebDriver driver = new ChromeDriver(@"C:\Users\Crina\Downloads\chromedriver_win32");

                // This will open up the URL
                driver.Navigate().GoToUrl($"http://doi.org/{doi}");
                try
                {
                    IWebElement element = driver.FindElement(By.XPath("//*[@id='Abs1-content']"));
                    string text = element.Text;
                    driver.Close();
                    return text.Replace(",", ";").Replace("\n", "").Replace("\r", "").Replace("Summary", "").Replace("Abstract", "").Trim(); ;
                }
                catch (System.Exception)
                {

                    IWebElement element = driver.FindElement(By.XPath("//section[@class='Abstract']"));
                    string text = element.Text;
                    driver.Close();
                    return text.Replace(",", ";").Replace("\n", "").Replace("\r", "").Replace("Summary", "").Replace("Abstract", "").Trim(); ;
                }

            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
        }

        public string  GetReferences(string doi)
        {
            try
            {
                IWebDriver driver = new ChromeDriver(@"C:\Users\Crina\Downloads\chromedriver_win32");

                // This will open up the URL
                driver.Navigate().GoToUrl($"http://doi.org/{doi}");
                try
                {
                    IWebElement element = driver.FindElement(By.XPath("//*[@class='c-article-references__text']"));
                    string text = element.Text;
                    driver.Close();
                    return text;
                }
                catch (System.Exception)
                {

                    IWebElement element = driver.FindElement(By.XPath("//ol[@class='BibliographyWrapper']"));
                    string text = element.Text;
                    driver.Close();
                    return text.Replace(",", ";").Replace("\n", "").Replace("\r", "").Replace("Abstract", "").Trim();
                }
            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }

            
        }
    }
}
