using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
//using TDBCore.Datasets;
using GedItter.ModelObjects;
using GedItter.MarriageRecords.BLL;
using TDBCore.BLL;
using TDBCore.Types;
using System.Diagnostics;
using GedItter.BLL;
using System.Web;
using TDBCore.EntityModel;
using System.Collections.Specialized;


namespace GedItter.MarriageRecords
{
    public class MarriagesFilterModel : EditorBaseModel<Guid>, IMarriageFilterModel
    {
        // need reference to editor here
        // which should be an interface
        // to the editorcontroller
        // so we can set default values
        // 

        bool showDebug = true;

        public EventHandler ShowDialogDuplicatesEvent;
        public EventHandler ShowDialogRelationsEvent;

        MarriagesEditorModel marriageEditorModel = new MarriagesEditorModel();



        #region variables
        IEditorUI iMarriageEditorUI = null;
        private ArrayList aList = new ArrayList();
        private bool isValidUpperMarriagebound = false;
        private bool isValidLowerMarriagebound = false;
        private bool isValidDupeInt = false;
    
        private int filterUpperMarriage = 0;
        private int filterLowerMarriage = 0;
        private string filterMaleCName = "";
        private string filterFemaleCName = "";
        private string filterMaleSName = "";
        private string filterFemaleSName = "";
        private string filterLocationCounty = "";
        private string filterLocation = "";
        private string filterMaleLocation = "";
        private string filterFemaleLocation = "";
        private string filterSource = "";
        private string filterDupeInterval = "";
        private string filterWitness = "";
        private int filterDupeInt = 0;

        private MarriageFilterTypes marriageFilterTypes = MarriageFilterTypes.STANDARD;

        private IList<MarriageResult> marriagesTable = null;


        ExportToHtml exportToHtml = null;
        string reportLocation = "";

        #endregion


        #region props

        public override bool IsValidEntry
        {
            get
            {
                if (filterUpperMarriage == 0 && filterLowerMarriage == 0 && this.SourceGuidListAsString != "")
                {
                    return true;
                }
                else
                {
                    if ((filterMaleCName == "" && filterFemaleCName == "" && filterMaleSName == "" && filterFemaleSName == "" &&
                        filterLocationCounty == "" && filterLocation == "" && filterMaleLocation == "" && filterMaleLocation == "" &&
                        filterFemaleLocation == "" && filterSource == "") || (filterUpperMarriage == 0 && filterLowerMarriage == 0))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
        }

        public IList<MarriageResult> MarriagesTable
        {
            get 
            {
                if (this.marriagesTable == null)
                    return new List<MarriageResult>();
                else
                    return this.marriagesTable;
            }
        }

        public IEditorUI IMarriageEditorUI
        {
            get
            {
                return this.iMarriageEditorUI;
            }
        }

        public MarriageFilterTypes FilterMode
        {
            get
            {
                return this.marriageFilterTypes;
            }
        }

        #region filter props
        public string FilterUpperMarriage
        {
            get 
            { 
                return this.filterUpperMarriage.ToString(); 
            }
        }

        public string FilterLowerMarriage
        {
            get 
            {
                return this.filterLowerMarriage.ToString();
            }
        }

        public string FilterMaleCName
        {
            get
            {
                return this.filterMaleCName;
            }
        }

        public string FilterMaleSName
        {
            get
            {
                return this.filterMaleSName;
            }
        }

        public string FilterFemaleCName
        {
            get
            {
                return this.filterFemaleCName;
            }
        }

        public string FilterFemaleSName
        {
            get
            {
                return this.filterFemaleSName;
            }
        }

        public string FilterLocationCounty
        {
            get
            {
                return this.filterLocationCounty;
            }
        }

        public string FilterLocation
        {
            get
            {
                return this.filterLocation;
            }
        }

        public string FilterMaleLocation
        {
            get
            {
                return this.filterMaleLocation;
            }
        }

        public string FilterFemaleLocation
        {
            get
            {
                return this.filterFemaleLocation;
            }
        }

        public string FilterMaleName
        {
            get
            {
                return this.filterMaleCName + " " + this.filterMaleSName;
            }
        }

        public string FilterFemaleName
        {
            get
            {
                return this.filterFemaleCName + " " + this.filterFemaleSName;
            }
        }

        public string FilterMarriageSource
        {
            get
            {
                return this.filterSource;
            }
        }

        public string FilterDupeInterval
        {
            get
            {
                return this.filterDupeInterval;
            }
        }

        #endregion

        public bool IsValidMarriageUpperBound
        {
            get
            {
                return this.isValidUpperMarriagebound;
            }
        }

        public bool IsValidMarriageLowerBound
        {
            get
            {
                return this.isValidLowerMarriagebound;
            }
        }

        public bool IsValidSelectedRecordId
        {
            get 
            {
                if (SelectedRecordId != System.Guid.Empty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
            }
        }

      

        #endregion

        public string FilterWitness
        {
            get 
            {
                return this.filterWitness;
            }
        }


        public MarriagesFilterModel()
        {
            this.SetFilterLowerBound("1550");
            this.SetFilterUpperBound("1850");
            this.isDataChanged = false;

        }





        public void SetMergeSources()
        {
            if (this.SelectedRecordId != Guid.Empty)
            {
                MarriageRecords.BLL.MarriagesBLL marriagesBLL = new GedItter.MarriageRecords.BLL.MarriagesBLL();

                Marriage _marriage = marriagesBLL.GetMarriageById2(this.SelectedRecordId).FirstOrDefault();

                if (_marriage != null)
                {

                    MergeDuplicateRecord(_marriage);

                    this.isDataChanged = true;
                    this.Refresh();
                }

            }
        }

        public void SetReorderDupes()
        {          
            if (this.SelectedRecordId != Guid.Empty)
            {
                MarriageRecords.BLL.MarriagesBLL marriagesBLL = new GedItter.MarriageRecords.BLL.MarriagesBLL();
                Guid retVal = Guid.Empty;

                foreach (Guid _g in this.SelectedRecordIds)
                { 
                    retVal = marriagesBLL.ReorderMarriages(this.SelectedRecordId);          
                }

                if(retVal != Guid.Empty)
                    this.SetSelectedRecordId(retVal);      

            }                               
        }



        #region set filters
        public void SetFilterWitness(string param)
        {
            if (filterWitness != param)
            {
                this.isDataChanged = true;
                this.filterWitness = param;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMode(MarriageFilterTypes param)
        {
            

            if (this.marriageFilterTypes != param)
            { 
                //

                this.marriageFilterTypes = param;
                this.isDataChanged = true;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMaleName(string cName, string sName)
        {

            if (this.filterMaleCName != cName ||
                this.filterMaleSName != sName)
            {

                this.isDataChanged = true;
                this.filterMaleCName = cName;
                this.filterMaleSName = sName;
                
                this.SetModelStatusFields();

            }


        }

        public void SetFilterFemaleName(string cName, string sName)
        {
            if (this.filterFemaleCName != cName ||
                this.filterFemaleSName != sName)
            {
                this.isDataChanged = true;
                this.filterFemaleCName = cName;
                this.filterFemaleSName = sName;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterLocation(string location)
        {
            if (this.filterLocation != location)
            {
                this.isDataChanged = true;
                this.filterLocation = location;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMaleLocation(string maleLocation)
        {
            if (this.filterMaleLocation != maleLocation)
            {
                this.isDataChanged = true;
                this.filterMaleLocation = maleLocation;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterFemaleLocation(string femaleLocation)
        {
            if (this.filterFemaleLocation != femaleLocation)
            {
                this.isDataChanged = true;
                this.filterFemaleLocation = femaleLocation;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterSource(string source)
        {
            if (this.filterSource != source)
            {
                this.isDataChanged = true;
                this.filterSource = source;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMarriageLocationCounty(string county)
        {
            if (this.filterLocationCounty != county)
            {
                this.isDataChanged = true;
                this.filterLocationCounty = county;
                this.SetModelStatusFields();
            }
        }

        public void SetFilterMaleName(string name)
        {
            string newCName = "";
            string newSName = "";


            CsUtils.ExtractChristianAndSurnames(name, out newCName, out newSName);


            if (filterMaleCName != newCName || filterMaleSName != newSName)
            {
                 

                this.isDataChanged = true;
                this.filterMaleCName = newCName;
                this.filterMaleSName = newSName;
                this.SetModelStatusFields();
            }

            
        }

        public void SetFilterFemaleName(string name)
        {
            string newCName = "";
            string newSName = "";

            CsUtils.ExtractChristianAndSurnames(name, out newCName, out newSName);

            if (filterFemaleCName != newCName || filterFemaleSName != newSName)
            {
                this.isDataChanged = true;
                this.filterFemaleCName = newCName;
                this.filterFemaleSName = newSName;
                this.SetModelStatusFields();
            }

        }

        public void SetFilterLowerBound(string lowerYear)
        {
            this.isValidLowerMarriagebound = true;

            if (lowerYear != "")
            {
                if (!Int32.TryParse(lowerYear, out filterLowerMarriage))
                {
                    this.isValidLowerMarriagebound = false;
                  //  this.filterLowerMarriage = 0;
                }
                else
                {
                    this.isDataChanged = true;
                    this.SetModelStatusFields();
                }
            }
            else
            {
                this.isValidLowerMarriagebound = false;
              
               // this.filterLowerMarriage = 0;
            }

           
        }

        public void SetFilterUpperBound(string upperYear)
        {
            this.isValidUpperMarriagebound = true;

            if (upperYear != "")
            {
                if (!Int32.TryParse(upperYear, out filterUpperMarriage))
                {
                    this.isValidUpperMarriagebound = false;
               //     this.filterUpperMarriage = 9999;
                }
                else
                {
                    this.isDataChanged = true;
                    this.SetModelStatusFields();
                }
            }
            else
            {
                this.isValidUpperMarriagebound = false;
            //    this.filterUpperMarriage = 9999;
            }
        }


        public void SetFilterDupeInterval(string interval)
        {
            this.isValidDupeInt = true;

            if (filterDupeInterval != "")
            {
                if (!Int32.TryParse(filterDupeInterval, out filterDupeInt))
                {
                    this.isValidDupeInt = false;
                    this.filterDupeInt = 0;
                }
            }
            else
            {
                this.isValidDupeInt = false;
                this.filterDupeInt = 0;
            }
        }


        #endregion






        public override void Refresh()
        {
            Debug.WriteLineIf(showDebug, "Refresh marriage filter model " + this.IsValidEntry.ToString() + " " + this.isDataChanged.ToString());

            if (!IsvalidSelect()) return;

            MarriageRecords.BLL.MarriagesBLL marriagesBLL = new GedItter.MarriageRecords.BLL.MarriagesBLL();
            MarriageWitnessesBLL mwb = new MarriageWitnessesBLL (); 

            if (this.IsValidEntry && isDataChanged)
            {


                Func<Marriage, MarriageResult> selectPred = new Func<Marriage, MarriageResult>(m => new MarriageResult() { UniqueRefStr = m.UniqueRef.GetValueOrDefault(), UniqueRef = m.UniqueRef.GetValueOrDefault(), MarriageSource = m.Source, MarriageId = m.Marriage_Id, FemaleCName = m.FemaleCName, FemaleSName = m.FemaleSName, MaleCName = m.MaleCName, MaleSName = m.MaleSName, MarriageLocation = m.MarriageLocation, MarriageTotalEvents = m.TotalEvents.Value, MarriageYear = m.YearIntVal });

                switch (this.marriageFilterTypes)
                {
                    case MarriageFilterTypes.RELATIONS:
                        break;
                    case MarriageFilterTypes.DUPLICATES:
                        if (this.ParentRecordIds.Count > 0 && this.ParentRecordIds[0] != Guid.Empty) 
                        {
                            marriagesTable = marriagesBLL.GetDataByUniqueRef2(this.ParentRecordIds[0]).Select(selectPred).OrderBy(o => o.MarriageYear).ToList();
                        }
                        else
                        {
                            marriagesTable = new List<MarriageResult>();
                        }
                        break;
                    case MarriageFilterTypes.STANDARD:
                        if (this.IsValidEntry)
                        {
                            if (this.SourceGuidListAsString != "")
                            {

                                if (this.filterLowerMarriage == 0 && this.filterUpperMarriage == 0)
                                {
                                    this.filterLowerMarriage = 1500;
                                    this.filterUpperMarriage = 1900;
                                }

                                marriagesTable = marriagesBLL.GetFilteredMarriagesBySource2(this.filterFemaleCName, "", this.filterFemaleLocation, this.filterFemaleSName,
                                    this.filterMaleCName, "",
                                    this.filterMaleLocation,
                                    this.filterMaleSName,
                                    this.filterLocationCounty,
                                    this.filterLocation,
                                    this.SourceGuidListAsString, this.filterLowerMarriage, this.filterUpperMarriage).Select(selectPred).OrderBy(o=>o.MarriageYear).ToList();
                            }
                            else
                            {
                                marriagesTable = marriagesBLL.GetFilteredMarriages2(this.filterFemaleCName, "", this.filterFemaleLocation, this.filterFemaleSName,
                                      this.filterMaleCName, "",
                                      this.filterMaleLocation,
                                      this.filterMaleSName,
                                      this.filterLocationCounty,
                                      this.filterLocation,
                                      this.SourceGuidListAsString, this.filterLowerMarriage, this.filterUpperMarriage).Select(selectPred).OrderBy(o => o.MarriageYear).ToList();

                            }


                            foreach (var _row in marriagesTable)
                            {
                                _row.Witnesses = mwb.GetWitnesseStringForMarriage(_row.MarriageId);
                            }

                          

                        }
                        break;
                    case MarriageFilterTypes.SIMPLE:
                        break;
                    default:
                        break;
                }

                this.isDataChanged = false;
            }
 

            this.NotifyObservers<MarriagesFilterModel>(this);
        }
        
        /// <summary>
        /// Deletes selected record(s) then calls refresh
        /// </summary>
        public override void DeleteSelectedRecords()
        {
            if (!IsValidDelete()) return;

            BLL.MarriagesBLL marriagesBll = new GedItter.MarriageRecords.BLL.MarriagesBLL();

            foreach (Guid marriageIdx in this.SelectedRecordIds)
            {
                marriagesBll.DeleteMarriageTemp2(marriageIdx);
            }

            this.SetSelectedRecordIds(new List<Guid>());

            this.isDataChanged = true;
            Refresh();
        }

        public override void EditSelectedRecord()
        {
            
         //   MarriagesEditorModel marriageEditorModel = new MarriagesEditorModel();


            if (!IsvalidSelect()) return;

            marriageEditorModel = new MarriagesEditorModel();
          //  marriageEditorModel.DataSaved += new EventHandler(marriageEditorModel_DataSaved);
            base.SetDataSaved(new EventHandler(marriageEditorModel_DataSaved));

            marriageEditorModel.SetSelectedRecordIds(this.SelectedRecordIds);

            this.isDataChanged = true;
            this.ShowDialogEdit(this);

         //   iMarriageEditorUI.SetEditorModal(marriageEditorModel);

           // iMarriageEditorUI.Show();

           
        }

        public override void InsertNewRecord()
        {
            if (!IsValidInsert()) return;

           
            marriageEditorModel = new MarriagesEditorModel();
          //  marriageEditorModel.DataSaved += new EventHandler(marriageEditorModel_DataSaved);
            base.SetDataSaved(new EventHandler(marriageEditorModel_DataSaved));

            marriageEditorModel.SetEditorFemaleCName(this.FilterFemaleCName);
            marriageEditorModel.SetEditorFemaleSName(this.FilterFemaleSName);
            marriageEditorModel.SetEditorMaleCName(this.FilterMaleCName);
            marriageEditorModel.SetEditorMaleSName(this.FilterMaleSName);
            marriageEditorModel.SetEditorMarriageCounty(this.FilterLocationCounty);
            marriageEditorModel.SetEditorLocation(this.FilterLocation);
            
            marriageEditorModel.SetSelectedRecordId(System.Guid.Empty);
            marriageEditorModel.SetSourceGuidList(this.SourceGuidList);

            marriageEditorModel.SetEditorFemaleLocation(this.FilterFemaleLocation);
            marriageEditorModel.SetEditorMaleLocation(this.FilterMaleLocation);
            this.isDataChanged = true;
            this.ShowDialogInsert(this);

             
           

        }


        public void SetEditorUI()
        {
            throw new Exception("Not implemented");
            //this.iMarriageEditorUI = new FrmMarriageEdit();
        }

        public void SetEditorUI(IEditorUI paramIEditorUI)
        {
            this.iMarriageEditorUI = paramIEditorUI;
        }



        void marriageEditorModel_DataSaved(object sender, EventArgs e)
        {
            this.Refresh();
        }



        public IMarriageEditorModel IMarriageEditorModel
        {
            get 
            {
                return this.marriageEditorModel;
            }
        }

      



        public static void UpdateDeletedMarriages()
        { 
            MarriagesBLL marriagesBLL = new MarriagesBLL();
       
         //   DsMarriages.MarriagesDataTable dsDBTemp = new DsMarriages.MarriagesDataTable();
          


            foreach (var pROw in marriagesBLL.GetMarriages2().Where(o => o.IsDeleted == true))
            {
                List<Guid> peopleToKeep = new List<Guid>();

                var dsDBTemp0 = marriagesBLL.GetMarriageById2(pROw.Marriage_Id).FirstOrDefault();


                if (dsDBTemp0 != null)
                {
                  //  dsDBTemp = marriagesBLL.GetDataByUniqueRef2(dsDBTemp0.UniqueRef);

                    foreach (var dupePerson in marriagesBLL.GetDataByUniqueRef2(dsDBTemp0.UniqueRef.GetValueOrDefault()))
                    {
                        if (pROw.Marriage_Id != dupePerson.Marriage_Id)
                        {
                            peopleToKeep.Add(dupePerson.Marriage_Id);
                        }
                    }

                }
                Guid newRef = Guid.NewGuid();

                int evtCount = 1;
                foreach (Guid id_ in peopleToKeep)
                {

                    marriagesBLL.UpdateMarriageUniqRef(id_, newRef, peopleToKeep.Count, evtCount);
                    evtCount++;
                }

                newRef = Guid.NewGuid();
                marriagesBLL.UpdateMarriageUniqRef(pROw.Marriage_Id, newRef, 1, 1);
            }

        }


        public static void MergeDuplicateRecords()
        {
            MarriagesBLL marriagesBLL = new MarriagesBLL();

            foreach (Marriage newRecord in marriagesBLL.GetMarriages2().Where(o => !o.IsDeleted.GetValueOrDefault() && o.TotalEvents > 1 && o.EventPriority == 0))
            {
                MergeDuplicateRecord(newRecord);
            }
        }

        public static void MergeDuplicateRecord(Marriage newMarriage)
        {
            MarriagesBLL marriagesBLL = new MarriagesBLL();
            SourceBLL sourceBll = new SourceBLL();
            SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();
            MarriageWitnessesBLL marriageWitBll = new MarriageWitnessesBLL();
            List<Guid> sourceList = new List<Guid>();
            List<Person> witnessList = new List<Person>();

            witnessList.AddRange(marriageWitBll.GetWitnessesForMarriage(newMarriage.Marriage_Id));

            foreach (var dupePerson in marriagesBLL.GetDataByDupeRefByMarriageId(newMarriage.Marriage_Id))
            {
                sourceList.AddRange(sourceBll.FillSourceTableByPersonOrMarriageId2(dupePerson.Marriage_Id).Select(dp => dp.SourceId).ToList());
                witnessList.AddRange(marriageWitBll.GetWitnessesForMarriage(dupePerson.Marriage_Id));
                newMarriage.MergeInto(dupePerson);
            }

            marriageWitBll.DeleteWitnessesForMarriage(newMarriage.Marriage_Id);

            witnessList.RemoveDuplicates();

            marriageWitBll.InsertWitnessesForMarriage(newMarriage.Marriage_Id, witnessList);

            sourceMappingsBll.WriteMarriageSources(newMarriage.Marriage_Id, sourceList, 1);
            
            marriagesBLL.ModelContainer.SaveChanges();

        }






        /// <summary>
        /// selected records must all be duplicate
        /// function removes the first record from the selected records list
        /// </summary>
        public void SetRemoveSelectedFromDuplicateList()
        {
            // marriage filter screen display the marriage starting at zero!!!!!
            int evtCount = 0;

            if (!IsValidEdit()) return;

            if (this.SelectedRecordIds.Count > 0)
            {


                MarriagesBLL marriagesBLL = new MarriagesBLL();
                List<Guid> marriagesToKeep = new List<Guid>();
                MarriageRelationsBLL marriageRelationsBLL = new MarriageRelationsBLL();
               
                foreach (var groupedMarriage in marriagesBLL.GetDataByDupeRefByMarriageId(this.SelectedRecordIds[0]))
                {
                    if (!SelectedRecordIds.Contains(groupedMarriage.Marriage_Id))
                    {
                        marriagesToKeep.Add(groupedMarriage.Marriage_Id);
                    }
                }


                Guid newRef = Guid.NewGuid();


                foreach (Guid id_ in marriagesToKeep)
                {
                    marriagesBLL.UpdateMarriageUniqRef(id_, newRef, marriagesToKeep.Count, evtCount);
                    evtCount++;
                }

                List<Guid> parentGuids = new List<Guid>();
                parentGuids.Add(newRef);

                this.SetParentRecordIds(parentGuids);

                // records to remove done here
                foreach (Guid marriageToRemove in SelectedRecordIds)
                {
                    newRef = Guid.NewGuid();
                    marriagesBLL.UpdateMarriageUniqRef(marriageToRemove, newRef, 1, 0);
                }


                if (marriagesToKeep.Count == 0)
                {
                    parentGuids.Clear();
                    parentGuids.Add(newRef);
                    this.SetParentRecordIds(newRef);
                }

            }


            this.isDataChanged = true;
            this.Refresh();

                    

        }


        public void SetSelectedDuplicateMarriage()
        {
            MarriageRelationsBLL relationsBll = new MarriageRelationsBLL();
            MarriagesBLL marriagesDll = new MarriagesBLL();

            if (!IsValidEdit()) return;


            if (this.SelectedRecordIds.Count > 1)
            {
           
                List<Marriage> marriageList = relationsBll.ModelContainer.Marriages.Where(m => this.SelectedRecordIds.Contains(m.Marriage_Id)).ToList();


                foreach (Marriage _marriage in marriageList)
                { 
                    Debug.WriteLine(_marriage.MaleCName + " " + _marriage.MaleSName + " " + _marriage.Date);    
                }

                if (marriageList.Count != this.SelectedRecordIds.Count)
                {
                    Debugger.Break();
                }


                List<Marriage> listToUpdate = new List<Marriage>();

                listToUpdate.AddRange(marriageList.Where(m => m.UniqueRef == Guid.Empty));

                foreach (Marriage dupeMarriage in marriageList.Where(m => m.UniqueRef != Guid.Empty))
                {

                    listToUpdate.AddRange(relationsBll.ModelContainer.Marriages.Where(sm => sm.UniqueRef == dupeMarriage.UniqueRef));

                }


                Guid newRef = Guid.NewGuid();

                int evtCount = 0;
                foreach (Marriage _marriageToUpdate in listToUpdate)
                {
                    marriagesDll.UpdateMarriageUniqRef(_marriageToUpdate.Marriage_Id, newRef, listToUpdate.Count, evtCount);
                    evtCount++;
                }

            }

            relationsBll.ModelContainer.SaveChanges();
            // this.RequestSetSelectedIds();
            base.SetSelectedRecordIds(new List<Guid>());
            this.isDataChanged = true;

            this.Refresh();
        }





        public void ViewDuplicates()
        {
            if (this.ShowDialogDuplicatesEvent != null)
                this.ShowDialogDuplicatesEvent(this, new EventArgs());
        }

        public void SetShowDialogDupes(EventHandler paramEventHandler)
        {
            ShowDialogDuplicatesEvent = paramEventHandler;

        }

        public void SetShowDialogRels(EventHandler paramEventHandler)
        {
            ShowDialogRelationsEvent = paramEventHandler;
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
            exportToHtml = new ExportToHtml(param);

            exportToHtml.ColumnsToIgnore.Add("MaleLocation");
            exportToHtml.ColumnsToIgnore.Add("FemaleLocation");

            exportToHtml.ColumnsToIgnore.Add("MaleId");
            exportToHtml.ColumnsToIgnore.Add("MaleInfo");
            exportToHtml.ColumnsToIgnore.Add("FemaleId");
            exportToHtml.ColumnsToIgnore.Add("FemaleInfo");
            //  frmPrinting.ColumnsToIgnore.Add("MarriageLocation");
            exportToHtml.ColumnsToIgnore.Add("YearIntVal");
            exportToHtml.ColumnsToIgnore.Add("MarriageCounty");
            exportToHtml.ColumnsToIgnore.Add("Witness1");
            exportToHtml.ColumnsToIgnore.Add("Witness2");
            exportToHtml.ColumnsToIgnore.Add("Witness3");
            exportToHtml.ColumnsToIgnore.Add("Witness4");
            exportToHtml.ColumnsToIgnore.Add("IsLicence");
            exportToHtml.ColumnsToIgnore.Add("IsBanns");
            exportToHtml.ColumnsToIgnore.Add("MaleIsKnownWidower");
            exportToHtml.ColumnsToIgnore.Add("FemaleIsKnownWidow");
            exportToHtml.ColumnsToIgnore.Add("FemaleOccupation");
            exportToHtml.ColumnsToIgnore.Add("MaleOccupation");
            exportToHtml.ColumnsToIgnore.Add("Marriage_Id");


            exportToHtml.ColumnsToIgnore.Add("UserId");
            exportToHtml.ColumnsToIgnore.Add("MaleLocationId");
            exportToHtml.ColumnsToIgnore.Add("FemaleLocationId");
            exportToHtml.ColumnsToIgnore.Add("MarriageLocationId");
            exportToHtml.ColumnsToIgnore.Add("Source");


            exportToHtml.SortColumn = "YearIntVal";



       

            if (isTabular)
                reportLocation = exportToHtml.LoadStandardTabular();
            else
                reportLocation = exportToHtml.LoadStandardNotes();
        }



        public override void SetFromQueryString(string param)
        {

            Debug.WriteLineIf(showDebug,"filter marriage model SetFromQueryString :" + param);

            NameValueCollection query = HttpUtility.ParseQueryString(param);

            query.ReadInErrorsAndSecurity(this);

            //if (query.AllKeys.Contains("error"))
            //{
            //    this.SetErrorState(query["error"] ?? "");
            //}

            Guid parentId = new Guid(query["parent"] ?? Guid.Empty.ToString());

            if (parentId == Guid.Empty)
            {
                this.SetFilterMode(MarriageFilterTypes.STANDARD);

                this.SetSelectedRecordIds(new List<Guid>());
                int pageNo = Convert.ToInt32(query["pg"] ?? "0");

                this.SetRecordStart(pageNo);

               

                this.SetFilterMaleName((query["mc"] ?? ""), (query["ms"] ?? ""));

              
                this.SetFilterFemaleName((query["fc"] ?? ""),(query["fs"] ?? ""));

                
                this.SetFilterLocation(query["lc"] ?? "");
                this.SetFilterMarriageLocationCounty(query["cn"] ?? "");


                this.SetFilterLowerBound(query["ld"] ?? "");
                this.SetFilterUpperBound(query["ud"] ?? "");

      
 

            }
            else
            {
                this.SetSelectedRecordIds(new List<Guid>());
                this.SetParentRecordIds(parentId);

                this.SetFilterMode(MarriageFilterTypes.DUPLICATES);

            }

        //    this.Refresh();
        }

        public string QueryString
        {
                      
            // iControl.RequestSetFilterLocation(this.txtLocation.Text);
            //iControl.RequestSetFilterMaleName(this.txtMaleCName.Text, this.txtMaleSName.Text);
            //iControl.RequestSetFilterFemaleName(this.txtFemaleCName.Text, this.txtFemaleSName.Text);
            //iControl.RequestSetFilterMarriageBoundLower(this.txtLowerDateRangeLower.Text);
            //iControl.RequestSetFilterMarriageBoundUpper(this.txtLowerDateRangeUpper.Text);


            //iControl.RequestSetFilterSource(this.iSourceModel.SourceIdsFound);
            //iControl.RequestSetSourceGuidsList(this.iSourceModel.SelectedRecordIds);

            //iControl.RequestSetFilterLocationCounty(this.txtCounty.Text);
            //iControl.RequestSetFilterWitness(this.txtWitSearch.Text);
           
            get 
            {
                var sb = new System.Text.StringBuilder();

                sb.Append("mcn=" + HttpUtility.UrlEncode(this.filterMaleCName) + "&");
                sb.Append("msn=" + HttpUtility.UrlEncode(this.FilterMaleSName) + "&");
                sb.Append("fcn=" + HttpUtility.UrlEncode(this.FilterFemaleCName) + "&");
                sb.Append("fsn=" + HttpUtility.UrlEncode(this.FilterFemaleName) + "&");
                sb.Append("lmd=" + HttpUtility.UrlEncode(this.FilterLowerMarriage) + "&");
                sb.Append("umd=" + HttpUtility.UrlEncode(this.FilterUpperMarriage) + "&");
                sb.Append("wit=" + HttpUtility.UrlEncode(this.FilterWitness) + "&");
                sb.Append("lco=" + HttpUtility.UrlEncode(this.FilterLocationCounty) + "&");
                sb.Append("sgl=" + HttpUtility.UrlEncode(this.SourceGuidListAsString) + "&");
                sb.Append("sif=" + HttpUtility.UrlEncode(this.SourceIdsFound) + "&");
                sb.Append("lpr=" + HttpUtility.UrlEncode(this.FilterLocation) + "&");

                sb.Remove(sb.Length - 1, 1); // Remove the final '&'

                return "?" +sb.ToString();

            }


        }





     
    }

    public enum MarriageFilterTypes
    {
        RELATIONS = 0,
        DUPLICATES = 1,
        STANDARD = 2,
        SIMPLE = 3
    }

}

#region unused



    //public class MarEvent
    //{

    //    TDBCore.Datasets.DsMarriages.MarriagesRow marriageRow = null;
    //    int yearOffset = 2;
    //    int metMarYear = 0;
    //    string metcountyRow;
    //    string metmNameRow;
    //    string metmSurnameRow;
    //    string metfNameRow;
    //    string metfSurnameRow;
    //    string metmarriageLocationRow;



    //    public MarEvent(TDBCore.Datasets.DsMarriages.MarriagesRow _marriageRow)
    //    {
    //        marriageRow = _marriageRow;


    //        metMarYear = marriageRow.YearIntVal;

    //        metcountyRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriageRow.MarriageCounty);
    //        metmNameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriageRow.MaleCName);
    //        metmSurnameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriageRow.MaleSName);
    //        metfNameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriageRow.FemaleCName);
    //        metfSurnameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriageRow.FemaleSName);
    //        metmarriageLocationRow = marriageRow.MarriageLocation.ToLower();



    //    }

    //    public override int GetHashCode()
    //    {

    //        return yearOffset.GetHashCode() +
    //         metcountyRow.GetHashCode() +
    //         metmNameRow.GetHashCode() +
    //         metmSurnameRow.GetHashCode() +
    //         metfNameRow.GetHashCode() +
    //         metfSurnameRow.GetHashCode() +
    //         metmarriageLocationRow.GetHashCode();


    //    }


    //    public override bool Equals(object obj)
    //    {
    //        var tp = obj as MarEvent;

    //        //var testMar = obj as MarEvent;
    //        TDBCore.Datasets.DsMarriages.MarriagesRow testMar = tp.marriageRow;

    //        if (testMar == null) return false;


    //        int testMar_yearOffset = testMar.YearIntVal;

    //        string testMar_metcountyRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(testMar.MarriageCounty);
    //        string testMar_metmNameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(testMar.MaleCName);
    //        string testMar_metmSurnameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(testMar.MaleSName);
    //        string testMar_metfNameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(testMar.FemaleCName);
    //        string testMar_metfSurnameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(testMar.FemaleSName);
    //        string testMar_metmarriageLocationRow = testMar.MarriageLocation.ToLower();

    //        if (testMar_metcountyRow == metcountyRow &&
    //               testMar_metmNameRow == metmNameRow &&
    //               testMar_metmSurnameRow == metmSurnameRow &&
    //               testMar_metfNameRow == metfNameRow &&
    //               testMar_metfSurnameRow == metfSurnameRow)
    //        {
    //            if (metMarYear >= (testMar_yearOffset - yearOffset)
    //                && metMarYear <= (testMar_yearOffset + yearOffset))
    //            {
    //                if (metmarriageLocationRow.Contains(testMar_metmarriageLocationRow)
    //                    || testMar_metmarriageLocationRow.Contains(metmarriageLocationRow))
    //                {
    //                    //marriagesDataTable[idx].UniqueRef = eventGuid.ToString();
    //                    // marriagesDataTable[idx].TotalEvents = count;
    //                    //count++;

    //                    return true;
    //                }
    //            }
    //        }

    //        //if (metMarYear >= (testMar_yearOffset - yearOffset)
    //        //        && metMarYear <= (testMar_yearOffset + yearOffset))
    //        //{
    //        //    return true;
    //        //}
    //        return false;

    //    }

    //}




//// move me
//        public  void UpdateMarriageHashs()
//        { 
//            // get a record that isnt marked with a unique ref
//            // find duplicates
//            // mark duplicates

//            DsMarriages.MarriagesDataTable marriagesDataTable = new DsMarriages.MarriagesDataTable();

//            MarriageRecords.BLL.MarriagesBLL marriagesBll = new GedItter.MarriageRecords.BLL.MarriagesBLL();

//          //  marriagesDataTable = marriagesBll.GetFilteredMarriages("", "", "", "", "", "", "", "", "", "", "", 1700, 1730);
//            marriagesDataTable = marriagesBll.GetMarriages();
//            int idx = 0;

//            while (idx < marriagesDataTable.Count)
//            {
//                if (marriagesDataTable[idx].UniqueRef == "") marriagesDataTable[idx].UniqueRef = Guid.NewGuid().ToString();

//                FindAndMark(ref marriagesDataTable,
//                                marriagesDataTable[idx].MarriageCounty,
//                                marriagesDataTable[idx].MaleCName,
//                                marriagesDataTable[idx].MaleSName,
//                                marriagesDataTable[idx].FemaleCName,
//                                marriagesDataTable[idx].FemaleSName,
//                                marriagesDataTable[idx].YearIntVal,
//                                marriagesDataTable[idx].MarriageLocation,
//                                marriagesDataTable[idx].UniqueRef);

//                idx++;
//                Console.WriteLine(idx.ToString());
                
//            }
//            idx = 0;
//            while (idx < marriagesDataTable.Count)
//            {
//                marriagesBll.UpdateMarriage(marriagesDataTable[idx].Marriage_Id,
//                                            marriagesDataTable[idx].Date,
//                                            marriagesDataTable[idx].FemaleCName,
//                                            marriagesDataTable[idx].FemaleId,
//                                            marriagesDataTable[idx].FemaleInfo,
//                                            marriagesDataTable[idx].FemaleLocation,
//                                            marriagesDataTable[idx].FemaleSName,
//                                            marriagesDataTable[idx].MaleCName,
//                                            marriagesDataTable[idx].MaleId,
//                                            marriagesDataTable[idx].MaleInfo,
//                                            marriagesDataTable[idx].MaleLocation,
//                                            marriagesDataTable[idx].MaleSName,
//                                            marriagesDataTable[idx].MarriageCounty,
//                                            marriagesDataTable[idx].MarriageLocation,
//                                            marriagesDataTable[idx].Source,
//                                            marriagesDataTable[idx].YearIntVal,
//                                            marriagesDataTable[idx].Witness1,
//                                            marriagesDataTable[idx].Witness2,
//                                            marriagesDataTable[idx].Witness3,
//                                            marriagesDataTable[idx].Witness4,
//                                            marriagesDataTable[idx].MaleOccupation,
//                                            marriagesDataTable[idx].FemaleOccupation,
//                                            marriagesDataTable[idx].IsLicence,
//                                            marriagesDataTable[idx].IsBanns,
//                                            marriagesDataTable[idx].MaleIsKnownWidower,
//                                            marriagesDataTable[idx].FemaleIsKnownWidow,
//                                            marriagesDataTable[idx].MarriageLocationId,
//                                            marriagesDataTable[idx].MaleLocationId,
//                                            marriagesDataTable[idx].FemaleLocationId,
//                                            2,
//                                            marriagesDataTable[idx].MaleBirthYear,
//                                            marriagesDataTable[idx].FemaleBirthYear,
//                                            marriagesDataTable[idx].UniqueRef,
//                                            marriagesDataTable[idx].TotalEvents,
//                                            marriagesDataTable[idx].EventPriority);
//                idx++;
//            }

//            //marriagesBll.UpdateMarriage(
//        }

//        public void FindAndMark(ref TDBCore.Datasets.DsMarriages.MarriagesDataTable marriagesDataTable,
//            string county,
//            string mName,
//            string mSurname,
//            string fName,
//            string fSurname,
//            int marriageYear,
//            string marriageLocation,
//            string eventGuid)
//        {
//            // get a record that isnt marked with a unique ref
//            // find duplicates
//            // mark duplicates
//            int yearOffset = this.filterDupeInt;
//            string metcounty = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(county);
//            string metmName = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(mName);
//            string metmSurname = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(mSurname);
//            string metfName = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(fName);
//            string metfSurname = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(fSurname);
//            string metmarriageLocation = marriageLocation.ToLower();




//            int idx = 0;
//            int count = 0;

//            while (idx < marriagesDataTable.Count)
//            {
//                if (marriagesDataTable[idx].UniqueRef != "")
//                {
//                    idx++;
//                    continue;
//                }

//                string metcountyRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriagesDataTable[idx].MarriageCounty);
//                string metmNameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriagesDataTable[idx].MaleCName);
//                string metmSurnameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriagesDataTable[idx].MaleSName);
//                string metfNameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriagesDataTable[idx].FemaleCName);
//                string metfSurnameRow = DoubleMetaphone.DoubleMetaphoneStringExtension.GenerateDoubleMetaphone(marriagesDataTable[idx].FemaleSName);
//                string metmarriageLocationRow = marriagesDataTable[idx].MarriageLocation.ToLower();



//                if (metcounty == metcountyRow &&
//                    metmName == metmNameRow &&
//                    metmSurname == metmSurnameRow &&
//                    metfName == metfNameRow &&
//                    metfSurname == metfSurnameRow)
//                {
//                    if (marriageYear >= (marriagesDataTable[idx].YearIntVal - yearOffset)
//                        && marriageYear <= (marriagesDataTable[idx].YearIntVal + yearOffset))
//                    {
//                        if (metmarriageLocationRow.Contains(metmarriageLocation) 
//                            || metmarriageLocation.Contains(metmarriageLocationRow))
//                        {
//                            marriagesDataTable[idx].UniqueRef = eventGuid.ToString();
//                           // marriagesDataTable[idx].TotalEvents = count;
//                            //count++;
//                        }
//                    }
//                }

//                idx++;
//            }



//        }

#endregion