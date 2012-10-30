using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.BLL;

namespace TDBCore.EntityModel
{
    public static class MarriageExtensions
    {


        public static void MergeInto(this Marriage _marriage, Marriage newMarriage)
        {

            Guid dummyGuid = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");

            if(_marriage.FemaleId.GetValueOrDefault() == Guid.Empty 
                && newMarriage.FemaleId.GetValueOrDefault() == Guid.Empty)
                _marriage.FemaleId = Guid.Empty;

            if (_marriage.MaleId.GetValueOrDefault() == Guid.Empty
                && newMarriage.MaleId.GetValueOrDefault() == Guid.Empty)
                _marriage.MaleId = Guid.Empty;

            if (_marriage.Date == "" && newMarriage.Date != "")
                _marriage.Date = newMarriage.Date;

            if(_marriage.FemaleBirthYear == 0 && newMarriage.FemaleBirthYear >0)
                _marriage.FemaleBirthYear = newMarriage.FemaleBirthYear;
          
            if (_marriage.YearIntVal == 0 && newMarriage.YearIntVal > 0)
                _marriage.YearIntVal = newMarriage.YearIntVal;

            if (_marriage.MaleBirthYear == 0 && newMarriage.MaleBirthYear > 0)
                _marriage.MaleBirthYear = newMarriage.MaleBirthYear;


            if (_marriage.FemaleCName == "" && newMarriage.FemaleCName != "")
                _marriage.FemaleCName = newMarriage.FemaleCName;

            if (_marriage.FemaleInfo == "" && newMarriage.FemaleInfo != "")
                _marriage.FemaleInfo = newMarriage.FemaleInfo;

            if ((_marriage.MaleLocationId.GetValueOrDefault() == dummyGuid || _marriage.MaleLocationId.GetValueOrDefault() == Guid.Empty)
                && (newMarriage.MaleLocationId.GetValueOrDefault() != dummyGuid && newMarriage.MaleLocationId.GetValueOrDefault() != Guid.Empty))
                _marriage.MaleLocationId = newMarriage.MaleLocationId;

            if ((_marriage.MarriageLocationId.GetValueOrDefault() == dummyGuid || _marriage.MarriageLocationId.GetValueOrDefault() == Guid.Empty)
                && (newMarriage.MarriageLocationId.GetValueOrDefault() != dummyGuid && newMarriage.MarriageLocationId.GetValueOrDefault() != Guid.Empty))
                _marriage.MarriageLocationId = newMarriage.MarriageLocationId;

            if ((_marriage.FemaleLocationId.GetValueOrDefault() == dummyGuid || _marriage.FemaleLocationId.GetValueOrDefault() == Guid.Empty)
                && (newMarriage.FemaleLocationId.GetValueOrDefault() != dummyGuid && newMarriage.FemaleLocationId.GetValueOrDefault() != Guid.Empty))
                _marriage.FemaleLocationId = newMarriage.FemaleLocationId;

            if (newMarriage.IsBanns.GetValueOrDefault())
                _marriage.IsBanns = newMarriage.IsBanns;

            if (newMarriage.IsLicence.GetValueOrDefault())
                _marriage.IsLicence = newMarriage.IsLicence;

            if (newMarriage.FemaleIsKnownWidow.GetValueOrDefault())
                _marriage.FemaleIsKnownWidow = newMarriage.FemaleIsKnownWidow;

            if (newMarriage.MaleIsKnownWidower.GetValueOrDefault())
                _marriage.MaleIsKnownWidower = newMarriage.MaleIsKnownWidower;

            if (_marriage.FemaleOccupation == "" && newMarriage.FemaleOccupation != "")
                _marriage.FemaleOccupation = newMarriage.FemaleOccupation;

            if (_marriage.FemaleSName == "" && newMarriage.FemaleSName != "")
                _marriage.FemaleSName = newMarriage.FemaleSName;

            if (_marriage.MaleCName == "" && newMarriage.MaleCName != "")
                _marriage.MaleCName = newMarriage.MaleCName;

            if (_marriage.MaleInfo == "" && newMarriage.MaleInfo != "")
                _marriage.MaleInfo = newMarriage.MaleInfo;

            if (_marriage.FemaleLocation == "" && newMarriage.FemaleLocation != "")
                _marriage.FemaleLocation = newMarriage.FemaleLocation;

            if (_marriage.MaleLocation == "" && newMarriage.MaleLocation != "")
                _marriage.MaleLocation = newMarriage.MaleLocation;

            if (_marriage.MaleOccupation == "" && newMarriage.MaleOccupation != "")
                _marriage.MaleOccupation = newMarriage.MaleOccupation;

            if (_marriage.MaleSName == "" && newMarriage.MaleSName != "")
                _marriage.MaleSName = newMarriage.MaleSName;

            if (_marriage.MarriageCounty == "" && newMarriage.MarriageCounty != "")
                _marriage.MarriageCounty = newMarriage.MarriageCounty;

            if (_marriage.MarriageLocation == "" && newMarriage.MarriageLocation != "")
                _marriage.MarriageLocation = newMarriage.MarriageLocation;

            if (_marriage.OrigFemaleSurname == "" && newMarriage.OrigFemaleSurname != "")
                _marriage.OrigFemaleSurname = newMarriage.OrigFemaleSurname;

            if (_marriage.OrigMaleSurname == "" && newMarriage.OrigMaleSurname != "")
                _marriage.OrigMaleSurname = newMarriage.OrigMaleSurname;

            if (_marriage.Source == "" && newMarriage.Source != "")
                _marriage.Source = newMarriage.Source;




        }

        public static string ToGeneralDescription(this Marriage _marriage)
        {
            string description = _marriage.MaleCName + " " + _marriage.MaleSName;



            if (_marriage.MaleIsKnownWidower.Value) description = " Wid";

            if (_marriage.FemaleIsKnownWidow.Value)
                description += " " + _marriage.MaleBirthYear.Value.ToString() + " " + _marriage.MaleOccupation.Trim() + " married " + _marriage.FemaleCName + " " + _marriage.FemaleSName + " " + _marriage.FemaleBirthYear.Value.ToString() + " Wid " + _marriage.FemaleOccupation;
            else
                description += " " + _marriage.MaleBirthYear.Value.ToString() + " " + _marriage.MaleOccupation.Trim() + " married " + _marriage.FemaleCName + " " + _marriage.FemaleSName + " " + _marriage.FemaleBirthYear.Value.ToString() + " " + _marriage.FemaleOccupation;



            description = description.Replace("0", "");

            if (_marriage.MaleLocation != "")
                description += " groom of " + _marriage.MaleLocation;

            if (_marriage.FemaleLocation != "")
                description += " bride of " + _marriage.FemaleLocation;

            if (_marriage.IsLicence.Value)
                description += " lic ";

            MarriageWitnessesBLL marriageWitBll = new MarriageWitnessesBLL();

            description += " wit: " + marriageWitBll.GetWitnesseStringForMarriage(_marriage.Marriage_Id);


            return description;
        }

        public static string ToBrideDescription(this Marriage _marriage)
        {
            string description = _marriage.FemaleCName + " " + _marriage.FemaleSName;

            if (_marriage.FemaleIsKnownWidow.Value) description = " Wid";

            if (_marriage.MaleIsKnownWidower.Value)
                description += " " + _marriage.FemaleBirthYear.Value.ToString() + " " + _marriage.FemaleOccupation.Trim() + " married " + _marriage.MaleCName + " " + _marriage.MaleSName + " " + _marriage.MaleBirthYear.Value.ToString() + " Wid " + _marriage.MaleOccupation;
            else
                description += " " + _marriage.FemaleBirthYear.Value.ToString() + " " + _marriage.FemaleOccupation.Trim() + " married " + _marriage.MaleCName + " " + _marriage.MaleSName + " " + _marriage.MaleBirthYear.Value.ToString() + " " + _marriage.MaleOccupation;

            description = description.Replace("0", "");

            if (_marriage.MaleLocation != "")
                description += " groom of " + _marriage.MaleLocation;

            if (_marriage.FemaleLocation != "")
                description += " bride of " + _marriage.FemaleLocation;

            if (_marriage.IsLicence.Value)
                description += " lic ";


            MarriageWitnessesBLL marriageWitBll = new MarriageWitnessesBLL();

            description += " wit: " + marriageWitBll.GetWitnesseStringForMarriage(_marriage.Marriage_Id);


            return description;
        }

        public static string ToGroomDescription(this Marriage _marriage)
        {
            string description = _marriage.MaleCName + " " + _marriage.MaleSName;

            if (_marriage.MaleIsKnownWidower.Value) description = " Wid";

            if (_marriage.FemaleIsKnownWidow.Value)
                description += " " + _marriage.MaleBirthYear.Value.ToString() + " " + _marriage.MaleOccupation.Trim() + " married " + _marriage.FemaleCName + " " + _marriage.FemaleSName + " " + _marriage.FemaleBirthYear.Value.ToString() + " Wid " + " " + _marriage.FemaleOccupation;

            else
                description += " " + _marriage.MaleBirthYear.Value.ToString() + " " + _marriage.MaleOccupation.Trim() + " married " + _marriage.FemaleCName + " " + _marriage.FemaleSName + " " + _marriage.FemaleBirthYear.Value.ToString() + " " + _marriage.FemaleOccupation;

            description = description.Replace("0", "");

            if (_marriage.MaleLocation != "")
                description += " groom of " + _marriage.MaleLocation;

            if (_marriage.FemaleLocation != "")
                description += " bride of " + _marriage.FemaleLocation;

            if (_marriage.IsLicence.Value)
                description += " lic ";

            if (_marriage.Witness1 != "" || _marriage.Witness2 != "" || _marriage.Witness3 != "" || _marriage.Witness4 != "")
                description += " wit: " + _marriage.Witness1 + " " + _marriage.Witness2 + " " + _marriage.Witness3 + " " + _marriage.Witness4;

            MarriageWitnessesBLL marriageWitBll = new MarriageWitnessesBLL();

            description += " wit: " + marriageWitBll.GetWitnesseStringForMarriage(_marriage.Marriage_Id);


            return description;
        }



    }
}
