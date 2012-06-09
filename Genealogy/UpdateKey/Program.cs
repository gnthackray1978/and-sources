using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateKey;
using System.Text.RegularExpressions;

namespace UpdateKey
{
    class Program
    {
        static void Main(string[] args)
        {
          

            //Regex regex = new Regex(@"\d\d\d\d");

            //MatchCollection mc = regex.Matches("xxc1884");

            //if (mc.Count > 0)
            //{
            //    Console.WriteLine(mc[0].Value);
            //}


            UpdateFolderInfo ufi = new UpdateFolderInfo();
            ufi.RootPath = @"C:\Users\george\Google Drive\familyhist\Images";

            ufi.Run();

            Console.ReadKey();

        }
    }
}
