using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GedItter.MarriageRecords;
using System.Text.RegularExpressions;

////using TDBCore.Datasets;
using GedItter.ModelObjects;
using GedItter.BLL;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;
using TDBCore.Types;

namespace GedItter.BirthDeathRecords
{
    public class DeathBirthEditorModel : EditorBaseModel<Guid>, IDeathBirthEditorModel
    {

        protected new ArrayList aList = new ArrayList();

        #region field variables

        #region validation

        bool isValidBapDate = false;
        bool isValidDeathDate = false;
        bool isValidBirthDate = false;
        bool isValidName = false;
        bool isValidLocation = false;
        bool isValidDeathLocation = false;
        bool isValidReferenceLocation = false;
        bool isValidReferenceDate = false;
        bool isValidSpouseCName = false;
        bool isValidSpouseSName = false;
        bool isValidFatherOccupation = false;
        bool isValidOccupation = false;
        bool isValidSurname = false;
        bool isValidFatherChristianName = false;
        bool isValidFatherSurname = false;
        bool isValidMotherChristianName = false;
        bool isValidMotherSurname = false;
        bool isValidBirthCountyLocation = false;
        bool isValidDeathCountyLocation = false;
        bool isValidBirthLocation = false;
        bool isValidSource = false;
        bool isValidNotes = false;
        bool isValidUniqueRef = false;
        bool isValidOriginalName = false;
        bool isValidOriginalFatherName = false;
        bool isValidOriginalMotherName = false;
        bool isValidDeathLocationId = false;
        bool isValidReferenceLocationId = false;
        bool isValidBirthLocationId = false;

        #endregion

        int birthInt = 0;
        string dateBirthString = "";
    
        int deathInt =0;
        string dateDeathString = "";

        int bapInt = 0;
        string dateBapString = "";

        int referenceDateInt = 0;

        string christianName = "";
        string surname = "";
        string fatherChristianName = "";
        string fatherSurname = "";
        string motherChristianName = "";
        string motherSurname = "";
        bool isMale = false;
        string birthLocation = "";
        string deathLocation = "";
        string birthCountyLocation = "";
        string deathCountyLocation = "";
        string source = "";
        string notes = "";
        string referenceLocation = "";
        string referenceDateString = "";
        string occupation = "";

        string spouseCName = "";
        string spouseSName = "";
        string fatherOccupation = "";
        // unknown parish
        Guid birthLocationId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");
        Guid deathLocationId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");
        Guid referenceLocationId = new Guid("a813a1ff-6093-4924-a7b2-c5d1af6ff699");

        string originalName = "";
        string originalFatherName = "";
        string originalMotherName = "";

        int estBirthInt=0;
        string estBirthStr = "";
        int estDeathInt=0;
        string estDeathStr = "";

        bool isEstBirth = false;
        bool isEstDeath = false;

        #endregion
      
        #region valid props
        
        public override string ErrorState
        {
            get
            {
                if (IsValidEntry)
                {
                    return "";
                }
                else
                {
                    return this.ErrorState;
                }
            }
        }

        public override bool IsValidEntry
        {
            get
            {
                
                
                if (IsValidBirthEntry || IsValidDeathEntry || IsValidReferenceEntry)
                {
                   
                    return true;
                }
                else
                {
       
                    return false;

                    
                }
            }
        }


        public bool IsValidBirthEntry
        { 
            get
            {
                if (IsValidName && isValidSurname && (IsValidBirthDate || IsValidBapDate) && IsValidLocation)
                    return true;
                else
                {
                    this.SetErrorState("InvalidBirthEntry");

                    return false;
                }

            }
        }
        public bool IsValidDeathEntry
        { 
            get
            {

                if (IsValidName && isValidSurname && IsValidDeathDate && IsValidDeathLocation)
                    return true;
                else
                {
                    this.SetErrorState("InvalidDeathEntry");

                    return false;
                }
                
            }
        }
        public bool IsValidReferenceEntry
        {
            get
            {
                if (IsValidName && isValidSurname && IsValidReferenceDate && IsValidReferenceLocation)
                    return true;
                else
                {
                    this.SetErrorState("InvalidReferenceEntry");

                    return false;
                }
            }
        }


        public bool IsValidSpouseCName
        {
            get
            {
                return this.isValidSpouseCName;
            }
        }

        public bool IsValidSpouseSName
        {
            get
            {
                return this.isValidSpouseSName;
            }
        }

        public bool IsValidFatherOccupation
        {
            get
            {
                return this.isValidFatherOccupation;
            }
        }

        public bool IsValidBirthLocationId
        {
            get 
            {
                return this.isValidBirthLocationId;
            }
        }

        public bool IsValidDeathLocationId
        {
            get 
            {
                return this.isValidDeathLocationId;
            }
        }

        public bool IsValidReferenceLocationId
        {
            get 
            {
                return this.isValidReferenceLocationId;
            }
        }

        public bool IsValidOccupation
        {
            get 
            {
                return this.isValidOccupation;
            }
        }

        public bool IsValidSurname
        {
            get 
            {
                return this.isValidSurname;
            }
        }

        public bool IsValidFatherChristianName
        {
            get 
            {
                return this.isValidFatherChristianName;
            }
        }

        public bool IsValidFatherSurname
        {
            get 
            {
                return this.isValidFatherSurname;
            }
        }

        public bool IsValidMotherChristianName
        {
            get 
            {
                return this.isValidMotherChristianName;
            }
        }

        public bool IsValidMotherSurname
        {
            get 
            {
                return this.isValidMotherSurname;
            }
        }

        public bool IsValidBirthCountyLocation
        {
            get 
            {
                return this.isValidBirthCountyLocation;
            }
        }

        public bool IsValidDeathCountyLocation
        {
            get 
            {
                return this.isValidDeathCountyLocation;
            }
        }

        public bool IsValidBirthLocation
        {
            get 
            {
                return this.isValidBirthLocation;
            }
        }

        public bool IsValidSource
        {
            get 
            {
                return this.isValidSource;
            }
        }

        public bool IsValidNotes
        {
            get 
            {
                return this.isValidNotes;
            }
        }

        public bool IsValidUniqueRef
        {
            get 
            {
                return this.isValidUniqueRef;
            }
        }

        public bool IsValidOriginalName
        {
            get 
            {
                return this.isValidOriginalName;
            }
        }

        public bool IsValidOriginalFatherName
        {
            get 
            {
                return this.isValidOriginalFatherName;
            }
        }

        public bool IsValidOriginalMotherName
        {
            get
            {
                return this.isValidOriginalMotherName;
            }
        }

        public bool IsValidBapDate
        {
            get
            {
                return this.isValidBapDate;
            }
        }

        public bool IsValidDeathDate
        {
            get
            {
                return this.isValidDeathDate;
            }
        }

        public bool IsValidBirthDate
        {
            get
            {
                return this.isValidBirthDate;
            }
        }

        public bool IsValidName
        {
            get
            {
                return this.isValidName;
            }
        }

        public bool IsValidDeathLocation
        {
            get
            {
                return this.isValidDeathLocation;
            }
        }

        public bool IsValidLocation
        {
            get
            {
                return this.isValidLocation;
            }
        }
       
        public bool IsValidReferenceDate
        {
            get
            {
                return this.isValidReferenceDate;
            }
        }

        public bool IsValidReferenceLocation
        {
            get
            {
                return this.isValidReferenceLocation;
            }
        }
      

        #endregion

        #region read only properties

        public string EditorSpouseCName
        {
            get
            {
                return this.spouseCName;
            }
        }

        public string EditorSpouseSName
        {
            get
            {
                return this.spouseSName;
            }
        }

        public string EditorFatherOccupation
        {
            get
            {
                return this.fatherOccupation;
            }
        }

        public int EstBirthInt
        {
            get
            {
                return this.estBirthInt;
            }
        }

        public int EstDeathInt
        {
            get
            {
                return this.estDeathInt;
            }
        }

        public bool IsEstBirth
        {
            get
            {
                return this.isEstBirth;
            }
        }

        public bool IsEstDeath
        {
            get
            {
                return this.isEstDeath;
            }
        }

        public string EditorReferenceDateString
        {
            get
            {
                return this.referenceDateString;
            }
        }

        public string EditorReferenceLocation
        {
            get
            {
                return this.referenceLocation;
            }
        }

        public string EditorOccupation
        {
            get
            {
                return this.occupation;
            }
        }

        public string EditorDateBirthString
        {
            get
            {
                return this.dateBirthString;
            }
        }

        public string EditorDateDeathString
        {
            get
            {
                return this.dateDeathString;
            }
        }

        public string EditorDateBapString
        {
            get
            {
                return this.dateBapString;
            }
        }

        public string EditorChristianName
        {
            get
            {
                return this.christianName;
            }
        }

        public string EditorSurnameName
        {
            get 
            {
                return this.surname;
            }
        }

        public string EditorFatherChristianName
        {
            get
            {
                return this.fatherChristianName;
            }
        }

        public string EditorFatherSurname
        {
            get
            {
                return this.fatherSurname;
            }
        }

        public string EditorMotherChristianName
        {
            get
            {
                return this.motherChristianName;
            }
        }

        public string EditorMotherSurname
        {
            get
            {
                return this.motherSurname;
            }
        }

        public bool EditorIsMale
        {
            get
            {
                return this.isMale;
            }
        }

        public string EditorBirthLocation
        {
            get
            {
                return this.birthLocation;
            }
        }

        public string EditorDeathLocation
        {
            get
            {
                return this.deathLocation;
            }
        }

        public string EditorSource
        {
            get
            {
                return this.source;
            }
        }

        public string EditorNotes
        {
            get
            {
                return this.notes;
            }
        }

       

        public string EditorBirthCountyLocation
        {
            get
            {
                return this.birthCountyLocation;
            }
        }

        public string EditorDeathCountyLocation
        {
            get
            {
                return this.deathCountyLocation;
            }
        }

        public Guid EditorBirthLocationId
        {
            get
            { 
                return this.birthLocationId;
            }
        }

        public Guid EditorDeathLocationId
        {
            get
            {
                return this.deathLocationId;
            }
        }

        public Guid EditorReferenceLocationId
        {
            get
            {
                return this.referenceLocationId;
            }
        }

        public string FilterOriginalName
        {
            get
            {
                return this.originalName;
            }
        }

        public string FilterOriginalFatherName
        {
            get
            {
                return this.originalFatherName;
            }
        }

        public string FilterOriginalMotherName
        {
            get
            {
                return this.originalMotherName;
            }
        }
        #endregion

        #region set editor fields
        //150
        public void SetEditorSpouseCName(string param)
        {
            if (this.spouseCName != param)
            {
                if (param.Length >= 0 && param.Length <= 150)
                {
                    this.isValidSpouseCName = true;
                }
                else
                {
                    this.isValidSpouseCName = false;
                }

                this.spouseCName = param;

                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorSpouseSName(string param)
        {
            if (this.spouseSName != param)
            {
                if (param.Length >=0 && param.Length <=150)
                {
                    this.isValidSpouseSName =true ;
                }
                else
                {
                    this.isValidSpouseSName = false;
                }

                this.spouseSName = param;

                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorFatherOccupation(string param)
        {
            if (this.fatherOccupation != param)
            {
                if (param.Length >= 0 && param.Length <=150)
                {
                    this.isValidFatherOccupation =true ;
                }
                else
                {
                    this.isValidFatherOccupation =false ;
                }

                this.fatherOccupation = param;

                this.SetModelStatusFields();
            }
        }
        //150
        public void SetFilterOriginalName(string param)
        {
            if (this.originalName != param)
            {
                if (param.Length >= 0 && param.Length <= 150)
                {
                    this.isValidOriginalName = true;
                }
                else
                {
                    this.isValidOriginalName = false;
                }
                this.originalName = param;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetFilterOriginalFatherName(string param)
        {
            if (this.originalFatherName != param)
            {
                if (param.Length > 0 && param.Length <= 150)
                {
                    this.isValidOriginalFatherName = true;
                }
                else
                {
                    this.isValidOriginalFatherName = false;
                }
                this.originalFatherName = param;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetFilterOriginalMotherName(string param)
        {
            if (this.originalMotherName != param)
            {
                if (param.Length >= 0 && param.Length <= 150)
                {
                    this.isValidOriginalMotherName = true;
                }
                else
                {
                    this.isValidOriginalMotherName = false;
                }

                this.originalMotherName = param;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorBirthCountyLocation(string bLocation)
        {
            if (this.birthCountyLocation != bLocation)
            {
                if (bLocation.Length >= 0 && bLocation.Length < 150)
                {
                    this.isValidBirthCountyLocation = true;
                }
                else
                {
                    this.isValidBirthCountyLocation = false;
                }

                this.birthCountyLocation = bLocation;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorDeathCountyLocation(string dLocation)
        {
            if (this.deathCountyLocation != dLocation)
            {

                if (dLocation.Length > 0 && dLocation.Length <= 150)
                {
                    this.isValidDeathCountyLocation = true;
                }
                else
                {
                    this.isValidDeathCountyLocation = false;
                }
                this.deathCountyLocation = dLocation;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorChristianName(string cName)
        {
            if (cName != this.christianName)
            {
                if (cName.Length > 0 && cName.Length <= 150)
                {
                    this.isValidName = true;
                }
                else
                {
                    this.isValidName = false;
                }

                this.christianName = cName;
                this.SetModelStatusFields();
            }
        }
        //100
        public void SetEditorSurnameName(string sName)
        {
            if (sName != this.surname)
            {

                //if (this.christianName == "" && this.surname == "")
                if (sName.Length > 0 && sName.Length <= 100)
                {
                    this.isValidSurname = true;
                }
                else
                {
                    this.isValidSurname = false;
                }

                this.surname = sName;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorFatherChristianName(string fCName)
        {
            if (fCName != this.fatherChristianName)
            {
                if (fCName.Length >= 0 && fCName.Length < 150)
                {
                    this.isValidFatherChristianName = true;
                }
                else
                {
                    this.isValidFatherChristianName = false;
                }

                this.fatherChristianName = fCName;
                this.SetModelStatusFields();
            }
        }
        //500
        public void SetEditorFatherSurname(string fSName)
        {
            if (fSName != this.fatherSurname)
            {

                if (fSName.Length >= 0 && fSName.Length <= 500)
                {
                    this.isValidFatherSurname = true;
                }
                else
                {
                    this.isValidFatherSurname = false;
                }
                this.fatherSurname = fSName;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorMotherChristianName(string mCName)
        {
            if (mCName != this.motherChristianName)
            {
                if(mCName.Length >=0 && mCName.Length <=150)
                {
                    this.isValidMotherChristianName = true;
                }
                else
                {
                    this.isValidMotherChristianName =false;
                }

                this.motherChristianName = mCName;
                this.SetModelStatusFields();
            }
        }
        //500
        public void SetEditorMotherSurname(string mSName)
        {
            if (mSName != this.motherSurname)
            {
                if (mSName.Length >= 0 && mSName.Length <= 500)
                {
                    this.isValidMotherSurname = true;
                }
                else
                {
                    this.isValidMotherSurname = false;
                }
                this.motherSurname = mSName;
                this.SetModelStatusFields();
            }
        }
        //500
        public void SetEditorBirthLocation(string bLocation)
        {
            if (bLocation != this.birthLocation)
            {
                if (bLocation.Length >= 0 && bLocation.Length <= 500)
                {
                    this.isValidLocation = true;
                }
                else
                {
                    this.isValidLocation = false;
                }

                this.birthLocation = bLocation;
                this.SetModelStatusFields();
            }
        }
        //500
        public void SetEditorDeathLocation(string dLocation)
        {
            if (dLocation != this.deathLocation)
            {

                if (dLocation.Length >= 0 && dLocation.Length <= 500)
                {
                    this.isValidDeathLocation = true;
                }
                else
                {
                    this.isValidDeathLocation = false;
                }

                this.deathLocation = dLocation;
                this.SetModelStatusFields();
            }
        }
        //50
        public void SetEditorSource(string source)
        {
            if (source != this.source)
            {

                if (source.Length >= 0 && source.Length <= 50)
                {
                    this.isValidSource = true;
                }
                else
                {
                    this.isValidSource = false;
                }
                this.source = source;
                this.SetModelStatusFields();

            }
        }
        //8000
        public void SetEditorNotes(string notes)
        {
            if (notes != this.notes)
            {
                if (notes.Length >= 0 && notes.Length <= 8000)
                {
                    this.isValidNotes = true;
                }
                else
                {
                    this.isValidNotes = false;
                }
                this.notes = notes;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorReferenceLocation(string paramReferenceLocation)
        {
            if (this.referenceLocation != paramReferenceLocation)
            {
                if (paramReferenceLocation.Length >=0 && paramReferenceLocation.Length <=150)
                {
                    this.isValidReferenceLocation = true;
                }
                else
                {
                    this.isValidReferenceLocation = false;
                }

                this.referenceLocation = paramReferenceLocation;
                this.SetModelStatusFields();
            }
        }
        //150
        public void SetEditorOccupation(string paramOccupation)
        {
            if (this.occupation != paramOccupation)
            {

                if (paramOccupation.Length >= 0 && paramOccupation.Length <= 150)
                {
                    this.isValidOccupation = true;
                }
                else
                {
                    this.isValidOccupation = false;
                }
                this.occupation = paramOccupation;
                this.SetModelStatusFields();
            }
        }


        // set editor values
        //public void SetEditorBirthEstBirthInt(string param)
        //{
        //    if (this.estBirthStr != param)
        //    {
        //        // just make sure there is a year in there !!


        //        if (ValidateDate(param, out estBirthStr, out this.estBirthInt))
        //        { 
                   
        //        }


        //        CsUtils.CalcEstDates(this.birthInt, this.bapInt, this.deathInt, out this.estBirthInt, out this.estDeathInt,
        //            out isEstBirth, out isEstDeath, this.fatherChristianName, this.motherChristianName);


        //        this.SetModelStatusFields();
        //    }
        //}

        //public void SetEditorBirthEstDeathInt(string param)
        //{
        //    if (this.estDeathStr != param)
        //    {
        //        // just make sure there is a year in there !!


        //        ValidateDate(param, out estDeathStr, out this.estDeathInt);

        //        CsUtils.CalcEstDates(this.birthInt, this.bapInt, this.deathInt, out this.estBirthInt, out this.estDeathInt,
        //            out isEstBirth, out isEstDeath, this.fatherChristianName, this.motherChristianName);
        //        this.SetModelStatusFields();
        //    }
        //}


        //public void SetEditorBirthIsEstBirth(bool param)
        //{
        //    if (this.isEstBirth != param)
        //    {
        //        this.isEstBirth = param;
        //        this.SetModelStatusFields();
        //    }
        //}

        //public void SetEditorBirthIsEstDeath(bool param)
        //{
        //    if (this.isEstDeath != param)
        //    {
        //        this.isEstDeath = param;
        //        this.SetModelStatusFields();
        //    }
        //}

        public void SetEditorReferenceLocationId(Guid param)
        {
            if (this.referenceLocationId != param)
            {
                if (param == Guid.Empty)
                {
                    this.isValidReferenceLocationId = false;
                }
                else
                {
                    this.isValidReferenceLocationId = true;
                }

                this.referenceLocationId = param;

                this.SetModelStatusFields();
            }
        }

        public void SetEditorBirthLocationId(Guid param)
        {
            if (this.birthLocationId != param)
            {
                if (param == Guid.Empty)
                {
                    this.isValidBirthLocationId = false;
                }
                else
                {
                    this.isValidBirthLocationId = true;
                }

                this.birthLocationId = param;
                this.SetModelStatusFields();
            }
        }

        public void SetEditorDeathLocationId(Guid param)
        {
            if (this.deathLocationId != param)
            {
                if (param == Guid.Empty)
                {
                    this.isValidDeathCountyLocation = false;
                }
                else
                {
                    this.isValidDeathCountyLocation = true;
                }

                this.deathLocationId = param;
                this.SetModelStatusFields();
            }
        }



        public void SetEditorDateBirthString(string dBirth)
        {
            if (this.dateBirthString != dBirth)
            {

                this.isValidBirthDate = CsUtils.ValidYear(dBirth, out birthInt);
            

                CsUtils.CalcEstDates(this.birthInt, this.bapInt, this.deathInt, out this.estBirthInt, out this.estDeathInt,
                    out isEstBirth, out isEstDeath, this.fatherChristianName, this.motherChristianName);

                this.dateBirthString = dBirth;

                this.SetModelStatusFields();
            }

        }

        public void SetEditorDateDeathString(string dDeath)
        {
            if (this.dateDeathString != dDeath)
            {
                //this.isValidDeathDate = CsUtils.ValidateDate(dDeath, out this.dateDeathString, out deathInt);

                this.isValidDeathDate = CsUtils.ValidYear(dDeath, out deathInt);

                CsUtils.CalcEstDates(this.birthInt, this.bapInt, this.deathInt, out this.estBirthInt, out this.estDeathInt,
                    out isEstBirth, out isEstDeath, this.fatherChristianName, this.motherChristianName);


                this.dateDeathString = dDeath;

                this.SetModelStatusFields();
            }
        }

        public void SetEditorDateBapString(string dBap)
        {
            if (this.dateBapString != dBap)
            {
          
                //this.isValidBapDate = CsUtils.ValidateDate(dBap, out this.dateBapString, out bapInt);

                this.isValidBapDate = CsUtils.ValidYear(dBap, out bapInt);


                CsUtils.CalcEstDates(this.birthInt, this.bapInt, this.deathInt, out this.estBirthInt, out this.estDeathInt,
                    out isEstBirth, out isEstDeath, this.fatherChristianName, this.motherChristianName);



                this.dateBapString = dBap;

                this.SetModelStatusFields();
            }
        }

        public void SetEditorReferenceDate(string paramReferenceDate)
        {
            if (this.referenceDateString != paramReferenceDate)
            {
                //this.isValidReferenceDate = CsUtils.ValidateDate(paramReferenceDate, out this.referenceDateString, out referenceDateInt);
                this.isValidReferenceDate = CsUtils.ValidYear(paramReferenceDate, out referenceDateInt);

                CsUtils.CalcEstDates(this.birthInt, this.bapInt, this.deathInt, out this.estBirthInt, out this.estDeathInt,
                    out isEstBirth, out isEstDeath, this.fatherChristianName, this.motherChristianName);


                this.referenceDateString = paramReferenceDate;

                this.SetModelStatusFields();
            }
        }       


        public void SetEditorIsMale(bool isMale)
        {
            if (isMale != this.isMale)
            {
                this.isMale = isMale;
                this.SetModelStatusFields();
            }
        }

 




       

        #endregion

        public override void Refresh()
        {
            if (!IsvalidSelect()) return;


            BLL.DeathsBirthsBLL deathBirthDLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();

            TDBCore.EntityModel.Person personsDataTable = deathBirthDLL.GetDeathBirthRecordById2(this.SelectedRecordId).FirstOrDefault();

            if (personsDataTable != null && this.SelectedRecordId != Guid.Empty)
            {


                this.SetEditorChristianName(personsDataTable.ChristianName);
                this.SetEditorSurnameName(personsDataTable.Surname);
                this.SetEditorSource(personsDataTable.Source);
                this.SetEditorNotes(personsDataTable.Notes);
                this.SetEditorIsMale(personsDataTable.IsMale);
                this.SetEditorMotherChristianName(personsDataTable.MotherChristianName);
                this.SetEditorMotherSurname(personsDataTable.MotherSurname);
                this.SetEditorFatherChristianName(personsDataTable.FatherChristianName);
                this.SetEditorFatherOccupation(personsDataTable.FatherOccupation);
                this.SetEditorFatherSurname(personsDataTable.FatherSurname);
                this.SetEditorDeathLocation(personsDataTable.DeathLocation);
                this.SetEditorDeathCountyLocation(personsDataTable.DeathCounty);
                this.SetEditorDateDeathString(personsDataTable.DeathDateStr);
                this.SetEditorBirthLocation(personsDataTable.BirthLocation);
                this.SetEditorBirthCountyLocation(personsDataTable.BirthCounty);
                this.SetEditorDateBirthString(personsDataTable.BirthDateStr);
                this.SetEditorDateBapString(personsDataTable.BaptismDateStr);
                this.SetEditorReferenceDate(personsDataTable.ReferenceDateStr);
                this.SetEditorReferenceLocation(personsDataTable.ReferenceLocation);
                this.SetEditorSpouseCName(personsDataTable.SpouseName);
                this.SetEditorSpouseSName(personsDataTable.SpouseSurname);
                this.SetEditorBirthLocationId(personsDataTable.BirthLocationId);
                this.SetEditorDeathLocationId(personsDataTable.DeathLocationId);
                this.SetEditorReferenceLocationId(personsDataTable.ReferenceLocationId);
                this.SetEditorOccupation(personsDataTable.Occupation);
                this.SetEditorEventPriority(personsDataTable.EventPriority);
                this.SetEditorTotalEvents(personsDataTable.TotalEvents);
                this.SetEditorUniqueRef(personsDataTable.UniqueRef);

                //this.SetEditorBirthEstBirthInt(personsDataTable.EstBirthYearInt.ToString());
                //this.SetEditorBirthEstDeathInt(personsDataTable.EstDeathYearInt.ToString());

                //this.SetEditorBirthIsEstBirth(personsDataTable.IsEstBirth);
                //this.SetEditorBirthIsEstDeath(personsDataTable.IsEstDeath);


                this.SetFilterOriginalFatherName(personsDataTable.OrigFatherSurname);
                this.SetFilterOriginalMotherName(personsDataTable.OrigMotherSurname);
                this.SetFilterOriginalName(personsDataTable.OrigSurname);

            }
            else
            {
                ResetModel();
            
            }

         
           // DsSourceMappings.SourceMappingsDataTable sourcesDataTable = null;
         //   var sourcesDataTable = _SourceMappingsBLL.GetByMarriageIdOrPersonId2(this.SelectedRecordId);

            if (this.SelectedRecordId != Guid.Empty)
            {
                SourceMappingsBLL _SourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();

                var idlist = _SourceMappingsBLL.GetByMarriageIdOrPersonId2(this.SelectedRecordId);

                var selection = idlist.Where(s=>s.Source != null).Select(o => o.Source.SourceId).ToList();

                this.SetSourceGuidList(selection);
            }


            if (personsDataTable != null )
            {
                this.NotifyObservers<DeathBirthEditorModel>(this);        
            }


        }

        private void ResetModel()
        {
            this.SetEditorChristianName("");
            this.SetEditorSurnameName("");
            this.SetEditorSource("");
            this.SetEditorNotes("");
            this.SetEditorIsMale(false);
            this.SetEditorMotherChristianName("");
            this.SetEditorMotherSurname("");
            this.SetEditorFatherChristianName("");
            this.SetEditorFatherOccupation("");
            this.SetEditorFatherSurname("");
            this.SetEditorDeathLocation("");
            this.SetEditorDeathCountyLocation("");
            this.SetEditorDateDeathString("");
            this.SetEditorBirthLocation("");
            this.SetEditorBirthCountyLocation("");
            this.SetEditorDateBirthString("");
            this.SetEditorDateBapString("");
            this.SetEditorReferenceDate("");
            this.SetEditorReferenceLocation("");
            this.SetEditorSpouseCName("");
            this.SetEditorSpouseSName("");
            this.SetEditorBirthLocationId(Guid.Empty);
            this.SetEditorDeathLocationId(Guid.Empty);
            this.SetEditorReferenceLocationId(Guid.Empty);
            this.SetEditorOccupation("");
            this.SetEditorEventPriority(0);
            this.SetEditorTotalEvents(0);
            this.SetEditorUniqueRef(Guid.Empty);

            this.SetFilterOriginalFatherName("");
            this.SetFilterOriginalMotherName("");
            this.SetFilterOriginalName("");
        }

        public override void DeleteSelectedRecords()
        {
            if (!IsValidDelete()) return;


            if (IsValidSelectedRecordId)
            {
                //if (DialogResult.OK == MessageBox.Show("WARNING", "Ok to Delete?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
               // {
                    BLL.DeathsBirthsBLL deathBirthDLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();


                    deathBirthDLL.DeleteDeathBirthRecord2(this.SelectedRecordId);
              //  }
            }
            else
            {
               // MessageBox.Show("WARNING", "Invalid Marriage ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.NotifyObservers<DeathBirthEditorModel>(this);
        }

        public override void EditSelectedRecord()
        {

            if (!IsValidEdit()) return;

            if (this.IsValidEntry)
            {
                

                BLL.DeathsBirthsBLL deathBirthDLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();

                //these should defaulted to 1 and 1
                if (this.totalEvents == 0 && this.eventPriority == 0)
                {
                    this.totalEvents = 1;
                    this.eventPriority = 1;
                }

                deathBirthDLL.UpdateBirthDeathRecord2(
                    this.SelectedRecordId, 
                    isMale, christianName,
                    surname, 
                    birthLocation,
                    dateBirthString,
                    dateBapString, 
                    dateDeathString, 
                    this.referenceDateString,
                    deathLocation,
                    fatherChristianName,
                    fatherSurname, 
                    motherChristianName, 
                    motherSurname, 
                    notes, 
                    source,
                    birthInt, 
                    bapInt,
                    deathInt,
                    this.referenceDateInt,
                    this.referenceLocation,
                    this.birthCountyLocation,
                    this.deathCountyLocation,
                    this.occupation,
                    this.fatherOccupation,
                    this.spouseCName,
                    this.spouseSName,
                    this.SelectedUserId,
                    
                    birthLocationId,
                    deathLocationId,
                    referenceLocationId,
                    this.totalEvents,
                    this.eventPriority,
                    this.uniqueRef,this.estBirthInt,this.estDeathInt,this.isEstBirth,this.isEstDeath, 
                    this.originalName, this.originalFatherName, this.originalMotherName);


                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();
                sourceMappingBll.WritePersonSources2(this.SelectedRecordId, this.SourceGuidList, this.SelectedUserId);

                base.EditSelectedRecord();

            }
                this.NotifyObservers<DeathBirthEditorModel>(this);
        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            if (this.IsValidEntry)
            {
                BLL.DeathsBirthsBLL deathBirthDLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();

                // check if we have a id or not
                // if we dont its not been saved yet
                // get any mapped source ids from the mapping control 
                // and add them here!!

                if (this.SelectedRecordId == Guid.Empty)
                { 
                  //  this.
                }

                if (this.totalEvents == 0 && this.eventPriority == 0)
                {
                    this.totalEvents = 1;
                    this.eventPriority = 1;
                }

                this.SetSelectedRecordId(
                    deathBirthDLL.InsertDeathBirthRecord2(
                    isMale, 
                    christianName, surname, 
                    birthLocation, 
                    dateBirthString, dateBapString, dateDeathString, 
                    deathLocation, 
                    fatherChristianName, fatherSurname, 
                    motherChristianName, motherSurname, 
                    source, notes, 
                    birthInt, bapInt, deathInt,this.birthCountyLocation,this.deathCountyLocation,
                    this.occupation,
                    this.referenceLocation,
                    this.referenceDateString,this.referenceDateInt,
                    this.spouseCName,this.spouseSName,
                    this.fatherOccupation,
                    this.birthLocationId,
                    this.SelectedUserId,
                    this.deathLocationId,
                    this.referenceLocationId,
                    this.totalEvents,
                    this.eventPriority,
                    this.uniqueRef,this.estBirthInt,this.estDeathInt,this.isEstBirth,this.isEstDeath,this.originalName,this.originalFatherName,this.originalMotherName));

                SourceMappingsBLL sourceMappingBll = new SourceMappingsBLL();
                sourceMappingBll.WritePersonSources2(this.SelectedRecordId, this.SourceGuidList, this.SelectedUserId);

                this.OnDataSaved();

                this.NotifyObservers<DeathBirthEditorModel>(this);
            }
        }

        public override void SetFromQueryString(string param)
        {
            Debug.WriteLine("editor model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);
            Guid selectedId = Guid.Empty;
          
            Guid.TryParse(query["id"],out selectedId);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            this.SetSelectedRecordId(selectedId);

         //   this.Refresh();

        }

       
    }
}
