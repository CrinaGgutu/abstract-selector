using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace Abstract_Selector
{
    public class TargetUrlReader
    {
        public async Task<string> GetUrlForDoiAsync(string doi) 
        {
            var client = new HttpClient();

            var targetUrl = "Not Found";

            if (doi != "-" && doi.ToLower() != "DOI not found on crossref".ToLower() && doi.ToLower() != "Error File is empty".ToLower())
            {
                try
                {
                    var response = await client.GetAsync("https://doi.org/" + doi);



                    if (response.StatusCode == HttpStatusCode.Redirect)
                    {
                        // doi original host address
                        targetUrl = response.Headers.Location.OriginalString;
                        //var abstractResult = await GetAbstractAsync(targetUrl);
                        //Console.WriteLine(abstractResult);
                    }
                    else if (response.StatusCode == HttpStatusCode.OK)
                    {
                        try
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();

                            targetUrl = ReadMetaRefresh1(responseBody);

                            if (string.IsNullOrWhiteSpace(targetUrl))
                            {
                                targetUrl = ReadMetaRefresh2(responseBody);
                            }

                            if (string.IsNullOrWhiteSpace(targetUrl))
                            {
                                targetUrl = ReadMetaRefresh3(responseBody);
                            }

                            if (string.IsNullOrWhiteSpace(targetUrl))
                            {
                                targetUrl = ReadMetaRefresh4(responseBody);
                            }

                            if (string.IsNullOrWhiteSpace(targetUrl))
                            {
                                targetUrl = ReadMetaRefresh5(responseBody);
                            }

                            //if (targetUrl != string.Empty)
                            //{
                            //    var abstractResult = await GetAbstractAsync(targetUrl);
                            //    Console.WriteLine(abstractResult);
                            //}

                            if (targetUrl == string.Empty)
                            {
                                targetUrl = "Not Found: Meta Refresh was not found";
                                Console.WriteLine(targetUrl);

                            }
                        }
                        catch (Exception)
                        {
                            targetUrl = "Not Found: Exception to find Paper address";
                        }
                        //string responseUri = response.RequestMessage.RequestUri.ToString();
                    }

                    //File.AppendAllText(urlFile, targetUrl + "\n");
                    Console.WriteLine(targetUrl);
                }
                catch (Exception ex)
                {
                    //File.AppendAllText(urlFile, "API Error\n");
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                //File.AppendAllText(urlFile, "\n");
                Console.WriteLine(doi);
            }
            return targetUrl;
        }

        string ReadMetaRefresh1(string responseBody)
        {
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody.ToLower());
                var result = htmlDoc.DocumentNode
                    ?.SelectSingleNode("//meta[@http-equiv='refresh']")
                    ?.Attributes["content"]
                    ?.Value
                    .ToString()
                    .Split("redirect=")
                    .ToArray()[1];

                return HttpUtility.UrlDecode(result);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        string ReadMetaRefresh2(string responseBody)
        {
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);
                return htmlDoc.DocumentNode
                    ?.SelectSingleNode("//meta[@property='og:url']")
                    ?.Attributes["content"]
                    ?.Value
                    .ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        string ReadMetaRefresh3(string responseBody)
        {
             try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);
                return htmlDoc.DocumentNode
                    ?.SelectSingleNode("//meta[@name='citation_pdf_url']")
                    ?.Attributes["content"]
                    ?.Value
                    .ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        string ReadMetaRefresh4(string responseBody)
        {
            //<link rel="canonical" href="https://www.emerald.com/insight/content/doi/10.1108/IJCCSM-03-2012-0014/full/html" />
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);
                return htmlDoc.DocumentNode
                    ?.SelectSingleNode("//link[@rel='canonical']")
                    ?.Attributes["href"]
                    ?.Value
                    .ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        string ReadMetaRefresh5(string responseBody)
        {
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);
                return htmlDoc.DocumentNode
                    ?.SelectSingleNode("//meta[@name='citation_fulltext_html_url']")
                    ?.Attributes["content"]
                    ?.Value
                    .ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


    }
}
