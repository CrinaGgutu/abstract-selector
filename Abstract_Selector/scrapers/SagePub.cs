//WORKS -> must clean up refs (remove links)
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Abstract_Selector.scrapers
{
    public class SagePub
    {
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

                var matchValue = "abstractSection abstractInFull";

                //var ClassToGet = "art-abstract";
                //foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*[@class='art-abstract in-tab hypothesis_container']"))
                var doiAbstract = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*"))
                {
                    if (matchValue == node.Attributes["class"]?.Value)
                    {

                        doiAbstract += $"{node.InnerText.Replace("\n", "").Replace("\r", "").Replace(",", ";").Replace("Abstract", "").Trim()}; ";
                    }
                }

                return doiAbstract.Replace("\n", "").Replace("\r", "").Replace(",", ";").Replace("Abstract", "").Trim(); ;

            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
        }

        public string GetReferences()
        {
            
            try
            {
                //Parce page string
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);

                //references

                var matchValue = "references";

                //var ClassToGet = "art-abstract";
                //foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*[@class='art-abstract in-tab hypothesis_container']"))
                var refs = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//table"))
                {
                    if (matchValue == node.Attributes["class"]?.Value)
                    {
                        refs += $"{node.InnerText.Replace("\n", "").Replace("\r", "").Trim()}; ";
                    }
                }

                return refs;

            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
        }
    }
}
