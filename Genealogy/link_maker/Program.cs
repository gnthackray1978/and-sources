using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace link_maker
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\george\Google Drive\gnthackraycouk\deeds");

            var files = di.GetFiles();

            //<a class="will" href="http://www.gnthackray.co.uk//Genealogy//Ralph1779//RalCopSur1.jpg"><u>Godmanchester 1779 Ralph Thackwray Conveyancing Document 1 </u></a>

            foreach (var _file in files)
            {
                Debug.WriteLine("<a class=\"will\" href=\"http://www.gnthackray.co.uk//Genealogy//deeds//" + _file.Name + "\"><u>Page 1 </u></a>");
            }


        }
    }
}
