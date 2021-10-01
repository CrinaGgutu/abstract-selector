//ingenta
using Abstract_Selector.scrapers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Abstract_Selector
{
    class Program
    {


        //fulltext_html_url

        static async Task Main()
        {
            var client = new HttpClient();
            //string finalData = @"C:\Users\Crina\Desktop\abstract_data.csv";
            string finalData = @"C:\Users\Crina\Desktop\test.csv";
            string[] doiList = File.ReadAllLines(@"C:\Users\Crina\Desktop\doi.txt");
            //string[] doiList = { "10.1080/09709274.2018.1452866" };
            

            Console.WriteLine($"Total doi count:{doiList.Count()}");

            var counter = 0;
            File.Delete(finalData);
            //.Skip(70).Take(40)

            foreach (string doi in doiList.Skip(1500).Take(182))
            {
                try
                {
                    Console.WriteLine($"Counter: {++counter}, doi:{doi}");
                    if (doi == "DOI not found on crossref" || doi == "-" || doi == "Error File is empty")
                    {
                        File.AppendAllText(finalData, "No DOI: Abstract cannot be found\n");
                    }
                    /*
                    else if (doi == "10.1111/jfr3.12157")
                    {
                        File.AppendAllText(finalData, "Website Exception (PDF) \n\n");
                    }
                    */
                    else
                    {
                        var targetUrlReader = new TargetUrlReader();
                        var targetUrl = await targetUrlReader.GetUrlForDoiAsync(doi);
                        var tandfRefUrl = "https://www.tandfonline.com/doi/ref/" + doi;

                        if (targetUrl.Contains("Not Found"))
                        {
                            File.AppendAllText(finalData, "Not Found\n");
                        }

                        if (!targetUrl.Contains("Not Found")){
                            if (targetUrl.Contains("link.springer.com", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new LinkSpringerComScraper();
                                var docAbstract = scraper.GetAbstract(doi);
                                var docRefs = scraper.GetReferences(doi);
                                Console.WriteLine(docAbstract);
                                Console.WriteLine("---------------");
                                Console.WriteLine(docRefs);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }
                            //doi + ": " +
                            else if (targetUrl.Contains("icevirtuallibrary.com", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new IceVirtualLibrary();
                                await scraper.InitAsync(targetUrl);
                                var docAbstract = scraper.GetAbstract();
                                scraper.GetReferences();
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData,docAbstract + "\n");

                            }

                            else if (targetUrl.Contains("tandfonline.com", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new TandafOnline();
                                await scraper.InitAsync(targetUrl, tandfRefUrl);
                                var docAbstract = scraper.GetAbstract();
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("mdpi.com", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new MDPI();
                                await scraper.InitAsync(targetUrl);
                                var docAbstract = scraper.GetAbstract();
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("jamba.org", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new Jamba();
                                await scraper.InitAsync(targetUrl);
                                var docAbstract = scraper.GetAbstract();
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("sciencedirect.com", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new ScienceDirect();
                                var docAbstract = scraper.GetAbstract(doi);
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("ecologyandsociety", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new EcologyAndSociety();
                                var docAbstract = scraper.GetAbstract(doi);
                                var docRefs = scraper.GetReferences(doi);
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("wiley", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new WileyOnlineLibrary();
                                var docAbstract = scraper.GetAbstract(targetUrl);
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData,  docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("cambridge", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new Cambridge();
                                await scraper.InitAsync(targetUrl);
                                var docAbstract = scraper.GetAbstract();
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("sagepub", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new SagePub();
                                await scraper.InitAsync(targetUrl);
                                var docAbstract = scraper.GetAbstract();
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }

                            else if (targetUrl.Contains("emerald", StringComparison.OrdinalIgnoreCase))
                            {
                                var scraper = new Emerald();
                                await scraper.InitAsync(targetUrl);
                                var docAbstract = scraper.GetAbstract();
                                Console.WriteLine(docAbstract);
                                File.AppendAllText(finalData, docAbstract + "\n");
                            }
                            else
                            {
                                File.AppendAllText(finalData, "no data\n");
                            }
                        }
                       
                    }
                }
                
                catch (Exception)
                {
                    File.AppendAllText(finalData, "Exception\n");

                }
            }
            
         
        }
   }
}

