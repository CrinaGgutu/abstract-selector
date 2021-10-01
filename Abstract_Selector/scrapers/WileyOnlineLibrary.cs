//Abstract works!
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
    public class WileyOnlineLibrary
    {
        /*
        private string responseBody;

        public async Task InitAsync(string targetUrl) 
        {
            var client = new HttpClient();
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

                var matchValue = "article-section__content en main";

                //var ClassToGet = "art-abstract";
                //foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*[@class='art-abstract in-tab hypothesis_container']"))
                var doiAbstract = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*"))
                {
                    if (matchValue == node.Attributes["class"]?.Value)
                    {
                        Console.WriteLine("Working");
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
        */

        public string GetAbstract(string targetUrl)
        {
            IWebDriver driver = new ChromeDriver(@"C:\Users\Crina\Downloads\chromedriver_win32");

            // This will open up the URL
            driver.Navigate().GoToUrl(targetUrl);
            IWebElement element = driver.FindElement(By.XPath("//div[@class='article-section__content en main']"));
            string text = element.Text;
            driver.Close();
            return text.Replace(",", ";").Replace("\n", "").Replace("\r", "").Replace("Abstract", "").Trim();
        }

        //accordion__control clicked js--open
        public string  GetReferences(string targetUrl)
        {
            /*
            try
            {
                //Parce page string
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);

                var matchValue = "c-article-references__text";

                //var ClassToGet = "art-abstract";
                //foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*[@class='art-abstract in-tab hypothesis_container']"))
                var refs = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*"))
                {
                    if (matchValue == node.Attributes["class"]?.Value)
                    {
                        refs += $"{node.InnerText.Replace("\n", "").Replace("\r", "").Trim()}; " ;
                    }
                }

                return refs;

            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
            */
            return "fix";
        }
    }
}
