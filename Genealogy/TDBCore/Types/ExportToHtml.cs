using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;

namespace TDBCore.Types
{
    public class ExportToHtml
    {
        string tempFile = "";
        string savePath = "";
        string sortColumn = "";
        List<string> columnsToIgnore = new List<string>();

        DataTable printTable = new DataTable();


        public ExportToHtml( string savePath)
        {

            this.savePath = savePath;

          //  this.printTable = printTable;

        }

        public ExportToHtml(DataTable printTable, string savePath)
        {

            this.savePath = savePath;

            this.printTable = printTable;

        }

        public List<string> ColumnsToIgnore
        {
            get
            {
                return this.columnsToIgnore;
            }
            set
            {
                this.columnsToIgnore = value;
            }
        }

        public string SortColumn
        {
            get
            {
                return this.sortColumn;
            }
            set
            {
                this.sortColumn = value;
            }
        }

        public string NewFile
        {
            get
            {
                return this.tempFile;
            }
        }


        private string ConvertToHtmlFileTabular(DataTable sentDataTable, List<string> columnsToIgnore)

        {

            DataView dv = new DataView(sentDataTable);

            dv.Sort = this.sortColumn ;

            // Check if the Sent DataTable is not empty or a Null

            if (sentDataTable == null)

            {

                throw new System.ArgumentNullException("sentDataTable");

            }

 

 

            //Get a worker object.

            StringBuilder myStringBuilder = new StringBuilder();

 

 

            //Open tags and write the top portion.

            myStringBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");

            myStringBuilder.Append("<head>");

            myStringBuilder.Append("<title>");

            myStringBuilder.Append("Page-");

            myStringBuilder.Append(Guid.NewGuid().ToString());

            myStringBuilder.Append("</title>");

            myStringBuilder.Append("</head>");

            myStringBuilder.Append("<body>");





            myStringBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");

            myStringBuilder.Append("style='border: solid 1px Silver; font-size: xx-small;'>");







            //Add the headings row.



            myStringBuilder.Append("<tr align='left' valign='top'>");



            foreach (DataColumn myColumn in sentDataTable.Columns)
            {

                if (!columnsToIgnore.Contains(myColumn.ColumnName))
                {
                    myStringBuilder.Append("<td align='left' valign='top'>");

                    myStringBuilder.Append(myColumn.ColumnName);

                    myStringBuilder.Append("</td>");
                }

            }



            myStringBuilder.Append("</tr>");



            //Add the data rows.


            IEnumerator viewCounter = dv.GetEnumerator();
            DataRowView myRow;

            while (viewCounter.MoveNext())
            {
                myRow = (DataRowView)viewCounter.Current;


                myStringBuilder.Append("<tr align='left' valign='top'>");



                foreach (DataColumn myColumn in sentDataTable.Columns)
                {
                    if (!columnsToIgnore.Contains(myColumn.ColumnName))
                    {
                        myStringBuilder.Append("<td align='left' valign='top'>");

                        myStringBuilder.Append(myRow[myColumn.ColumnName].ToString());

                        myStringBuilder.Append("</td>");
                    }

                }



                myStringBuilder.Append("</tr>");

            }



            //Close tags.

            myStringBuilder.Append("</table>");

            myStringBuilder.Append("</body>");

            myStringBuilder.Append("</html>");

 

            return myStringBuilder.ToString();

        }



        private string ConvertToHtmlFileNoteForm(DataTable sentDataTable, List<string> columnsToIgnore)
        {

            DataView dv = new DataView(sentDataTable);

            dv.Sort = this.sortColumn;

            // Check if the Sent DataTable is not empty or a Null

            if (sentDataTable == null)
            {

                throw new System.ArgumentNullException("sentDataTable");

            }

          

            
            StringBuilder myStringBuilder = new StringBuilder();


                        //Open tags and write the top portion.

            myStringBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");

            myStringBuilder.Append("<head>");

            myStringBuilder.Append("<title>");

            myStringBuilder.Append("Page-");

            myStringBuilder.Append(Guid.NewGuid().ToString());

            myStringBuilder.Append("</title>");

            myStringBuilder.Append("</head>");

            myStringBuilder.Append("<body>");

            
            //Get a worker object.


            IEnumerator viewCounter = dv.GetEnumerator();
            DataRowView myRow;

            if (sentDataTable.TableName == "Persons")
            {
                while (viewCounter.MoveNext())
                {
                    myRow = (DataRowView)viewCounter.Current;

                    myStringBuilder.Append("<BR>");
                    myStringBuilder.Append("Baptism");
                    myStringBuilder.Append("<BR>");
                    myStringBuilder.Append(myRow["BaptismDateStr"].ToString());

                    if (!myRow["BirthDateStr"].ToString().ToLower().Contains("abt"))
                    {
                        myStringBuilder.Append("<BR>");
                        myStringBuilder.Append(myRow["BirthDateStr"].ToString());
                    }
                    
                    myStringBuilder.Append("<BR>");
                    myStringBuilder.Append(myRow["ChristianName"].ToString());
                  //  myStringBuilder.Append(myRow["Surname"].ToString());

                    
                    if(Convert.ToBoolean( myRow["IsMale"]))
                    {
                        myStringBuilder.Append(" son of ");
                    }
                    else
                    {
                        myStringBuilder.Append(" daughter of ");
                    }
                    

                    myStringBuilder.Append(myRow["FatherChristianName"].ToString());
                    myStringBuilder.Append(" ");
                    myStringBuilder.Append(myRow["FatherSurname"].ToString());

                    if(myRow["MotherChristianName"].ToString() != "" )
                    {
                        myStringBuilder.Append(" and ");
                        myStringBuilder.Append(myRow["MotherChristianName"].ToString());
                    }
                    
                    if(myRow["MotherSurname"].ToString() != "" )
                    {
                        myStringBuilder.Append(" ");
                        myStringBuilder.Append(myRow["MotherSurname"].ToString());
                    }
                    myStringBuilder.Append("<BR>");
                    myStringBuilder.Append(myRow["BirthLocation"].ToString());



                    myStringBuilder.Append("<BR>");
                    
                }


            }



            if (sentDataTable.TableName == "Marriages")
            {

                myStringBuilder.AppendLine(sentDataTable.Rows[0]["MarriageLocation"].ToString());
                myStringBuilder.Append("<BR>");

                while (viewCounter.MoveNext())
                {
                    myRow = (DataRowView)viewCounter.Current;

                    myStringBuilder.Append("Marriage");
                    myStringBuilder.Append("<BR>");
                    myStringBuilder.Append(myRow["Date"].ToString());
                    myStringBuilder.Append("<BR>");
                    myStringBuilder.Append(myRow["MaleCName"].ToString());
                    myStringBuilder.Append(" ");
                    myStringBuilder.Append(myRow["MaleSName"].ToString());

                    if(myRow["MaleLocation"].ToString() != "")
                    {
                        myStringBuilder.Append(" of ");
                        myStringBuilder.Append(myRow["MaleLocation"].ToString());
                    }
                    
                    myStringBuilder.Append(" and ");

                    myStringBuilder.Append(myRow["FemaleCName"].ToString());
                    myStringBuilder.Append(" ");
                    myStringBuilder.Append(myRow["FemaleSName"].ToString());

                    if(myRow["FemaleLocation"].ToString() != "")
                    {
                        myStringBuilder.Append(" of ");
                        myStringBuilder.Append(myRow["FemaleLocation"].ToString());
                    }

                    if (myRow["Witness1"].ToString() != "")
                    {
                        myStringBuilder.Append("Wit: ");
                        myStringBuilder.Append(myRow["Witness1"].ToString());
                    }
                    

                    if (myRow["Witness2"].ToString() != "")
                    {
                        myStringBuilder.Append(",");
                        myStringBuilder.Append(myRow["Witness2"].ToString());
                    }

                    if (myRow["Witness3"].ToString() != "")
                    {
                        myStringBuilder.Append(",");
                        myStringBuilder.Append(myRow["Witness3"].ToString());
                    }
                    if (myRow["Witness4"].ToString() != "")
                    {

                        myStringBuilder.Append(",");
                        myStringBuilder.Append(myRow["Witness4"].ToString());
                    }


                    myStringBuilder.Append("<BR>");

                }

            }











            myStringBuilder.Append("</body>");

            myStringBuilder.Append("</html>");



            return myStringBuilder.ToString();

        }



        public string LoadStandardTabular()
        {
            string docName = DateTime.Now.Ticks.ToString() + ".html";

            tempFile = Path.Combine(savePath, docName);

            StreamWriter sw = new StreamWriter(tempFile);

            sw.Write(ConvertToHtmlFileTabular(printTable, columnsToIgnore));

            sw.Close();


            return docName;
           // System.Uri url = new Uri(tempFile);
            //webBrowser1.Url = url;
        }


        public string LoadStandardNotes()
        {
            string docName = DateTime.Now.Ticks.ToString() + ".html";

            tempFile = Path.Combine(savePath, docName);

            StreamWriter sw = new StreamWriter(tempFile);

            sw.Write(ConvertToHtmlFileNoteForm(printTable, columnsToIgnore));

            sw.Close();

            return docName;

           // System.Uri url = new Uri(tempFile);
          //  webBrowser1.Url = url;
        }

    }
}
