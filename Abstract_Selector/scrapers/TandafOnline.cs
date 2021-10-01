//Abstract + refs work
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Abstract_Selector.scrapers
{
    public class TandafOnline
    {
        private string responseBody;
        private string responseBodyRef;

        public async Task InitAsync(string targetUrl, string tandfRefUrl)
        {
            var client = new HttpClient();
            // load doi page
            var doiResponse = await client.GetAsync(targetUrl);
            var refResponse = await client.GetAsync(tandfRefUrl);
            responseBody = await doiResponse.Content.ReadAsStringAsync();
            responseBodyRef = await refResponse.Content.ReadAsStringAsync();
        }

        public string GetAbstract()
        {
            try
            {
                //Parce page string
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);

                var matchValue = "abstractSection abstractInFull";

                var doiAbstract = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div"))
                {
                    if (matchValue == node.Attributes["class"]?.Value)
                    {
                        doiAbstract += node.InnerText;
                    }
                }

                return doiAbstract.Replace("\n", "").Replace("\r", "").Replace(",", ";").Replace("Abstract", "").Trim();

            }
            catch (HttpRequestException e)
            {
                return "\nException Caught!\n" + "Message: " + e.Message;
            }
        }

        public string GetReferences()
        {
            //https://www.tandfonline.com/doi/ref/10.1080/17565529.2018.1442786
            try
            {

                //Parce page string
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBodyRef);

                var matchValue = "references numeric-ordered-list";

                var refs = "";
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//*"))
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


