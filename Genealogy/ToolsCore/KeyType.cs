using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ToolsCore
{

    public class FolderDescriptor
    {
        public string Date { get; set; }
        public string FullName { get; set; }
        public string Location { get; set; }
        public string LocationCounty { get; set; }
        public string Type { get; set; }
        public string MiscInner { get; set; }
        public string MiscOuter { get; set; }

        public string LowerDateRange()
        {
            return "1 Jan " + Date;
        }


        public string UpperDateRange()
        {                
            return  "31 Dec " + Date;
        }
    }


    public class KeyType
    {



        public Guid SourceId { get; set; }
        public string SourceRef { get; set; }
        public string SourceDateFrom { get; set; }
        public string SourceDateTo { get; set; }
        public string SourceType { get; set; }
        public string Location { get; set; }
        public string County { get; set; }

        public string SubjectChristianName { get; set; }
        public string SubjectSurname { get; set; }
        public string SubjectOccupation { get; set; }

        public string SubjectRelation { get; set; }
        public string OthersideChristianName { get; set; }
        public string OthersideSurname { get; set; }
        public string OthersideOccupation { get; set; }
        public string PhysicalPath { get; set; }


        public int Date
        {
            get
            {
                return this.SourceDateTo.ToInt();
            }

        }

        public int DateFrom
        {
            get
            {
                return this.SourceDateFrom.ToInt();
            }

        }

        public string WriteString()
        {
            return SourceId.ToString() + "," + SourceRef + "," + SourceType + "," + PhysicalPath + "," + SourceDateFrom + ","
                + SourceDateTo + "," + Location + "," + County + ","
                + SubjectChristianName + "," + SubjectSurname + "," + SubjectOccupation + "," + SubjectRelation +
                "," + OthersideChristianName + "," + OthersideSurname + "," + OthersideOccupation + "," + PhysicalPath;
        }

        public static string WriteHeaderString()
        {
            return "SourceId,sourceRef,Type,PhysicalPath,From,To,Location,County,SubjectChristianName,SubjectSurname,SubjectOccupation" +
                ",SubjectRelation,OthersideChristianName,OthersideSurname,OthersideOccupation,PhysicalPath";
        }
    }


    public class SourceFileReference
    {
        public FileInfo SourceFileInfo { get; set; }
        public Guid FileId { get; set; }
    }
}
