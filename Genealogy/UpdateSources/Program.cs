using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateSources
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateSources ufi = new UpdateSources();
            ufi.RootPath = @"C:\Users\george\Google Drive\familyhist\Images";

            ufi.Run();

            Console.ReadKey();
        }
    }
}
