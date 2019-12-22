using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ReviewSite
{
    public ReviewSite(Uri link2Crawl)
    {
        ReviewSiteURL = link2Crawl;
    }
    public Uri ReviewSiteURL { get; set; }
    public List<String> links2Scrape = new List<string>();
    public List<String> titleOfLinks2Scrape = new List<string>();
    public List<String> innerTextOfLinks2Scrape = new List<string>();
    public List<String> movieNames = new List<string>();
}