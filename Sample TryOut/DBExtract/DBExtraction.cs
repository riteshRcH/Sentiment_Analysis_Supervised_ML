using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DBExtract
{
    class DBExtraction
    {
        //static StreamReader fin;
        static SqlCommand sqlCmd;
        static SqlConnection sqlConn;
        static SqlDataReader sqlDR;
        static DataSet ds = new DataSet();
        static List<String> words = new List<String>();

        static string stmt = "deflagrate fog up put out fog up";

        static List<int> IDList = new List<int>();
        static List<String> categoryList = new List<String>();
        static List<String> termList = new List<String>();
        static List<int> scoreList = new List<int>();
        static List<int> negScoreListSenti = new List<int>();
        static List<int> posScoreListSenti = new List<int>();

        static void Main(string[] args)
        {
            words.Add("deflagrate");
            words.Add("fog");
            words.Add("up");
            words.Add("put");
            words.Add("out");
            //fin = new StreamReader("C:\\Documents and Settings\\Pranali\\Desktop\\WordDB.txt");
            sqlConn = new SqlConnection("server=localhost\\SqlExpress; DataBase=WordDBForBlogMiner; integrated security=true");
            sqlConn.Open();

            sqlCmd = new SqlCommand("select * from KWTable where category='positive' or category='negative' order by Len(Synset_term) desc", sqlConn);

            //SentiWord if (!((sqlDR[4].ToString()[i] >= (int)'A' && sqlDR[4].ToString()[i] <= (int)'Z') || (sqlDR[4].ToString()[i] >= (int)'a' && sqlDR[4].ToString()[i] <= (int)'z') || (sqlDR[4].ToString()[i] >= (int)'0' && sqlDR[4].ToString()[i] <= (int)'9') || (sqlDR[4].ToString()[i] == '_') || (sqlDR[4].ToString()[i] == '-') || (sqlDR[4].ToString()[i]=='.')))
            //KWTable if (!((termList[k].ToString()[i] >= (int)'A' && termList[k].ToString()[i] <= (int)'Z') || (termList[k].ToString()[i] >= (int)'a' && termList[k].ToString()[i] <= (int)'z') || (termList[k].ToString()[i] >= (int)'0' && termList[k].ToString()[i] <= (int)'9') || (termList[k].ToString()[i] == '-')))

            sqlDR = sqlCmd.ExecuteReader();
            while (sqlDR.Read())
            {
                IDList.Add(Int32.Parse(sqlDR["ID"].ToString()));
                scoreList.Add(Int32.Parse(sqlDR["Score"].ToString()));
                //negScoreListSenti.Add(Int32.Parse(sqlDR["NegScore"].ToString()));
                //posScoreListSenti.Add(Int32.Parse(sqlDR["PosScore"].ToString()));
                termList.Add(sqlDR["Synset_term"].ToString());
                categoryList.Add(sqlDR["Category"].ToString());
            }

            for (int k = 0; k < IDList.Count; k++)
            {
                if (termList[k].StartsWith("not"))
                    termList[k] = termList[k].Replace("not", "not ").Replace("very", "very ");
                for (int i = 0; i < termList[k].Length; i++)
                {
                    //if (!((termList[k].ToString()[i] >= (int)'A' && termList[k].ToString()[i] <= (int)'Z') || (termList[k].ToString()[i] >= (int)'a' && termList[k].ToString()[i] <= (int)'z') || (termList[k].ToString()[i] >= (int)'0' && termList[k].ToString()[i] <= (int)'9')))
                    if (!((termList[k].ToString()[i] >= (int)'A' && termList[k].ToString()[i] <= (int)'Z') || (termList[k].ToString()[i] >= (int)'a' && termList[k].ToString()[i] <= (int)'z') || (termList[k].ToString()[i] >= (int)'0' && termList[k].ToString()[i] <= (int)'9')))
                    {
                        //Console.WriteLine(sqlDR[0] + "\t" + sqlDR[1] + "\t" + sqlDR[2] + "\t" + termList[k].ToString().Replace('-', ' ').Replace("not", "not ").Replace("very", "very "));
                        termList[k] = termList[k].ToString().Replace('_', ' ').Replace('-', ' ').Replace('.', ' ');
                        break;
                    }
                }
            }

            foreach (string s in termList)
                Console.WriteLine(s);

            /*foreach (String term in termList)
            {
                String[] arr = term.Split(' ');
                if (arr.Length > 1)
                {
                    do
                    {
                        int index = words.FindIndex(wordParam => wordParam.Equals(arr[0]));
                        if (index.Equals(-1))
                            break;
                        else
                        {
                            Console.Write(arr[0] + " found in words list at index: " + index + " for word as~~~~~~~~~~~");
                            if (index + arr.Length < words.Count)
                            {
                                bool hasAllWords = true;
                                for (int i = 1; i < arr.Length; i++)
                                {
                                    if (!(words[index + i].Equals(arr[i])))
                                    {
                                        hasAllWords = false;
                                        break;
                                    }
                                }
                                if (hasAllWords)
                                {
                                    for (int i = 1; i < arr.Length; i++)
                                    {
                                        String temp = words[index + i];
                                        words.RemoveAt(index + i);
                                        words[index] += temp + ' ';
                                    }
                                    break;
                                }
                            }
                            else
                                break;
                        }

                    } while (true);
                }
                //else
                    //Console.WriteLine(arr[0]);
            }
              ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            for(int i=0;i<words.Count;i++)
            {
                foreach (String term in termList)
                {
                    String[] arr = term.Split(' ');
                    if (arr[0].Equals(words[i]))
                    {
                        if (arr.Length > 1)
                        { 
                            bool hasAllParts = true;
                            for(int b=1;b<arr.Length;b++)
                            {
                                if (!(arr[i + b].Equals(arr[b])))
                                {
                                    hasAllParts = false;
                                    break;
                                }
                            }
                            if (hasAllParts)
                            {
                                words[i] = "";
                                for (int y = 0; y < arr.Length; y++)
                                    words[i] += arr[y] + " ";
                                words[i].Trim();
                                for (int h = i; h < arr.Length; h++)
                                    words.RemoveAt(h);
                            }
                            else
                                continue;
                        }
                    }
                }
            }
            foreach (String s in words)
                Console.WriteLine(s);*/

            //int posScore = 0, negScore = 0, posWordCnt = 0, negWordCnt = 0;
            //for (int i = 0; i < termList.Count; i++)
                //if (stmt.IndexOf(termList[i].ToString()) != -1)
                    //stmt = stmt.Replace(termList[i].ToString(), categoryList[i].ToString().Equals("positive") ? "`~" + posScoreListSenti[i].ToString() + "~`" : "~" + negScoreListSenti[i].ToString() + "~");

                    /*if (categoryList[i].ToString().Equals("positive"))
                    {
                        posScore += Int32.Parse(posScoreListSenti[i].ToString());
                        ++posWordCnt;
                        char[] arr = stmt.ToCharArray();
                        for (int k = stmt.IndexOf(termList[i].ToString()); i <= (stmt.IndexOf(termList[i].ToString()) + termList[i].ToString().Length); i++)
                            arr[i] = ' ';
                        stmt = new String(arr);
                    }
                    else
                    {
                        negScore += Int32.Parse(negScoreListSenti[i].ToString());
                        ++negWordCnt;
                        char[] arr = stmt.ToCharArray();
                        for (int k = stmt.IndexOf(termList[i].ToString()); i <= (stmt.IndexOf(termList[i].ToString()) + termList[i].ToString().Length); i++)
                            arr[i] = ' ';
                        stmt = new String(arr);
                        //for (int k = stmt.IndexOf(termList[i].ToString()); i <= (stmt.IndexOf(termList[i].ToString()) + termList[i].ToString().Length); i++)
                            //stmt[i] = ' ';
                        //stmt = stmt.Replace(termList[i], "negative: " + negScoreListSenti[i].ToString() + "\t").Trim();
                    }*/

                //if (stmt.Contains(termList[i]))
                //{
                  //  Console.WriteLine(termList[i] + " found in stmt having score as " + (categoryList[i] == "positive" ? posScoreListSenti[i] : negScoreListSenti[i]));
//                    stmt.Replace(termList[i], "haha");
  //              }

            Console.WriteLine(stmt);
            //Console.WriteLine(posWordCnt+"\t"+posScore+"\t"+negScore+"\t"+negWordCnt);
            Console.Read();
        }
    }
}
/*SqlCommand sqlUpdateCmd = new SqlCommand("update KWTable SET Synset_term='" + termList[k].ToString().Replace('-', ' ').Replace("not", "not ").Replace("very", "very ") + "' where ID = " + Int32.Parse(sqlDR[0]) + " and Score = " + Int32.Parse(sqlDR[1]) + " and Category = '" + sqlDR[2] + "'", sqlConn);
                        sqlUpdateCmd.ExecuteNonQuery();*/