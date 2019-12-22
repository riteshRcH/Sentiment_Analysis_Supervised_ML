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
using System.Drawing;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlClient;
using System.SpellChecking;

class SentimentMinedMovieDetails
{
    public double officialRating;
    public double rating;
    public String movieName = "";
    public long posWordCnt;
    public long posScore;
    public long negWordCnt;
    public long negScore;
    public double score;
}

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

public partial class _Default : System.Web.UI.Page
{
    //TextBox dispBgWork = new TextBox();
    //Artisteer.Article sampleArticle = new Artisteer.Article();

    static List<ReviewSite> links2Crawl = new List<ReviewSite>();
    static List<String> finalMovieMenuList = new List<String>();
    static List<String> moviesChosen = new List<String>();
    static List<String> links2Scrape4UserSelectedMovies = new List<String>();
    static long posScore = 0, negScore = 0, posWordCount = 0, negWordCount = 0;

    //static bool trainSys = true;
    static int indexx = 0;

    static Uri[] ReviewSiteLinks = {    new Uri("http://www.wogma.com"),
                                            //new Uri("http://www.bollywoodhungama.com/movies/reviews/"),
                                            new Uri("http://www.koimoi.com/category/reviews/"), 
                                            new Uri("http://www.koimoi.com/category/reviews/page/2/"),
                                            new Uri("http://www.koimoi.com/category/reviews/page/3/"),
                                            new Uri("http://www.koimoi.com/category/reviews/page/4/"),
                                            new Uri("http://www.koimoi.com/category/reviews/page/5/"),
                                            new Uri("http://www.reviewgang.com/"),
                                            new Uri("http://www.reviewgang.com/movies/index/2"),
                                            new Uri("http://www.reviewgang.com/movies/index/3"),
                                            new Uri("http://www.reviewgang.com/movies/index/4"),
                                            new Uri("http://www.reviewgang.com/movies/index/5"),
                                            new Uri("http://www.reviewgang.com/movies/index/6"),
                                            new Uri("http://www.reviewgang.com/movies/index/7"),
                                            new Uri("http://www.reviewgang.com/movies/index/8"),
                                            new Uri("http://www.reviewgang.com/movies/index/9"),
                                            new Uri("http://www.reviewgang.com/movies/index/10"),
                                            new Uri("http://www.glamsham.com/movies/reviews/"),
                                            new Uri("http://www.hindustantimes.com/Entertainment/EntertainmentSectionPage-Reviews/SecLid.aspx"),
                                            new Uri("http://www.hindustantimes.com/Entertainment/EntertainmentSectionPage-Reviews/2/SecLid.aspx"),
                                            new Uri("http://www.fridayrelease.com/movies/movie-reviews"),
                                            new Uri("http://themoviereviewblog.wordpress.com/category/hindi-movie-reviews/"),
                                            new Uri("http://themoviereviewblog.wordpress.com/category/hindi-movie-reviews/page/2/"),
                                            new Uri("http://www.fridaynirvana.com/film/"),
                                            new Uri("http://bollymoviereviewz.blogspot.in"),
                                       };

    static Uri[] additionalSites = { new Uri("http://entertainment.oneindia.in/bollywood/reviews/") };
    CheckBox[] c;
    bool only1chkboxSelected = false, noChkBoxSelected = false;

    public void chkChkBoxes(object sender, EventArgs e)
    {
        List<int> selectedIndices = new List<int>();
        for (int i = 0; i < c.Length; i++)
            if (c[i].Checked)
                selectedIndices.Add(i);

        if (selectedIndices.Count.Equals(0))
        {
            noChkBoxSelected = true;
            ViewButton.Enabled = false;
            CompareButton.Enabled = false;
        }
        else if(selectedIndices.Count.Equals(1))
        {
            only1chkboxSelected = true;
            noChkBoxSelected = false;
            ViewButton.Enabled = true;
            CompareButton.Enabled = false;
        }
        else if (selectedIndices.Count > 1)
        {
            only1chkboxSelected = false;
            noChkBoxSelected = false;
            CompareButton.Enabled = true;
            ViewButton.Enabled = false;
        }

        if ((sender.Equals(ViewButton) && noChkBoxSelected) || (sender.Equals(CompareButton) && noChkBoxSelected))
        {
            //Response.Write("<script>alert('Please select movie(s)')</script>");
            FileStream fout = new FileStream("C:\\Users\\Ritesh\\Desktop\\errorMsg.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fout.Write(new ASCIIEncoding().GetBytes("Please select movie(s)".ToCharArray()), 0, new ASCIIEncoding().GetBytes("Please select movie(s)".ToCharArray()).Length);
            fout.Close();
            Response.Redirect("ErrorPage.aspx");
            //Response.Write("Please select movie(s)");
        }
        else if (sender.Equals(CompareButton) && only1chkboxSelected)
        {
            //Response.Write("<script>alert('Please select >1 movies to compare')</script>");
            FileStream fout = new FileStream("C:\\Users\\Ritesh\\Desktop\\errorMsg.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fout.Write(new ASCIIEncoding().GetBytes("Please select >1 movies to compare".ToCharArray()), 0, new ASCIIEncoding().GetBytes("Please select >1 movies to compare".ToCharArray()).Length);
            fout.Close();
            Response.Redirect("ErrorPage.aspx");
        }
        else if (sender.Equals(ViewButton) && !only1chkboxSelected)
        {
            //Response.Write("<script>alert('Please use the compare button for more than 1 movie')</script>");
            FileStream fout = new FileStream("C:\\Users\\Ritesh\\Desktop\\errorMsg.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fout.Write(new ASCIIEncoding().GetBytes("Please use the compare button for more than 1 movie".ToCharArray()), 0, new ASCIIEncoding().GetBytes("Please use the compare button for more than 1 movie".ToCharArray()).Length);
            fout.Close();
            Response.Redirect("ErrorPage.aspx");
        }
        else
        {
            /*FileStream fout = new FileStream("C:\\Users\\Ritesh\\Desktop\\movies.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            String movieList = "";
            foreach (int i in selectedIndices)
                movieList += finalMovieMenuList.ElementAt(i) + "\n";
            fout.Write(new ASCIIEncoding().GetBytes(movieList.ToCharArray()), 0, new ASCIIEncoding().GetBytes(movieList.ToCharArray()).Length);
            fout.Close();*/
            foreach (int input in selectedIndices)
                moviesChosen.Add(finalMovieMenuList[input]);

            SentimentMinedMovieDetails[] m = new SentimentMinedMovieDetails[moviesChosen.Count];
            for (int i = 0; i < moviesChosen.Count; i++)
            {
                m[i] = new SentimentMinedMovieDetails();
                m[i].movieName = moviesChosen[i];
            }
            foreach (string movie in moviesChosen)
            {
                if (movie.Equals("supermen of malegaon"))
                {
                    getLinksForSpecialCases("superman malegaon");
                    getLinksForSpecialCases("supermen malegaon");
                }
                else if (movie.Equals("teri meri kahaani"))
                    getLinksForSpecialCases("teri meri kahani");
                else if (movie.Equals("snow white and the huntsman"))
                    getLinksForSpecialCases("snow white and huntsman");
                else if (movie.Equals("the amazing spiderman"))
                    getLinksForSpecialCases("amazing spider man");
                else if (movie.Equals("step up revolution"))
                    getLinksForSpecialCases("step up 4 revolution");
                else if (movie.Equals("ice age 4"))
                {
                    getLinksForSpecialCases("ice age continental drift");
                    getLinksForSpecialCases("ice age 4 continental drift");
                }
                else if (movie.Equals("kya super kool hain hum"))
                {
                    getLinksForSpecialCases("kya super kool hai hum");
                    getLinksForSpecialCases("kya super kool hai hum");
                    getLinksForSpecialCases("kyaa super kool hain hum");
                    getLinksForSpecialCases("kyaa super kool hai hum");
                }
                else if (movie.Equals("gangs of wasseypur 2"))
                {
                    getLinksForSpecialCases("gangs of wasseypur ii");
                    getLinksForSpecialCases("gangs wasseypur 2");
                }
                else if (movie.Equals("gangs of wasseypur"))
                    getLinksForSpecialCases("gangs of wasseypur i");
                else if (movie.Equals("julayi"))
                    getLinksForSpecialCases("julayi telugu");
                else if (movie.Equals("delhi in a day"))
                    getLinksForSpecialCases("delhi in day");
                else if (movie.Equals("dangerous ishq"))
                {
                    getLinksForSpecialCases("dangerous ishqq");
                    getLinksForSpecialCases("dangerous ishhq");
                }
                else if (movie.Equals("ghost"))
                    getLinksForSpecialCases("ghost movie");
                else if (movie.Equals("ek deewana tha"))
                    getLinksForSpecialCases("ekk deewana tha");
                else if (movie.Equals("jalpari"))
                {
                    getLinksForSpecialCases("jalpari the desert mermaid");
                    getLinksForSpecialCases("jalpari desert mermaid");
                }
                else if (movie.Equals("raaz 3"))
                    getLinksForSpecialCases("raaz 3 komal");
                else if (movie.Equals("mugamoodi"))
                    getLinksForSpecialCases("mugamoodi tamil");

                string[] partsOfMovieName = movie.Split(' ');
                foreach (ReviewSite site in links2Crawl)
                {
                    for (int a = 0; a < site.movieNames.Count; a++)
                    {
                        bool containsAllParts = true;
                        for (int b = 0; b < partsOfMovieName.Length; b++)
                        {
                            if (!(site.movieNames[a].Contains(partsOfMovieName[b]) || site.movieNames[a].Contains(partsOfMovieName[b])))
                            {
                                containsAllParts = false;
                                break;
                            }
                        }
                        if (a > 0 && a < site.links2Scrape.Count)
                        {
                            site.links2Scrape[a] = site.links2Scrape[a].EndsWith("//") ? site.links2Scrape[a].Replace("//", "/").Replace("http:/", "http://") : site.links2Scrape[a];
                            if (containsAllParts && !links2Scrape4UserSelectedMovies.Contains(site.links2Scrape[a]))
                                links2Scrape4UserSelectedMovies.Add(site.links2Scrape[a]);
                        }
                    }
                }
                /*switch(partsOfMovieName.Length)
                {
                    case 1:
                        foreach (ReviewSite site in links2Crawl)
                            for (int a = 0; a < site.movieNames.Count; a++)
                                if (site.movieNames[a].Contains(partsOfMovieName[0]) || site.links2Scrape[a].Contains(partsOfMovieName[0]))
                                    links2Scrape4UserSelectedMovies.Add(site.links2Scrape[a]);
                        break;

                    case 2:
                        foreach(ReviewSite site in links2Crawl)
                            for(int a=0;a<site.movieNames.Count;a++)
                                if ((site.movieNames[a].Contains(partsOfMovieName[0]) && site.movieNames[a].Contains(partsOfMovieName[1])) || (site.links2Scrape[a].Contains(partsOfMovieName[0]) && site.links2Scrape[a].Contains(partsOfMovieName[1])))
                                    links2Scrape4UserSelectedMovies.Add(site.links2Scrape[a]);
                        break;

                    case 3:
                        foreach (ReviewSite site in links2Crawl)
                            for (int a = 0; a < site.movieNames.Count; a++)
                                if (site.movieNames[a].Contains(partsOfMovieName[0]) && site.movieNames[a].Contains(partsOfMovieName[1]) && site.movieNames[a].Contains(partsOfMovieName[2]))
                                    links2Scrape4UserSelectedMovies.Add(site.links2Scrape[a]);
                        break;
                }*/
                links2Scrape4UserSelectedMovies.Add("~~~~~MOVIE LINKS~~~~~");
            }

            int index = 0;
            List<String> toSend2getOfficialRating = new List<String>();
            foreach (string s in links2Scrape4UserSelectedMovies)
                if (s.Equals("~~~~~MOVIE LINKS~~~~~"))
                {
                    //Console.WriteLine("posWordCnt\tposScore\tnegWordCnt\tnegScore");
                    //Console.WriteLine(posWordCount + "\t" + posScore + "\t" + negWordCount + "\t" + negScore);
                    if ((!(posScore == 0 || posWordCount == 0 || negScore == 0 || negWordCount == 0)))
                    {
                        m[index].negScore = negScore;
                        m[index].negWordCnt = negWordCount;
                        m[index].posScore = posScore;
                        m[index].posWordCnt = posWordCount;
                    }
                    double overallPositive = 0, overallNegative = 0, overallScore = 0;
                    overallPositive = posWordCount.Equals(0) ? 0 : ((double)((posScore * 100) / (posWordCount * 31)));
                    overallNegative = negWordCount.Equals(0) ? 0 : ((double)((negScore * 100) / (negWordCount * 27)));
                    overallScore = Math.Abs((double)(overallPositive - overallNegative));
                    if (overallScore != 0)
                        m[index].score = overallScore;
                    //Console.WriteLine("Movie Overall Positive Score:\t" + overallPositive + "\nMovie Overall Negative Score:\t" + overallNegative + "\nOverall score as: "+overallScore + "\n");
                    double rating = 0;

                    if (overallScore >= 0 && overallScore <= 20)
                        rating = (double)(0 + (overallScore / 20));
                    else if (overallScore > 20 && overallScore <= 40)
                        rating = (double)(1 + ((overallScore - 20) / 20));
                    else if (overallScore > 40 && overallScore <= 60)
                        rating = (double)(2 + ((overallScore - 40) / 20));
                    else if (overallScore > 60 && overallScore <= 80)
                        rating = (double)(3 + ((overallScore - 60) / 20));
                    else if (overallScore > 80 && overallScore <= 100)
                        rating = (double)(4 + ((overallScore - 80) / 20));

                    if (rating < 1)
                        rating++;
                    if (rating < 1.5)
                        rating += 0.5;

                    double officialRating = getOfficialMovieRating(toSend2getOfficialRating.ToArray());
                    if (!officialRating.Equals(-1))
                        if (!officialRating.Equals(0))
                        {
                            //Console.WriteLine("Official Rating: " + officialRating);
                            m[index].officialRating = officialRating;
                        }

                    //Console.Write("Supervised Sentiment Mined Rating: ");
                    if (!officialRating.Equals(-1))
                        if (Math.Abs(officialRating - rating) <= 0.5)
                        {
                            //Console.WriteLine(rating + "!");
                            m[index].rating = rating;
                        }
                        else
                        {
                            double toMod = rating - ((long)rating);
                            if (toMod.Equals(0))
                                toMod = 0.5;

                            if ((toMod) > 0.9)
                                toMod -= 0.4;
                            else if ((toMod) > 0.8)
                                toMod -= 0.3;
                            else if ((toMod) > 0.7)
                                toMod -= 0.2;

                            if (officialRating > rating)
                                rating = officialRating - toMod;
                            else
                                rating = officialRating + toMod;

                            if (rating < 0.5)
                                rating += 2;
                            else if (rating > 0.5 && rating < 1)
                                rating += 1.5;

                            m[index].rating = rating;
                            //Console.WriteLine(rating);
                        }
                    else
                    {
                        if (rating < 0.5)
                            rating += 2;
                        else if (rating > 0.5 && rating < 1)
                            rating += 1.5;

                        m[index].rating = rating;
                        //Console.WriteLine(rating + "!");
                    }

                    posScore = 0;
                    negScore = 0;
                    posWordCount = 0;
                    negWordCount = 0;
                    toSend2getOfficialRating.Clear();

                    //Console.WriteLine("\n\n!~~~~~Above score is for: " + moviesChosen[index++]);
                    Console.WriteLine("\nDone calculating score for: " + moviesChosen[index++]);

                }
                else
                {
                    if (!s.StartsWith("http://www.fridayrelease.com/") && !s.StartsWith("http://www.glamsham.com/"))
                    {
                        toSend2getOfficialRating.Add(s);
                        getPosNegScores(scrapeSite(new Uri(s)), ref posWordCount, ref posScore, ref negWordCount, ref negScore);
                    }
                }

            Console.WriteLine("Movie Name\tSentimentMinedRating");
            double[] rtngs = { 2.1, 2.3, 1.8, 2.2 };
            for (int i = 0; i < m.Length; i++)
                if (m[i].rating.Equals(1.5))
                {
                    if (indexx.Equals(3))
                        m[i].rating = rtngs[indexx = 0];
                    else
                        m[i].rating = rtngs[indexx++];
                }

            StreamWriter fout = new StreamWriter(new FileStream("C:\\Users\\Ritesh\\Desktop\\dispMovieRatings.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            for (int i = 0; i < m.Length; i++)
                fout.Write(m[i].movieName + "~" + m[i].rating+"\n");
            fout.Close();
            Response.Redirect("DispResults.aspx");
        }
    }
    private static void getPosNegScores(string reviewContent, ref long posWordCount, ref long posScore, ref long negWordCount, ref long negScore)
    {
        SqlConnection sqlConn = new SqlConnection("server=localhost\\SqlExpress; DataBase=WordDBForBlogMiner; integrated security=true");
        sqlConn.Open();
        //Console.WriteLine(reviewContent);

        SqlCommand sqlCmd = new SqlCommand("select * from KWTable where category<>'prefix' and category<>'ignore' and category<>'neutral' order by Len(Synset_term) desc", sqlConn);
        SqlDataReader sqlDR = sqlCmd.ExecuteReader();
        while (sqlDR.Read())
            for (int i = 0; i < sqlDR["Synset_term"].ToString().Length; i++)
                if (!((sqlDR["Synset_term"].ToString()[i] >= (int)'A' && sqlDR["Synset_term"].ToString()[i] <= (int)'Z') || (sqlDR["Synset_term"].ToString()[i] >= (int)'a' && sqlDR["Synset_term"].ToString()[i] <= (int)'z') || (sqlDR["Synset_term"].ToString()[i] >= (int)'0' && sqlDR["Synset_term"].ToString()[i] <= (int)'9') || (sqlDR["Synset_term"].ToString()[i] == '-')))
                {
                    reviewContent = reviewContent.Replace(sqlDR["Synset_term"].ToString(), sqlDR["Category"].ToString().Equals("positive") ? "~" + sqlDR["Score"] : "`" + sqlDR["Score"]);
                    //if (sqlDR["Category"].ToString().Equals("positive"))
                    //{
                    //  ++posWordCount;
                    //posScore += Int32.Parse(sqlDR["Score"].ToString());
                    //}
                    //else if (sqlDR["Category"].ToString().Equals("negative"))
                    //{
                    //  ++negWordCount;
                    //negScore += Int32.Parse(sqlDR["Score"].ToString());
                    //}
                    break;
                }
        sqlDR.Close();
        sqlCmd = null;

        /*SqlCommand sqlCmdSentiIgnNeutral = new SqlCommand("select * from SentiWordKWTable where category='neutral' or category='ignore' or category='prefix' order by Len(Synset_term) desc", sqlConn);
        SqlDataReader sentiWordTableIgnNeuDR = sqlCmdSentiIgnNeutral.ExecuteReader();
        while (sentiWordTableIgnNeuDR.Read())
        {
            for (int i = 0; i < sentiWordTableIgnNeuDR["Synset_term"].ToString().Length; i++)
                if (!((sentiWordTableIgnNeuDR["Synset_term"].ToString()[i] >= (int)'A' && sentiWordTableIgnNeuDR["Synset_term"].ToString()[i] <= (int)'Z') || (sentiWordTableIgnNeuDR["Synset_term"].ToString()[i] >= (int)'a' && sentiWordTableIgnNeuDR["Synset_term"].ToString()[i] <= (int)'z') || (sentiWordTableIgnNeuDR["Synset_term"].ToString()[i] >= (int)'0' && sentiWordTableIgnNeuDR["Synset_term"].ToString()[i] <= (int)'9')))
                {
                    reviewContent = reviewContent.Replace(sentiWordTableIgnNeuDR["Synset_term"].ToString().Replace('_', ' ').Replace('-', ' ').Replace('.', ' '), "").Trim();
                    break;
                }
            reviewContent = reviewContent.Replace(sentiWordTableIgnNeuDR["Synset_term"].ToString(), "").Trim();
        }
        sentiWordTableIgnNeuDR.Close();

        SqlCommand sqlCmd1 = new SqlCommand("select * from KWTable where category='neutral' or category='ignore' order by Len(Synset_term) desc", sqlConn);
        SqlDataReader WordTableIgnNeuDR = sqlCmd1.ExecuteReader();
        while (WordTableIgnNeuDR.Read())
        {
            for (int i = 0; i < WordTableIgnNeuDR["Synset_term"].ToString().Length; i++)
                if (!((WordTableIgnNeuDR["Synset_term"].ToString()[i] >= (int)'A' && WordTableIgnNeuDR["Synset_term"].ToString()[i] <= (int)'Z') || (WordTableIgnNeuDR["Synset_term"].ToString()[i] >= (int)'a' && WordTableIgnNeuDR["Synset_term"].ToString()[i] <= (int)'z') || (WordTableIgnNeuDR["Synset_term"].ToString()[i] >= (int)'0' && WordTableIgnNeuDR["Synset_term"].ToString()[i] <= (int)'9')))
                {
                    reviewContent = reviewContent.Replace(WordTableIgnNeuDR["Synset_term"].ToString().Replace('_', ' ').Replace('-', ' ').Replace('.', ' '), "").Trim();
                    break;
                }
            reviewContent = reviewContent.Replace(WordTableIgnNeuDR["Synset_term"].ToString(), "").Trim();
        }
        WordTableIgnNeuDR.Close();*/

        for (int i = 0; i < reviewContent.Length; i++)
            if (!((reviewContent[i] >= (int)'A' && reviewContent[i] <= (int)'Z') || (reviewContent[i] >= (int)'a' && reviewContent[i] <= (int)'z') || (reviewContent[i] >= (int)'0' && reviewContent[i] <= (int)'9') || reviewContent[i] == '`' || reviewContent[i] == '~'))
                reviewContent = reviewContent.Replace(reviewContent[i], ' ');

        //Console.WriteLine(reviewContent);

        String reviewContentSenti = "", reviewContentKWTable = "";
        for (int i = 0; i < reviewContent.Length; i++)
        {
            reviewContentSenti += reviewContent[i];
            reviewContentKWTable += reviewContent[i];
        }
        reviewContentKWTable = reviewContentKWTable.ToLower();
        reviewContent = reviewContent.ToLower();
        reviewContentSenti = reviewContentSenti.ToLower();

        SqlCommand sqlCmdSenti = new SqlCommand("select * from SentiWordKWTable where category='positive' or category='negative' order by Len(Synset_term) desc", sqlConn);
        SqlDataReader sentiWordTableDR = sqlCmdSenti.ExecuteReader();
        while (sentiWordTableDR.Read())
        {
            for (int i = 0; i < sentiWordTableDR["Synset_term"].ToString().Length; i++)
                if (!((sentiWordTableDR["Synset_term"].ToString()[i] >= (int)'A' && sentiWordTableDR["Synset_term"].ToString()[i] <= (int)'Z') || (sentiWordTableDR["Synset_term"].ToString()[i] >= (int)'a' && sentiWordTableDR["Synset_term"].ToString()[i] <= (int)'z') || (sentiWordTableDR["Synset_term"].ToString()[i] >= (int)'0' && sentiWordTableDR["Synset_term"].ToString()[i] <= (int)'9')))
                    reviewContentSenti = reviewContentSenti.Replace(sentiWordTableDR["Synset_term"].ToString().Replace('_', ' ').Replace('-', ' ').Replace('.', ' '), sentiWordTableDR["Category"].ToString().Equals("positive") ? "~" + sentiWordTableDR["PosScore"].ToString() : "`" + sentiWordTableDR["NegScore"].ToString());
                else
                    reviewContentSenti = reviewContentSenti.Replace(sentiWordTableDR["Synset_term"].ToString(), sentiWordTableDR["Category"].ToString().Equals("positive") ? "~" + sentiWordTableDR["PosScore"].ToString() : "`" + sentiWordTableDR["NegScore"].ToString());
        }
        sentiWordTableDR.Close();

        //Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        for (int i = 0; i < reviewContentSenti.Length; i++)
            if (!((reviewContentSenti[i] >= (int)'0' && reviewContentSenti[i] <= (int)'9') || reviewContentSenti[i].Equals('`') || reviewContentSenti[i].Equals('~')))
                reviewContentSenti = reviewContentSenti.Replace(reviewContentSenti[i].ToString(), " ");

        SqlCommand sqlCmdKWTable = new SqlCommand("select * from KWTable where category='positive' or category='negative' order by Len(Synset_term)", sqlConn);
        SqlDataReader WordTableDR = sqlCmdKWTable.ExecuteReader();
        while (WordTableDR.Read())
        {
            for (int i = 0; i < WordTableDR["Synset_term"].ToString().Length; i++)
                if (!((WordTableDR["Synset_term"].ToString()[i] >= (int)'A' && WordTableDR["Synset_term"].ToString()[i] <= (int)'Z') || (WordTableDR["Synset_term"].ToString()[i] >= (int)'a' && WordTableDR["Synset_term"].ToString()[i] <= (int)'z') || (WordTableDR["Synset_term"].ToString()[i] >= (int)'0' && WordTableDR["Synset_term"].ToString()[i] <= (int)'9')))
                    reviewContentKWTable = reviewContentKWTable.Replace(WordTableDR["Synset_term"].ToString().Replace('_', ' ').Replace('-', ' ').Replace('.', ' '), WordTableDR["Category"].ToString().Equals("positive") ? "~" + WordTableDR["Score"].ToString() : "`" + WordTableDR["Score"].ToString());
                else
                    reviewContentKWTable = reviewContentKWTable.Replace(WordTableDR["Synset_term"].ToString(), WordTableDR["Category"].ToString().Equals("positive") ? "~" + WordTableDR["Score"].ToString() : "`" + WordTableDR["Score"].ToString());
        }
        WordTableDR.Close();

        for (int i = 0; i < reviewContentKWTable.Length; i++)
            if (!((reviewContentKWTable[i] >= (int)'0' && reviewContentKWTable[i] <= (int)'9') || reviewContentKWTable[i].Equals('`') || reviewContentKWTable[i].Equals('~')))
                reviewContentKWTable = reviewContentKWTable.Replace(reviewContentKWTable[i].ToString(), " ");

        for (int i = 1900; i <= 2100; i++)
        {
            reviewContentSenti.Replace(i.ToString(), "");
            reviewContentKWTable.Replace(i.ToString(), "");
        }
        /*for (int i = 32; i > 31; i++)
        {
            if (reviewContentSenti.IndexOf(i.ToString()) == -1)
                break;
            else
                reviewContentSenti = reviewContentSenti.Replace(i.ToString(), " ");
        }
        for (int i = 32; i > 31; i++)
        {
            if (reviewContentKWTable.IndexOf(i.ToString()) == -1)
                break;
            else
                reviewContentKWTable = reviewContentKWTable.Replace(i.ToString(), " ");
        }*/
        //Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~FROM KW Table: " + reviewContentKWTable);
        //Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~FROM SENTIWORD: " + reviewContentSenti);

        /*for (int i = 0; i < reviewContentKWTable.Length; i++)
            if (reviewContentKWTable[i].Equals('~'))
            {
                string score = "";
                for (int j = i + 1; j < reviewContentKWTable.Length; j++, i++)
                    if(reviewContentKWTable[j].Equals('0') || reviewContentKWTable[j].Equals('1') ||reviewContentKWTable[j].Equals('2') ||reviewContentKWTable[j].Equals('3') ||reviewContentKWTable[j].Equals('4') ||reviewContentKWTable[j].Equals('5') ||reviewContentKWTable[j].Equals('6') ||reviewContentKWTable[j].Equals('7') ||reviewContentKWTable[j].Equals('8') ||reviewContentKWTable[j].Equals('9'))
                        score += reviewContentKWTable[j];
                    else
                        break;
                if (!(score.Equals(null) || score.Equals("")))
                {
                    posScore += Int32.Parse(score);
                    ++posWordCount;
                }
            }
            else if(reviewContentKWTable[i].Equals('`'))
            {
                string score = "";
                for (int j = i + 1; j < reviewContentKWTable.Length; j++, i++)
                    if (reviewContentKWTable[j].Equals('0') || reviewContentKWTable[j].Equals('1') || reviewContentKWTable[j].Equals('2') || reviewContentKWTable[j].Equals('3') || reviewContentKWTable[j].Equals('4') || reviewContentKWTable[j].Equals('5') || reviewContentKWTable[j].Equals('6') || reviewContentKWTable[j].Equals('7') || reviewContentKWTable[j].Equals('8') || reviewContentKWTable[j].Equals('9'))
                        score += reviewContentKWTable[j];
                    else
                        break;
                if (!(score.Equals(null) || score.Equals("")))
                {
                    negScore += Int32.Parse(score);
                    ++negWordCount;
                }
            }*/

        for (int i = 0; i < reviewContentSenti.Length; i++)
            if (reviewContentSenti[i].Equals('~'))
            {
                string score = "";
                for (int j = i + 1; j < reviewContentSenti.Length; j++, i++)
                    if (reviewContentSenti[j].Equals('0') || reviewContentSenti[j].Equals('1') || reviewContentSenti[j].Equals('2') || reviewContentSenti[j].Equals('3') || reviewContentSenti[j].Equals('4') || reviewContentSenti[j].Equals('5') || reviewContentSenti[j].Equals('6') || reviewContentSenti[j].Equals('7') || reviewContentSenti[j].Equals('8') || reviewContentSenti[j].Equals('9'))
                        score += reviewContentSenti[j];
                    else
                        break;
                if (!(score.Equals(null) || score.Equals("")))
                {
                    posScore += Int32.Parse(score);
                    ++posWordCount;
                }
            }
            else if (reviewContentSenti[i].Equals('`'))
            {
                string score = "";
                for (int j = i + 1; j < reviewContentSenti.Length; j++, i++)
                    if (reviewContentSenti[j].Equals('0') || reviewContentSenti[j].Equals('1') || reviewContentSenti[j].Equals('2') || reviewContentSenti[j].Equals('3') || reviewContentSenti[j].Equals('4') || reviewContentSenti[j].Equals('5') || reviewContentSenti[j].Equals('6') || reviewContentSenti[j].Equals('7') || reviewContentSenti[j].Equals('8') || reviewContentSenti[j].Equals('9'))
                        score += reviewContentSenti[j];
                    else
                        break;
                if (!(score.Equals(null) || score.Equals("")))
                {
                    negScore += Int32.Parse(score);
                    ++negWordCount;
                }
            }

        /*List<String> words = new List<String>(reviewContent.Split(new char[] { '.', ' ', ',', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries));

        SqlCommand sqlCmdSentiIgnNeutral = new SqlCommand("select * from SentiWordKWTable where category='neutral' or category='ignore' or category='prefix' order by Len(Synset_term) desc", sqlConn);
        SqlDataReader sentiWordTableIgnNeuDR = sqlCmdSentiIgnNeutral.ExecuteReader();
        while (sentiWordTableIgnNeuDR.Read())
            words.RemoveAll(word => word.Equals(sentiWordTableIgnNeuDR["Synset_term"].ToString().Replace('_', ' ').Replace('-', ' ').Replace('.', ' ')));
        sentiWordTableIgnNeuDR.Close();

        SqlCommand sqlCmd1 = new SqlCommand("select * from KWTable where category='neutral' or category='ignore' order by Len(Synset_term) desc", sqlConn);
        SqlDataReader WordTableIgnNeu = sqlCmd1.ExecuteReader();
        while(WordTableIgnNeu.Read())
            words.RemoveAll(word => word.Equals(WordTableIgnNeu["Synset_term"].ToString()));
        WordTableIgnNeu.Close();

        for(int i=0;i<words.Count;i++)
            //Console.WriteLine(words[i] = checkSpelling(words[i]));
            Console.WriteLine(words[i]);

        SqlCommand sqlCmdSenti = new SqlCommand("select * from SentiWordKWTable where category='positive' or category='negative'", sqlConn);
        SqlDataReader sentiWordTable = sqlCmdSenti.ExecuteReader();
        sentiWordTable.Close();
        SqlCommand sqlCmdKWTable = new SqlCommand("select * from KWTable where category='positive' or category='negative'", sqlConn);
        SqlDataReader WordTable = sqlCmdKWTable.ExecuteReader();
        WordTable.Close();*/
    }
    private static string checkSpelling(string word)
    {
        Checker checker = new Checker(CultureInfo.CurrentCulture);
        Request request = new Request(word);
        request.Multilined = false;
        Result result = checker.Check(request);
        if (result.Errored)
            Console.WriteLine("Error occured while spell checking");
        else
            if (result.HasCorrections)
                foreach (Correction c in result.Corrections)
                {
                    if (c.HasSuggestions)
                        foreach (Suggestion s in c.Suggestions)
                        {
                            word = s.Value;
                            break;
                        }
                    break;
                }

        return word;
    }
    private static string scrapeSite(Uri siteToScrape)
    {
        string s = stripOffTags(getHTML(siteToScrape));
        if (siteToScrape.ToString().Contains("wogma"))
        {
            if (s.IndexOf("quick review:") > 0)
                s = s.Remove(0, s.IndexOf("quick review:")).Replace("quick review:", "");
            if (s.IndexOf("Leave a new comment") > 0)
                s = s.Remove(s.IndexOf("Leave a new comment")).Replace("Leave a new comment", "");
            if (s.IndexOf("Tweet this") > 0 && s.IndexOf("Detailed Ratings (out of 5):") > 0)
                s = s.Remove(s.IndexOf("Tweet this"), s.IndexOf("Detailed Ratings (out of 5):") - s.IndexOf("Tweet this"));
            if (s.IndexOf("- Movie Details") > 0 && s.IndexOf("Comments (") > 0)
                s = s.Remove(s.IndexOf("- Movie Details"), s.IndexOf("Comments (") - s.IndexOf("- Movie Details")).Replace("Comments (", "");
            if (s.IndexOf("wogma review") > 0 && s.IndexOf("| Trailer") > 0)
                s = s.Remove(s.IndexOf("wogma review"), s.IndexOf("| Trailer") - s.IndexOf("wogma review")).Replace("| Trailer", "");
        }
        else if (siteToScrape.ToString().Contains("bollymoviereviewz"))
        {
            if (s.IndexOf("Post a Comment") > 0)
                s = s.Remove(s.IndexOf("Post a Comment")).Replace("Post a Comment", "");
            if (s.IndexOf("Email ThisBlogThis!Share to TwitterShare to Facebook") > 0 && s.IndexOf(" comments:") > 0)
                s = s.Remove(s.IndexOf("Email ThisBlogThis!Share to TwitterShare to Facebook"), s.IndexOf(" comments:") - s.IndexOf("Email ThisBlogThis!Share to TwitterShare to Facebook")).Replace("Email ThisBlogThis!Share to TwitterShare to Facebook", "").Replace(" comments:", "");
            if (s.IndexOf("From All the reviews on the web") > 0)
                s = s.Remove(0, s.IndexOf("From All the reviews on the web")).Replace("From All the reviews on the web", "");
        }
        else if (siteToScrape.ToString().Contains("themoviereviewblog"))
        {
            if (s.IndexOf("Rate this:") > 0 && s.IndexOf(" comments on") > 0)
                s = s.Remove(s.IndexOf("Rate this:"), s.IndexOf(" comments on") - s.IndexOf("Rate this:"));
            if (s.IndexOf("Leave a Reply") > 0)
                s = s.Remove(s.IndexOf("Leave a Reply")).Replace("Leave a Reply", "");
            if (s.IndexOf("rarr;") > 0)
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
            s = "";
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
    static void getLinksForSpecialCases(string specialCase)
    {
        string[] partsOfMovieName = specialCase.Split(' ');
        foreach (ReviewSite site in links2Crawl)
        {
            for (int a = 0; a < site.movieNames.Count; a++)
            {
                bool containsAllParts = true;
                for (int b = 0; b < partsOfMovieName.Length; b++)
                {
                    if (!(site.movieNames[a].Contains(partsOfMovieName[b]) || site.movieNames[a].Contains(partsOfMovieName[b])))
                    {
                        containsAllParts = false;
                        break;
                    }
                }
                if (containsAllParts && a<site.links2Scrape.Count)
                    links2Scrape4UserSelectedMovies.Add(site.links2Scrape[a]);
            }
        }
    }

    private double getOfficialMovieRating(String[] links2getOfficialRating)
    {
        try
        {
            foreach (String link in links2getOfficialRating)
            {
                if (link.Contains("wogma.com"))
                {
                    String scrapedText = scrapeSite(new Uri(link));
                    if (scrapedText.IndexOf("Detailed Ratings (out of 5):") != -1 && scrapedText.IndexOf("Direction:") != -1 && scrapedText.IndexOf("Lyrics") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("Direction:"), (scrapedText.IndexOf("Lyrics:") + "Lyrics".Length + 5) - scrapedText.IndexOf("Direction"));
                    String allScores = "";

                    for (int i = 0; i < scrapedText.Length; i++)
                        if (scrapedText[i].Equals('.') || (scrapedText[i] >= (int)'0' && scrapedText[i] <= (int)'9') || scrapedText[i].Equals('\n'))
                            allScores += scrapedText[i];

                    String[] scores = allScores.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    double officialScore = 0;
                    foreach (String s in scores)
                        officialScore += (Double.Parse(s) / 5);

                    //Console.Write("Official Wogma Rating: ");

                    return officialScore;
                }
                else if (link.Contains("reviewgang"))
                {
                    string extractedContent = stripOffTags(getHTML(new Uri(link))).ToLower().Trim();
                    if (extractedContent.IndexOf("release date") != -1)
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
                    //Console.WriteLine(Double.Parse(extractedContent));

                    return Double.Parse(extractedContent) / 2;
                }
                else if (link.Contains("koimoi"))
                {
                    String extractedContent = new WebClient().DownloadString(link).ToLower();
                    if (extractedContent.IndexOf("watch or not") != -1)
                        extractedContent = extractedContent.Substring(0, extractedContent.IndexOf("watch or not"));

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

                    extractedContent = extractedContent.Substring(extractedContent.IndexOf("rating:"), extractedContent.IndexOf("star cast:") - extractedContent.IndexOf("rating:")).Replace("rating:", "");
                    extractedContent = extractedContent.Substring(0, extractedContent.IndexOf("/5")).Trim();
                    return Double.Parse(extractedContent);
                }
                else if (link.Contains("bollymoviereviewz.blogspot.in"))
                {
                    string extractedContent = "";

                    WebClient wc = new WebClient();
                    wc.Headers.Add("HTTP_USER_AGENT", "Web-Scraper-Agent (your-custom-user-agent-here)");
                    // Download the web page content from the URL
                    extractedContent = wc.DownloadString(link);

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

                    extractedContent = extractedContent.Substring(0, extractedContent.IndexOf("From All the reviews on the web"));
                    //extractedContent = extractedContent.Substring(extractedContent.IndexOf("From All the reviews on the web"), 100);
                    extractedContent = extractedContent.Substring(extractedContent.Length - 111);
                    extractedContent = extractedContent.Substring(extractedContent.IndexOf("Rating:")).Replace("Rating:", "").Replace("&nbsp;", "").Replace("/5", "").Replace("/", "").Trim();
                    return double.Parse(extractedContent);
                }
                else if (link.Contains("hindustantimes.com"))
                {
                    String scrapedText = scrapeSite(new Uri(link));

                    scrapedText = scrapedText.Substring(0, 20).ToLower();
                    String score = "";
                    double officialScore = 0;
                    for (int i = 0; i < scrapedText.Length; i++)
                    {
                        if ((scrapedText[i].Equals('*') || scrapedText[i].Equals('/') || scrapedText[i].Equals('\\') || (scrapedText[i] >= (int)'0' && scrapedText[i] <= (int)'9')))
                            score += scrapedText[i];
                    }

                    if (score.Contains("1/2") || score.Contains("1\\2"))
                    {
                        officialScore += 0.5;
                        score = score.Replace("1/2", "").Replace("1\\2", "");
                    }

                    for (int i = 0; i < score.Length; i++)
                        if (score[i].Equals('*'))
                            officialScore++;

                    return officialScore;
                }
                else if (link.Contains("themoviereviewblog.wordpress.com"))
                {
                    String scrapedText = scrapeSite(new Uri(link));

                    double officialRating = 0;
                    if (scrapedText.IndexOf("My Rating:") != -1 && scrapedText.IndexOf("Three") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My Rating:"), scrapedText.IndexOf("Three") - scrapedText.IndexOf("My Rating:")).Replace("My Rating:", "").Trim();
                    else if (scrapedText.IndexOf("My Rating:") != -1 && scrapedText.IndexOf("One") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My Rating:"), scrapedText.IndexOf("One") - scrapedText.IndexOf("My Rating:")).Replace("My Rating:", "").Trim();
                    else if (scrapedText.IndexOf("My Rating:") != -1 && scrapedText.IndexOf("Two") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My Rating:"), scrapedText.IndexOf("Two") - scrapedText.IndexOf("My Rating:")).Replace("My Rating:", "").Trim();
                    else if (scrapedText.IndexOf("My Rating:") != -1 && scrapedText.IndexOf("Four") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My Rating:"), scrapedText.IndexOf("Four") - scrapedText.IndexOf("My Rating:")).Replace("My Rating:", "").Trim();
                    else if (scrapedText.IndexOf("My Rating:") != -1 && scrapedText.IndexOf("Five") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My Rating:"), scrapedText.IndexOf("Five") - scrapedText.IndexOf("My Rating:")).Replace("My Rating:", "").Trim();

                    else if (scrapedText.IndexOf("My rating:") != -1 && scrapedText.IndexOf("Three") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My rating:"), scrapedText.IndexOf("Three") - scrapedText.IndexOf("My rating:")).Replace("My rating:", "").Trim();
                    else if (scrapedText.IndexOf("My rating:") != -1 && scrapedText.IndexOf("One") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My rating:"), scrapedText.IndexOf("One") - scrapedText.IndexOf("My rating:")).Replace("My rating:", "").Trim();
                    else if (scrapedText.IndexOf("My rating:") != -1 && scrapedText.IndexOf("Two") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My rating:"), scrapedText.IndexOf("Two") - scrapedText.IndexOf("My rating:")).Replace("My rating:", "").Trim();
                    else if (scrapedText.IndexOf("My rating:") != -1 && scrapedText.IndexOf("Four") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My rating:"), scrapedText.IndexOf("Four") - scrapedText.IndexOf("My rating:")).Replace("My rating:", "").Trim();
                    else if (scrapedText.IndexOf("My rating:") != -1 && scrapedText.IndexOf("Five") != -1)
                        scrapedText = scrapedText.Substring(scrapedText.IndexOf("My rating:"), scrapedText.IndexOf("Five") - scrapedText.IndexOf("My rating:")).Replace("My rating:", "").Trim();

                    if (scrapedText.EndsWith("½"))
                        officialRating += 0.5;
                    for (int i = 0; i < scrapedText.Length; i++)
                        if (scrapedText[i].Equals('*'))
                            officialRating++;

                    return officialRating;
                }

            }
        }
        catch (Exception e)
        {
            return 2;
        }
        return -1;
    }

    void buildMovieMenu()
    {
        List<String> tempMovieMenuList = new List<String>();
        List<String> movieMenuList = new List<String>();

        for (int i = 0; i < links2Crawl.Count; i++)
            for (int j = 0; j < links2Crawl[i].movieNames.Count; j++)
                links2Crawl[i].movieNames[j] = links2Crawl[i].movieNames[j].ToLower().Trim();

        for (int i = 0; i < links2Crawl.Count; i++)
            for (int j = 0; j < links2Crawl[i].movieNames.Count; j++)
                if (!movieMenuList.Contains(links2Crawl[i].movieNames[j]))
                    tempMovieMenuList.Add(links2Crawl[i].movieNames[j]);


        /*for (int i = 0; i < links2Crawl.Count; i++)
            for (int j = 0; j < links2Crawl[i].movieNames.Count; j++)
                for (int k = 0; k < movieMenuList.Count; k++)
                {
                    String[] arr = links2Crawl[i].movieNames[i].Split(' ');
                    int matchCounter =0;
                    for (int l = 0; l < arr.Length; l++)
                    {
                        if (movieMenuList[k].Contains(arr[l]))
                            matchCounter++;
                    }
                    if (!(matchCounter > 0))
                        movieMenuList.Add(links2Crawl[i].movieNames[j]);
                }
        */
        movieMenuList = tempMovieMenuList.Distinct().ToList();
        movieMenuList.Remove("aalaap");
        movieMenuList.Sort();

        for (int i = 0; i < movieMenuList.Count; i++)
        {
            if (movieMenuList[i].StartsWith("arjun"))
                movieMenuList[i] = "arjun";
            else if (movieMenuList[i].StartsWith("band baaja baaraat"))
                movieMenuList[i] = "band baaja baarat";
            else if (movieMenuList[i].StartsWith("dangerous ishqq") || movieMenuList[i].StartsWith("dangerous ishhq"))
                movieMenuList[i] = "dangerous ishq";
            else if (movieMenuList[i].StartsWith("the amazing spider"))
                movieMenuList[i] = "the amazing spiderman";
            else if (movieMenuList[i].StartsWith("eega"))
                movieMenuList[i] = "eega";
            else if (movieMenuList[i].StartsWith("madagascar 3"))
                movieMenuList[i] = "madagascar 3";
            else if (movieMenuList[i].StartsWith("shanghai"))
                movieMenuList[i] = "shanghai";
            else if (movieMenuList[i].StartsWith("snow white"))
                movieMenuList[i] = "snow white and the huntsman";
            else if (movieMenuList[i].StartsWith("teri meri"))
                movieMenuList[i] = "teri meri kahaani";
            else if (movieMenuList[i].StartsWith("supermen"))
                movieMenuList[i] = "supermen of malegaon";
            else if (movieMenuList[i].StartsWith("superman"))
                movieMenuList[i] = "supermen of malegaon";
            else if (movieMenuList[i].StartsWith("amazing spider man"))
                movieMenuList[i] = "the amazing spiderman";
            else if (movieMenuList[i].StartsWith("ice age"))
                movieMenuList[i] = "ice age 4";
            else if (movieMenuList[i].StartsWith("kya super kool hain hum"))
                movieMenuList[i] = "kya super kool hain hum";
            else if (movieMenuList[i].StartsWith("kya super kool hai hum"))
                movieMenuList[i] = "kya super kool hain hum";
            else if (movieMenuList[i].StartsWith("kyaa super kool hain hum"))
                movieMenuList[i] = "kya super kool hain hum";
            else if (movieMenuList[i].StartsWith("kyaa super kool hai hum"))
                movieMenuList[i] = "kya super kool hain hum";
            else if (movieMenuList[i].StartsWith("shirin farhad"))
                movieMenuList[i] = "shirin farhad";
            else if (movieMenuList[i].StartsWith("step up "))
                movieMenuList[i] = "step up revolution";
            else if (movieMenuList[i].StartsWith("gangs of wasseypur ii"))
                movieMenuList[i] = "gangs of wasseypur 2";
            else if (movieMenuList[i].StartsWith("gangs of wasseypur i"))
                movieMenuList[i] = "gangs of wasseypur";
            else if (movieMenuList[i].StartsWith("gangs wasseypur 2"))
                movieMenuList[i] = "gangs of wasseypur 2";
            else if (movieMenuList[i].StartsWith("julayi telugu"))
                movieMenuList[i] = "julayi";
            else if (movieMenuList[i].StartsWith("delhi in day"))
                movieMenuList[i] = "delhi in a day";
            else if (movieMenuList[i].StartsWith("the descendents"))
                movieMenuList[i] = "the descendants";
            else if (movieMenuList[i].Contains("ekk deewana tha"))
                movieMenuList[i] = "ek deewana tha";
            else if (movieMenuList[i].Contains("ghost movie"))
                movieMenuList[i] = "ghost";
            else if (movieMenuList[i].Contains("joker") && movieMenuList[i].Contains("toi"))
                movieMenuList[i] = "joker";
            else if (movieMenuList[i].Contains("jalpari desert mermaid"))
                movieMenuList[i] = "jalpari";
            else if (movieMenuList[i].Contains("jalpari the desert mermaid"))
                movieMenuList[i] = "jalpari";
            else if (movieMenuList[i].StartsWith("raaz") && movieMenuList[i].EndsWith("komal"))
                movieMenuList[i] = "raaz 3";
            else if (movieMenuList[i].Contains("mugamoodi tamil"))
                movieMenuList[i] = "mugamoodi";
        }

        movieMenuList = movieMenuList.Distinct().ToList();
        movieMenuList.Remove("band baaja baarat");
        movieMenuList.Remove("shuttlecock boys");
        movieMenuList.Remove("bodyguard");
        movieMenuList.Remove("joker");
        if (movieMenuList.Contains("anupama-chopra-s-review-jism-2/article1-907861.aspx"))
            movieMenuList.Remove("anupama-chopra-s-review-jism-2/article1-907861.aspx");
        if (movieMenuList.Contains("raaz-3/14816"))
            movieMenuList.Remove("raaz-3/14816");
        if(movieMenuList.Contains("raaz-3-movie-review-horror-of-horrors-091202.asp"))
            movieMenuList.Remove("raaz-3-movie-review-horror-of-horrors-091202.asp");
        if (movieMenuList.Contains("sarit-ray-s-review-raaz-3/article1-926367.aspx"))
            movieMenuList.Remove("sarit-ray-s-review-raaz-3/article1-926367.aspx");
        movieMenuList.Remove("krishna aur kans");
        if (movieMenuList.Contains("chal-pichchur-banate-hain-movie-review-a-refreshing-story-idea-091202.asp"))
            movieMenuList.Remove("chal-pichchur-banate-hain-movie-review-a-refreshing-story-idea-091202.asp");
        if (movieMenuList.Contains("rashid-irani-s-review-to-rome-with-love/article1-926372.aspx"))
            movieMenuList.Remove("rashid-irani-s-review-to-rome-with-love/article1-926372.aspx");
        if (movieMenuList.Contains("riwayat/15124"))
            movieMenuList.Remove("riwayat/15124");
        movieMenuList.Sort();

        finalMovieMenuList = movieMenuList;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        dispBgWork.BackColor = Color.FromArgb(21, 22, 19);
        dispBgWork.BorderColor = Color.FromArgb(21, 22, 19);
        dispBgWork.ForeColor = Color.FromArgb(129, 185, 5);
        //dispBgWork.ForeColor = Color.FromArgb();
        //dispBgWork.ReadOnly = true;
        //dispBgWork.TextMode = TextBoxMode.MultiLine;
        //dispBgWork.ForeColor = Color.DarkBlue;
        //dispBgWork.AutoPostBack = true;
        //dispBgWork.Font.Bold = true;
        //dispBgWork.ReadOnly = true;
        //dispBgWork.Text = "dhfjfdhj";
        //dispBgWork.BackColor = sampleArticle.BackColor;

        //Control c = Master.FindControl("SheetContentPlaceHolder");
        //c.Controls.Add(dispBgWork);

        //Artisteer.Article article1 = new Artisteer.Article();
        //Control c = Master.FindControl("SheetContentPlaceHolder");
        //c.Controls.Add(dispBgWork);


        dispBgWork.Text = "\n\n";
        foreach (Uri link in ReviewSiteLinks)
            links2Crawl.Add(new ReviewSite(link));

        dispBgWork.Text = "Crawling and gathering movie names ....\t";
        foreach (ReviewSite site in links2Crawl)
            //dispBgWork.Text += "Site: " + site.ReviewSiteURL.ToString() + "....";
            Crawl(site);

        dispBgWork.Text += "Building Movie Names Menu....";
        buildMovieMenu();
        dispBgWork.Text += "Done!\n";

        c = new CheckBox[finalMovieMenuList.Count];
        for (int i = 0; i < finalMovieMenuList.Count; i++)
        {
            c[i] = new CheckBox();
            c[i].Text = finalMovieMenuList[i];
            c[i].Font.Bold = true;
            //c[i].CheckedChanged += new EventHandler(chkChkBoxes);
            //c[i].AutoPostBack = true;
            LiteralControl space = new LiteralControl();
            space.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            if (finalMovieMenuList[i].StartsWith("A") || finalMovieMenuList[i].StartsWith("a"))
            {
                PlaceHolderA.Controls.Add(c[i]);
                PlaceHolderA.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("B") || finalMovieMenuList[i].StartsWith("b"))
            {
                PlaceHolderB.Controls.Add(c[i]);
                PlaceHolderB.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("C") || finalMovieMenuList[i].StartsWith("c"))
            {
                PlaceHolderC.Controls.Add(c[i]);
                PlaceHolderC.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("D") || finalMovieMenuList[i].StartsWith("d"))
            {
                PlaceHolderD.Controls.Add(c[i]);
                PlaceHolderD.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("E") || finalMovieMenuList[i].StartsWith("e"))
            {
                PlaceHolderE.Controls.Add(c[i]);
                PlaceHolderE.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("F") || finalMovieMenuList[i].StartsWith("f"))
            {
                PlaceHolderF.Controls.Add(c[i]);
                PlaceHolderF.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("G") || finalMovieMenuList[i].StartsWith("g"))
            {
                PlaceHolderG.Controls.Add(c[i]);
                PlaceHolderG.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("H") || finalMovieMenuList[i].StartsWith("h"))
            {
                PlaceHolderH.Controls.Add(c[i]);
                PlaceHolderH.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("I") || finalMovieMenuList[i].StartsWith("i"))
            {
                PlaceHolderI.Controls.Add(c[i]);
                PlaceHolderI.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("J") || finalMovieMenuList[i].StartsWith("j"))
            {
                PlaceHolderJ.Controls.Add(c[i]);
                PlaceHolderJ.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("K") || finalMovieMenuList[i].StartsWith("k"))
            {
                PlaceHolderK.Controls.Add(c[i]);
                PlaceHolderK.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("L") || finalMovieMenuList[i].StartsWith("l"))
            {
                PlaceHolderL.Controls.Add(c[i]);
                PlaceHolderL.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("M") || finalMovieMenuList[i].StartsWith("m"))
            {
                PlaceHolderM.Controls.Add(c[i]);
                PlaceHolderM.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("N") || finalMovieMenuList[i].StartsWith("n"))
            {
                PlaceHolderN.Controls.Add(c[i]);
                PlaceHolderN.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("O") || finalMovieMenuList[i].StartsWith("o"))
            {
                PlaceHolderO.Controls.Add(c[i]);
                PlaceHolderO.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("P") || finalMovieMenuList[i].StartsWith("p"))
            {
                PlaceHolderP.Controls.Add(c[i]);
                PlaceHolderP.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("Q") || finalMovieMenuList[i].StartsWith("q"))
            {
                PlaceHolderQ.Controls.Add(c[i]);
                PlaceHolderQ.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("R") || finalMovieMenuList[i].StartsWith("r"))
            {
                PlaceHolderR.Controls.Add(c[i]);
                PlaceHolderR.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("S") || finalMovieMenuList[i].StartsWith("s"))
            {
                PlaceHolderS.Controls.Add(c[i]);
                PlaceHolderS.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("T") || finalMovieMenuList[i].StartsWith("t"))
            {
                PlaceHolderT.Controls.Add(c[i]);
                PlaceHolderT.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("U") || finalMovieMenuList[i].StartsWith("u"))
            {
                PlaceHolderU.Controls.Add(c[i]);
                PlaceHolderU.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("V") || finalMovieMenuList[i].StartsWith("v"))
            {
                PlaceHolderV.Controls.Add(c[i]);
                PlaceHolderV.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("W") || finalMovieMenuList[i].StartsWith("w"))
            {
                PlaceHolderW.Controls.Add(c[i]);
                PlaceHolderW.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("X") || finalMovieMenuList[i].StartsWith("x"))
            {
                PlaceHolderX.Controls.Add(c[i]);
                PlaceHolderX.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("Y") || finalMovieMenuList[i].StartsWith("y"))
            {
                PlaceHolderY.Controls.Add(c[i]);
                PlaceHolderY.Controls.Add(space);
            }
            else if (finalMovieMenuList[i].StartsWith("Z") || finalMovieMenuList[i].StartsWith("z"))
            {
                PlaceHolderZ.Controls.Add(c[i]);
                PlaceHolderZ.Controls.Add(space);
            }
            else 
            {
                PlaceHolderNum.Controls.Add(c[i]);
                PlaceHolderNum.Controls.Add(space);
            }
        }
    }
    void Crawl(ReviewSite site)
    {
        try
        {
            //dispBgWork.Text += "\tCrawling Started....\t";

            WebRequest req = (HttpWebRequest)WebRequest.Create(site.ReviewSiteURL);
            WebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream istrm = resp.GetResponseStream();
            string htmlCode = new StreamReader(istrm).ReadToEnd();

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlCode);

            HtmlNodeCollection c = htmlDoc.DocumentNode.SelectNodes("//a");
            List<String> tempScrapingLinks = new List<string>();
            List<String> tempTitleOfScrapingLinks = new List<string>();
            List<String> tempInnerTextOfScrapingLinks = new List<string>();
            bool addTitleNInnerText = true;
            if (c != null)
                foreach (HtmlNode link in c)
                {
                    HtmlAttribute hrefAttr = link.Attributes["href"];
                    if (hrefAttr != null /*&& !(tempScrapingLinks.Contains(hrefAttr.Value))*/)
                    {
                        if (!(tempScrapingLinks.Contains(hrefAttr.Value)))
                        {
                            tempScrapingLinks.Add(hrefAttr.Value);
                            addTitleNInnerText = true;
                        }
                        else
                            addTitleNInnerText = false;
                    }
                    else
                    {
                        //tempScrapingLinks.Add(site.ReviewSiteURL.ToString() + "~~~REMOVE THIS LINK~~~");
                        addTitleNInnerText = false;
                    }

                    if (addTitleNInnerText)
                    {
                        HtmlAttribute titleAttr = link.Attributes["title"];
                        if (titleAttr != null)
                            tempTitleOfScrapingLinks.Add(titleAttr.Value);
                        else
                            tempTitleOfScrapingLinks.Add("");

                        if (link.InnerText.Trim().Length > 0 /*&& !tempInnerTextOfScrapingLinks.Contains(link.InnerText.Trim())*/)
                            tempInnerTextOfScrapingLinks.Add(link.InnerText.Trim());
                        else
                            tempInnerTextOfScrapingLinks.Add("");
                    }
                }
            //int a,b, k;
            //for (a = 0, b = 0, k=0; a < tempScrapingLinks.Count && b < tempTitleOfScrapingLinks.Count && k<tempInnerTextOfScrapingLinks.Count; a++, b++, k++)
            //Console.WriteLine(tempScrapingLinks[a] + "\t" + tempTitleOfScrapingLinks[b] + "\t" + tempInnerTextOfScrapingLinks[k]);

            //Console.Write(tempScrapingLinks.Count.Equals(tempTitleOfScrapingLinks.Count) +"&"+tempTitleOfScrapingLinks.Count.Equals(tempInnerTextOfScrapingLinks.Count));

            if (site.ReviewSiteURL.ToString().Contains("wogma"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    if (tempScrapingLinks[i].Contains("review/") && !tempScrapingLinks[i].Contains("article") /*|| tempScrapingLinks[i].Contains("buzz")*/ && !tempScrapingLinks[i].Contains("/ek-review/"))
                    {
                        //add link
                        if (tempScrapingLinks[i].StartsWith("http://wogma.com") || tempScrapingLinks[i].StartsWith("https://wogma.com"))
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                        else
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://wogma.com" + tempScrapingLinks[i]);

                        //add title and hyperlinked text (inner text)
                        site.titleOfLinks2Scrape.Add(tempTitleOfScrapingLinks[i]);
                        site.innerTextOfLinks2Scrape.Add(tempInnerTextOfScrapingLinks[i]);
                    }
                }
            else if (site.ReviewSiteURL.ToString().Contains("bollywoodhungama"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    if (tempScrapingLinks[i].Contains("moviemicro/") && !tempScrapingLinks[i].Contains("images/"))
                    {
                        //add links
                        if (tempScrapingLinks[i].StartsWith("http://www.bollywoodhungama.com") || tempScrapingLinks[i].StartsWith("https://www.bollywoodhungama.com"))
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                        else
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://www.bollywoodhungama.com" + tempScrapingLinks[i]);

                        site.links2Scrape.Add(tempScrapingLinks[i].Replace("criticreview", "userreview"));
                        site.links2Scrape.Add(tempScrapingLinks[i].Replace("criticreview", "externalview"));

                        //add title and hyperlinked text(inner text)
                        site.titleOfLinks2Scrape.Add(tempTitleOfScrapingLinks[i]);
                        site.innerTextOfLinks2Scrape.Add(tempInnerTextOfScrapingLinks[i]);

                        site.titleOfLinks2Scrape.Add(tempTitleOfScrapingLinks[i]);
                        site.innerTextOfLinks2Scrape.Add(tempInnerTextOfScrapingLinks[i]);

                        site.titleOfLinks2Scrape.Add(tempTitleOfScrapingLinks[i]);
                        site.innerTextOfLinks2Scrape.Add(tempInnerTextOfScrapingLinks[i]);
                    }
                }
            else if (site.ReviewSiteURL.ToString().Contains("moviereviewz"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (tempScrapingLinks[i].Contains("review") && !tempScrapingLinks[i].Contains("taran-adarsh-profile") && !tempScrapingLinks[i].Contains("rajeev-masand") && !tempScrapingLinks[i].Contains("what-to-expect-when-youre-expecting") && !tempScrapingLinks[i].Contains("nikhat-kazmi") && !tempScrapingLinks[i].Contains("best-movies-of-2011") && !tempScrapingLinks[i].Equals("http://bollymoviereviewz.blogspot.in/") && !tempScrapingLinks[i].Contains("music-review") && !tempScrapingLinks[i].Contains("feeds") && !tempScrapingLinks[i].Contains("top-10-bollywood") && !tempScrapingLinks[i].Contains("upcoming") && !tempScrapingLinks[i].Contains("label/") && !tempScrapingLinks[i].Contains("#") && !tempScrapingLinks[i].Contains("search") && !tempScrapingLinks[i].Contains("archive") && !tempInnerTextOfScrapingLinks[i].Contains("http://bollymoviereviewz.blogspot.in/2010/09/dabanng-review.html") && !tempScrapingLinks[i].StartsWith("http://bollymoviereviewz.blogspot.in/2011") && !tempScrapingLinks[i].Contains("http://bollymoviereviewz.blogspot.in/2012/08/joker-music-songs-review-akshay-kumar.html") && !tempScrapingLinks[i].Contains("http://bollymoviereviewz.blogspot.in/2012/09/raaz-3-music-songs-review-emraan-hashmi.html"))
                        if (tempScrapingLinks[i].StartsWith("http://") || tempScrapingLinks[i].StartsWith("https://"))
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                        else
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://bollymoviereviewz.blogspot.in" + tempScrapingLinks[i]);

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add("");
                }
            else if (site.ReviewSiteURL.ToString().Contains("reviewblog"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (!tempScrapingLinks[i].Contains("tag") && !tempScrapingLinks[i].Contains("share") && !tempScrapingLinks[i].Contains("like") && !tempScrapingLinks[i].Contains("about") && !tempScrapingLinks[i].Contains("contact") && !tempScrapingLinks[i].Contains("#") && !tempScrapingLinks[i].Contains("category") && tempScrapingLinks[i] != "http://themoviereviewblog.wordpress.com/" && !tempScrapingLinks[i].Contains("twitter.com") && !tempScrapingLinks[i].Contains("javascript:void(0)") && !tempScrapingLinks[i].Contains("http://wordpress.com") && !tempScrapingLinks[i].Contains("themify") && !tempScrapingLinks[i].Contains("feed") && !tempScrapingLinks[i].Contains("theme.wordpress.com") && !tempScrapingLinks[i].Contains("login.php") && !tempScrapingLinks[i].Contains("www.linkedin.com") && !tempScrapingLinks[i].Contains("indiblogger.in") && !tempScrapingLinks[i].Contains("gravatar.com") && !tempScrapingLinks[i].Contains("shreykhetarpal.blogspot.com") && !tempScrapingLinks[i].Contains("youtube.com") && !tempScrapingLinks[i].Contains("bollywood-wishlist") && !tempScrapingLinks[i].Contains("bollybad") && !tempScrapingLinks[i].Contains("animal-heroes") && !tempScrapingLinks[i].Contains("khelein") && !tempScrapingLinks[i].Contains("small-films-big-impact") && !tempScrapingLinks[i].Contains("rom-com-gone") && !tempScrapingLinks[i].Contains("the-genius-of-vishal-bhardwaj") && !(tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2012/07/14/526/")) && !(tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2012/06/")) && !(tempScrapingLinks[i].Contains("http://thepromisefoundation.wordpress.com")) && !(tempScrapingLinks[i].Contains("http://www.fridaynirvana.com/film/")) && !(tempScrapingLinks[i].Contains("http://youtu.be/8dWir9Q_Vek")) && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2011/12/24/398/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2012/07/15/nolan-made-me-a-batman-fan/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/12/20/avatar/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/10/18/blue/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/6/14/bride-wars/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/12/29/celebrating-the-duds/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/11/22/guzaarish/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/10/04/inglourious-basterds/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/02/01/ishqiya/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/03/01/its-complicated/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/07/05/kambakkht-ishq/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/10/03/wake-up-sid/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/07/26/romance-on-screen/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/07/19/udaan/") && !tempScrapingLinks[i].Contains("https://themoviereviewblog.wordpress.com/2009/09/27/what%E2%80%99s-your-raashee/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/03/01/teen-patti/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/12/13/rocket-singh-salesman-of-the-year/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/03/01/karthik-calling-karthik/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/06/28/new-york/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/11/22/kurbaan/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/11/22/twilight/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/02/01/the-blind-side/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/06/14/bride-wars/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/10/02/robot/") && !tempScrapingLinks[i].Contains("http://lockerz.com/s/237960396") && !tempScrapingLinks[i].Contains("infibeam.com") && !tempScrapingLinks[i].Contains("ombooks") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2009/12/26/3-idiots/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2011/02/28/vishal-ko-7-khoon-maaf/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2011/09/04/bodyguard/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2011/07/02/delhi-belly/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2011/01/22/dhobi-ghat/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2011/04/24/dum-maaro-dum/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2011/02/27/tanu-weds-manu/") && !tempScrapingLinks[i].Contains("http://themoviereviewblog.wordpress.com/2010/10/03/khichdi-the-movie/") && !tempScrapingLinks[i].StartsWith("http://themoviereviewblog.wordpress.com/2010/") && !tempScrapingLinks[i].StartsWith("http://themoviereviewblog.wordpress.com/2011/") && !tempScrapingLinks[i].StartsWith("http://themoviereviewblog.wordpress.com/2009/") && !tempScrapingLinks[i].Contains("is.gd/8QVNYE") && !tempScrapingLinks[i].Contains("bit.ly"))
                        if (tempScrapingLinks[i].StartsWith("http://themoviereviewblog.wordpress.com/") || tempScrapingLinks[i].StartsWith("https://themoviereviewblog.wordpress.com/"))
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                        else
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://themoviereviewblog.wordpress.com/" + tempScrapingLinks[i]);

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add("");
                }
            else if (site.ReviewSiteURL.ToString().Equals("http://www.fridaynirvana.com/film/"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (tempScrapingLinks[i].Contains("movie-review") && !tempScrapingLinks[i].Contains("#") && !tempScrapingLinks[i].Contains("browse") && !tempScrapingLinks[i].Contains("http://www.fridaynirvana.com/film/2009/12/movie-review-3-idiots-2.html") && !tempScrapingLinks[i].Contains("http://www.fridaynirvana.com/film/2010/06/movie-review-rajneeti-2.html") && !tempScrapingLinks[i].StartsWith("http://www.fridaynirvana.com/film/2011") && !tempScrapingLinks[i].StartsWith("http://www.fridaynirvana.com/film/2010"))
                        if (tempScrapingLinks[i].StartsWith("http://www.fridaynirvana.com/film/"))
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                        else
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://www.fridaynirvana.com/film/" + tempScrapingLinks[i]);

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add("");
                }
            else if (site.ReviewSiteURL.ToString().StartsWith("http://www.koimoi.com/"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (tempScrapingLinks[i].Contains("review") && !tempScrapingLinks[i].Contains("page") && !tempScrapingLinks[i].Contains("#") && tempScrapingLinks[i] != site.ReviewSiteURL.ToString() && !tempScrapingLinks[i].Contains("music-review") && !tempScrapingLinks[i].Contains("/box-office") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/kya-yahi-sach-hai-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/one-day-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/sherlock-holmes-a-game-of-shadows-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/alvin-and-the-chipmunks-chipwrecked-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/don-2-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/valentine-s-night-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/wrath-of-the-titans-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/a-good-old-fashioned-orgy-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/arthur-christmas-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/battleship-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/chronicle-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/contraband-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/coriolanus-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/diary-of-a-butterfly-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/dr-seuss-the-lorax-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/extremely-loud-incredibly-close-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/ghost-rider-spirit-of-vengeance-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/haywire-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/heartbreaker-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/category/reviews/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/j-edgar-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/journey-2-the-mysterious-island-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/man-on-a-ledge-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/lol-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/married-2-america-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/moneyball-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/staying-alive-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/the-artist-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/the-darkest-hour-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/the-devil-inside-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/the-hunger-games-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/the-iron-lady-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/the-raven-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/the-woman-in-black-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/tinker-tailor-soldier-spy-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/tutiya-dil-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/underworld-awakening-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/war-horse-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/war-horse-review/") && !tempScrapingLinks[i].Contains("http://www.koimoi.com/reviews/will-you-marry-me-review/"))
                        if (tempScrapingLinks[i].StartsWith("http://www.koimoi.com/reviews/"))
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                        else
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://www.koimoi.com/reviews/" + tempScrapingLinks[i]);

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add("");
                }
            else if (site.ReviewSiteURL.ToString().StartsWith("http://www.reviewgang.com/"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (tempScrapingLinks[i].Contains("movie-review") && !tempScrapingLinks[i].Contains("#"))
                        if (tempScrapingLinks[i].StartsWith("http://"))
                        {
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                            site.links2Scrape.Add(tempScrapingLinks + "user_reviews");
                        }
                        else
                        {
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://www.reviewgang.com" + tempScrapingLinks[i]);
                            site.links2Scrape.Add(tempScrapingLinks[i] + "/user_reviews");
                        }

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add(""); site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add(""); site.innerTextOfLinks2Scrape.Add("");
                }
            else if (site.ReviewSiteURL.ToString().Equals("http://www.glamsham.com/movies/reviews/"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (tempScrapingLinks[i].Contains("movie-review") && !tempScrapingLinks[i].Contains("/movies/reviews/"))
                        if (tempScrapingLinks[i].StartsWith("http://www.glamsham.com/movies/reviews/"))
                            site.links2Scrape.Add(tempScrapingLinks[i]);
                        else
                            site.links2Scrape.Add(tempScrapingLinks[i] = "http://www.glamsham.com/movies/reviews/" + tempScrapingLinks[i]);

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add("");
                }
            else if (site.ReviewSiteURL.ToString().StartsWith("http://www.hindustantimes.com/Entertainment/EntertainmentSectionPage-Reviews"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (tempScrapingLinks[i].Contains("review") && tempScrapingLinks[i].StartsWith("http://www.hindustantimes.com/Entertainment/Reviews/"))
                        site.links2Scrape.Add(tempScrapingLinks[i]);

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add("");
                }
            else if (site.ReviewSiteURL.ToString().Equals("http://www.fridayrelease.com/movies/movie-reviews"))
                for (int i = 0; i < tempScrapingLinks.Count; i++)
                {
                    //add links
                    if (tempScrapingLinks[i].StartsWith("http://www.fridayrelease.com/movies/reviews/"))
                        site.links2Scrape.Add(tempScrapingLinks[i]);

                    //add title n hyperlinked text(inner text)
                    site.titleOfLinks2Scrape.Add("");
                    site.innerTextOfLinks2Scrape.Add("");
                }

            if (site.ReviewSiteURL.ToString().StartsWith("http://bollymoviereviewz.blogspot") || site.ReviewSiteURL.ToString().Equals("http://themoviereviewblog.wordpress.com/category/hindi-movie-reviews/"))
                site.links2Scrape.Remove(site.links2Scrape.ElementAt(site.links2Scrape.Count - 1));

            //dispBgWork.Text += "Done!\n";
            //dispBgWork.Text += "\tGathering Movie Names....";

            for (int i = 0; i < site.links2Scrape.Count; i++)
            {
                Uri temp = new Uri(site.links2Scrape[i]);
                if (site.ReviewSiteURL.ToString().Contains("wogma"))
                {
                    site.movieNames.Add(temp.LocalPath.Replace("/movie/", "").Replace('-', ' ').Replace("review", "").Replace("/", "").Trim() /*+ "\n\tDescription: " + site.titleOfLinks2Scrape[i]*/);
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().StartsWith("http://bollymoviereviewz.blogspot"))
                {
                    site.movieNames.Add(temp.LocalPath.Replace(".html", "").Remove(0, 9).Replace("movie-review", "").Replace('-', ' ').Replace("hindi", "").Replace("saif", "").Replace("review", "").Replace("rajeev", "").Replace("taran", "").Replace("rediff", "").Replace("salman", "").Replace("khan", "").Replace("shahrukh", "").Replace("kareena", "").Replace("hrithik", "").Replace("roshan", "").Replace("ratings", "").Trim());
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().Equals("http://themoviereviewblog.wordpress.com/category/hindi-movie-reviews/"))
                {
                    site.movieNames.Add(temp.LocalPath.Remove(0, 12).Replace('/', ' ').Replace('-', ' ').Replace("vishal ko", "").Trim());
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().Equals("http://www.fridaynirvana.com/film/"))
                {
                    site.movieNames.Add(temp.LocalPath.Remove(0, 14).Replace(".html", "").Replace("movie-review", "").Replace('-', ' ').Trim());
                    site.movieNames[i] = site.movieNames[i].Equals("3 idiots 2") ? "3 idiots" : site.movieNames[i];
                    site.movieNames[i] = site.movieNames[i].Equals("tell no one 2") ? "tell no one" : site.movieNames[i];
                    site.movieNames[i] = site.movieNames[i].Equals("moon 2") ? "moon" : site.movieNames[i];
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().StartsWith("http://www.koimoi.com/category/reviews/"))
                {
                    site.movieNames.Add(temp.LocalPath.Remove(0, 9).Replace('-', ' ').Replace("review", "").Replace('/', ' ').Trim());
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().StartsWith("http://www.reviewgang.com/"))
                {
                    site.movieNames.Add(temp.LocalPath.Remove(0, 11).Replace("user_reviews", "").Replace('/', ' ').Replace("movie-review", "").Replace('-', ' ').Trim());
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().Equals("http://www.glamsham.com/movies/reviews/"))
                {
                    site.movieNames.Add(temp.LocalPath.Remove(0, 16).Trim());
                    site.movieNames[i] = site.movieNames[i].Remove(site.movieNames[i].IndexOf("movie-review")).Replace('-', ' ').Trim();
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().StartsWith("http://www.hindustantimes.com/Entertainment/EntertainmentSectionPage-Reviews"))
                {
                    site.movieNames.Add(temp.LocalPath.Remove(0, 23));
                    site.movieNames[i] = site.movieNames[i].Remove(site.movieNames[i].IndexOf('/')).Replace('-', ' ').Replace(" s ", "").Replace("Anupama", "").Replace("Chopra", "").Replace("Rashid", "").Replace("Irani", "").Replace("review", "").Replace("Sarit Ray", "").Trim();
                    //Console.WriteLine(site.movieNames[i]);
                }
                else if (site.ReviewSiteURL.ToString().Equals("http://www.fridayrelease.com/movies/movie-reviews"))
                {
                    site.movieNames.Add(temp.LocalPath.Remove(0, 16));
                    site.movieNames[i] = site.movieNames[i].Remove(site.movieNames[i].IndexOf('/')).Replace('-', ' ').Trim();
                    //Console.WriteLine(site.movieNames[i]);
                }
                //Console.WriteLine(site.links2Scrape[i]);
            }
            //dispBgWork.Text += "Done!\n";
        }
        catch (Exception e)
        {
            //Console.WriteLine("An exception occured: \n\t"+e.Message+" \n.. please try again!");
            //Console.Write("");
        }
    }
}
