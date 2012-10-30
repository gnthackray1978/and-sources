using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
////using TDBCore.Datasets;
using TDBCore.Types;

using System.IO;


namespace GedItter.ModelObjects
{
    public class SourceTypeMappingModel : EditorBaseModel<int>, ISourceTypeMapSourceModel
    {
        SourceTypeEditorModel sourceTypeEditorModel = new SourceTypeEditorModel();
     //   ISourceTypeFilterModel _sourceTypes = null;

        IList<TDBCore.EntityModel.SourceType> _SourceTypesDataTable = null;

        IList<TDBCore.EntityModel.SourceType> _SourceTypesMappedToSourceDataTable = null;


        string sourceTypeDesc = "";
        Guid parentId = Guid.Empty;
        int selectedSourceMapTypeId = 0;
        int selectedMappedSourceTypeId = 0;

        ISourceTypeEditorUI iSourceTypeEditorUI;

        public IList<TDBCore.EntityModel.SourceType> SourceTypesDataTable
        {
            get
            {
                return this._SourceTypesDataTable;
            }

        }


        public IList<TDBCore.EntityModel.SourceType> SourceTypesMappedToSourceDataTable
        {
            get
            {
                return this._SourceTypesMappedToSourceDataTable;
            }
        }

        public ISourceTypeEditorUI ISourceTypeEditorUI
        {
            get
            {
                return this.iSourceTypeEditorUI;
            }
        }


        public int SelectedSourceMapTypeId
        {
            get
            {
                return this.selectedSourceMapTypeId;
            }
        }

        public int SelectedMappedSourceTypeId
        {
            get
            {
                return selectedMappedSourceTypeId;
            }
        }
        public void SetSelectedMappedSourceTypeId(int mapTypeId)
        {
            if (this.selectedMappedSourceTypeId != mapTypeId)
            {
                //Debug.WriteLine("selected map type " + this.selectedMappedSourceTypeId.ToString());
                this.selectedMappedSourceTypeId = mapTypeId;
                //MessageBox.Show("selected source type: " + mapTypeId.ToString());
                this.SetModelStatusFields();
            }
        }

        public void SetSelectedSourceMapTypeId(int mapTypeId)
        {


            if (this.selectedSourceMapTypeId != mapTypeId)
            {

                this.selectedSourceMapTypeId = mapTypeId;
                this.SetModelStatusFields();
            }

        }


        public void RemoveMapping()
        {
            if (!IsValidDelete()) return;

            if (this.parentId != Guid.Empty)
            {
                //MessageBox.Show("request remove with: " + this.selectedMappedSourceTypeId.ToString());
                BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();

                sourceMappingsBLL.DeleteByMapTypeIdAndSourceId(this.ParentId, this.selectedMappedSourceTypeId);
            }
            else
            {
                RemoveDisconnectedSourceMapRow(this.selectedMappedSourceTypeId);
            }

            this.Refresh();

        }

        public void AddMapping()
        {


            if (!IsValidInsert()) return;

            if (this.parentId != Guid.Empty)
            {
                BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();

                sourceMappingsBLL.Insert(this.ParentId, null, null, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), this.selectedSourceMapTypeId);
            }
            else
            {
                AddDisconnectedSourceMapRow(this.selectedSourceMapTypeId);
            }
            this.Refresh();
        }



        public void AddDisconnectedSourceMapRow(int paramInt)
        {
            //BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();
            BLL.SourceTypesBLL sourceTypeBLL = new GedItter.BLL.SourceTypesBLL();

            var tpTable = sourceTypeBLL.GetSourceTypeById2(paramInt).FirstOrDefault(); // new DsSourceTypes.SourceTypesDataTable();


            if (_SourceTypesMappedToSourceDataTable == null) _SourceTypesMappedToSourceDataTable = new List<TDBCore.EntityModel.SourceType>();

            TDBCore.EntityModel.SourceType sRow = new TDBCore.EntityModel.SourceType();

            sRow.SourceDateAdded = DateTime.Today.ToShortDateString();
            sRow.SourceTypeDesc = tpTable.SourceTypeDesc;
            sRow.SourceTypeId = tpTable.SourceTypeId;
            sRow.SourceUserAdded = tpTable.SourceUserAdded;

            //todo REWRITE!!!!
            bool isFound = false;
            foreach (var _sRow in _SourceTypesMappedToSourceDataTable)
            {
                if (_sRow.SourceTypeId == paramInt) isFound = true;
            }

            if (!isFound)
            {

                this._SourceTypesMappedToSourceDataTable.Add(sRow);
            }

           

            //this._SourceTypesMappedToSourceDataTable.AcceptChanges();

        }

        public void RemoveDisconnectedSourceMapRow(int paramInt)
        {
            if (_SourceTypesMappedToSourceDataTable != null)
            {
                // extension methods they're fucking brilliant!              
                _SourceTypesMappedToSourceDataTable.Remove(st => st.SourceTypeId == paramInt);


                // int idx = 0;
                //while (idx < _SourceTypesMappedToSourceDataTable.Count)
                //{
                //    if (_SourceTypesMappedToSourceDataTable[idx].SourceTypeId == paramInt)
                //    {
                //        _SourceTypesMappedToSourceDataTable[idx].Delete();
                //    }
                //    idx++;
                //}

                ////this._SourceTypesMappedToSourceDataTable.remo
                //this._SourceTypesMappedToSourceDataTable.AcceptChanges();
            }
        }

        public void AddOutstandingMappings(Guid oldParentId, Guid newParentId)
        {
            if (!IsValidInsert()) return;

            if (oldParentId == Guid.Empty &&
                newParentId != Guid.Empty)
            {
                if (_SourceTypesMappedToSourceDataTable != null)
                {
                    foreach (var sRow in _SourceTypesMappedToSourceDataTable)
                    {
                        BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();

                        sourceMappingsBLL.Insert(newParentId, null, null, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), sRow.SourceTypeId);

                    }
                }
            }
        }


        public override void Refresh()
        {

            if (!IsvalidSelect()) return;

            BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();

            _SourceTypesDataTable = sourceTypesBll.GetSourceTypeByFilter2(sourceTypeDesc, this.SelectedUserId).ToList();

            if (this.parentId != Guid.Empty && !this.IsReadOnly)
            {
                _SourceTypesMappedToSourceDataTable = sourceTypesBll.GetSourceTypeBySourceId2(this.parentId).ToList();

            }


            List<int> tpList = new List<int>();
            if (_SourceTypesMappedToSourceDataTable != null)
            {
                foreach (var stRow in _SourceTypesMappedToSourceDataTable)
                {
                    tpList.Add(stRow.SourceTypeId);
                }
            
            }
            this.SetSelectedRecordIds(tpList);

            this.NotifyObservers<SourceTypeMappingModel>(this);
        }


        #region insert edit and delete source types

        // add new source type
        public override void InsertNewRecord()
        {
            if (!IsValidInsert()) return;

            sourceTypeEditorModel = new SourceTypeEditorModel();
           // sourceTypeEditorModel.DataSaved += new EventHandler(sourceTypeEditorModel_DataSaved);
            this.SetDataSaved(new EventHandler(sourceTypeEditorModel_DataSaved));

            this.ShowDialogInsert(this);

        }

        void sourceTypeEditorModel_DataSaved(object sender, EventArgs e)
        {
            this.Refresh();
        }

        // edit source type
        public override void EditSelectedRecord()
        {
            if (!IsvalidSelect()) return;


            sourceTypeEditorModel = new SourceTypeEditorModel();
            //sourceTypeEditorModel.DataSaved += new EventHandler(sourceTypeEditorModel_DataSaved);
            this.SetDataSaved(new EventHandler(sourceTypeEditorModel_DataSaved));

            this.ShowDialogEdit(this);

        }
        // delete source type
        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            BLL.SourceTypesBLL sourceTypesBll = new GedItter.BLL.SourceTypesBLL();
            sourceTypesBll.DeleteSourceType(this.selectedSourceMapTypeId);
            
            this.selectedMappedSourceTypeId = 0;
            
            this.Refresh();
        }

        #endregion



        public void SetEditorUI()
        {
        //    this.iSourceTypeEditorUI = new FrmEditSourceTypes();
        }


        public void SetSourceTypesDescrip(string param)
        {
            if (this.sourceTypeDesc != param)
            {
                this.sourceTypeDesc = param;
                this.Refresh();
                this.SetModelStatusFields();
            }
        }

        public Guid ParentId
        {
            get
            {
                return this.parentId;
            }
        }

        public void SetParent(Guid paramGuid)
        {
            if (paramGuid != parentId)
            {

                AddOutstandingMappings(parentId, paramGuid);
                this.parentId = paramGuid;
                this.Refresh();

                this.SetModelStatusFields();

            }


        }



        //#region observers

        //public void AddObserver(ISourceTypeMapSourceView paramView)
        //{
        //    aList.Add(paramView);
        //}

        //public void RemoveObserver(ISourceTypeMapSourceView paramView)
        //{
        //    aList.Remove(paramView);
        //}

        //public void NotifyObservers()
        //{
        //    foreach (ISourceTypeMapSourceView view in aList)
        //    {
        //        view.Update(this);
        //    }
        //}

        //#endregion

        public string SourceTypeDescription
        {
            get
            {
                return this.sourceTypeDesc;
            }

        }

        public ISourceTypeEditorModel ISourceTypeEditorModel
        {
            get
            {
                return this.sourceTypeEditorModel;
            }
        }




        public void ClearMappings()
        {
            _SourceTypesMappedToSourceDataTable = new List<TDBCore.EntityModel.SourceType>();

            this.Refresh();
        }
    }
}
