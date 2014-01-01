namespace TDBCore.Types.libs
{
    public class YearRange
    {

        int startYear = 0;

        public int StartYear
        {
            get { return startYear; }
            set { startYear = value; }
        }

        int endYear = 0;

        public int EndYear
        {
            get { return endYear; }
            set { endYear = value; }
        }

        public YearRange(int _startYear, int _endYear)
        {
            startYear = _startYear;
            endYear = _endYear;
        }

        public bool ContainsYearRange(int startYearRngToTest, int endYearRngToTest)
        {

            if ((startYearRngToTest >= this.startYear && endYearRngToTest <= this.endYear))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }
}