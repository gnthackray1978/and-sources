using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using GedItter.Interfaces;
using GedItter.ModelObjects;
using GedItter.BLL;
using TDBCore.BLL;
using TDBCore.EntityModel;
using GedItter.BirthDeathRecords.BLL;
using TDBCore.Types;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;
//using TDBCore.Datasets;

namespace GedItter.MarriageRecords
{



    public class WitnessDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public Guid LocationId { get; set; }
    }

    public class marriageWitness
    {
        public Person Person { get; set; }
        public string Description { get; set; }
     
        public static List<marriageWitness> AddWitnesses(List<WitnessDto> witnessDtos)
        {
            var witnesses = new List<marriageWitness>();

            foreach (WitnessDto witnessDto in witnessDtos)
            {
                Person witPers3 = new Person();
                marriageWitness nMarriageWitness = new marriageWitness();

                witPers3.ReferenceDateInt = witnessDto.Year;
                witPers3.ReferenceDateStr = witnessDto.Date;
                witPers3.ReferenceLocation = witnessDto.Location;
                witPers3.ReferenceLocationId = witnessDto.LocationId;
                witPers3.ChristianName = witnessDto.Name;
                witPers3.Surname = witnessDto.Surname;
                nMarriageWitness.Description = witnessDto.Description;
                nMarriageWitness.Person = witPers3;
                witnesses.Add(nMarriageWitness);
            }

            return witnesses;

        }

    }




    public class MarriagesEditorModel : EditorBaseModel<Guid>, IMarriageEditorModel
    {
        #region variables

        private List<marriageWitness> marriageWitnesses = new List<marriageWitness>();

        private string maleBirthYearStr = "";
        private string femaleBirthYearStr = "";

        private Marriage _marriage = new Marriage();

        private MarriageValidation _marriageValidation = new MarriageValidation();

        #endregion

        #region properties

        #region validation fields

        public bool IsValidWitnesses
        {
            get { return marriageWitnesses.Count > 0; }
        }

        public bool IsValidMaleSurname
        {
            get 
            {
                return this._marriageValidation.IsValidMaleSurname;
            }
        }

        public bool IsValidFemaleSurname
        {
            get
            {
                return this._marriageValidation.IsValidFemaleSurname ;
            }
        }

        public bool IsValidLocation
        {
            get
            {
                return this._marriageValidation.IsValidLocation ;
            }
        }

        public bool IsValidMaleLocation
        {
             get
            {
                return this._marriageValidation.IsValidMaleLocation ;
            }
        }

        public bool IsValidFemaleLocation
        {
             get
            {
                return this._marriageValidation.IsValidFemaleLocation ;
            }
        }

        public bool IsValidMaleInfo
        {
             get
            {
                return this._marriageValidation.IsValidMaleInfo ;
            }
        }

        public bool IsValidFemaleInfo
        {
             get
            {
                return this._marriageValidation.IsValidFemaleInfo ;
            }
        }

        public bool IsValidMarriageCounty
        {
             get
            {
                return this._marriageValidation.IsValidMarriageCounty ;
            }
        }

        public bool IsValidSource
        {
            get
            {
                return this._marriageValidation.IsValidSource ;
            }
        }

     

        public bool IsValidMaleOccupation
        {
            get
            {
                return this._marriageValidation.IsValidMaleOccupation ;
            }
        }

        public bool IsValidFemaleOccupation
        {
            get
            {
                return this._marriageValidation.IsValidFemaleOccupation ;
            }
        }

        public bool IsValidOriginalName
        {
            get
            {
                return this._marriageValidation.IsValidOriginalName ;
            }
        }

        public bool IsValidOriginalFemaleName
        {
            get
            {
                return this._marriageValidation.IsValidOriginalFemaleName ;
            }
        }

        public bool IsValidMaleBirthYear
        {
            get
            {
                return this._marriageValidation.IsValidMaleBirthYear;
            }
        }

        public bool IsValidFemaleBirthYear
        {
            get
            {
                return this._marriageValidation.IsValidFemaleBirthYear;
            }
        }

        public bool IsValidMarriageDate
        {
            get
            {
                return this._marriageValidation.IsValidMarriageDate;
            }
        }

        public bool IsValidMaleName
        {
            get
            {
                return this._marriageValidation.IsValidMaleName;
            }
        }

        public bool IsValidFemaleName
        {
            get
            {
                return this._marriageValidation.IsValidFemaleName;
            }
        }



        public override bool IsValidEntry
        {
            get
            {
                if (IsValidMaleName && IsValidFemaleName && IsValidMaleSurname && IsValidFemaleSurname && IsValidMarriageDate)
                {
                    this.SetErrorState("");
                    return true;
                }
                else
                {
                    this.SetErrorState("");
                    if (!IsValidMaleName || !IsValidMaleSurname)
                        this.SetErrorState(this.ErrorState + " Invalid Male Name");
                    if (!IsValidFemaleName || !IsValidFemaleSurname)
                        this.SetErrorState(this.ErrorState + " Invalid Female Name");
                    if (!IsValidMarriageDate)
                        this.SetErrorState(this.ErrorState + " Invalid Marriage Date");
                    return false;
                }
            }
        }

        #endregion

        #region read only fields

        public Guid EditorMaleId
          {
            get
            {
                return this._marriage.MaleId.GetValueOrDefault();
            }
        }

        public Guid EditorFemaleId
        {
            get
            {
                return this._marriage.FemaleId.GetValueOrDefault();
            }
        }

        //public string EditorOrigName
        //{
        //    get
        //    {
        //        return this._marriage.orig;
        //    }
        //}

        public string EditorOrigFemaleName
        {
            get
            {
                return this._marriage.OrigFemaleSurname;
            }
        }

        public string EditorOrigMaleName
        {
            get
            {
                return this._marriage.OrigMaleSurname;
            }
        }

        public string EditorFemaleBirthYear
        {
            get
            {
                return this.femaleBirthYearStr;
            }
        }

        public string EditorMaleBirthYear
        {
            get
            {
                return this.maleBirthYearStr;
            }
        }

        public string EditorDateMarriageString
        {
            get 
            {
                return this._marriage.Date;
                
            
            }
        }

        public string EditorMaleCName
        {
            get
            {
                return this._marriage.MaleCName;
            }
        }

        public string EditorMaleSName
        {
            get
            {
                return this._marriage.MaleSName;
            }
        }

        public string EditorFemaleCName
        {
            get
            {
                return this._marriage.FemaleCName;
            }
        }

        public string EditorFemaleSName
        {
            get
            {
                return this._marriage.FemaleSName;
            }
        }

        public string EditorLocation
        {
            get
            {
                return this._marriage.MarriageLocation;
            }
        }

        public string EditorMaleLocation
        {
            get
            {
                return this._marriage.MaleLocation;
            }
        }

        public string EditorFemaleLocation
        {
            get
            {
                return this._marriage.FemaleLocation;
            }
        }

        public string EditorMaleInfo
        {
            get
            {
                return this._marriage.MaleInfo;
            }
        }

        public string EditorFemaleInfo
        {
            get
            {
                return this._marriage.FemaleInfo;
            }
        }

        public string EditorMarriageCounty
        {
            get
            {
                return this._marriage.MarriageCounty;
            }
        }

        public string EditorSource
        {
            get
            {
                return this._marriage.Source;
            }
        }

        public List<marriageWitness> EditorWitnesses
        {
            get
            {
                return this.marriageWitnesses;
            }
        }



        public Guid EditorMarriageLocationId
        {
            get
            {
                return this._marriage.MarriageLocationId.GetValueOrDefault();
            }
        }

        public Guid EditorMaleLocationId
        {
            get
            {
                return this._marriage.MaleLocationId.GetValueOrDefault();
            }
        }

        public Guid EditorFemaleLocationId
        {
            get
            {
                return this._marriage.FemaleLocationId.GetValueOrDefault();
            }
        }



        public string EditorMaleOccupation
        {
            get
            {
                return this._marriage.MaleOccupation;
            }
        }

        public string EditorFemaleOccupation
        {
            get
            {
                return this._marriage.FemaleOccupation;
            }
        }

        public bool EditorIsWidow
        {
            get
            {
                return this._marriage.FemaleIsKnownWidow.GetValueOrDefault();
            }
        }

        public bool EditorIsWidower
        {
            get
            {
                return this._marriage.MaleIsKnownWidower.GetValueOrDefault();
            }
        }

        public bool EditorIsLicence
        {
            get
            {
                return this._marriage.IsLicence.GetValueOrDefault();
            }
        }

        public bool EditorIsBanns
        {
            get
            {
                return this._marriage.IsBanns.GetValueOrDefault();
            }
        }

        public int EditorMarriageYear
        {
            get {
                return this._marriage.YearIntVal; 
            }
        }

        #endregion

        #endregion



        private void ResetModelFields()
        {
            _marriage = new Marriage();

            this.SetEditorMarriageDate("");
            this.SetEditorLocation("");
            this.SetEditorMarriageCounty("");
            this.SetEditorSource("");
            this._marriage.YearIntVal = 0;
            this.SetEditorMaleCName("");
            this.SetEditorMaleSName("");
            this.SetEditorMaleInfo("");
            this.SetEditorMaleLocation("");
            this.SetEditorFemaleCName("");
            this.SetEditorFemaleSName("");
            this.SetEditorFemaleInfo("");
            this.SetEditorFemaleLocation("");
            this.SetEditorFemaleOccupation("");
            this.SetEditorMaleOccupation("");
            this.SetEditorIsBanns(false);
            this.SetEditorIsLicence(false);
            this.SetEditorIsWidow(false);
            this.SetEditorIsWidower(false);
            this.SetMarriageLocationId(Guid.Empty);
            this.SetMaleLocationId(Guid.Empty);
            this.SetFemaleLocationId(Guid.Empty);
            this.SetEditorMaleBirthYear("0");
            this.SetEditorFemaleBirthYear("0");
            this.SetEditorEventPriority(0);
            this.SetEditorTotalEvents(0);
            this.SetEditorUniqueRef(Guid.Empty);
            this.SetEditorFemaleId(Guid.Empty);
            this.SetEditorMaleId(Guid.Empty);
            this.SetEditorOrigMaleName("");
            this.SetEditorOrigFemaleName("");
          
            this.SetEditorWitness1(new List<marriageWitness>());
          
        }

        public override void Refresh()
        {

            if (!IsvalidSelect()) return;

            BLL.MarriagesBLL marriagesBLL = new GedItter.MarriageRecords.BLL.MarriagesBLL();
            MarriageWitnessesBll _marriageWitnessBLL = new MarriageWitnessesBll();

            var marriageRecord = 
                marriagesBLL.GetMarriageById2(this.SelectedRecordId).FirstOrDefault();

            if (marriageRecord != null && this.SelectedRecordId != Guid.Empty)
            {

                this.SetEditorMarriageDate(marriageRecord.Date);
                this.SetEditorLocation(marriageRecord.MarriageLocation);
                this.SetEditorMarriageCounty(marriageRecord.MarriageCounty);
                this.SetEditorSource(marriageRecord.Source);
                this._marriage.YearIntVal = marriageRecord.YearIntVal;
                this.SetEditorMaleCName(marriageRecord.MaleCName);
                this.SetEditorMaleSName(marriageRecord.MaleSName);
                this.SetEditorMaleInfo(marriageRecord.MaleInfo);
                this.SetEditorMaleLocation(marriageRecord.MaleLocation);
                this.SetEditorFemaleCName(marriageRecord.FemaleCName);
                this.SetEditorFemaleSName(marriageRecord.FemaleSName);
                this.SetEditorFemaleInfo(marriageRecord.FemaleInfo);
                this.SetEditorFemaleLocation(marriageRecord.FemaleLocation);
                this.SetEditorFemaleOccupation(marriageRecord.FemaleOccupation);
                this.SetEditorMaleOccupation(marriageRecord.MaleOccupation);
                this.SetEditorIsBanns(marriageRecord.IsBanns.GetValueOrDefault());
                this.SetEditorIsLicence(marriageRecord.IsLicence.GetValueOrDefault());
                this.SetEditorIsWidow(marriageRecord.FemaleIsKnownWidow.GetValueOrDefault());
                this.SetEditorIsWidower(marriageRecord.MaleIsKnownWidower.GetValueOrDefault());
                this.SetMarriageLocationId(marriageRecord.MarriageLocationId.GetValueOrDefault());
                this.SetMaleLocationId(marriageRecord.MaleLocationId.GetValueOrDefault());
                this.SetFemaleLocationId(marriageRecord.FemaleLocationId.GetValueOrDefault());
                this.SetEditorMaleBirthYear(marriageRecord.MaleBirthYear.ToString());
                this.SetEditorFemaleBirthYear(marriageRecord.FemaleBirthYear.ToString());
                this.SetEditorEventPriority(marriageRecord.EventPriority.GetValueOrDefault());
                this.SetEditorTotalEvents(marriageRecord.TotalEvents.GetValueOrDefault());
                this.SetEditorUniqueRef(marriageRecord.UniqueRef.GetValueOrDefault());
                this.SetEditorFemaleId(marriageRecord.FemaleId.GetValueOrDefault());
                this.SetEditorMaleId(marriageRecord.MaleId.GetValueOrDefault());
                this.SetEditorOrigMaleName(marriageRecord.OrigMaleSurname);
                this.SetEditorOrigFemaleName(marriageRecord.OrigFemaleSurname);


                //get wits
 
                this.SetEditorWitness1(_marriageWitnessBLL.GetWitnessesForMarriage(this.SelectedRecordId));

                if (this.SelectedRecordId != Guid.Empty)
                {
                    SourceMappingsBLL _SourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();
                    this.SetSourceGuidList(_SourceMappingsBLL.GetByMarriageIdOrPersonId2(this.SelectedRecordId).Select(o => o.Source.SourceId).ToList());
                }

            }
            else
            {
                ResetModelFields();
            }

            this.NotifyObservers<MarriagesEditorModel>(this);

        }

        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            if (IsValidSelectedRecordId)
            {
                BLL.MarriagesBLL marriageBLL = new GedItter.MarriageRecords.BLL.MarriagesBLL();
                marriageBLL.DeleteMarriageTemp2(this.SelectedRecordId);


                MarriageWitnessesBll mwits = new MarriageWitnessesBll();

                mwits.MarkWitnessesAsDeleted(this.SelectedRecordId);

              //  mwits.DeleteWitnessesForMarriage(this.SelectedRecordId);
            }
            

            this.NotifyObservers<MarriagesEditorModel>(this);
        }

        public override void EditSelectedRecord()
        {
            if (!IsValidEdit()) return;


            if (IsValidSelectedRecordId &&
                IsValidMaleName &&
                IsValidFemaleName)
            {
                
                BLL.MarriagesBLL marriageBLL = new GedItter.MarriageRecords.BLL.MarriagesBLL();

                var m = marriageBLL.GetMarriageById2(this.SelectedRecordId).FirstOrDefault();

                if (m != null)
                {
                    this.totalEvents = m.TotalEvents.GetValueOrDefault();
                    this.eventPriority = m.EventPriority.GetValueOrDefault();
                    this.uniqueRef = m.UniqueRef.GetValueOrDefault();

                }

                marriageBLL.UpdateMarriage2(this.SelectedRecordId,
                    this._marriage.Date,
                    this._marriage.FemaleCName,
                    this._marriage.FemaleId.GetValueOrDefault(),
                    this._marriage.FemaleInfo,
                    this._marriage.FemaleLocation,
                    this._marriage.FemaleSName,
                    this._marriage.MaleCName,
                    this._marriage.MaleId.GetValueOrDefault(),
                    
                    this._marriage.MaleInfo,
                    this._marriage.MaleLocation,
                    this._marriage.MaleSName,
                    this._marriage.MarriageCounty,
                    this._marriage.MarriageLocation,
                    this._marriage.Source,
                    this._marriage.YearIntVal,
        
                    this._marriage.MaleOccupation,
                    this._marriage.FemaleOccupation,
                    this._marriage.IsLicence.GetValueOrDefault(),
                    this._marriage.IsBanns.GetValueOrDefault(),
                    this._marriage.MaleIsKnownWidower.GetValueOrDefault(),
                    this._marriage.FemaleIsKnownWidow.GetValueOrDefault(),
                    this._marriage.MarriageLocationId.GetValueOrDefault(),
                    this._marriage.MaleLocationId.GetValueOrDefault(),
                    this._marriage.FemaleLocationId.GetValueOrDefault(),
                    this.SelectedUserId,
                    this._marriage.MaleBirthYear.GetValueOrDefault(),
                    this._marriage.FemaleBirthYear.GetValueOrDefault(),
                    this.uniqueRef,
                    this.totalEvents,
                    this.eventPriority,
                    this._marriage.OrigFemaleSurname,
                    this._marriage.OrigMaleSurname);

                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();
                sourceMappingBll.WriteMarriageSources(this.SelectedRecordId, this.SourceGuidList,this.SelectedUserId);

                SetWitnesses();


            }
            else
            {
              //  MessageBox.Show("WARNING", "Invalid Marriage ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.EditSelectedRecord();
            this.NotifyObservers<MarriagesEditorModel>(this);
        }

        private void SetWitnesses()
        {
            var mwits = new MarriageWitnessesBll();
            var deathsBirthsBll = new DeathsBirthsBLL();
            //delete existing entries
            mwits.DeleteWitnessesForMarriage(this.SelectedRecordId);

            foreach (var marriages in marriageWitnesses)
            {
                deathsBirthsBll.InsertPerson(marriages.Person);
            }

            mwits.InsertWitnessesForMarriage(this.SelectedRecordId, marriageWitnesses);

        }

        public override void InsertNewRecord()
        {
            if (!IsValidInsert()) return;


            if (this.IsValidFemaleName &&
                this.IsValidMaleName &&
                this.IsValidMarriageDate)
            {
                BLL.MarriagesBLL marriageBLL = new GedItter.MarriageRecords.BLL.MarriagesBLL();

                if (this.totalEvents == 0 && this.eventPriority == 0)
                    this.totalEvents = 1;

                this.SetSelectedRecordId(marriageBLL.InsertMarriage2(
                    this._marriage.Date,
                    this._marriage.FemaleCName,
                    this._marriage.FemaleId.GetValueOrDefault(),
                    this._marriage.FemaleInfo,
                    this._marriage.FemaleLocation,
                    this._marriage.FemaleSName,
                    this._marriage.MaleCName,
                    this._marriage.MaleId.GetValueOrDefault(),
                    this._marriage.MaleInfo,
                    this._marriage.MaleLocation,
                    this._marriage.MaleSName,
                    this._marriage.MarriageCounty,
                    this._marriage.MarriageLocation,
                    this._marriage.Source,

                    this._marriage.YearIntVal,
                    this._marriage.MaleOccupation,
                    this._marriage.FemaleOccupation,
                    this._marriage.IsLicence.GetValueOrDefault(),
                    this._marriage.IsBanns.GetValueOrDefault(),
                    this._marriage.FemaleIsKnownWidow.GetValueOrDefault(),
                    this._marriage.MaleIsKnownWidower.GetValueOrDefault(),
                    this.SelectedUserId,
                    this._marriage.MarriageLocationId.GetValueOrDefault(),
                    this._marriage.MaleLocationId.GetValueOrDefault(),
                    this._marriage.FemaleLocationId.GetValueOrDefault(),
                    this._marriage.MaleBirthYear.GetValueOrDefault(),
                    this._marriage.FemaleBirthYear.GetValueOrDefault(),
                    this.uniqueRef,
                    this.totalEvents,
                    this.eventPriority,
                    this._marriage.OrigMaleSurname,
                    this._marriage.OrigFemaleSurname));

                    //this.witness1,
                    //this.witness2,
                    //this.witness3,
                    //this.witness4,

                    SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();
                    sourceMappingBll.WriteMarriageSources(this.SelectedRecordId, this.SourceGuidList, this.SelectedUserId);


                    this.SetWitnesses();

                    this.OnDataSaved();
            }

            //if (this.IsValidSelectedRecordId)
            //    base.InsertNewRecord();


            this.NotifyObservers<MarriagesEditorModel>(this);
        }


       

        #region set fields 

        public void SetEditorWitness1(List<marriageWitness> witnesses)
        {
            if (witnesses != null)
            {
                this.marriageWitnesses = witnesses;
                this._marriageValidation.IsValidWitnesses = true;
            }
            else
            {
                this._marriageValidation.IsValidWitnesses = false;
            }



            SetModelStatusFields();
        }



        public void SetMarriageLocationId(Guid param)
        {
            if (this._marriage.MarriageLocationId.GetValueOrDefault() != param)
            {
                // this.marriageLocationId = param;
                this._marriage.MarriageLocationId = param;
                SetModelStatusFields();
            }
        }

        public void SetMaleLocationId(Guid param)
        {
            if (this._marriage.MaleLocationId.GetValueOrDefault() != param)
            {
                //  this.maleLocationId = param;
                this._marriage.MaleLocationId = param;
                SetModelStatusFields();
            }
        }

        public void SetFemaleLocationId(Guid param)
        {
            if (this._marriage.FemaleLocationId.GetValueOrDefault() != param)
            {
                // this.femaleLocationId = param;
                this._marriage.FemaleLocationId = param;
                SetModelStatusFields();
            }
        }

        public void SetEditorMaleId(Guid param)
        {
            if (this._marriage.MaleId.GetValueOrDefault() != param)
            {
                this._marriage.MaleId = param;
                SetModelStatusFields();
            }
        }

        public void SetEditorFemaleId(Guid param)
        {
            if (this._marriage.FemaleId.GetValueOrDefault() != param)
            {
                this._marriage.FemaleId = param;
                SetModelStatusFields();
            }
        }



        public void SetEditorOrigFemaleName(string param)
        {
            if (this._marriage.OrigFemaleSurname != param)
            {
                if (param.Length >= 0 && param.Length <= 150)
                {
                    this._marriageValidation.IsValidOriginalFemaleName = true;
                }
                else
                {
                    this._marriageValidation.IsValidOriginalFemaleName = false;
                }

                this._marriage.OrigFemaleSurname = param;
                SetModelStatusFields();
            }
        }


     
        


        public void SetEditorOrigMaleName(string param)
        {
            if (this._marriage.OrigMaleSurname != param)
            {
                if (param.Length >= 0 && param.Length <= 150)
                {
                    this._marriageValidation.IsValidOriginalName = true;
                }
                else
                {
                    this._marriageValidation.IsValidOriginalName = false;
                }
                this._marriage.OrigMaleSurname = param;
                SetModelStatusFields();
            }
        }

        public void SetEditorMaleCName(string cName)
        {
            if (this._marriage.MaleCName != cName)
            {
                if (cName.Length >= 0 && cName.Length <= 50)
                {
                    this._marriageValidation.IsValidMaleName = true;
                }
                else
                {
                    this._marriageValidation.IsValidMaleName = false;
                }

                this._marriage.MaleCName = cName;
                SetModelStatusFields();
            }
        }

        public void SetEditorMaleSName(string sName)
        {
            if (this._marriage.MaleSName != sName)
            {
                if (sName.Length >= 0 && sName.Length <= 50)
                {
                    this._marriageValidation.IsValidMaleSurname = true;
                }
                else
                {
                    this._marriageValidation.IsValidMaleSurname = false;
                }

                this._marriage.MaleSName = sName;
                SetModelStatusFields();
            }
        }

        public void SetEditorFemaleCName(string cName)
        {
            if (this._marriage.FemaleCName != cName)
            {
                if (cName.Length >= 0 && cName.Length <= 50)
                {
                    this._marriageValidation.IsValidFemaleName = true;
                }
                else
                {
                    this._marriageValidation.IsValidFemaleName = false;
                }

                this._marriage.FemaleCName = cName;
                SetModelStatusFields();
            }
        }

        public void SetEditorFemaleSName(string sName)
        {
            if (this._marriage.FemaleSName != sName)
            {
                if (sName.Length >= 0 && sName.Length <= 50)
                {
                    this._marriageValidation.IsValidFemaleSurname = true;
                }
                else
                {
                    this._marriageValidation.IsValidFemaleSurname = false;
                }

                this._marriage.FemaleSName = sName;
                SetModelStatusFields();
            }
        }

        public void SetEditorMarriageDate(string date)
        {
            if (date != this._marriage.Date)
            {
                int result =0;


                if (CsUtils.ValidYear(date, out result))
                {
                    this._marriageValidation.IsValidMarriageDate = true;
                    this._marriage.YearIntVal = result;
                    this._marriage.Date = date;
                    //this.marriageYear = result.Year;
                    //this.marriageDate = result.ToShortDateString();
                }
                else
                {
                    this._marriageValidation.IsValidMarriageDate = false;
                    this._marriage.Date = date;
                }

                SetModelStatusFields();
            }
        }

        public void SetEditorLocation(string location)
        {
            if (this._marriage.MarriageLocation != location)
            {
                if (location.Length >= 0 && location.Length <= 50)
                {
                    this._marriageValidation.IsValidLocation = true;
                }
                else
                {
                    this._marriageValidation.IsValidLocation = false;
                }

                this._marriage.MarriageLocation = location;
                SetModelStatusFields();
            }
        }

        public void SetEditorMaleLocation(string maleLocation)
        {
            if (this._marriage.MaleLocation != maleLocation)
            {
                if (maleLocation.Length >= 0 && maleLocation.Length <= 50)
                {
                    this._marriageValidation.IsValidMaleLocation = true;
                }
                else
                {
                    this._marriageValidation.IsValidMaleLocation = false;
                }


                this._marriage.MaleLocation = maleLocation;
                SetModelStatusFields();
            }
        }

        public void SetEditorFemaleLocation(string femaleLocation)
        {
            if (this._marriage.FemaleLocation != femaleLocation)
            {
               // this.femaleLocation = femaleLocation;
                if (femaleLocation.Length >= 0 && femaleLocation.Length <= 50)
                {
                    this._marriageValidation.IsValidFemaleLocation = true;
                }
                else
                {
                    this._marriageValidation.IsValidFemaleLocation = false;
                }

                this._marriage.FemaleLocation = femaleLocation;
                SetModelStatusFields();
            }
        }

        public void SetEditorMarriageCounty(string marriageCounty)
        {
            if (this._marriage.MarriageCounty != marriageCounty)
            {
                //this.marriageLocationCounty = marriageCounty;
                if (marriageCounty.Length >= 0 && marriageCounty.Length <= 50)
                {
                    this._marriageValidation.IsValidMarriageCounty = true;
                }
                else
                {
                    this._marriageValidation.IsValidMarriageCounty = false;
                }

                this._marriage.MarriageCounty = marriageCounty;
                SetModelStatusFields();
            }
        }

        public void SetEditorSource(string source)
        {
            if (this._marriage.Source != source)
            {
               // this.marriageSource = source;
                if (source.Length >= 0 && source.Length <= 50)
                {
                    this._marriageValidation.IsValidSource = true;
                }
                else
                {
                    this._marriageValidation.IsValidSource = false;
                }

                this._marriage.Source = source;
                SetModelStatusFields();
            }
        }



        public void SetEditorFemaleInfo(string finfo)
        {
            if (this._marriage.FemaleInfo != finfo)
            {
                if (finfo.Length >= 0 && finfo.Length <= 500)
                {
                    this._marriageValidation.IsValidFemaleInfo = true;
                }
                else
                {
                    this._marriageValidation.IsValidFemaleInfo = false;
                }

                this._marriage.FemaleInfo = finfo;
                SetModelStatusFields();
            }
        }

        public void SetEditorMaleInfo(string minfo)
        {
            if (this._marriage.MaleInfo != minfo)
            {
                if (minfo.Length >= 0 && minfo.Length <= 500)
                {
                    this._marriageValidation.IsValidMaleInfo = true;
                }
                else
                {
                    this._marriageValidation.IsValidMaleInfo = false;
                }


                this._marriage.MaleInfo = minfo;
                SetModelStatusFields();
            }
        }



        public void SetEditorMaleOccupation(string paramOccupation)
        {
            if (this._marriage.MaleOccupation != paramOccupation)
            {
                if (paramOccupation.Length >= 0 && paramOccupation.Length <= 500)
                {
                    this._marriageValidation.IsValidMaleOccupation = true;
                }
                else
                {
                    this._marriageValidation.IsValidMaleOccupation = false;
                }
                this._marriage.MaleOccupation = paramOccupation;
                this.SetModelStatusFields();
            }
        }

        public void SetEditorFemaleOccupation(string paramOccupation)
        {
            if (this._marriage.FemaleOccupation != paramOccupation)
            {
                if (paramOccupation.Length >= 0 && paramOccupation.Length <= 500)
                {
                    this._marriageValidation.IsValidFemaleOccupation = true;
                }
                else
                {
                    this._marriageValidation.IsValidFemaleOccupation = false;
                }

                this._marriage.FemaleOccupation = paramOccupation;
                this.SetModelStatusFields();
            }
        }

        public void SetEditorIsWidow(bool paramIsWidow)
        {
            if (this._marriage.FemaleIsKnownWidow.GetValueOrDefault() != paramIsWidow)
            {                
                this._marriage.FemaleIsKnownWidow = paramIsWidow;
                this.SetModelStatusFields();
            }
        }

        public void SetEditorIsWidower(bool paramIsWidower)
        {
            if (this._marriage.MaleIsKnownWidower.GetValueOrDefault() != paramIsWidower)
            {
                this._marriage.MaleIsKnownWidower = paramIsWidower;
                this.SetModelStatusFields();
            }
        }

        public void SetEditorIsLicence(bool paramIsLicence)
        {
            if (this._marriage.IsLicence.GetValueOrDefault() != paramIsLicence)
            {
                this._marriage.IsLicence = paramIsLicence;
                this.SetModelStatusFields();
            }
        }

        public void SetEditorIsBanns(bool paramBanns)
        {
            if (this._marriage.IsBanns.GetValueOrDefault() != paramBanns)
            {
               // this.isBanns = paramBanns;
                this._marriage.IsBanns = paramBanns;
                this.SetModelStatusFields();
            }
        }
        

        public void SetEditorFemaleBirthYear(string param)
        {
            if (param != femaleBirthYearStr)
            {
                int result;
                this.femaleBirthYearStr = param;
                
                if (Int32.TryParse(param, out result))
                {

                    if (result > 1300 && result < 2100)
                    {
                        this._marriageValidation.IsValidFemaleBirthYear = true;
                        this._marriage.FemaleBirthYear = result;
                    }
                    else
                    {
                        this._marriageValidation.IsValidFemaleBirthYear = false;
                        this._marriage.FemaleBirthYear = 0;
                    }
                }
                else
                {
                    this._marriageValidation.IsValidFemaleBirthYear = false;
                    this._marriage.FemaleBirthYear = 0;
                }

                SetModelStatusFields();
            }
        }

        public void SetEditorMaleBirthYear(string param)
        {
            if (param != maleBirthYearStr)
            {
                int result;
                this.maleBirthYearStr = param;

                if (Int32.TryParse(param, out result))
                {

                    if (result > 1300 && result < 2100)
                    {
                        this._marriageValidation.IsValidMaleBirthYear = true;

                        this._marriage.MaleBirthYear = result;
                    }
                    else
                    {
                        this._marriageValidation.IsValidMaleBirthYear = false;
                        this._marriage.MaleBirthYear = 0;
                    }
                }
                else
                {
                    this._marriageValidation.IsValidMaleBirthYear = false;
                    this._marriage.MaleBirthYear = 0;
                }

                SetModelStatusFields();
            }
        }
                       
        #endregion

        public void Show()
        { 
            
        }



        public override void SetFromQueryString(string param)
        {
            Debug.WriteLine("editor model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);
            Guid selectedId = Guid.Empty;

            Guid.TryParse(query["id"], out selectedId);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            //if (query.AllKeys.Contains("permission"))
            //{
            //    this.SetPermissionState(query["permission"] ?? "");
            //}

            this.SetSelectedRecordId(selectedId);

           // this.Refresh();

        }


    }
}
