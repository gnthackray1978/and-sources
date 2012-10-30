using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace UnitTests
{
    public class TestData
    {

        protected string GetStringByLen(int len)
        {

            StringBuilder sb = new StringBuilder();
            int idx = 0;
            while (idx < len)
            {
                sb.Append("x");
                idx++;
            }

            return sb.ToString();
        }

        public IEnumerable InvalidDates
        {
            get
            {

                List<string> InvalidDatesList = new List<string>(new string[] { "1 Jan xxxx", "xxxx", "x", "", "1", "Jan", "20000", "999" });
                foreach (string testCaseData in InvalidDatesList)
                {
                    yield return testCaseData;
                }
            }
        }

        public IEnumerable ValidDates
        {
            get
            {

                List<string> InvalidDatesList = new List<string>(new string[] { "1 Jan 1500", "1/1/1500", "1800", "abt 1800" });
                foreach (string testCaseData in InvalidDatesList)
                {
                    yield return testCaseData;
                }
            }
        }


    }

}
