using System;

namespace TDBCore.Types.libs
{
    public class SimpleTimer
    {
        private long StartTime = 0;
       

        public void StartTimer()
        {
            StartTime = DateTime.Now.Ticks;
        }

        public string EndTimer(string note)
        {
            //   TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks - StartTime);
            long retVal = (DateTime.Now.Ticks - StartTime) / 10000;

            return note + " " + retVal.ToString() + "ms";

            //   Debug.WriteLine( note + " " + elapsedSpan.Milliseconds.ToString());
        }
    }
}