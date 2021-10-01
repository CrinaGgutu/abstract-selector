//FIX - Error with response Body
using System;
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
    public class IngentaConnect
    {
        private string responseBody;

        /*
        public async Task InitAsync(string targetUrl) 
        {
            var client = new HttpClient();
            // load doi page
            var doiResponse = await client.GetAsync(targetUrl);
            responseBody = await doiResponse.Content.ReadAsStringAsync();
        }
        */ 

        public string GetAbstract(string targetUrl)
        {
            IWebDriver driver = new ChromeDriver(@"C:\Users\Crina\Downloads\chromedriver_win32");

            // This will open up the URL
            driver.Navigate().GoToUrl(targetUrl);
            IWebElement element = driver.FindElement(By.XPath("//div[@class='tab-content']"));
            string text = element.Text;
            driver.Close();
            return text.Replace(",", ";").Replace("\n", "").Replace("\r", "").Replace("Abstract", "").Trim();
        }    
    }
}
