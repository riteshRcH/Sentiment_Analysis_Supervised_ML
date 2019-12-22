using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SentiWordDBPopulate
{
    class DBPopulateSentiWord
    {
        static SqlCommand sqlCmd;
        static SqlConnection sqlConn;

        static void Main(string[] args)
        {
            try
            {
                sqlConn = new SqlConnection("server=localhost\\SqlExpress; DataBase=WordDBForBlogMiner; integrated security=true");
                sqlConn.Open();
                //sqlCmd = new SqlCommand("select * from KWTable", sqlConn);

                //skip 1st 13 lines
                //for (int i = 1; i <= 13; i++)
                //fin.ReadLine();

                string[] lines = File.ReadAllLines("C:\\Users\\Ritesh\\Desktop\\WordDB.txt");

                Console.WriteLine("Done reading! Press enter to insert everything into DB.");
                Console.Read();

                long IDCounter = 0;
                //enter next 117658 lines into DB
                for (int i = 13; i < lines.Length; i++)
                {
                    //string line =fin.ReadLine();
                    string[] s = (lines[i].Remove(lines[i].IndexOf('#')).Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries));
                    //s[5] = s[6] = null;
                    s[2] = s[2].Equals("NaN") ? "0" : s[2];
                    s[3] = s[3].Equals("NaN") ? "0" : s[3];
                    float tempNegScore = float.Parse(s[3]), tempPosScore = float.Parse(s[2]);
                    int negScore = (int)Math.Round((tempNegScore * 27), MidpointRounding.AwayFromZero);
                    int posScore = (int)Math.Round((tempPosScore * 31), MidpointRounding.AwayFromZero);

                    if (posScore <= 31 && negScore <= 27)
                    {
                        //Console.WriteLine((++IDCounter) + "\t => " + s[0] + "\t" + posScore + "\t" + negScore + "\t" + s[4]);
                        string category = "";
                        if (posScore.Equals(0) && negScore.Equals(0))
                            category = "neutral";
                        else if (posScore > 0 && negScore.Equals(0))
                            category = "positive";
                        else if (posScore.Equals(0) && negScore > 0)
                            category = "negative";
                        else if (posScore > 0 && negScore > 0)
                            category = "PosNeg";

                        string query = "insert into SentiWordKWTable values ('" + s[0] + "', " + (++IDCounter) + ", " + posScore + ", " + negScore + ", '" + s[4].Replace("'", "") + "', '" + category + "')";
                        sqlCmd = new SqlCommand(query, sqlConn);
                        sqlCmd.ExecuteNonQuery();
                        Console.WriteLine("Record Num: " + IDCounter + " successfully inserted!");
                    }
                    else
                    {
                        Console.WriteLine((++IDCounter) + "\t => " + s[0] + "\t" + posScore + "\t" + negScore + "\t" + s[4]);
                        break;
                    }

                    tempNegScore = tempPosScore = 0;
                    posScore = negScore = 0;
                }
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured => " + e.Message);
                sqlCmd = new SqlCommand("delete from SentiWordKWTable", sqlConn);
                sqlCmd.ExecuteNonQuery();
                Console.WriteLine("all rows of table deleted");
            }
            Console.Read();
        }
    }
}
