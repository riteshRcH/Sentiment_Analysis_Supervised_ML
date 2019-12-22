using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace SampleScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            String scrapedText = scrapeSite(new Uri("http://www.hindustantimes.com/News-Feed/RashidIrani/Rashid-Irani-s-review-Intouchables/Article1-888620.aspx"));

            //string extractedContent = stripOffTags(getHTML(new Uri("http://www.reviewgang.com/movies/321-Jism-2-movie-review"))).ToLower().Trim();
            scrapedText = scrapedText.Substring(0, 20).ToLower();
            String score = "";
            double officialScore = 0;
            for (int i = 0; i < scrapedText.Length; i++)
            {
                if((scrapedText[i].Equals('*') || scrapedText[i].Equals('/') || scrapedText[i].Equals('\\') || (scrapedText[i]>=(int)'0' && scrapedText[i]<=(int)'9')))
                    score += scrapedText[i];
            }

            if (score.Contains("1/2") || score.Contains("1\\2"))
            {
                officialScore += 0.5;
                score = score.Replace("1/2", "").Replace("1\\2", "");
            }

            for(int i=0;i<score.Length;i++)
                if(score[i].Equals('*'))
                    officialScore++;

            Console.WriteLine(officialScore);
 
        //WebClient wc = new WebClient();
        //WebRequest req = WebRequest.Create("http://www.koimoi.com/reviews/the-expendables-2-review/");
        //WebResponse resp = req.GetResponse().GetResponseStream();
        //wc.Headers.Add("HTTP_USER_AGENT", "Web-Scraper-Agent (your-custom-user-agent-here)");
            // Download the web page content from the URL
            //extractedContent = wc.DownloadString("http://www.koimoi.com/reviews/the-expendables-2-review/");
        //Console.WriteLine(resp);
        //extractedContent = new WebClient().DownloadString("http://www.reviewgang.com/movies/313-Shirin-Farhad-Ki-Toh-Nikal-Padi-movie-review").ToLower();
            /*if(extractedContent.IndexOf("release date")!=-1)
                extractedContent = extractedContent.Substring(0, extractedContent.IndexOf("release date"));

            extractedContent = extractedContent.Substring(extractedContent.IndexOf("critic rating"), extractedContent.IndexOf("10") - extractedContent.IndexOf("critic rating")).Replace("critic rating", "").Replace("on", "").Replace(":", "").Replace("/", "");
 
            //Remove CSS styles, if any found
            extractedContent = Regex.Replace(extractedContent, "<style(.| )*?>*</style>", "");
            //Remove script blocks
            extractedContent = Regex.Replace(extractedContent, "<script(.| )*?>*</script>", "");
            // Remove all images
            extractedContent = Regex.Replace(extractedContent, "<img(.| )*?/>", "");
            // Remove all HTML tags, leaving on the text inside.
            extractedContent = Regex.Replace(extractedContent, "<(.| )*?>", "");
            // Remove all extra spaces, tabs and repeated line-breaks
            extractedContent = Regex.Replace(extractedContent, "(x09)?", "");
            extractedContent = Regex.Replace(extractedContent, "(x20){2,}", " ");
            extractedContent = Regex.Replace(extractedContent, "(x0Dx0A)+", " ");

            //extractedContent = extractedContent.Substring(extractedContent.IndexOf("rating:"), extractedContent.IndexOf("star cast:") - extractedContent.IndexOf("rating:")).Replace("rating:", "");
            //extractedContent = extractedContent.Substring(0, extractedContent.IndexOf("/5")).Trim();
            Console.WriteLine(Double.Parse(extractedContent));*/
            Console.Read();
        }
        private static string scrapeSite(Uri siteToScrape)
        {
            string s = stripOffTags(getHTML(siteToScrape));
            if (siteToScrape.ToString().Contains("wogma"))
            {
                s = s.Remove(0, s.IndexOf("quick review:")).Replace("quick review:", "");
                s = s.Remove(s.IndexOf("Leave a new comment")).Replace("Leave a new comment", "");
                s = s.Remove(s.IndexOf("Tweet this"), s.IndexOf("Detailed Ratings (out of 5):") - s.IndexOf("Tweet this"));
                s = s.Remove(s.IndexOf("- Movie Details"), s.IndexOf("Comments (") - s.IndexOf("- Movie Details")).Replace("Comments (", "");
                s = s.Remove(s.IndexOf("wogma review"), s.IndexOf("| Trailer") - s.IndexOf("wogma review")).Replace("| Trailer", "");
            }
            else if (siteToScrape.ToString().Contains("bollymoviereviewz"))
            {
                s = s.Remove(s.IndexOf("Post a Comment")).Replace("Post a Comment", "");
                s = s.Remove(s.IndexOf("Email ThisBlogThis!Share to TwitterShare to Facebook"), s.IndexOf(" comments:") - s.IndexOf("Email ThisBlogThis!Share to TwitterShare to Facebook")).Replace("Email ThisBlogThis!Share to TwitterShare to Facebook", "").Replace(" comments:", "");
                s = s.Remove(0, s.IndexOf("From All the reviews on the web")).Replace("From All the reviews on the web", "");
            }
            else if (siteToScrape.ToString().Contains("themoviereviewblog"))
            {
                s = s.Remove(s.IndexOf("Rate this:"), s.IndexOf(" comments on") - s.IndexOf("Rate this:"));
                s = s.Remove(s.IndexOf("Leave a Reply")).Replace("Leave a Reply", "");
                s = s.Remove(0, s.IndexOf("rarr;")).Replace("rarr;", "");
            }
            else if (siteToScrape.ToString().Contains("fridaynirvana"))
            {
                if (s.IndexOf("This entry was posted in") > 0)
                    s = s.Remove(s.IndexOf("This entry was posted in")).Replace("This entry was posted in", "");
                if (s.IndexOf("Kid rating") > 0)
                    s = s.Remove(0, s.IndexOf("Kid rating")).Replace("Kid rating", "");
                if (s.IndexOf("DiggRating") > 0)
                    s = s.Remove(0, s.IndexOf("DiggRating")).Replace("DiggRating", "");
                if (s.IndexOf("Posted on") > 0)
                    s = s.Remove(0, s.IndexOf("Posted on")).Replace("Posted on", "");
                if (s.IndexOf("/5") > 0)
                    s = s.Remove(0, s.IndexOf("/5")).Replace("/5", "");
            }
            else if (siteToScrape.ToString().Contains("http://www.koimoi.com/"))
            {
                if (s.IndexOf("Leave a comment") > 0)
                    s = s.Remove(s.IndexOf("Leave a comment")).Replace("Leave a comment", "");
                if (s.IndexOf("Follow @Koimoi") > 0 && s.IndexOf("Comment via Facebook") > 0)
                    s = s.Remove(s.IndexOf("Follow @Koimoi"), s.IndexOf("Comment via Facebook") - s.IndexOf("Follow @Koimoi")).Replace("Comment via Facebook", ""); ;
                if (s.IndexOf("star)") > 0)
                    s = s.Remove(0, s.IndexOf("star)")).Replace("star)", "");
                if (s.IndexOf("stars)") > 0)
                    s = s.Remove(0, s.IndexOf("stars)")).Replace("stars)", "");
            }
            else if (siteToScrape.ToString().Contains("http://www.reviewgang.com/"))
            {
                if (s.IndexOf("Release Date:") > 0)
                    s = s.Remove(0, s.IndexOf("Release Date:"));
                if (s.IndexOf("&copy;") > 0 && s.IndexOf("account automatically") > 0)
                    s = s.Remove(s.IndexOf("&copy;"), s.IndexOf("account automatically") - s.IndexOf("&copy;")).Replace("account automatically", "");
            }
            else if (siteToScrape.ToString().Contains("http://www.hindustantimes.com"))
            {
                if (s.LastIndexOf("more from this section") > 0)
                    s = s.Remove(s.IndexOf("more from this section")).Replace("more from this section", "");
                if (s.IndexOf("Rating:") > 0)
                    s = s.Remove(0, s.IndexOf("Rating:")).Replace("Rating:", "");
                if (s.IndexOf("Rating") > 0)
                    s = s.Remove(0, s.IndexOf("Rating")).Replace("Rating", "");
            }
            else if (siteToScrape.ToString().Contains("http://www.fridayrelease.com/"))
            {
                s = "lala";
            }
            return s.Replace("&nbsp;", "").Replace("'", "").Trim();
        }
        private static string stripOffTags(string HTMLContent)
        {
            // Remove JS
            HTMLContent = Regex.Replace(HTMLContent, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // Remove inline CSS
            HTMLContent = Regex.Replace(HTMLContent, "<style.*?</style>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // Remove HTML tags
            HTMLContent = Regex.Replace(HTMLContent, "</?[a-z][a-z0-9]*[^<>]*>", "");
            // Remove HTML comments
            HTMLContent = Regex.Replace(HTMLContent, "<!--(.|\\s)*?-->", "");
            // Remove Doctype
            HTMLContent = Regex.Replace(HTMLContent, "<!(.|\\s)*?>", "");
            HTMLContent = Regex.Replace(HTMLContent, "\n+", "\n");
            // Remove excessive whitespace
            //HTMLContent = Regex.Replace(HTMLContent, "[\t\r]", " ");

            return HTMLContent;
        }

        private static string getHTML(Uri siteToScrape)
        {
            try
            {
                string response = "";
                WebResponse resp;
                WebRequest req = System.Net.HttpWebRequest.Create(siteToScrape);
                resp = req.GetResponse();
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    response = sr.ReadToEnd();
                    sr.Close();
                }
                return response;
            }
            catch (Exception e)
            {
                return "Exception occured:" + e.Message + " .... please try again!";
            }
        }
    }
}
