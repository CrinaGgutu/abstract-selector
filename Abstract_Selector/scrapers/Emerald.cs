//Abstract WORKS - cannot find refs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Abstract_Selector.scrapers
{
    public class Emerald
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

                var matchValue = "intent_sub_content Abstract__block__text";

                var doiAbstract = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*"))
                {
                    if (matchValue == node.Attributes["class"]?.Value)
                    {
                        doiAbstract += $"{node.InnerText.Replace("\n", "").Replace("\r", "").Replace(",", ";").Replace("Abstract", "").Trim()}; ";
                        //refs += $"{node.InnerText.Replace("\n", "").Replace("\r", "").Trim()}; " ;
                    }
                }

                return doiAbstract.Replace("\n", "").Replace("\r", "").Replace(",", ";").Replace("Abstract", "").Trim(); ;

            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
        }
    }
}
