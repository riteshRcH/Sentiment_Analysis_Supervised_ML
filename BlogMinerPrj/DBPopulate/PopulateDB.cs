using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;

namespace DBPopulate
{
    class PopulateDB
    {
        static StreamReader fin;
        static SqlCommand sqlCmd;
        static SqlConnection sqlConn;
        static SqlDataReader sqlDR;
        static string[] s;
        static int counter = 0, IDCounter = 0;
        //eyes = : , we have = instead          -->     in prefix, ignore and neutral category of files
        //wink = ; , we have # instead          -->     in prefix, ignore and neutral category of files


        //eyes = : , we have ^ instead          -->     in negative category of file
        //wink = ; , we have & instead          -->     in negative category of file


        //eyes = : , we have ~ instead          -->     in positive category of file
        //wink = ; , we have & instead          -->     in positive categroy of file

        static void Main(string[] args)
        {
            sqlConn = new SqlConnection("server=localhost\\SqlExpress; DataBase=WordDBForBlogMiner; integrated security=true");
            sqlConn.Open();

            fillDB("C:\\My Data\\BlogMinerPrj\\JWHennessey-phpInsight-afb0e53\\JWHennessey-phpInsight-afb0e53\\data\\data.prefix.txt", "prefix");
            fillDB("C:\\My Data\\BlogMinerPrj\\JWHennessey-phpInsight-afb0e53\\JWHennessey-phpInsight-afb0e53\\data\\data.ign.txt", "ignore");
            fillDB("C:\\My Data\\BlogMinerPrj\\JWHennessey-phpInsight-afb0e53\\JWHennessey-phpInsight-afb0e53\\data\\data.neg.txt", "negative");
            fillDB("C:\\My Data\\BlogMinerPrj\\JWHennessey-phpInsight-afb0e53\\JWHennessey-phpInsight-afb0e53\\data\\data.neu.txt", "neutral");
            fillDB("C:\\My Data\\BlogMinerPrj\\JWHennessey-phpInsight-afb0e53\\JWHennessey-phpInsight-afb0e53\\data\\data.pos.txt", "positive");
/***********************************************************data.neu***********************************************************************************************/

                /*fin = new StreamReader("C:\\My Data\\BlogMinerPrj\\JWHennessey-phpInsight-afb0e53\\JWHennessey-phpInsight-afb0e53\\data\\data.neu.txt");
                //sqlCmd = new SqlCommand("select * from KWTable", sqlConn);

                s = (fin.ReadToEnd().Replace("\n", "").Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries));
                counter = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].Equals("i") | s[i].Equals("s"))
                    {
                        counter = 0;
                        for (int j = i; j < s.Length - 1; j++)
                        {
                            s[j] = s[j + 1];
                            counter++;
                        }
                    }
                }

                for (int i = s.Length - counter + 1; i < s.Length; i++)
                    s[i] = null;

                foreach (string str in s)
                    if (str != null)
                        Console.WriteLine(str);

                fin.Close();*/

                /*float negScore = float.Parse(s[3]), posScore = float.Parse(s[2]);

                string query = "insert into KWTable values ('" + s[0] + "', " + i + ", " + posScore + ", " + negScore + ", '" + s[4] + "', "+( 1 - (posScore + negScore) )+")";
                sqlCmd = new SqlCommand(query, sqlConn);
                sqlCmd.ExecuteNonQuery();
                Console.WriteLine("Record successfully inserted");*/
            //}
            Console.Read();
        }
        static void fillDB(string pathToFile, string category)
        {
            fin = new StreamReader(pathToFile);
            //sqlCmd = new SqlCommand("select * from KWTable", sqlConn);

            //read, split ID, score and word .. remove 'i' and 's' by shifting and remove repititive words forming at end
            s = (fin.ReadToEnd().Replace("\n", "").Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals("i") | s[i].Equals("s"))
                {
                    counter = 0;
                    for (int j = i; j < s.Length - 1; j++)
                    {
                        s[j] = s[j + 1];
                        counter++;
                    }
                }
            }

            for (int i = s.Length - counter + 1; i < s.Length; i++)
                s[i] = null;

            //remove indices received from file by putting null and then shifiting
            for (int i = 0; i < s.Length; i += 3)
                s[i] = null;

            for (int i = 0; i < s.Length; i++)
                if (s[i] == null)
                    for (int j = i; j < s.Length - 1; j++)
                        s[j] = s[j + 1];

            //finding out actual length ignoring null --> to generate max ID for subgroup
            int len = 0;
            if (s.Length % 2 == 0)
                for (int i = 0; i < s.Length; i++)
                    if (s[i] != null)
                        ++len;

            //Display on Console after trimming and removing double quotes from all words
            //removing single quotes too .. because that would conflict while insert query execution
            bool toggle = false;
            string query = default(string);
            for(int i=0;i<s.Length;i++)
            {
                toggle = !toggle;
                if (s[i] != null)
                {
                    if (toggle)
                        Console.WriteLine("Record Num: " + (++IDCounter) + " has been successfully inserted");
                    //Console.Write(++IDCounter + "\t" + s[i] + "\t");
                    else
                    {

                        //replace = with :  and # with ; to get original smileys(replaced because ; and : act as delimiters while splitting)
                        if(category.Equals("neutral"))
                        {
                            if (s[i].IndexOf('=') != -1 || s[i].IndexOf('#')!=-1)
                                s[i] = s[i].Replace('=', ':').Replace('#', ';');
                            else{}
                        }
                        else if (category.Equals("negative"))
                        {
                            if (s[i].IndexOf('^') != -1)
                                s[i] = s[i].Replace('^', ':');
                            else { }
                        }
                        else if (category.Equals("positive"))
                        {
                            if (s[i].IndexOf('~') != -1 || s[i].IndexOf('&') != -1)
                                s[i] = s[i].Replace('~', ':').Replace('&', ';');
                            else { }
                        }

                        //remove double quotes and single quotes(as it would cause conflicts in insert query) from all words for all categories
                        if (s[i].IndexOf('\'') != -1) // => there is a occurance of single quote present inside word
                        {
                            query = "insert into KWTable values(" + IDCounter + ", " + Int64.Parse(s[i - 1]) + ", '" + category + "', '" + (s[i] = s[i].Trim().Remove(0, 1).Remove(s[i].Length - 2, 1).Remove(s[i].IndexOf('\'') - 1, 1)) + "', null, null, null, null, null)";
                            //Console.WriteLine((s[i] = s[i].Trim().Remove(0, 1).Remove(s[i].Length - 2, 1).Remove(s[i].IndexOf('\'') - 1, 1)));
                        }
                        else
                        {
                            query = "insert into KWTable values(" + IDCounter + ", " + Int64.Parse(s[i - 1]) + ", '" + category + "', '" + (s[i] = s[i].Trim().Remove(0, 1).Remove(s[i].Length - 2, 1)) + "', null, null, null, null, null)";
                            //Console.WriteLine((s[i] = s[i].Trim().Remove(0, 1).Remove(s[i].Length - 2, 1)));
                        }
                        sqlCmd = new SqlCommand(query, sqlConn);
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }

            //Close Streams and put all other used variables to default values
            fin.Close();
            fin = default(StreamReader);
            s = default(string[]);
            counter = default(int);
        }
    }
}
