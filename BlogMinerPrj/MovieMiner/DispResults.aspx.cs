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
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //StreamReader fin = new StreamReader(new FileStream("C:\\Users\\Ritesh\\Desktop\\dispMovieRatings.txt", FileMode.Open, FileAccess.Read));
        String[] allLines = File.ReadAllLines("C:\\Users\\Ritesh\\Desktop\\dispMovieRatings.txt");
        string[] movieNames = new string[allLines.Length];
        double[] movieRatings= new double[allLines.Length];
        for(int i=0;i<allLines.Length;i++)
        {
            //string[] temp = allLines[i].Split(new char[]{'~'},StringSplitOptions.RemoveEmptyEntries);
            string[] temp = allLines[i].Split('~');
            movieNames[i] = temp[0];
            movieRatings[i] = Double.Parse(temp[1]);
        }
        var dt = new DataTable();
        dt.Columns.Add(new DataColumn("movieName", typeof(string)));
        dt.Columns.Add(new DataColumn("Rating", typeof(double)));
        for (int i = 0; i < movieNames.Length; i++)
        {
            var dr = dt.NewRow();
            dr[0] = movieNames[i];
            dr[1] = movieRatings[i];
            dt.Rows.Add(dr);
        }
        Chart1.DataSource = dt;
        Chart1.DataBind();
        //Chart1.DataBindTable(dt);
    }
}