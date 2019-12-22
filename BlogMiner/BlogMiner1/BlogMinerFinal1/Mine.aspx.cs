using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Text.RegularExpressions;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TextBox1.Text = scrapeWebsite("http://wogma.com/movie/bol-bachchan-review/");
    }
    public string scrapeWebsite(string url)
    {
        string extractedContent = "";

        WebClient wc = new WebClient();
        wc.Headers.Add("HTTP_USER_AGENT", "Web-Scraper-Agent (your-custom-user-agent-here)");
        try
        {
            // Download the web page content from the URL
            extractedContent = wc.DownloadString(url);

            //Remove CSS styles, if any found
            extractedContent = Regex.Replace(extractedContent, "<style(.| )*?>*</style>", "");
            //Remove script blocks
            extractedContent = Regex.Replace(extractedContent, "<script(.| )*?>*</script>", "");
            // Remove all images
            extractedContent = Regex.Replace(extractedContent, "<img(.| )*?/>", "");
            // Remove all links
            extractedContent = Regex.Replace(extractedContent, "<a(.| )*?/>", "");
            // Remove all HTML tags, leaving on the text inside.
            extractedContent = Regex.Replace(extractedContent, "<(.| )*?>", "");
            // Remove all extra spaces, tabs and repeated line-breaks
            extractedContent = Regex.Replace(extractedContent, "(x09)?", "");
            extractedContent = Regex.Replace(extractedContent, "(x20){2,}", " ");
            extractedContent = Regex.Replace(extractedContent, "(x0Dx0A)+", " ");
        }
        catch (Exception e)
        {
            extractedContent = "Error on downloading: " + e.Message;
        }
        return extractedContent;
    }
}
