//Abstract works
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.ChromeDriver;

namespace Abstract_Selector.scrapers
{
    public class ScienceDirect
    {

        public string GetAbstract(string doi)
        {
            IWebDriver driver = new ChromeDriver(@"C:\Users\Crina\Downloads\chromedriver_win32");

            // This will open up the URL
            driver.Navigate().GoToUrl($"http://doi.org/{doi}");
            IWebElement element = driver.FindElement(By.XPath("//div[@class='abstract author']"));
            string text = element.Text;
            driver.Close();
            return text.Replace(",", ";").Replace("\n", "").Replace("\r", "").Replace("Summary", "").Replace("Abstract", "").Trim();
        }   
    }
}


/*
             public async Task InitAsync(string targetUrl)
        {
            targetUrl = "https://www.sciencedirect.com/science/article/abs/pii/S0143622815001964";
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);
            // load doi page
            var doiResponse = await client.GetAsync(targetUrl);
            responseBody = await doiResponse.Content.ReadAsStringAsync();
        }

        public string GetAbstract()
        {
            try
            {
                //Parce page string
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);

                var matchValue = "abstract author";

                //var ClassToGet = "art-abstract";
                //foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*[@class='art-abstract in-tab hypothesis_container']"))
                var doiAbstract = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div"))
                {
                    if (matchValue == node.Attributes["class"]?.Value)
                    {
                        doiAbstract += node.InnerText;
                    }
                }

                return doiAbstract;

            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
        }
        
        public string GetReferences()
        {
            IWebDriver driver = new ChromeDriver(@"C:\Users\Crina\Downloads\chromedriver_win32");

            // This will open up the URL
            driver.Url = "https://www.geeksforgeeks.org/";
            return driver.Url;
        }
            */