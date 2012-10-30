using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using GedItter.MarriageRecords;
////using TDBCore.Datasets;
using GedItter.ModelObjects;
using GedItter.BirthDeathRecords.BLL;
using TDBCore;
using System.Diagnostics;
//using GedItter.DLL;
using TDBCore.BLL;
using TDBCore.ModelObjects;
using TDBCore.Types;
using TDBCore.EntityModel;
using GedItter.BLL;
using System.Collections.Specialized;
using System.Web;


namespace GedItter.BirthDeathRecords
{
    public class DeathBirthFilterModel : EditorBaseModel<Guid>, IDeathBirthFilterModel
    {
        public EventHandler ShowDialogDuplicatesEvent;
        public EventHandler ShowDialogRelationsEvent;

        DeathBirthEditorModel deathBirthEditorModel = new DeathBirthEditorModel();
        ServicePersonObject personsDataTable = null;

       // private ArrayList aList = new ArrayList();
        string location = "";
        string source = "";
        string cName = "";
        string sName = "";
        string moCName = "";
        string moSName = "";
        string faCName = "";
        string faSName = "";
        string birthLower = "";
        string birthUpper = "";
        string deathLower = "";
        string deathUpper = "";
        string county = "";

        int birthLowerInt = 0;

        int birthUpperInt = 0;

        int deathLowerInt = 0;

        int deathUpperInt = 0;


        int lowerDate = 0;
        int upperDate = 0;
        string combinedlocation = "";

        bool isValidBirthUpperBound = false;
        bool isValidBirthLowerBound = false;
        bool isValidDeathUpperBound = false;
        bool isValidDeathLowerBound = false;

        string birthCountyLocation = "";
        string deathCountyLocation = "";
        string deathLocation = "";
        IDeathBirthEditorUI iDeathBirthEditorUI = null;

        string spouseCName = "";
        string spouseSName = "";
        string fatherOccupation = "";
        bool filterTreeResults = true;

        
    //    DsRelationTypes.RelationTypesDataTable relationTypesDataTable = new DsRelationTypes.RelationTypesDataTable();
        DeathBirthFilterTypes deathBirthFilterTypes = DeathBirthFilterTypes.SIMPLE;
        List<int> relations = new List<int>();
        int relationTypeId = 0;
        ExportToHtml exportToHtml = null;
        string reportLocation = "";
        bool isIncludeBirths = false;
        bool isIncludeDeaths = false;



        public bool IsIncludeDeaths
        {
            get { return isIncludeDeaths; }
            
        }


        public bool IsIncludeBirths
        {
            get { return isIncludeBirths; }
            
        }

        public void SetFilterIsIncludeDeaths(bool param)
        {
            if (this.isIncludeDeaths != param)
            {
                this.isDataChanged = true;
                this.isIncludeDeaths = param;
            }
        }

        public void SetFilterIsIncludeBirths(bool param)
        {
            if (this.isIncludeBirths != param)
            {
                this.isDataChanged = true;
                this.isIncludeBirths = param;
            }
        }




        public DeathBirthFilterModel()
        {
            //RelationsBLL relationsBLL = new RelationsBLL();
            //relationTypesDataTable = relationsBLL.GetRelationTypes();

            this.SetFilterLowerBirth("1550");
            this.SetFilterUpperBirth("1850");
            this.SetFilterTreeResults(false);
            this.SetFilterIsIncludeBirths(true);
            this.SetFilterIsIncludeDeaths(false);
        }

        #region IDeathBirthFilterModel Members

        #region validation properties

        public override bool IsValidEntry
        {
            get
            {
                if (location == "" && source == "" && cName == "" && sName == "" &&
                moCName == "" && moSName == "" && faCName == "" && faSName == "" 
                && this.ParentRecordIds.FirstOrDefault() == Guid.Empty &&
                    this.deathBirthFilterTypes != DeathBirthFilterTypes.TREE)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsValidBirthUpperBound
        {
            get 
            { 
                return this.isValidBirthUpperBound; 
            }
        }

        public bool IsValidBirthLowerBound
        {
            get
            {
                return this.isValidBirthLowerBound;
            }
        }

        public bool IsValidDeathUpperBound
        {
            get
            {
                return this.isValidDeathUpperBound;
            }
        }

        public bool IsValidDeathLowerBound
        {
            get
            {
                return this.isValidDeathLowerBound;
            }
        }

        #endregion

        public ServicePersonObject PersonsDataTable
        {
            get
            {
                if (this.personsDataTable == null)
                    return new ServicePersonObject();
                else
                    return this.personsDataTable;
            }
        }

        public IDeathBirthEditorUI IDeathBirthEditorUI
        {
            get
            {
                return this.iDeathBirthEditorUI;
            }
        }

        #region filter props


        public IDeathBirthEditorModel DeathBirthEditorModel
        {
            get
            {
                return this.deathBirthEditorModel;
            }

        }

        public string FilterSpouseCName
        {
            get
            {
                return this.spouseCName;
            }
        }

        public string FilterSpouseSName
        {
            get
            {
                return this.spouseSName;
            }
        }

        public string FilterFatherOccupation
        {
            get
            {
                return this.birthUpper;
            }
        }

      


        //public DsRelationTypes.RelationTypesDataTable RelationTypesDataTable
        //{
        //    get
        //    {
        //        return this.relationTypesDataTable;
        //    }
        //}

        public DeathBirthFilterTypes FilterMode
        {
            get
            {
                return this.deathBirthFilterTypes;
            }
        }


        public List<int> FilterRelations
        {
            get 
            {
                return this.relations;
            }
        }

        public string FilterUpperBirth
        {
            get 
            {
                return this.birthUpper;
            }
        }

        public string FilterLowerBirth
        {
            get
            {
                return this.birthLower;
            }
        }

        public string FilterUpperDeath
        {
            get
            {
                return this.deathUpper;
            }
        }

        public string FilterLowerDeath
        {
            get
            {
                return this.deathLower; ;
            }
        }



        public string FilterCName
        {
            get
            {
                return this.cName;
            }

        }

        public string FilterSName
        {
            get
            {
                return this.sName;
            }
        }

        public string FilterMotherCName
        {
            get
            {
                return this.moCName;
            }
        }

        public string FilterMotherSName
        {
            get
            {
                return this.moSName;
            }
        }

        public string FilterFatherCName
        {
            get
            {
                return this.faCName;
            }
        }

        public string FilterFatherSName
        {
            get
            {
                return this.faSName;
            }
        }

        public string FilterLocation
        {
            get
            {
                return this.location;
            }
        }

        public string FilterSource
        {
            get
            {
                return this.source;
            }
        }



        public string FilterCountyLocation
        {
            get
            {
                return this.birthCountyLocation;
            }
        }

        public string FilterDeathCountyLocation
        {
            get 
            {
                return this.deathCountyLocation;
            }
        }


        public string FilterDeathLocation
        {
            get
            {
                return this.deathLocation;
            }
        }


        public int RelationTypeId
        {
            get
            {
                return this.relationTypeId;
            }
        }

        #endregion

        #region set filters


        public bool FilterTreeResults
        {
            get
            {
                return this.filterTreeResults;
            }
        }

        public void SetFilterTreeResults(bool param)
        {
            if (this.filterTreeResults != param)
            {
                this.isDataChanged = true;
                this.filterTreeResults = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterSpouseCName(string param)
        {
            if (this.spouseCName != param)
            {
                this.isDataChanged = true;
                this.spouseCName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterSpouseSName(string param)
        {
            if (this.spouseSName != param)
            {
                this.isDataChanged = true;
                this.spouseSName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterFatherOccupation(string param)
        {
            if (this.fatherOccupation != param)
            {
                this.isDataChanged = true;
                this.fatherOccupation = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMode(DeathBirthFilterTypes param)
        {
            if (deathBirthFilterTypes != param)
            {
                this.isDataChanged = true;
                deathBirthFilterTypes = param;
                this.SetModelStatusFields();
            }
            
          //  this.Refresh();
        }

        public void SetFilterDeathLocation(string param)
        {
            if (this.deathLocation != param)
            {
                this.isDataChanged = true;
                this.deathLocation = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterDeathCountyLocation(string param)
        {
            if (this.deathCountyLocation != param)
            {
                this.isDataChanged = true;
                this.deathCountyLocation = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterCountyLocation(string param)
        {
            if (this.birthCountyLocation != param)
            {
                this.isDataChanged = true;
                this.birthCountyLocation = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterUpperBirth(string param)
        {
            if (this.birthUpper != param)
            {
                int _birthYear = 0;
                if (Int32.TryParse(param, out _birthYear))
                {
                    this.isValidBirthUpperBound = true;
                    this.birthUpperInt = _birthYear;
                    this.birthUpper = param;
                }
                else
                {
                    this.isValidBirthUpperBound = false;
                }

                this.isDataChanged = true;
                this.SetModelStatusFields();
            }


        }

        public void SetFilterLowerBirth(string param)
        {
            if (this.birthLower != param)
            {
                int _birthYear = 0;
                if (Int32.TryParse(param, out _birthYear))
                {
                    this.isValidBirthLowerBound = true;
                    this.birthLowerInt = _birthYear;
                    this.birthLower = param;
                }
                else
                {
                    this.isValidBirthLowerBound = false;
                }

                this.isDataChanged = true;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterUpperDeath(string param)
        {
            if (this.deathUpper != param)
            {
                int _birthYear = 0;
                if (Int32.TryParse(param, out _birthYear))
                {
                    this.isValidDeathUpperBound = true;
                    this.deathUpperInt = _birthYear;
                    this.deathUpper = param;
                }
                else
                {
                    this.isValidDeathUpperBound = false;
                }

                this.isDataChanged = true;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterLowerDeath(string param)
        {
            if (this.deathLower != param)
            {
                int _birthYear = 0;
                if (Int32.TryParse(param, out _birthYear))
                {
                    this.isValidDeathLowerBound = true;
                    this.deathLowerInt = _birthYear;
                    this.deathLower = param;
                }
                else
                {
                    this.isValidDeathLowerBound = false;
                }

                this.isDataChanged = true;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterCName(string param)
        {
            param = param.Replace(",","");

            if (this.cName != param)
            {
                this.isDataChanged = true;
                this.cName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterSName(string param)
        {
            if (this.sName != param)
            {
                this.isDataChanged = true;
                this.sName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMotherCName(string param)
        {
            if (this.moCName != param)
            {
                this.isDataChanged = true;
                this.moCName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMotherSName(string param)
        {
            if (this.moSName != param)
            {
                this.isDataChanged = true;
                this.moSName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterFatherCName(string param)
        {
            if (this.faCName != param)
            {
                this.isDataChanged = true;
                this.faCName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterFatherSName(string param)
        {
            if (this.faSName != param)
            {
                this.isDataChanged = true;
                this.faSName = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterLocation(string param)
        {
            if (this.location != param)
            {
                this.isDataChanged = true;
                this.location = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterSource(string param)
        {
            if (this.source != param)
            {
                this.isDataChanged = true;
                this.source = param;
                this.SetModelStatusFields();
            }
        }

        public void SetRelation(List<int> typeId)
        {
            //throw new NotImplementedException();
            if (typeId != null)
            {
                this.relations = typeId;
                this.isDataChanged = true;
            }

        }

        public void SetRelation(int typeId)
        {
            if (this.relations.Contains(typeId))
                this.relations.Remove(typeId);
            else
                this.relations.Add(typeId);

            this.isDataChanged = true;
        }

        public void SetEditorUI()
        {
            //   this.iDeathBirthEditorUI = new Forms.FrmDeathBirthEditor();
        }

        public void SetEditorUI(IDeathBirthEditorUI paramIDeathBirthEditorUI)
        {
            this.iDeathBirthEditorUI = paramIDeathBirthEditorUI;
        }


        public void SetShowDialogDupes(EventHandler paramEventHandler)
        {
            ShowDialogDuplicatesEvent = paramEventHandler;

        }

        public void SetShowDialogRels(EventHandler paramEventHandler)
        {
            ShowDialogRelationsEvent = paramEventHandler;
        }

        #endregion


        void deathBirthEditorModel_DataSaved(object sender, EventArgs e)
        {
            this.Refresh();
        }

 
        #endregion



        public void ViewDuplicates()
        {
            if (!IsvalidSelect()) return;

            if (this.ShowDialogDuplicatesEvent != null)
                this.ShowDialogDuplicatesEvent(this, new EventArgs());
        }

        public void ViewRelations()
        {
            if (!IsvalidSelect()) return;

            if (this.ShowDialogRelationsEvent != null)
                this.ShowDialogRelationsEvent(this, new EventArgs());
        }





        public override void Refresh()
        {
            Debug.WriteLine("DataBirthFilterModel Refresh - data changed " + IsDataChanged.ToString());

            if (this.IsValidEntry && isDataChanged)
            {

                if (!IsvalidSelect()) return;


                IList<Person> tempTable = new List<Person>();        

                BLL.DeathsBirthsBLL deathsBirthsBLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();

                switch (this.deathBirthFilterTypes)
                {

                    case DeathBirthFilterTypes.TREE:

                         List<ServicePersonLookUp> personsInTree = new List<ServicePersonLookUp>();

                           
                      
 
                        int startInt = this.birthLowerInt;
                        int endInt = this.birthUpperInt;

                        tempTable = deathsBirthsBLL.GetBySourceId2(this.SelectedRecordIds[0]).Where(ss => ss.BirthInt >= startInt
                            && ss.BirthInt <= endInt).OrderBy(o => o.Surname).ThenBy(t => t.BirthInt).ToList();


                            //.Select(s => new ServicePersonLookUp()
                            //{
                            //    PersonId = s.Person_id,
                            //    ChristianName = s.ChristianName,
                            //    Surname = s.Surname,
                            //    BirthYear = s.BirthInt,
                            //    BirthLocation = s.BirthLocation,
                            //    XREF = s.OrigSurname

                            //}).ToList();


                        break;


                    case DeathBirthFilterTypes.RELATIONS:

                        if (this.ParentRecordIds.Count > 0)
                        {
                            tempTable = deathsBirthsBLL.GetRelationDeathBirthRecord2(this.ParentRecordIds[0]).ToList();
                        }
                        else
                        {
                            personsDataTable = new ServicePersonObject();
                        }
                        break;
                    case DeathBirthFilterTypes.DUPLICATES:

                        if (this.ParentRecordIds.Count > 0 && this.ParentRecordIds[0] != Guid.Empty)
                        {
                            tempTable = deathsBirthsBLL.GetDataByDupeRef2(this.ParentRecordIds[0]).ToList();
                        }
                        else
                        {
                            personsDataTable = new ServicePersonObject();
                        }

                        break;
                    case DeathBirthFilterTypes.STANDARD:
                        tempTable = deathsBirthsBLL.GetFilterDeathBirthRecord2(this.cName,
                            this.sName,
                            this.location,
                            this.birthLowerInt,
                            this.birthUpperInt,
                            0,
                            2000,
                            this.deathLowerInt,
                            this.deathUpperInt,
                            this.deathLocation,
                            this.faCName,
                            faSName,
                            moCName,
                            moSName,
                            source,
                            this.birthCountyLocation,
                            this.deathCountyLocation, this.spouseCName, this.spouseSName, this.fatherOccupation).OrderBy(o=>o.EstBirthYearInt).ToList();
                        break;

                    case DeathBirthFilterTypes.SIMPLE:
                        this.lowerDate = this.deathLowerInt;
                        this.upperDate = this.deathUpperInt;


                        this.county = this.deathCountyLocation;

                        if (this.birthCountyLocation != "")
                            this.county = this.birthCountyLocation;

                        if (this.lowerDate == 0 && this.upperDate == 0)
                        {
                            this.lowerDate = this.birthLowerInt;
                            this.upperDate = this.birthUpperInt;
                        }


                        if (this.lowerDate == 0 && this.upperDate == 0 && this.source != "")
                        {
                            this.lowerDate = 1550;
                            this.upperDate = 1850;
                        }


                        this.combinedlocation = this.deathLocation;

                        if (this.combinedlocation == "")
                            this.combinedlocation = this.location;


                        tempTable = deathsBirthsBLL.GetFilterSimple2(this.cName, this.sName, this.combinedlocation, this.lowerDate, this.upperDate, this.faCName, this.faSName, this.moCName, this.moSName, this.SourceGuidListAsString, this.county, this.spouseCName).OrderBy(o => o.EstBirthYearInt).ToList();



                        if (SourceGuidList.Count == 0)
                        {
                            if (filterTreeResults)
                            {
                              //  int rowIdx = 0;

                                tempTable = tempTable.Where(p => !p.OrigSurname.ToLower().Contains("xref")).ToList();

                                //while (rowIdx < personsDataTable.Count)
                                //{
                                //    if (personsDataTable[rowIdx].OrigSurname.ToLower().Contains("xref"))
                                //        personsDataTable[rowIdx].Delete();
                                //    rowIdx++;
                                //}

                             //   personsDataTable.AcceptChanges();
                            }
                        }


                        if (isIncludeDeaths)
                        {
                            tempTable = tempTable.Where(p => p.IsEstDeath == false).ToList();

                            //foreach (var _row in personsDataTable.Where(o => o.IsEstDeath.Value))
                            //{
                            //    _row.Delete();
                            //}
                        }

                        if (isIncludeBirths)
                        {
                            tempTable = tempTable.Where(p => p.IsEstBirth == false).ToList();

                            //foreach (var _row in personsDataTable.Where(o => o.IsEstBirth.Value))
                            //{
                            //    _row.Delete();
                            //}
                        }

                      //  personsDataTable.AcceptChanges();

                        break;

                }


                this.personsDataTable = tempTable.ToServicePersonObject(this.SortColumn, this.RecordPageSize, this.RecordStart);

                this.isDataChanged = false;

            }
            
            
       //     UpdateDateEstimates();
            this.NotifyObservers<DeathBirthFilterModel>(this);
        }
        



        public override void DeleteSelectedRecords()
        {
            if (!IsValidDelete()) return;

            BLL.DeathsBirthsBLL deathsBirthsBLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();

            foreach (Guid deathbirthRecordId in this.SelectedRecordIds)
            {
                deathsBirthsBLL.DeleteDeathBirthRecord2(deathbirthRecordId);
            }

            this.SetSelectedRecordIds(new List<Guid>());
            this.isDataChanged = true;
            Refresh();
        }

        public override void EditSelectedRecord()
        {

            if (!IsvalidSelect()) return;

            deathBirthEditorModel = new DeathBirthEditorModel();
            //this.deathBirthEditorModel.DataSaved += new EventHandler(deathBirthEditorModel_DataSaved);
            base.SetDataSaved(new EventHandler(deathBirthEditorModel_DataSaved));

            deathBirthEditorModel.SetSelectedRecordIds(this.SelectedRecordIds);
            this.isDataChanged = true;
            this.ShowDialogEdit(this);
        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            deathBirthEditorModel = new DeathBirthEditorModel();
            //this.deathBirthEditorModel.DataSaved += new EventHandler(deathBirthEditorModel_DataSaved);
            this.SetDataSaved(new EventHandler(deathBirthEditorModel_DataSaved));
         
            this.deathBirthEditorModel.SetEditorBirthCountyLocation(this.birthCountyLocation);
            this.deathBirthEditorModel.SetEditorBirthLocation(this.location);
            this.deathBirthEditorModel.SetEditorChristianName(this.cName);
            this.deathBirthEditorModel.SetEditorSurnameName(this.sName);

            this.deathBirthEditorModel.SetEditorDeathCountyLocation(this.deathCountyLocation);
            this.deathBirthEditorModel.SetEditorDeathLocation(this.deathLocation);

            this.deathBirthEditorModel.SetEditorMotherChristianName(this.moCName);
            this.deathBirthEditorModel.SetEditorMotherSurname(this.moSName);
            this.deathBirthEditorModel.SetEditorFatherChristianName(this.faCName);
            this.deathBirthEditorModel.SetEditorFatherSurname(this.faSName);

            this.deathBirthEditorModel.SetSourceGuidList(this.SourceGuidList);
            this.deathBirthEditorModel.SetSelectedRecordId(System.Guid.Empty);

            this.isDataChanged = true;

            ShowDialogInsert(this);

        }

        public void DeleteRelation()
        {
            if (!IsValidDelete()) return;

            if (this.relations.Count > 1)
            {
                RelationsBLL relationsBll = new RelationsBLL();

                foreach (int personId in this.relations)
                {
                    relationsBll.DeleteRelationMapping(personId);
                }
            }

            this.isDataChanged = true;
        }

        public void SetRelationTypeId(int relationTypeId)
        {
            Debug.WriteLine("SetRelationTypeId begin:" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);

            if (!IsValidEdit()) return;

            // mark the hidden unique referenced entries as well
            if (relationTypeId != this.relationTypeId)
            {
                this.relationTypeId = relationTypeId;
            }

            if (this.SelectedRecordIds.Count > 1)
            {
                RelationsBLL relationsBll = new RelationsBLL();

                Guid personA = this.SelectedRecordIds[0];

                int idx = 1;

                List<int> mewRpws = new List<int>();

                while (idx < this.SelectedRecordIds.Count)
                {
                    mewRpws.Add(relationsBll.InsertRelation(personA, this.SelectedRecordIds[idx], this.relationTypeId, this.SelectedUserId));
                    idx++;
                }

                if (relationTypeId == 1)
                {
                    foreach (int relMapId in mewRpws)
                    {
                        foreach (var _rrow in relationsBll.GetRelationsById2(relMapId).ToList())
                        {
                            DeathsBirthsBLL _DeathsBirthsBLL = new DeathsBirthsBLL();
                            _DeathsBirthsBLL.UpdateDuplicateRefs2(_rrow.PersonA.Person_id, _rrow.PersonB.Person_id);
                        }
                    }
                }


            }
            else
            {
                this.SetErrorState("You need to select more than source!");
            }

            this.isDataChanged = true;
            base.SetSelectedRecordIds(new List<Guid>());
            this.Refresh();          
        }




        public string SetDefaultPersonForTree(Guid param)
        {
            if (!IsValidEdit()) return "false";

            SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();

            if (this.SelectedRecordIds.Count > 1)
            {
                sourceMappingsBll.SetDefaultTreePerson(param, this.SelectedRecordIds[0]);

                this.isDataChanged = true;
                return "true";
            }
            else
            {
                return "false";
            } 
        }

        //public List<ServicePersonLookUp> GetJSONTreePeople(string sourceId, string start, string end)
        //{
        //    DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();

        //    return deathsBirthsBLL.GetPersonsForTree(sourceId, start, end);
        //}




        public void RemoveAllRelationType()
        { 
            if (!IsValidEdit()) return;



            if (this.SelectedRecordIds.Count > 0)
            {
                DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();
                DeathsBirthsBLL deathsBirthsDLL = new DeathsBirthsBLL();


                RelationsBLL relationsBll = new RelationsBLL();
                relationsBll.DeleteRelationMapping(this.SelectedRecordIds);


                Guid personA = this.SelectedRecordIds[0];

                foreach (var pRow in deathsBirthsBLL.GetUniqRefDuplicates(personA).ToList())
                {
                    deathsBirthsDLL.UpdateUniqueRefs2(pRow.Person_id, Guid.NewGuid(), 1, 1);
                }


                this.isDataChanged = true;
                this.Refresh();
            }
            
        }



        public void RemoveRelationType()
        {
          //  Debug.WriteLine("SetRelationTypeId begin:" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);

            if (!IsValidEdit()) return;

        

            if (this.SelectedRecordIds.Count > 0)
            {
                DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();
                DeathsBirthsBLL deathsBirthsDLL = new DeathsBirthsBLL();     
                

                RelationsBLL relationsBll = new RelationsBLL();

                Guid personA = this.SelectedRecordIds[0];



             
                relationsBll.DeleteRelationMapping(this.SelectedRecordIds);


                // what we are left with is the identical unique refs in the persons table
                // remember we are deleting family links as well so this wont apply to them!
                //find the unique ref for the two people
                // look up everyone with that unique ref give our 2 people we are detaching new unique refs 
                // and update the count of the remaining ones



                List<Guid> peopleToKeep = new List<Guid>();
                


                // get list of unique refs if they exist for the 2 persons

          
                 Guid newRef = Guid.NewGuid();

                int evtCount = 1;               



                foreach (var pRow in deathsBirthsBLL.GetUniqRefDuplicates(personA))
                {
                    if (!SelectedRecordIds.Contains(pRow.Person_id))
                    {
                        peopleToKeep.Add(pRow.Person_id);


                    }
                }

                foreach (Guid id_ in peopleToKeep)
                {
                    deathsBirthsDLL.UpdateUniqueRefs2(id_, newRef, peopleToKeep.Count, evtCount);
                    evtCount++;
                }

                List<Guid> parentGuids = new List<Guid>();
                parentGuids.Add(newRef);

                this.SetParentRecordIds(parentGuids);

                // records to remove done here
                foreach (Guid personToRemove in SelectedRecordIds)
                {
                    newRef = Guid.NewGuid();
                    deathsBirthsDLL.UpdateUniqueRefs2(personToRemove, newRef, 1, 1);
                }


                if (peopleToKeep.Count == 0)
                {
                    parentGuids.Clear();
                    parentGuids.Add(newRef);
                    this.SetParentRecordIds(newRef);
                }

            }

            this.isDataChanged = true;
            this.Refresh();

        
        }



        public void UpdateDateEstimates()
        {
            CsUtils.UpdateDateEstimates();
           //// if (!IsValidEdit()) return;


           // DsDeathsBirths.PersonsDataTable dsDeathsBirths = new DsDeathsBirths.PersonsDataTable();
           // DsDeathsBirths.PersonsDataTable dsDBTemp = new DsDeathsBirths.PersonsDataTable();
           // DeathsBirthsBLL deathsBirthsBLL = new DeathsBirthsBLL();
           // BirthsDeathsDLL deathsBirthsDLL = new BirthsDeathsDLL();


           // dsDeathsBirths = deathsBirthsBLL.GetDeathsBirths();
           // //dsDeathsBirths = deathsBirthsDLL.FetchBirths("d122a203-c53a-4d35-9f4f-e58aa0e2903d");

           // foreach (DsDeathsBirths.PersonsRow pRow in dsDeathsBirths)
           // {

           //     int estBirthYear = 0;
           //     int estDeathYear = 0;
           //     bool isEstBirth = false;
           //     bool isEstDeath = false;


           //     CsUtils.CalcEstDates(pRow.BirthInt, pRow.BapInt, pRow.DeathInt, out estBirthYear,
           //         out estDeathYear, out isEstBirth, out isEstDeath, pRow.FatherChristianName, pRow.MotherChristianName);

           //     deathsBirthsBLL.UpdateBirthDeathRecord(pRow.Person_id, pRow.IsMale, pRow.ChristianName, pRow.Surname,
           //         pRow.BirthLocation, pRow.BirthDateStr, pRow.BaptismDateStr, pRow.DeathDateStr, pRow.ReferenceDateStr,
           //         pRow.DeathLocation, pRow.FatherChristianName, pRow.FatherSurname, pRow.MotherChristianName, pRow.MotherSurname,
           //         pRow.Notes, pRow.Source, pRow.BapInt, pRow.BirthInt, pRow.DeathInt, pRow.ReferenceDateInt, pRow.ReferenceLocation,
           //         pRow.BirthCounty, pRow.DeathCounty, pRow.Occupation, pRow.FatherOccupation, pRow.SpouseName, pRow.SpouseSurname,
           //         pRow.UserId, pRow.BirthLocationId, pRow.DeathLocationId, pRow.ReferenceLocationId, pRow.TotalEvents, pRow.EventPriority, pRow.UniqueRef,
           //         estBirthYear, estDeathYear, isEstBirth, isEstDeath, pRow.OrigSurname, pRow.OrigFatherSurname, pRow.OrigMotherSurname);

           // }

            this.isDataChanged = true;

            this.Refresh();
        }




        public void MergeDuplicates()
        {
            if (this.SelectedRecordId != Guid.Empty)
            {
                 DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
                 var record = deathsBirthsBll.GetDeathsBirths2().Where(o => o.Person_id == this.SelectedRecordId).FirstOrDefault();

                 MergeDuplicateRecord(record);
                 this.Refresh();
            }
        }


        public static void PathUpSources()
        {


            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
          //  BirthsDeathsDLL deathsBirthsDLL = new BirthsDeathsDLL();
           // DsDeathsBirths.PersonsDataTable pdt = deathsBirthsBll.GetDeathsBirths();
           // DsDeathsBirths.PersonsDataTable dsDBTemp = new DsDeathsBirths.PersonsDataTable();

        //    IEnumerable<DsDeathsBirths.PersonsRow> filtRows = null;

           
            // find all records which are IGI but IGI
            SourceMappingsBLL sourceMappingsBLL = new SourceMappingsBLL();
           // DsSourceMappings.SourceMappingsDataTable sourceMappingsDataTable = new DsSourceMappings.SourceMappingsDataTable();
            
           // Debug.WriteLine("row count: "+filtRows.Count());

            int row_count= 0;
            foreach (var pROw in deathsBirthsBll.GetDeathsBirths2().Where(o => o.Source == "IGI"))
            {

                Debug.WriteLine("cur_rec: "+row_count);
               // filtRows.Count<DsDeathsBirths.PersonsRow>
                //filtRows.Count(

               // sourceMappingsDataTable = sourceMappingsBLL.GetByMarriageIdOrPersonId(pROw.Person_id);

                foreach (var smr in sourceMappingsBLL.GetByMarriageIdOrPersonId2(pROw.Person_id))
                { 
                    //smr.MappingId

                    sourceMappingsBLL.DeleteBySourceIdMarriageIdOrPersonId(smr.Source.SourceId, smr.Person.Person_id);
                }

                List<Guid> sources = new List<Guid>();
                sources.Add(new Guid("bd938707-21fb-4d10-9ddd-72dbe6dd44f6"));
                sourceMappingsBLL.WritePersonSources2(pROw.Person_id, sources,1);

                row_count++;
            }


          
        }

        public static void UpdateDeletedBirths()
        {

            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
         //   BirthsDeathsDLL deathsBirthsDLL = new BirthsDeathsDLL();
            //DsDeathsBirths.PersonsDataTable pdt = deathsBirthsBll.GetDeathsBirths();
            //DsDeathsBirths.PersonsDataTable dsDBTemp = new DsDeathsBirths.PersonsDataTable();

            //IEnumerable<DsDeathsBirths.PersonsRow> filtRows = null;

        //    filtRows = pdt.Where(o => o.IsDeleted == true).Where(o=>o.TotalEvents > 1);
            //Debug.WriteLine("Number of deleted rows with more than 1 event: "+ filtRows.Count());

            foreach (var pROw in deathsBirthsBll.GetDeathsBirths2().Where(m=>m.IsDeleted == true && m.TotalEvents > 1))
            {
                List<Guid> peopleToKeep = new List<Guid>();

                var dsDBTemp = deathsBirthsBll.GetDeathBirthRecordById2(pROw.Person_id).FirstOrDefault();

                if (dsDBTemp != null)
                {
                    //dsDBTemp = deathsBirthsBll.GetDataByDupeRef2(dsDBTemp.UniqueRef);

                    foreach (var dupePerson in deathsBirthsBll.GetDataByDupeRef2(dsDBTemp.UniqueRef))
                    {
                        if (pROw.Person_id != dupePerson.Person_id)
                        {
                            peopleToKeep.Add(dupePerson.Person_id);
                        }
                    }
                }
                Guid newRef = Guid.NewGuid();

                int evtCount = 1;
                foreach (Guid id_ in peopleToKeep)
                {
                    deathsBirthsBll.UpdateUniqueRefs2(id_, newRef, peopleToKeep.Count, evtCount);
                    evtCount++;
                }

                newRef = Guid.NewGuid();
                deathsBirthsBll.UpdateUniqueRefs2(pROw.Person_id, newRef, 1, 1);
            }


           
        }

        public static void MergeDuplicateRecords()
        {
            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();
            
            var records = deathsBirthsBll.GetDeathsBirths2().Where(o => !o.IsDeleted && o.TotalEvents > 1 && o.EventPriority == 1).ToList();

     
            int idx = 0;
            foreach (Person newRecord in records)
            {
                MergeDuplicateRecord(newRecord);

                Debug.WriteLine(idx.ToString() + " of " + records.Count.ToString());
                idx++;
            }
        }


        public static void MergeDuplicateRecord(Person newRecord)
        {
            DeathsBirthsBLL deathsBirthsBll = new DeathsBirthsBLL();

            SourceBLL sourceBll = new SourceBLL();
            SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();

            
            List<Guid> sourceList = new List<Guid>();

            foreach (var dupePerson in deathsBirthsBll.GetUniqRefDuplicates(newRecord.Person_id))
            {
                sourceList.AddRange(sourceBll.FillSourceTableByPersonOrMarriageId2(dupePerson.Person_id).Select(dp => dp.SourceId).ToList());
                
                newRecord.MergeInto(dupePerson);
            }

          //  Guid newPersonId = deathsBirthsBll.InsertPerson(newRecord);


         


            sourceMappingsBll.WritePersonSources2(newRecord.Person_id, sourceList, 1);

            deathsBirthsBll.ModelContainer.SaveChanges();

           // deathsBirthsBll.DeleteDeathBirthRecord2(newRecord.Person_id);
             
        }



        public static void UpdateTidyBirthLocations()
        {
            // get all persons with no county
            // look in the countydictionary
            // and fill out the county using the dictionary
            LocationDictionaryBLL locationDictionaryBLL = new LocationDictionaryBLL();
          

            DeathsBirthsBLL deathBirthsBll = new DeathsBirthsBLL();

            Guid dummyLocationdId = new Guid("A813A1FF-6093-4924-A7B2-C5D1AF6FF699");



            foreach (var prow in deathBirthsBll.GetByBirthLocationId2(dummyLocationdId).Where(d=>d.BirthCounty!= "" && d.BirthLocation !="" && d.BirthLocation.ToLower() != "unknown" && d.BirthLocation.Contains(d.BirthCounty)))
            {
                char[] charsToTrim = { ',', '.', ' ' };
                string newLocation = prow.BirthLocation.Replace(prow.BirthCounty, "").TrimEnd(charsToTrim);

                newLocation = newLocation.Replace(",,", ",");

                deathBirthsBll.UpdateBirthLocation2(prow.Person_id, newLocation);
            }

           

        }


        public static void UpdateLocationIdsFromDictionary()
        { 
          
            LocationDictionaryBLL locationDictionaryBLL = new LocationDictionaryBLL();
   

            DeathsBirthsBLL deathBirthsBll = new DeathsBirthsBLL();

            Guid dummyLocationdId = new Guid("A813A1FF-6093-4924-A7B2-C5D1AF6FF699");

        //    DsDeathsBirths.PersonsDataTable personDataTable = deathBirthsBll.GetByBirthLocationId(dummyLocationdId);



            foreach (var prow in deathBirthsBll.GetByBirthLocationId2(dummyLocationdId).Where(p=>p.BirthLocation != ""))// personDataTable.Where(o => o.BirthLocation != ""))
            {

               
                var dictEntry = locationDictionaryBLL.GetEntryByLocatAndCounty(prow.BirthLocation, prow.BirthCounty);

                if (dictEntry != null)
                    deathBirthsBll.UpdateBirthLocationId(prow.Person_id, dictEntry.ParishId.Value);
            }

           
            
        }

        public static void UpdateLocationIdsFromParishTable()
        {

            UpdateTidyBirthLocations();

            LocationDictionaryBLL locationDictionaryBLL = new LocationDictionaryBLL();

            ParishsBLL parishsBll = new ParishsBLL();
          //  DsParishs.ParishsDataTable parishDataTable = new DsParishs.ParishsDataTable();
            DeathsBirthsBLL deathBirthsBll = new DeathsBirthsBLL();
         //   DsLocationDictionary.LocationDictionaryDataTable dtLocationDictionary = new DsLocationDictionary.LocationDictionaryDataTable();
            
            
            Guid dummyLocationdId = new Guid("A813A1FF-6093-4924-A7B2-C5D1AF6FF699");

           // DsDeathsBirths.PersonsDataTable personDataTable = deathBirthsBll.GetByBirthLocationId2(dummyLocationdId);

            List<string> counties = deathBirthsBll.GetByBirthLocationId2(dummyLocationdId).Select(o => o.BirthCounty).Distinct().ToList();
            int reccount = 0;
            foreach (string county in counties)
            {


              //  parishDataTable = parishsBll.GetParishsByCounty2(county);
             //   dtLocationDictionary = locationDictionaryBLL.GetDictionaryByCounty(county);


                foreach (var personRec in deathBirthsBll.GetByBirthLocationId2(dummyLocationdId).Where(o => o.BirthCounty == county))
                {
                    var var2 = parishsBll.GetParishsByCounty2(county).Where(p => personRec.BirthLocation.Contains(p.ParishName)).FirstOrDefault();

                    if (var2 != null)
                    {
                        deathBirthsBll.UpdateBirthLocationId(personRec.Person_id, var2.ParishId);
                        reccount++;
                    }
                    
                    var var3 = locationDictionaryBLL.GetEntryByLocatAndCounty(personRec.BirthLocation, county);
                    if (var3 != null)
                    {
                        deathBirthsBll.UpdateBirthLocationId(personRec.Person_id, var3.ParishId.Value);
                    }
                   
                }

                


            }

          //  LocationDictionaryBLL locationDictionaryBLL = new LocationDictionaryBLL();



            //foreach (DsDeathsBirths.PersonsRow prow in personDataTable.Where(o => o.BirthLocation != "").Where(p=>p.BirthCounty!=""))
            //{


            //}
            Debug.WriteLine(reccount.ToString());
        }

        public static void UpdateCounties()
        { 
            // find all persons with no locationid
            // look in the locationdictionary

            DeathsBirthsBLL deathBirthsBll = new DeathsBirthsBLL();

          
            CountyDictionaryBLL countyDictionaryBLL = new CountyDictionaryBLL();

           
      //      DsDeathsBirths.PersonsDataTable personDataTable = deathBirthsBll.GetByMissingBirthCounty();

            foreach (var personRow in deathBirthsBll.GetByMissingBirthCounty2())
            {
                var retVal = countyDictionaryBLL.GetDictionary2(personRow.BirthLocation);

               if (retVal != null && retVal.dictCounty != "")
               { 
                   deathBirthsBll.UpdateBirthCounty2(personRow.Person_id, retVal.dictCounty);
               }

            }



            //foreach(
        }


        public string FilteredPrintableResults
        {
            get 
            {

                return this.reportLocation;
            }
        }

        public void SetFilteredPrintableResults(string param, bool isTabular)
        {
            exportToHtml = new ExportToHtml( param);

            exportToHtml.ColumnsToIgnore.Add("Surname");
            //frmPrinting.ColumnsToIgnore.Add("FatherSurname");
            exportToHtml.ColumnsToIgnore.Add("ReferenceDateInt");
            exportToHtml.ColumnsToIgnore.Add("ReferenceDateStr");

            exportToHtml.ColumnsToIgnore.Add("ReferenceLocation");
            exportToHtml.ColumnsToIgnore.Add("Occupation");
            exportToHtml.ColumnsToIgnore.Add("OrigMotherSurname");
            exportToHtml.ColumnsToIgnore.Add("OrigFatherSurname");

            exportToHtml.ColumnsToIgnore.Add("OrigSurname");
            exportToHtml.ColumnsToIgnore.Add("Person_id");
            exportToHtml.ColumnsToIgnore.Add("BirthCounty");
            exportToHtml.ColumnsToIgnore.Add("DeathCounty");
            exportToHtml.ColumnsToIgnore.Add("Notes");
            exportToHtml.ColumnsToIgnore.Add("IsMale");
            exportToHtml.ColumnsToIgnore.Add("FatherId");
            exportToHtml.ColumnsToIgnore.Add("MotherId");
            exportToHtml.ColumnsToIgnore.Add("BirthInt");
            exportToHtml.ColumnsToIgnore.Add("BapInt");
            exportToHtml.ColumnsToIgnore.Add("DeathInt");
            exportToHtml.ColumnsToIgnore.Add("BirthDateStr");



            exportToHtml.SortColumn = "BirthInt";

            if (isTabular)
                reportLocation = exportToHtml.LoadStandardTabular();
            else
                reportLocation = exportToHtml.LoadStandardNotes();


        }


        public Guid MyGuid
        {
            get 
            {
               // DsDeathsBirths.PersonsDataTable personsDataTableLocal = new DsDeathsBirths.PersonsDataTable();
                BLL.DeathsBirthsBLL deathsBirthsBLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();

             //   personsDataTableLocal = deathsBirthsBLL.GetByXRef2();

                var pRow = deathsBirthsBLL.GetByXRef2().Where(o => o.ChristianName == "George Nicholas").Where(o => o.Surname == "Thackray").Where(o => o.BirthInt == 1978).FirstOrDefault();

                if (pRow != null)
                    return pRow.Person_id;
                else
                    return Guid.Empty;

            }
        }

        public Guid MyFurthestAncestorGuid
        {
            get 
            {
               // DsDeathsBirths.PersonsDataTable personsDataTableLocal = new DsDeathsBirths.PersonsDataTable();
                BLL.DeathsBirthsBLL deathsBirthsBLL = new GedItter.BirthDeathRecords.BLL.DeathsBirthsBLL();

              //  personsDataTableLocal = deathsBirthsBLL.GetByXRef();

                var pRow = deathsBirthsBLL.GetByXRef2().Where(o => o.ChristianName == "William").Where(o => o.Surname == "Thackray").Where(o => o.BirthInt == 1688).FirstOrDefault();

                if (pRow != null)
                    return pRow.Person_id;
                else
                    return Guid.Empty;
            }
        }


        public override void SetFromQueryString(string param)
        {

            Debug.WriteLine("filter model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            Guid parentId = new Guid(query["parent"] ?? Guid.Empty.ToString());

            if (parentId == Guid.Empty)
            {
                this.SetFilterMode(DeathBirthFilterTypes.SIMPLE);

                this.SetSelectedRecordIds(new List<Guid>());
                int pageNo = Convert.ToInt32(query["pg"] ?? "0");

                this.SetRecordStart(pageNo);


                this.SetFilterCName(query["cn"] ?? "");
                this.SetFilterSName(query["sn"] ?? "");
                this.SetFilterFatherCName(query["fc"] ?? "");
                this.SetFilterFatherSName(query["fs"] ?? "");
                this.SetFilterMotherCName(query["mc"] ?? "");
                this.SetFilterMotherSName(query["ms"] ?? "");
                this.SetFilterLocation(query["lc"] ?? "");
                this.SetFilterLowerBirth(query["ld"] ?? "");
                this.SetFilterUpperBirth(query["ud"] ?? "");


                if ((query["it"] ?? "") != "")
                {
                    if ((query["it"] ?? "").ToLower() == "false")
                        this.SetFilterTreeResults(false);
                    else
                        this.SetFilterTreeResults(true);
                }


                if ((query["incb"] ?? "") != "")
                {
                    if ((query["incb"] ?? "").ToLower() == "false")
                        this.SetFilterIsIncludeBirths(false);
                    else
                        this.SetFilterIsIncludeBirths(true);
                }


                if ((query["incd"] ?? "") != "")
                {
                    if ((query["incd"] ?? "").ToLower() == "false")
                        this.SetFilterIsIncludeDeaths(false);
                    else
                        this.SetFilterIsIncludeDeaths(true);
                }
            }
            else
            {
                this.SetSelectedRecordIds(new List<Guid>());
                this.SetParentRecordIds(parentId);

                this.SetFilterMode(DeathBirthFilterTypes.DUPLICATES);

            }
           
         //   this.Refresh();
        }
    }


    public enum DeathBirthFilterTypes
    { 
        RELATIONS =0,
        DUPLICATES =1,
        STANDARD =2,
        SIMPLE =3,
        TREE =4

    }
}
