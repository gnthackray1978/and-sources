using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types
{
    public class MarriageResult
    {
        private int marriageYear;

        private int marriageTotalEvents;

        private string maleCName;

        private string maleSName;

        private string femaleCName;

        private string femaleSName;

        private string marriageLocation;

        private string witnesses= "";

        private Guid marriageGuid;

        private string marriageSource;

        private Guid uniqueRefStr;

        private Guid uniqueRef;

        public void ReadTest(string _param)
        { 
            
        }

        public Guid UniqueRef
        {
            get 
            {
      
                    return uniqueRef;
              

            }
            set { uniqueRef = value; }
        }

        public Guid UniqueRefStr
        {
            get { return uniqueRefStr; }
            set { uniqueRefStr = value; }
        }
        
        public string MarriageSource
        {
            get { return marriageSource; }
            set { marriageSource = value; }
        }
        
        public Guid MarriageId
        {
            get { return marriageGuid; }
            set { marriageGuid = value; }
        }
        
        public string Witnesses
        {
            get { return witnesses; }
            set { witnesses = value; }
        }
        
        public string MarriageLocation
        {
            get { return marriageLocation; }
            set { marriageLocation = value; }
        }
        
        public string FemaleSName
        {
            get { return femaleSName; }
            set { femaleSName = value; }
        }
        
        public string FemaleCName
        {
            get { return femaleCName; }
            set { femaleCName = value; }
        }
       
        public string MaleSName
        {
            get { return maleSName; }
            set { maleSName = value; }
        }

        public string MaleCName
        {
            get { return maleCName; }
            set { maleCName = value; }
        }
        
        public int MarriageTotalEvents
        {
            get { return marriageTotalEvents; }
            set { marriageTotalEvents = value; }
        }
        
        public int MarriageYear
        {
            get { return marriageYear; }
            set { marriageYear = value; }
        }

        public MarriageResult()
        { 
        
        }



    }
}
