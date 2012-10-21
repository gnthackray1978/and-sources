using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
//using System.Windows.Forms;
//using TDBCore.Datasets;
using GedItter.BLL;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Web;
using TDBCore.Types;

namespace GedItter.ModelObjects
{
    public class SourceEditorModel : EditorBaseModel<Guid>, ISourceEditorModel
    {

        string sourceDateStr = "";
        string sourceDateToStr = "";
        int sourceDateYearToInt = 2000;
        int sourceDateYearInt = 0;
        //todo implement sourceref
        string sourceRef = "";
        string sourceDescription = "";
        string sourceOriginalLocation = "";
        bool? isCopyHeld = false;
        bool? isViewed = false;
        bool? isThackrayFound = false;
        List<Guid> sourceFileIds = new List<Guid>();
        List<int> sourceTypeIdList = new List<int>();
        List<Guid> sourceParishs = new List<Guid>();

        string sourceNotes = "";
        string sourceFileCount = "0";

        bool isValidCount = false;


        #region validation vars
        bool isValidSourceDate = false;
        bool isValidSourceDateTo = false;
        bool isValidSourceDescription = false;
        bool isValidSourceOriginalLocation = false;
        bool isValidSourceRef = false;
        #endregion

        #region validation props
        public bool IsValidSourceDate
        {
            get 
            {
                return this.isValidSourceDate;

            }
        }

        public bool IsValidSourceDateTo
        {
            get 
            {
                return this.isValidSourceDateTo;
            }
        }

        public bool IsValidSourceDescription
        {
            get
            {
                return this.isValidSourceDescription;
            }
        }

        public bool IsValidSourceOriginalLocation
        {
            get
            {
                return this.isValidSourceOriginalLocation;
            }
        }

        public bool IsValidSourceRef
        {
            get
            {
                return this.isValidSourceRef;
            }
        }

        #endregion

        #region ISourceEditorModel Members


        #region field props

        public string SourceNotes
        {
            get 
            {
                return this.sourceNotes;
            }
        }

        public string SourceFileCount
        {
            get 
            {
                return this.sourceFileCount;
            }
        }

        public string SourceDateStr
        {
            get
            {
                return sourceDateStr;
                
            }
        }

        public string SourceDateToStr
        {
            get
            {
                return sourceDateToStr;

            }
        }

        public string SourceDescription
        {
            get
            {
                return sourceDescription;

            }
        }

        public string SourceOriginalLocation
        {
            get
            {
                return sourceOriginalLocation;

            }
        }

        public bool? IsCopyHeld
        {
            get
            {
                return isCopyHeld;

            }
        }

        public bool? IsViewed
        {
            get
            {
                return isViewed;

            }
        }

        public bool? IsThackrayFound
        {
            get
            {
                return isThackrayFound;

            }
        }

         

        #endregion


        public void SetSourceNotes(string param)
        {
            if (this.sourceNotes != param)
            {
                this.sourceNotes = param;

                SetModelStatusFields();
            }
        }

        public void SetSourceFileCount(string param)
        {
            if (this.sourceFileCount != param)
            {
                int tp =0;

                if (Int32.TryParse(param, out tp))
                {
                    this.isValidCount = true;
                    this.sourceFileCount = param;
                }
                else
                {
                    this.isValidCount = false;
                    this.sourceFileCount = "";
                }


                    
                SetModelStatusFields();
            }
        }

        public void SetSourceDateStr(string param)
        {

            param = param.Trim();

            if (this.sourceDateStr != param)
            {
               
                int year = CsUtils.GetDateYear(param);

                if (CsUtils.ValidYear(year))
                {
                    this.isValidSourceDate = true;
                    this.sourceDateYearInt = year;
                    this.sourceDateStr = param;
                }
                else
                {
                    this.isValidSourceDate = false;
                    this.sourceDateStr = param;
                }

                SetModelStatusFields();
            }
        }

        public void SetSourceDateToStr(string param)
        {

            param = param.Trim();

            

            if (this.sourceDateToStr != param)
            {
                int year = CsUtils.GetDateYear(param);

                if (CsUtils.ValidYear(year))
                {
                    this.isValidSourceDateTo = true;
                    this.sourceDateYearToInt = year;
                    this.sourceDateToStr = param;
                }
                else
                {
                    this.isValidSourceDateTo = false;
                    this.sourceDateToStr = param;
                }

                //DateTime result;

                //if (DateTime.TryParse(param, out result))
                //{
                //    this.isValidSourceDateTo = true;
                //    this.sourceDateYearToInt = result.Year;
                //    this.sourceDateToStr = result.ToShortDateString();
                //}
                //else
                //{
                //    this.isValidSourceDateTo = false;
                //    this.sourceDateToStr = param;
                //}

                SetModelStatusFields();
            }
        }

        public void SetSourceDescription(string param)
        {
            if (this.sourceDescription != param)
            {
                if (param.Length > 0 && param.Length <=1000)
                {
                    this.isValidSourceDescription = true;
                    this.sourceDescription = param;
                    
                }
                else
                {
                    if (param.Length == 0)
                        this.SetErrorState("Description too short");
                    if (param.Length > 1000)
                        this.SetErrorState("Description max length 1000 characters");

                    this.isValidSourceDescription = false;
                    this.sourceDescription = param;
                }

                SetModelStatusFields();
            }
        }

        public void SetSourceOriginalLocation(string param)
        {
            if (this.sourceOriginalLocation != param)
            {
                if (param.Length > 0 && param.Length <= 100)
                {
                    this.isValidSourceOriginalLocation = true;
                    this.sourceOriginalLocation = param;
                }
                else
                {
                    if (param.Length == 0)
                        this.SetErrorState("Original Loc. too short");
                    if (param.Length > 100)
                        this.SetErrorState("Original Loc. max length 1000 characters");

                    this.isValidSourceOriginalLocation = false;
                    this.sourceOriginalLocation = param;
                }

                SetModelStatusFields();
            }
        }

        public void SetIsCopyHeld(bool? param)
        {
            if (this.isCopyHeld != param)
            {

                this.isCopyHeld = param;
                SetModelStatusFields();
            }
        }

        public void SetIsViewed(bool? param)
        {
            if (this.isViewed != param)
            {
                this.isViewed = param;
                SetModelStatusFields();
                
            }
        }

        public void SetIsThackrayFound(bool? param)
        {
            if (this.isThackrayFound != param)
            {
                this.isThackrayFound = param;
                SetModelStatusFields();
            }
        }
      
 
        #endregion


        
        public string SourceRef
        {
            get
            {
                return this.sourceRef;
            }
        }

        public void SetSourceRef(string param)
        {
            if (this.sourceRef != param)
            {
                if (param.Length > 0 && param.Length <= 500)
                {
                    this.isValidSourceRef = true;
                    this.sourceRef = param;
                }
                else
                {
                    if (param.Length == 0)
                        this.SetErrorState("Ref too short");
                    if (param.Length > 500)
                        this.SetErrorState("Ref max length 1000 characters");

                    this.isValidSourceRef = false;
                    this.sourceRef = param;
                }

                SetModelStatusFields();
            }
        }



        public override void DeleteSelectedRecords()
        {
            if (!IsValidDelete()) return;

            if (IsValidSelectedRecordId)
            {
                BLL.SourceBLL sourceBll = new GedItter.BLL.SourceBLL();
                sourceBll.DeleteSource2(this.SelectedRecordId);


            }
            
           
            this.NotifyObservers<SourceEditorModel>(this);
        }

        public override void Refresh()
        {

            if (!IsvalidSelect()) return;

            IList<TDBCore.EntityModel.SourceType> _SourceTypesMappedToSourceDataTable = null;

            IList<TDBCore.EntityModel.File> filesMappedDataTable = null;

           // DsSources.SourcesDataTable sdt = null;
            
         //   DsSourceMappingParishs.SourceMappingParishsDataTable smpDataTable = null;
            IList<TDBCore.EntityModel.SourceMappingParish> smpDataTable = null;
            BLL.SourceBLL sourceBll = new GedItter.BLL.SourceBLL();
            BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();
            BLL.FilesBLL filesBLL = new FilesBLL();
            BLL.SourceMappingParishsBLL sourceMappingParishsBLL = new SourceMappingParishsBLL();


            var sdt = sourceBll.FillSourceTableById2(this.SelectedRecordId);

            if (sdt != null)
            {
                Debug.WriteLine("Data found");
                isValidSourceDate = true;
                isValidSourceDateTo = true;

                this.SetIsCopyHeld(sdt.IsCopyHeld);
                this.SetIsViewed(sdt.IsViewed);
                this.SetIsThackrayFound(sdt.IsThackrayFound);
                this.SetSelectedUserId(sdt.UserId);
                this.SetSourceDateStr(sdt.SourceDateStr);
                this.SetSourceDateToStr(sdt.SourceDateStrTo);
                this.SetSourceDescription(sdt.SourceDescription);
                this.SetSourceOriginalLocation(sdt.OriginalLocation);
                this.SetSourceRef(sdt.SourceRef);
                this.SetSourceFileCount(sdt.SourceFileCount.ToString());
                this.SetSourceNotes(sdt.SourceNotes);

            }
            else
            {
                Debug.WriteLine("Data not found");
                this.SetIsCopyHeld(false);
                this.SetIsViewed(false);
                this.SetIsThackrayFound(false);
                this.SetSelectedUserId(1);
                this.SetSourceDateStr("");
                this.SetSourceDateToStr("");
                this.SetSourceDescription("");
                this.SetSourceOriginalLocation("");
                this.SetSourceRef("");
                this.SetSourceFileCount("0");
                this.SetSourceNotes("");
            }


           // filesDataTable = filesBLL.GetFiles(fileName, fileDescription);

            if (this.SelectedRecordId != Guid.Empty)
                filesMappedDataTable = filesBLL.GetFilesByParentId2(this.SelectedRecordId).ToList();

            List<Guid> fileids = new List<Guid>();
            if (filesMappedDataTable != null)
            {
                if (filesMappedDataTable.Count > 0)
                {
                    foreach (var fRow in filesMappedDataTable)
                    {
                        fileids.Add(fRow.FiletId);
                    }

                    this.SetSourceFileIds(fileids);
                }
            }
            //this.SetSourceFileIds(

            //_SourceTypesDataTable = sourceTypesBll.GetSourceTypeByFilter(sourceTypeDesc, this.SelectedUserId);

            if (this.SelectedRecordId != Guid.Empty)
                _SourceTypesMappedToSourceDataTable = sourceTypesBll.GetSourceTypeBySourceId2(this.SelectedRecordId).ToList();

            List<int> sourceTypes = new List<int>();
            if (_SourceTypesMappedToSourceDataTable != null)
            {

                if (_SourceTypesMappedToSourceDataTable.Count > 0)
                {
                    foreach (var sTypeRow in _SourceTypesMappedToSourceDataTable)
                    {
                        sourceTypes.Add(sTypeRow.SourceTypeId);
                    }

                    this.SetSourceTypeIdList(sourceTypes);
                }
            }



            smpDataTable = sourceMappingParishsBLL.GetDataBySourceId2(this.SelectedRecordId).ToList();


            List<Guid> sourceParishs = new List<Guid>();

            if (smpDataTable != null)
            {
                if (smpDataTable.Count > 0)
                {
                    foreach (var smpRow in smpDataTable)
                    {
                        sourceParishs.Add(smpRow.Parish.ParishId);
                    }

                    this.SetSourceParishs(sourceParishs);
                }
            }

            this.NotifyObservers<SourceEditorModel>(this);

        }


        public override void EditSelectedRecord()
        {
            if (!IsValidEdit()) return;

            if (IsValidEntry)
            {

                int imgCount = 0;

                Int32.TryParse(this.sourceFileCount, out imgCount);

               // DsSources.SourcesDataTable sdt = null;
                BLL.SourceBLL sourceBll = new GedItter.BLL.SourceBLL();
                BLL.SourceMappingsBLL smBll = new SourceMappingsBLL();
                BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();
                

                sourceBll.UpdateSource2(this.SelectedRecordId,
                    this.SourceDescription, this.SourceOriginalLocation,
                    this.IsCopyHeld.Value,
                    this.IsViewed.Value,
                    this.IsThackrayFound.Value,
                    this.SelectedUserId,
                    this.SourceDateStr,
                    this.SourceDateToStr,
                    this.sourceDateYearInt,
                    this.sourceDateYearToInt,
                    this.sourceRef,
                    imgCount,
                    this.sourceNotes);

                smBll.WriteFilesToSource(this.SelectedRecordId, this.SourceFileIds, this.SelectedUserId);

                smBll.WriteSourceTypesToSource(this.SelectedRecordId, this.SourceTypeIdList, this.SelectedUserId);

                smBll.WriteParishsToSource(this.SelectedRecordId, this.SourceParishs, this.SelectedUserId);
            }
            else
            {
               // MessageBox.Show("WARNING", "Invalid Source ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            
            base.EditSelectedRecord();
            //this.Refresh();
            this.NotifyObservers<SourceEditorModel>(this);
        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            int imgCount = 0;

            Int32.TryParse(this.sourceFileCount, out imgCount);


            if (IsValidEntry)
            {
              //  DsSources.SourcesDataTable sdt = null;
                BLL.SourceBLL sourceBll = new GedItter.BLL.SourceBLL();
                BLL.SourceMappingsBLL smBll = new SourceMappingsBLL();
                BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();
            

                this.SetSelectedRecordId(
                sourceBll.InsertSource2(
                    this.SourceDescription, this.SourceOriginalLocation,
                    this.IsCopyHeld.Value,
                    this.IsViewed.Value,
                    this.IsThackrayFound.Value,
                    this.SelectedUserId,
                    this.SourceDateStr,
                    this.SourceDateToStr,
                    this.sourceDateYearInt,
                    this.sourceDateYearToInt,sourceRef,
                    imgCount,
                    this.sourceNotes)
                    );

                smBll.WriteFilesToSource(this.SelectedRecordId, this.SourceFileIds, this.SelectedUserId);

                smBll.WriteSourceTypesToSource(this.SelectedRecordId, this.SourceTypeIdList, this.SelectedUserId);

                smBll.WriteParishsToSource(this.SelectedRecordId, this.SourceParishs, this.SelectedUserId);

                this.OnDataSaved();
            }


            this.NotifyObservers<SourceEditorModel>(this);
        }
  
        public override bool IsValidEntry
        {
            get
            {
                if (this.IsValidSourceDate 
                    && IsValidSourceDateTo 
                    && IsValidSourceDescription)
                {
                    this.SetErrorState("");
                    return true;
                }
                else
                {
                    this.SetErrorState("");

                    if (!IsValidSourceDate)
                        this.SetErrorState(this.ErrorState + " Invalid Source Date");
                    if (!IsValidSourceDateTo)
                        this.SetErrorState(this.ErrorState + " Invalid Source Date To");
                    if (!IsValidSourceDescription)
                        this.SetErrorState(this.ErrorState + " Invalid Source Desc.");
                    return false;
                }
            }
        }

        public override void SetModelStatusFields()
        {
            
            
            base.SetModelStatusFields();
        }

        public List<Guid> SourceFileIds
        {
            get 
            {
                return this.sourceFileIds;
            }
        }

        public List<int> SourceTypeIdList
        {
            get 
            {
                return this.sourceTypeIdList;
            }
        }

        public void SetSourceFileIds(List<Guid> param)
        {

            this.sourceFileIds = param;
            SetModelStatusFields();
            
        }

        public void SetSourceTypeIdList(List<int> param)
        {
            this.sourceTypeIdList = param;
            SetModelStatusFields();
        }

        public List<Guid> SourceParishs
        {
            get 
            {
                return this.sourceParishs;
            }
        }

        public void SetSourceParishs(List<Guid> param)
        {
            this.sourceParishs = param;
            this.SetModelStatusFields();
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

            this.SetSelectedRecordId(selectedId);

       //     this.Refresh();

        }
 
    }
}
