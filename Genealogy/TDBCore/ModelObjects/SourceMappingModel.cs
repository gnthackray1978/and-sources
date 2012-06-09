using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
//using TDBCore.Datasets;
using TDBCore.Types;
using System.Diagnostics;

namespace GedItter.ModelObjects
{
    public class SourceMappingModel : EditorBaseModel<Guid>, ISourceMappingModel
    {
        bool isMarriage = false;
        
        Guid parentId;
        Guid selectedSourceId;
        Guid selectedMappedSourceId;
        string sourceRef = "";
        IList<TDBCore.EntityModel.Source> sourcesDataTable = null;
        IList<TDBCore.EntityModel.Source> sourcesMappedDataTable = null;
        ISourceEditorUI iSourceEditorUi = null;
       // SourceEditorModel iSourceEditorModel = null;

        #region constructor
        public SourceMappingModel()
        {
            this.SetIsReadOnly(true);
        }
        #endregion


        #region properties

        public Guid ParentId
        {
            get 
            {
                return this.parentId;
            }
        }

        public Guid SelectedSourceId
        {
            get 
            {
                return this.selectedSourceId;
            }
        }

        public Guid SelectedMappedSourceId
        {
            get 
            {
                return this.selectedMappedSourceId;
            }
        }

        public string SourceDescription
        {
            get 
            {
                return this.sourceRef;
            }
        }

        public IList<TDBCore.EntityModel.Source> SourcesDataTable
        {
            get 
            {
                return this.sourcesDataTable;
            }
        }

        public IList<TDBCore.EntityModel.Source> SourcesMappedDataTable
        {
            get
            {
                return this.sourcesMappedDataTable;
            }
        }

        public ISourceEditorUI ISourceEditorUI
        {
            get 
            {
                return this.iSourceEditorUi;
            }
        }

        public string SourceRef
        {
            get
            {
                return this.sourceRef;
            }


        }


        #endregion



        public void SetSelectedSourceId(Guid mapFileId)
        {
            if (mapFileId != selectedSourceId)
            {
                this.selectedSourceId = mapFileId;
                this.SetModelStatusFields();
            }
        }

        public void SetSelectedMappedSourceId(Guid mapFileId)
        {
            if (mapFileId != selectedMappedSourceId)
            {
                this.selectedMappedSourceId = mapFileId;
                this.SetModelStatusFields();
            }
        }

        public void SetParentId(Guid param)
        {
            if (param != this.parentId)
            {

                AddOutstandingMappings(this.parentId, param);
                this.parentId = param;
                this.Refresh();
                this.SetModelStatusFields();
            }            
        }

        public void SetParentPersonId(Guid parentId)
        {
            isMarriage = false;
            SetParentId(parentId);

        }

        public void SetParentMarriageId(Guid parentId)
        {
            isMarriage = true;
            SetParentId(parentId);


        }

        public void SetSourceDescrip(string param)
        {
            //todo sort me out
        }

        public void SetSourceRef(string param)
        {
            if (param != sourceRef)
            {
                this.sourceRef = param;
                this.SetModelStatusFields();
            }
        }



        public void RemoveMapping()
        {


            if (!IsValidDelete()) return;

            if (this.parentId != Guid.Empty && !this.IsReadOnly)
            {
                Debug.WriteLine("RemoveMapping DeleteBySourceIdMarriageIdOrPersonId");

                BLL.SourceMappingsBLL _SourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();

                _SourceMappingsBLL.DeleteBySourceIdMarriageIdOrPersonId(this.parentId, this.selectedMappedSourceId);

            }
            else
            {
                Debug.WriteLine("RemoveMapping RemoveDisconnectedSourceMapRow");

                RemoveDisconnectedSourceMapRow(this.selectedMappedSourceId);
            }

            this.Refresh();
        }

        public void AddMapping()
        {
            if (!IsValidInsert()) return;

            if (this.parentId != Guid.Empty && !this.IsReadOnly)
            {
                InsertMapping(this.SelectedSourceId, this.parentId);
            }
            else
            { 
                AddDisconnectedSourceMapRow(this.selectedSourceId);            
            }

            this.Refresh();
        }

        private void InsertMapping(Guid sourceId, Guid paramGuid)
        {
            if (!IsValidInsert()) return;

            if (!this.IsReadOnly)
            {
                BLL.SourceMappingsBLL _SourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();
                if (isMarriage)
                    _SourceMappingsBLL.Insert(sourceId, null, paramGuid, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), null);
                else
                    _SourceMappingsBLL.Insert(sourceId, null, null, this.SelectedUserId, paramGuid, DateTime.Today.ToShortDateString(), null);
            }
        }



        public void AddDisconnectedSourceMapRow(Guid sourceRowId)
        {
            //DsSources.SourcesDataTable sourcesMappedDataTable = null;
            if (sourceRowId == Guid.Empty) return;

            BLL.SourceBLL sourcesBll = new GedItter.BLL.SourceBLL();
            var tpTable = sourcesBll.FillSourceTableById2(sourceRowId);

            if (sourcesMappedDataTable == null) sourcesMappedDataTable = new List<TDBCore.EntityModel.Source>();

            //DsSources.SourcesRow sRow = sourcesMappedDataTable.NewSourcesRow();

            TDBCore.EntityModel.Source sRow = new TDBCore.EntityModel.Source();


            sRow.SourceId = tpTable.SourceId;
            sRow.UserId = tpTable.UserId;
            sRow.SourceDescription = tpTable.SourceDescription;
            sRow.SourceRef = tpTable.SourceRef;
            
            //todo REWRITE!!!!
            bool isFound = false;
            foreach (var _sRow in sourcesMappedDataTable)
            {
                if (_sRow.SourceId == sourceRowId) isFound = true;
            }
    
            if(!isFound )
                this.sourcesMappedDataTable.Add(sRow);


        //    this.SourcesMappedDataTable.AcceptChanges();

            this.Refresh();
        }

        public void RemoveDisconnectedSourceMapRow(Guid sourceRowId)
        {

            Debug.WriteLine("RemoveDisconnectedSourceMapRow");

            if (sourcesMappedDataTable != null)
            {
                

                sourcesMappedDataTable.Remove(s => s.SourceId == sourceRowId);
                


                this.Refresh();
            }
        }

        public void AddOutstandingMappings(Guid oldParentId, Guid newParentId)
        {
            // if we are going from no id to a id, then 
            // we must be adding a new record
            // in which case we can add any mappings we have that arent assigned
            // because we never had a id
            if (oldParentId == Guid.Empty &&
                newParentId != Guid.Empty)
            {
                if (sourcesMappedDataTable != null)
                {
                    foreach (var sRow in sourcesMappedDataTable)
                    {
                        InsertMapping(sRow.SourceId, newParentId);
                    }
                }
            }
        }





        public void SetEditorUI()
        {
           // this.iSourceEditorUi = new FrmEditSource();
        }



        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            base.DeleteSelectedRecords();

            BLL.SourceBLL sourceBLL = new GedItter.BLL.SourceBLL();

            sourceBLL.DeleteSource2(this.selectedSourceId);

        }

        public override void EditSelectedRecord()
        {
            if (!IsvalidSelect()) return;

            SourceEditorModel iSourceEditorModel = new SourceEditorModel();
            this.SetEditorUI();
            iSourceEditorModel.SetSelectedRecordId(this.selectedSourceId);

            if (ISourceEditorUI != null)
            {
                iSourceEditorUi.SetEditorModal(iSourceEditorModel);
                iSourceEditorUi.Show();
            }
            else
            {
                this.isDataChanged = true;
                this.ShowDialogEdit(this);

            }
        }

        public override void InsertNewRecord()
        {

            if (!IsValidInsert()) return;

            SourceEditorModel iSourceEditorModel = new SourceEditorModel();
            this.SetEditorUI();
            iSourceEditorModel.SetSelectedRecordId(System.Guid.Empty);

            iSourceEditorUi.SetEditorModal(iSourceEditorModel);
            iSourceEditorUi.Show();
        }

        public override void Refresh()
        {
            if (!IsvalidSelect()) return;

            BLL.SourceBLL sourceBLL = new GedItter.BLL.SourceBLL ();
            


            if(this.parentId != Guid.Empty && !this.IsReadOnly)
                sourcesMappedDataTable = sourceBLL.FillSourceTableByPersonOrMarriageId2(this.parentId).ToList();

            sourcesDataTable = sourceBLL.FillSourceTableBySourceRef2(sourceRef).ToList();

            List<Guid> tpList = new List<Guid>();

            if (sourcesMappedDataTable != null)
            {
                foreach (var _sRow in sourcesMappedDataTable)
                    tpList.Add(_sRow.SourceId);
            }
            
            this.SetSelectedRecordIds(tpList);

            this.NotifyObservers<SourceMappingModel>(this);
        }

        public override void SetModelStatusFields()
        {
            base.SetModelStatusFields();
        }

    }
}
