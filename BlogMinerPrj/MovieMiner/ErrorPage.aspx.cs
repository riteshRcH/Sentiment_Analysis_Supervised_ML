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
        StreamReader fin = new StreamReader(new FileStream("C:\\Users\\Ritesh\\Desktop\\errorMsg.txt", FileMode.Open, FileAccess.Read));
        dispErrorMsg.Text = fin.ReadToEnd();
        fin.Close();
        File.Delete("C:\\Users\\Ritesh\\Desktop\\errorMsg.txt");
    }
}
