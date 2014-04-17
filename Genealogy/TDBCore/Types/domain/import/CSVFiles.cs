using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TDBCore.Types.domain.import
{
    public class CSVFiles
    {
        public static readonly IList<CSVField> MarriageFieldList = new ReadOnlyCollection<CSVField>(new [] { CSVField.MaleCName, CSVField.MaleSName, CSVField.MaleLocation, CSVField.MaleInfo,
            CSVField.FemaleCName, CSVField.FemaleSName, CSVField.FemaleLocation, CSVField.FemaleInfo, CSVField.Date, CSVField.MarriageLocation, 
            CSVField.YearIntVal, CSVField.MarriageCounty, CSVField.Source, 
            CSVField.Witness1CName, CSVField.Witness1SName, CSVField.Witness1Desc, 
            CSVField.Witness2CName, CSVField.Witness2SName, CSVField.Witness2Desc, 
            CSVField.Witness3CName, CSVField.Witness3SName, CSVField.Witness3Desc, 
            CSVField.Witness4CName, CSVField.Witness4SName, CSVField.Witness4Desc, 
            CSVField.Witness5CName, CSVField.Witness5SName, CSVField.Witness5Desc, 
            CSVField.Witness6CName, CSVField.Witness6SName, CSVField.Witness6Desc, 
            CSVField.OrigMaleSurname, CSVField.OrigFemaleSurname, 
            CSVField.MaleOccupation, CSVField.FemaleOccupation, CSVField.FemaleIsKnownWidow, CSVField.MaleIsKnownWidower, CSVField.IsBanns,
            CSVField.IsLic,CSVField.SourceId,CSVField.MaleAge,CSVField.FemaleAge,
            CSVField.FemaleFatherCName,CSVField.FemaleFatherSName,
            CSVField.MaleFatherCName,CSVField.MaleFatherSName,
            CSVField.FemaleFatherOccupation,CSVField.MaleFatherOccupation , CSVField.LocationId });

        public static readonly IList<CSVField> BDFieldList = new ReadOnlyCollection<CSVField>(new[] { CSVField.IsMale, CSVField.ChristianName, CSVField.Surname, CSVField.BirthLocation, 
            CSVField.BirthDateStr, CSVField.BaptismDateStr, CSVField.DeathDateStr, CSVField.DeathLocation, CSVField.FatherChristianName, CSVField.FatherSurname,
            CSVField.MotherChristianName, CSVField.MotherSurname, CSVField.Notes, CSVField.Source, CSVField.BirthInt, CSVField.BapInt , CSVField.DeathInt , CSVField.DeathCounty , CSVField.BirthCounty , CSVField.Occupation , CSVField.ReferenceLocation , CSVField.ReferenceDateStr , 
            CSVField.ReferenceDateInt , CSVField.SpouseName , CSVField.SpouseSurname , CSVField.FatherOccupation , CSVField.AgeYear ,
            CSVField.AgeMonth , CSVField.AgeDay , CSVField.AgeWeek , CSVField.Notes2 , CSVField.SourceId , CSVField.LocationId , CSVField.DeathLocationId,CSVField.PersonId });

        public static readonly IList<CSVField> SourceFieldList = new ReadOnlyCollection<CSVField>(new[] {
            CSVField.SourceDesc,  CSVField.SourceOrigLocat,CSVField.IsCopyHeld,CSVField.IsViewed,
            CSVField.IsThackrayFound,CSVField.SourceDate,CSVField.SourceDateTo,CSVField.SourceRef,CSVField.Notes,CSVField.SourceParish,CSVField.SourceType,CSVField.SourceId
        });

        public CSVFiles()
        {
        }
    }
}