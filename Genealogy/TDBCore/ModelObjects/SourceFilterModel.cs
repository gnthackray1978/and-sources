using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
//using TDBCore.Datasets;
using GedItter.BLL;
using TDBCore.Types;
using System.Collections.Specialized;
using System.Web;


namespace GedItter.ModelObjects
{
    public class SourceFilterModel : EditorBaseModel<Guid>, ISourceFilterModel
    {
       // SourceEditorModel iSourceEditorModel = new SourceEditorModel();
        IList<TDBCore.EntityModel.Source> sourcesDataTable = null;
        ISourceEditorUI iSourceEditorUI = null;

        string filterSourceDescription = "";
        string filterSourceOriginalLocation = "";
        string filterSourceRef = "";
        bool? filterIsCopyHeld = false;
        bool? filterIsViewed = false;
        bool? filterIsThackrayFound = false;
        string sourceFileCount = "";

        List<int> filterSourceTypeList = new List<int>();

        int intDateUpperBound = 0;
        int intDateLowerBound = 0;
        int intToDateUpperBound = 0;
        int intToDateLowerBound = 0;

        string filterSourceDateUpperBound = "";
        string filterSourceDateLowerBound = "";
        string filterSourceToDateUpperBound = "";
        string filterSourceToDateLowerBound = "";

        ExportToHtml exportToHtml = null;
        string reportLocation = "";


        public override bool IsValidEntry
        {
            get
            {
                if (filterSourceRef == "" && filterSourceOriginalLocation == "" && filterSourceDescription == "" &&
                    intToDateLowerBound == 0 && intToDateUpperBound == 0 && intDateLowerBound == 0 && intDateUpperBound == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #region validation vars
        bool isValidSourceDateUpperBound = false;
        bool isValidSourceDateLowerBound = false;
        bool isValidSourceToDateUpperBound = false;
        bool isValidSourceToDateLowerBound = false;
        bool isValidSourceDescription = false;
        bool isValidSourceRef = false;
        #endregion

     

        public IList<TDBCore.EntityModel.Source> SourcesDataTable
        {
            get 
            {
                if (this.sourcesDataTable == null)
                    return new List<TDBCore.EntityModel.Source>();
                else
                    return this.sourcesDataTable;
            
            }
        }

        public ISourceEditorUI ISourceEditorUI
        {
            get
            {
                return this.iSourceEditorUI;
            }
        }

        public SourceFilterModel()
        {
            this.SetFilterSourceDateLowerBound("1500");
            this.SetFilterSourceDateUpperBound("2000");

            this.SetFilterSourceToDateLowerBound("1500");
            this.SetFilterSourceToDateUpperBound("2000");

            this.SetFilterIsCopyHeld(null,null);
            this.SetFilterIsThackrayFound(null, null);
            this.SetFilterIsViewed(null, null);

            //this.SetFilterIsThackrayFound(null);
            //this.SetFilterIsCopyHeld(null);
            //this.SetFilterIsViewed(null);
        }



       #region properties

       #region validation props

       public bool IsValidSourceRef
        {
            get
            {
                return this.isValidSourceRef;
            }
        }

        public bool IsValidSourceDateUpperBound
        {
            get 
            {
                return this.isValidSourceDateUpperBound;
            }
        }

        public bool IsValidSourceDateLowerBound
        {
            get 
            { 
                return this.isValidSourceDateLowerBound; 
            }
        }

        public bool IsValidSourceToDateUpperBound
        {
            get 
            { 
                return this.isValidSourceToDateUpperBound; 
            }
        }

        public bool IsValidSourceToDateLowerBound
        {
            get 
            { 
                return this.isValidSourceToDateLowerBound; 
            }
        }

        public bool IsValidSourceDescription
        {
            get 
            { 
                return this.isValidSourceDescription; 
            }
        }

        #endregion


 

        public string FilterSourceDescription
        {
            get 
            {
                return this.filterSourceDescription;
            }
        }

        public string FilterSourceOriginalLocation
        {
            get
            {
                return this.filterSourceOriginalLocation;
            }

        }

        public bool? FilterIsCopyHeld
        {
            get 
            {
                return this.filterIsCopyHeld;
            }
        }

        public bool? FilterIsViewed
        {
            get 
            {
                return this.filterIsViewed;
            }
        }

        public bool? FilterIsThackrayFound
        {
            get
            {
                return this.filterIsThackrayFound;
            }
        }

        public List<int> FilterSourceTypeList
        {
            get 
            {
                return this.filterSourceTypeList;
            }
        }

        public string FilterSourceRef
        {
            get
            {
                return this.filterSourceRef;
            }
        }

        public string FilterSourceDateUpperBound
        {
            get 
            { 
                return this.filterSourceDateUpperBound ;
            }
        }

        public string FilterSourceDateLowerBound
        {
            get 
            { 
                return this.filterSourceDateLowerBound ;
            }
        }

        public string FilterSourceToDateUpperBound
        {
            get 
            { 
                return this.filterSourceToDateUpperBound ;
            }
        }

        public string FilterSourceToDateLowerBound
        {
            get 
            { 
                return this.filterSourceToDateLowerBound ;
            }
        }

        public string FilterSourceFileCount
        {
            get
            {
                return this.sourceFileCount;
            }
        }

       #endregion


        public void SetFilterSourceRef(string param)
        {
            if (this.filterSourceRef != param)
            {
                if (this.filterSourceRef != "")
                {
                    this.isValidSourceRef = true;
                    this.filterSourceRef = param;
                }
                else
                {
                    this.isValidSourceRef = false;
                    this.filterSourceRef = param;
                }

                this.isDataChanged = true;

                SetModelStatusFields();
            }
        }

        public void SetFilterSourceDescription(string param)
        {
            if (this.filterSourceDescription != param)
            {
                if (this.filterSourceDescription != "")
                {
                    this.isValidSourceDescription = true;
                    this.filterSourceDescription = param;
                }
                else
                {
                    this.isValidSourceDescription = false;
                    this.filterSourceDescription = param;
                }
                this.isDataChanged = true;
                SetModelStatusFields();
            }
        }

        public void SetFilterSourceOriginalLocation(string param)
        {
            if (this.filterSourceOriginalLocation != param)
            {
                
                this.filterSourceOriginalLocation = param;

                this.isDataChanged = true;
                SetModelStatusFields();
            }
        }

        public void SetFilterIsCopyHeld(bool? param, bool? useParam)
        {
            if (this.filterIsCopyHeld != param)
            {

                if (useParam.HasValue && useParam.Value)
                    this.filterIsCopyHeld = param;
                else
                    this.filterIsCopyHeld = null;

                this.isDataChanged = true;
                SetModelStatusFields();
            }
        }

        public void SetFilterIsViewed(bool? param, bool? useParam)
        {
            if (this.filterIsViewed != param)
            {
                if (useParam.HasValue && useParam.Value)
                    this.filterIsViewed = param;
                else
                    this.filterIsViewed = null;
                
                this.isDataChanged = true;
                SetModelStatusFields();
            }
        }

        public void SetFilterIsThackrayFound(bool? param, bool? useParam)
        {
            if (this.filterIsThackrayFound != param)
            {
                if (useParam.HasValue && useParam.Value)
                    this.filterIsThackrayFound = param;
                else
                    this.filterIsThackrayFound = null;

                this.isDataChanged = true;
                SetModelStatusFields();
            }
        }

        public void SetFilterSourceTypeList(List<int> param)
        {
            if (this.filterSourceTypeList != param)
            {

                this.filterSourceTypeList = param;

                this.isDataChanged = true;
                SetModelStatusFields();
            }
        }

        public void SetFilterSourceDateUpperBound(string param)
        {
            this.isValidSourceDateUpperBound = true;

            if (this.filterSourceDateUpperBound != param)
            {
                if (!DateTools.TryParseYear(param, out intDateUpperBound))
                {
                    this.isValidSourceDateUpperBound = false;
                   // this.intDateUpperBound = 0;
                }
                else
                {
                    this.isValidSourceDateUpperBound = true;
                    this.isDataChanged = true;
                }

                SetModelStatusFields();
            }

            
            this.filterSourceDateUpperBound = param;
        }

        public void SetFilterSourceToDateUpperBound(string param)
        {
            this.isValidSourceToDateUpperBound = true;

            if (this.filterSourceToDateUpperBound != param)
            {
                if (!DateTools.TryParseYear(param, out intToDateUpperBound))
                {
                    this.isValidSourceToDateUpperBound = false;
                }
                else
                {
                    this.isValidSourceToDateUpperBound = true;
                    this.isDataChanged = true;
                }

                SetModelStatusFields();
            }


            
            this.filterSourceToDateUpperBound = param;
        }

        public void SetFilterSourceDateLowerBound(string param)
        {
            this.isValidSourceDateLowerBound = true;

            if (this.filterSourceDateLowerBound != param)
            {
                if (!DateTools.TryParseYear(param, out intDateLowerBound))
                {
                    this.isValidSourceDateLowerBound = false;
                  //  this.intDateLowerBound = 0;
                }
                else
                {
                    this.isValidSourceDateLowerBound = true;
                    this.isDataChanged = true; 
                }


                SetModelStatusFields();
            }

            
            this.filterSourceDateLowerBound = param;
        }

        public void SetFilterSourceToDateLowerBound(string param)
        {
            this.isValidSourceToDateLowerBound = true;

            if (this.filterSourceToDateLowerBound != param)
            {
                if (!DateTools.TryParseYear(param, out intToDateLowerBound))
                {
                    this.isValidSourceToDateLowerBound = false;
                  //  this.intToDateLowerBound = 0;
                }
                else
                {
                    this.isValidSourceToDateLowerBound = true;
                    this.isDataChanged = true; 
                }

                SetModelStatusFields();
            }
            
            this.filterSourceToDateLowerBound = param;
        }

        public void SetFilterSourceFileCount(string param, bool useParam)
        {
            if (useParam)
            {
                if (this.sourceFileCount != param)
                {
                    this.sourceFileCount = param;
                    this.isDataChanged = true; 
                }
            }
            else
            {
                this.sourceFileCount = "";
            }


        }


        public override void Refresh()
        {

            if (!IsvalidSelect()) return;

           
            if (this.IsValidEntry && isDataChanged)
            {

                this.sourcesDataTable = new List<TDBCore.EntityModel.Source>();
                BLL.SourceBLL sourceBLL = new GedItter.BLL.SourceBLL();


                this.sourcesDataTable = sourceBLL.FillSourceTableByFilter2(this.filterSourceDescription,
                    this.filterSourceOriginalLocation,
                    this.filterIsCopyHeld,
                    this.filterIsViewed,
                    this.filterIsThackrayFound,
                    this.intDateUpperBound,
                    this.intDateLowerBound,
                    this.intToDateUpperBound,
                    this.intToDateLowerBound,

                    this.SelectedUserId, this.filterSourceTypeList, this.FilterDateAddedFrom, this.FilterDateAddedTo, filterSourceRef,
                    this.sourceFileCount).ToList();


                this.isDataChanged = false;
            }

            this.NotifyObservers<SourceFilterModel>(this);
        }

        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            BLL.SourceBLL sourceBLL = new GedItter.BLL.SourceBLL();
           

            foreach (Guid marriageIdx in this.SelectedRecordIds)
            {
                sourceBLL.DeleteSource2(marriageIdx);
            }

            this.SetSelectedRecordIds(new List<Guid>());


            this.isDataChanged = true;
            Refresh();

            


         
        }

        public override void EditSelectedRecord()
        {
            if (!IsvalidSelect()) return;
         
           // this.SetISourceEditorModel(
          //  iSourceEditorModel.SetSelectedRecordId(this.SelectedRecordId);
            this.SetISourceEditorModel(new SourceEditorModel());

            this.ISourceEditorModel.SetSelectedRecordId(this.SelectedRecordId);
            this.ISourceEditorModel.SetDataSaved(new EventHandler(iSourceEditorModel_DataSaved));

            this.isDataChanged = true;
            this.ShowDialogEdit(this);

          //  iSourceEditorUI.SetEditorModal(iSourceEditorModel);
          //  iSourceEditorUI.Show();


        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            this.SetISourceEditorModel(new SourceEditorModel());
            //ISourceEditorModel = ;
         //   SourceEditorModel sem = new SourceEditorModel();
         //   sem.DataSaved

            ISourceEditorModel.SetDataSaved(new EventHandler(iSourceEditorModel_DataSaved));

         //   ISourceEditorModel.DataSaved += ;

            ISourceEditorModel.SetSelectedRecordId(System.Guid.Empty);

            this.ShowDialogInsert(this);

            //iSourceEditorUI.SetEditorModal(iSourceEditorModel);
           // iSourceEditorUI.Show();

          
        }

        void iSourceEditorModel_DataSaved(object sender, EventArgs e)
        {
             this.Refresh();
        }

        public List<string> SourceRefs
        {
            get
            {
                SourceBLL sourceBll = new SourceBLL();

                if (this.SelectedRecordIds.Count >0)
                {
                    return sourceBll.GetsourceRefs2(this.SelectedRecordIds);
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        public IList<TDBCore.EntityModel.Source> GetSources(string sort)
        {
            Refresh();
            return sourcesDataTable;

        }

        public void SetEditorUI(ISourceEditorUI paramISourceEditorUI)
        {
            iSourceEditorUI = paramISourceEditorUI;
        }

        public void SetEditorUI()
        {
        //    iSourceEditorUI = new FrmEditSource();
        }

        public int SourceRecordCount
        {
            get
            {
                if (SourcesDataTable != null)
                    return this.SourcesDataTable.Count();
                else
                    return 0;
            }
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


            exportToHtml.ColumnsToIgnore.Add("SourceId");

            exportToHtml.ColumnsToIgnore.Add("OriginalLocation");
            exportToHtml.ColumnsToIgnore.Add("IsCopyHeld");
            exportToHtml.ColumnsToIgnore.Add("IsViewed");
            exportToHtml.ColumnsToIgnore.Add("IsThackrayFound");
            exportToHtml.ColumnsToIgnore.Add("DateAdded");

            exportToHtml.ColumnsToIgnore.Add("UserId");
            exportToHtml.ColumnsToIgnore.Add("SourceDateStr");
            exportToHtml.ColumnsToIgnore.Add("SourceDateStrTo");
            exportToHtml.ColumnsToIgnore.Add("SourceList");


            exportToHtml.SortColumn = "SourceDate";

            if (isTabular)
                reportLocation = exportToHtml.LoadStandardTabular();
            else
                reportLocation = exportToHtml.LoadStandardNotes();

        }


        public override void SetFromQueryString(string param)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(param);
            this.SetSelectedRecordIds(new List<Guid>());

            if (query.Count > 0)
            {
                //if (query.AllKeys.Contains("error"))
                //{
                //    this.SetErrorState(query["error"] ?? "");
                //}

                query.ReadInErrorsAndSecurity(this);

                int pageNo = Convert.ToInt32(query["p"] ?? "0");

                this.SetRecordStart(pageNo);

                bool isThac = false;
                bool isCopy = false;
                bool isView = false;
                bool isCheck = false;

                Boolean.TryParse((query["isthac"] ?? ""), out isThac);
                Boolean.TryParse((query["iscopy"] ?? ""), out isCopy);
                Boolean.TryParse((query["isview"] ?? ""), out isView);
                Boolean.TryParse((query["isCheck"] ?? ""), out isCheck);

                

                List<int> sourceTypes = (query["stype"] ?? "").Split(',').ToList<string>().ConvertAll<int>(delegate(string i) { int ret = 0; Int32.TryParse(i, out ret); return ret; }).ToList();

                sourceTypes.RemoveAll(i => i == 0);


                this.SetFilterSourceTypeList(sourceTypes);

                this.SetFilterSourceRef(query["sref"] ?? "");
                this.SetFilterSourceDescription(query["sdesc"] ?? "");
                this.SetFilterSourceOriginalLocation(query["origloc"] ?? "");

                this.SetFilterSourceDateLowerBound(query["ldrl"] ?? "");
                this.SetFilterSourceToDateLowerBound(query["ldru"] ?? "");

                this.SetFilterSourceDateUpperBound(query["udrl"] ?? "");
                this.SetFilterSourceToDateUpperBound(query["udru"] ?? "");


                this.SetFilterSourceFileCount((query["count"] ?? ""), true);



                //

                this.SetFilterIsThackrayFound(isThac, isCheck);

                this.SetFilterIsCopyHeld(isCopy, isCheck);


                this.SetFilterIsViewed(isView, isCheck);



              //  this.Refresh();
            }
        }
    }
}




//          var sref = $('[id*=txtSourceRef]').val()
//var sdesc = $('[id*=txtSourceDescription]').val()

//var origloc = $('[id*=txtOriginalLocation]').val()

//var ldrl = $('[id*=txtLowerDateRangeLower]').val()
//var ldru = $('[id*=txtLowerDateRangeUpper]').val()
//var udrl = $('[id*=txtUpperDateRangeLower]').val()
//var udru = $('[id*=txtUpperDateRangeUpper]').val()



//var count = $('[id*=txtCountNo]').val()



//var isthac = $('[id*=chkIsThackrayFound]').is(':checked')

//var iscopy = $('[id*=chkIsCopyHeld]').is(':checked')
//var isview = $('[id*=chkIsViewed]').is(':checked')
//isCheck