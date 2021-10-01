//Abstract works -> no weay to grab refs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Abstract_Selector
{
    public class EcologyAndSociety
    {
        private string responseBody;

        public async Task InitAsync(string targetUrl) 
        {
            //var client = new HttpClient();
            //// load doi page
            //var doiResponse = await client.GetAsync(targetUrl);
            //responseBody = await doiResponse.Content.ReadAsStringAsync();
        }

        public string GetAbstract(string doi)
        {
            try
            {
                //Parce page string
                IWebDriver driver = new ChromeDriver(@"C:\Users\Crina\Downloads\chromedriver_win32");

                // This will open up the URL
                driver.Navigate().GoToUrl($"http://doi.org/{doi}");

                IWebElement element = driver.FindElement(By.XPath("//div[@id='abstract_block']"));
                string text = element.Text;
                text.Replace("\n", "").Replace("\r", "").Trim();
                driver.Close();
                return text.Replace(",", ";").Replace("\n", "").Replace("\r", "").Replace("Abstract", "").Trim();
            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
        }

    }
}
