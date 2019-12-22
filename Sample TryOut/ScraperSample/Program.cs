using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using HtmlAgilityPack;

namespace ScraperSample
{
    class Program
    {
        static void Main(string[] args)
        {
            scrapeWebsite2("http://wogma.com/movie/cocktail-review/");
        }
        public static void scrapeWebsite(string url)
        {

            WebRequest req = (HttpWebRequest)WebRequest.Create(url);
            WebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream s = resp.GetResponseStream();
            string str = new StreamReader(s).ReadToEnd();
            s.Close();
            //wc.Headers.Add("HTTP_USER_AGENT", "Web-Scraper-Agent (your-custom-user-agent-here)");
            try
            {
                // Download the web page content from the URL

                //Remove CSS styles, if any found
                str = Regex.Replace(str, "<style(.| )*?>*</style>", "");
                //Remove script blocks
                str = Regex.Replace(str, "<script(.| )*?>*</script>", "");
                // Remove all images
                str = Regex.Replace(str, "<img(.| )*?/>", "");
                // Remove all images
                //str = Regex.Replace(str, "<div(.| )*?/>*</div>", "");
                // Remove all HTML tags, leaving on the text inside.
                str = Regex.Replace(str, "<(.| )*?>", "");
                // Remove all extra spaces, tabs and repeated line-breaks
                str = Regex.Replace(str, "(x09)?", "");
                str = Regex.Replace(str, "(x20){2,}", " ");
                str = Regex.Replace(str, "(x0Dx0A)+", " ");
            }
            catch (Exception e)
            {
                str = "Error on downloading: " + e.Message;
            }
            Console.WriteLine(str);
            Console.Read();
        }
        public static void scrapeWebsite2(string url)
        {

            WebRequest req = (HttpWebRequest)WebRequest.Create(url);
            WebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream s = resp.GetResponseStream();
            string str = new StreamReader(s).ReadToEnd();
            s.Close();
            //wc.Headers.Add("HTTP_USER_AGENT", "Web-Scraper-Agent (your-custom-user-agent-here)");
            try
            {
                // Download the web page content from the URL

                //Remove CSS styles, if any found
                str = Regex.Replace(str, "<style(.| )*?>*</style>", "");
                //Remove script blocks
                str = Regex.Replace(str, "<script(.| )*?>*</script>", "");
                // Remove all images
                str = Regex.Replace(str, "<img(.| )*?/>", "");
                // Remove all images
                //str = Regex.Replace(str, "<div(.| )*?/>*</div>", "");
                // Remove all HTML tags, leaving on the text inside.
                // Remove all extra spaces, tabs and repeated line-breaks
                str = Regex.Replace(str, "(x09)?", "");
                str = Regex.Replace(str, "(x20){2,}", " ");
                str = Regex.Replace(str, "(x0Dx0A)+", " ");
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(str);
                HtmlNode n = htmlDoc.DocumentNode;
                List<String> list = new List<String>();
                for (HtmlNode i = n; i.Descendants().Count() > 0; i = i.Descendants()) 
                    list.Add(i.InnerText());

                foreach (string strng in list)
                    Console.Write(strng);
            }
            catch (Exception e)
            {
                str = "Error on downloading: " + e.Message;
            }
            Console.WriteLine(str);
            Console.Read();
        }
    }
}
