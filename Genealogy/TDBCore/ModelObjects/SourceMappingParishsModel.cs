using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
//using TDBCore.Datasets;

namespace GedItter.ModelObjects
{
    public class SourceMappingParishsModel : EditorBaseModel<Guid>, ISourceMappingParishsModel
    {

        Guid parentId = Guid.Empty;
        Guid selectedParishId = Guid.Empty;
        Guid selectedMappedParishId = Guid.Empty;

        ParishsEditorModel parishEditorModel = new ParishsEditorModel();
 
        string parishDescription = "";
       

       // DsParishs.ParishsDataTable parishsDataTable = null;
        IList<TDBCore.EntityModel.Parish> parishsDataTable = null;
        
       // DsParishs.ParishsDataTable sourceMappingParishsDataTable = null;
        IList<TDBCore.EntityModel.Parish> sourceMappingParishsDataTable = null;


        
        
        
        public SourceMappingParishsModel()
        {
            this.SetIsReadOnly(false);
        }


        public IParishsEditorModel IParishsEditorModel
        {
            get 
            {
                return this.parishEditorModel;
            }
        }

        #region ISourceMappingParishsModel Members

        public Guid ParentId
        {
            get 
            {
                return parentId;
            }
        }

        public Guid SelectedParishId
        {
            get
            {
                return selectedParishId;
            }

        }

        public Guid SelectedMappedParishId
        {
            get 
            {
                return selectedMappedParishId; 
            }
        }



        public string ParishDescription
        {
            get 
            {
                return this.parishDescription;
            }
        }

        public IList<TDBCore.EntityModel.Parish> ParishsDataTable
        {
            get 
            {
                return this.parishsDataTable;
            }
        }

        public IList<TDBCore.EntityModel.Parish> SourceMappingParishsDataTable
        {
            get
            {
                return this.sourceMappingParishsDataTable;
            }
        }




        public void SetSelectedParishId(Guid paramGuid)
        {
            if (this.selectedParishId != paramGuid)
            {
               
                this.selectedParishId = paramGuid;
          
                this.SetModelStatusFields();
            }
        }

        public void SetSelectedMappedParishId(Guid paramGuid)
        {
            if (this.selectedMappedParishId != paramGuid)
            {

                this.selectedMappedParishId = paramGuid;

                this.SetModelStatusFields();
            }
        }



        public void SetParent(Guid parentId)
        {
            if (this.parentId != parentId)
            {
                AddOutstandingMappings(this.parentId, parentId);
                this.parentId = parentId;
                this.SetModelStatusFields();
            }
        }

        public void SetParishDescription(string param)
        {
            if (this.parishDescription != param)
            {
                this.parishDescription = param;
                this.SetModelStatusFields();
            }
        }



        public void RemoveMapping()
        {

            if (!IsValidDelete()) return;

            if (this.parentId != Guid.Empty)
            {
                BLL.SourceMappingParishsBLL sourceMappingParishsBLL = new GedItter.BLL.SourceMappingParishsBLL();

                sourceMappingParishsBLL.DeleteBySourceIdAndParishId2(this.parentId,this.selectedMappedParishId);
          

            }
            else
            {
                RemoveDisconnectedSourceMapRow(this.selectedMappedParishId);
            }

            this.Refresh();
        }

        public void AddMapping()
        {

            if (!IsValidInsert()) return;

            if (this.parentId != Guid.Empty)
            {
                BLL.SourceMappingParishsBLL sourceMappingParishsBLL = new GedItter.BLL.SourceMappingParishsBLL();

                sourceMappingParishsBLL.InsertSourceMappingParish2(this.selectedParishId, this.parentId, 1);
               // sourceMappingsBLL.Insert(this.ParentId, null, null, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), this.selectedSourceMapTypeId);
            }
            else
            {
                AddDisconnectedSourceMapRow(this.selectedParishId);
            }
            this.Refresh();
        }

        public void AddDisconnectedSourceMapRow(Guid paramGuid)
        {
            //BLL.SourceTypesBLL sourceTypeBLL = new GedItter.BLL.SourceTypesBLL();
            BLL.ParishsBLL parishsBLL = new GedItter.BLL.ParishsBLL();

            //DsSourceTypes.SourceTypesDataTable tpTable = sourceTypeBLL.GetSourceTypeById(paramInt); // new DsSourceTypes.SourceTypesDataTable();


            TDBCore.EntityModel.Parish tpTable = parishsBLL.GetParishById2(paramGuid);

            if (this.sourceMappingParishsDataTable == null) sourceMappingParishsDataTable = new List<TDBCore.EntityModel.Parish>();

            //DsSourceTypes.SourceTypesRow sRow = _SourceTypesMappedToSourceDataTable.NewSourceTypesRow();

          //  DsParishs.ParishsRow pRow = this.sourceMappingParishsDataTable.NewParishsRow();
            
            TDBCore.EntityModel.Parish newParish = new TDBCore.EntityModel.Parish();

            newParish = tpTable;
           //pRow.ItemArray = tpTable[0].ItemArray;


            //sRow.SourceDateAdded = DateTime.Today;
            //sRow.SourceTypeDesc = tpTable[0].SourceTypeDesc;
            //sRow.SourceTypeId = tpTable[0].SourceTypeId;
            //sRow.SourceUserAdded = tpTable[0].SourceUserAdded;

            ////todo REWRITE!!!!
            bool isFound = false;

            foreach (var _pRow in this.sourceMappingParishsDataTable)
            {
                if (_pRow.ParishId == paramGuid) isFound = true;
            }
            //foreach (DsSourceTypes.SourceTypesRow _sRow in _SourceTypesMappedToSourceDataTable)
            //{
            //    if (_sRow.SourceTypeId == paramInt) isFound = true;
            //}

            if (!isFound)
                this.sourceMappingParishsDataTable.Add(newParish);

            //    this._SourceTypesMappedToSourceDataTable.AddSourceTypesRow(sRow);


            //this.sourceMappingParishsDataTable.AcceptChanges();
            //this._SourceTypesMappedToSourceDataTable.AcceptChanges();

        }

        public void RemoveDisconnectedSourceMapRow(Guid paramGuid)
        {


            if (this.sourceMappingParishsDataTable != null)
            {
              
                int idx = 0;
              

                while (idx < this.sourceMappingParishsDataTable.Count)
                {
                 
                    if (this.sourceMappingParishsDataTable[idx].ParishId == paramGuid)
                    {
                        this.sourceMappingParishsDataTable.RemoveAt(idx);
                        break;
                    }
                  
                    idx++;
                }
                
            }
        }

        public void AddOutstandingMappings(Guid oldParentId, Guid newParentId)
        {

            if (!IsValidInsert()) return;

            if (oldParentId == Guid.Empty &&
                 newParentId != Guid.Empty)
            {
                if (this.sourceMappingParishsDataTable != null)
                {
                    foreach (var sRow in sourceMappingParishsDataTable)
                    {
                      //  BLL.SourceMappingsBLL sourceMappingsBLL = new GedItter.BLL.SourceMappingsBLL();
                        BLL.SourceMappingParishsBLL sourceMappingParishBLL = new GedItter.BLL.SourceMappingParishsBLL();
                        sourceMappingParishBLL.InsertSourceMappingParish2(sRow.ParishId, this.parentId, this.SelectedUserId);
                        //sourceMappingsBLL.Insert(newParentId, null, null, this.SelectedUserId, null, DateTime.Today.ToShortDateString(), sRow.SourceTypeId);

                    }
                }
            }
        }


        public override void Refresh()
        {


            if (!IsvalidSelect()) return;


            BLL.ParishsBLL parishsBLL = new GedItter.BLL.ParishsBLL();


            //_SourceTypesDataTable = sourceTypesBll.GetSourceTypeByFilter(sourceTypeDesc, this.SelectedUserId);
            this.parishsDataTable = parishsBLL.GetParishByNameFilter2(this.parishDescription).ToList();


            if (this.parentId != Guid.Empty && !this.IsReadOnly)
                this.sourceMappingParishsDataTable = parishsBLL.GetParishBySourceId2(this.parentId).ToList();


            List<Guid> tpList = new List<Guid>();

            if (this.sourceMappingParishsDataTable != null)
            {

               // this.sourceMappingParishsDataTable.AcceptChanges();
                foreach (var stRow in this.sourceMappingParishsDataTable)
                {
                    tpList.Add(stRow.ParishId);
                }

            }
            this.SetSelectedRecordIds(tpList);

            this.NotifyObservers<SourceMappingParishsModel>(this);
        }


  
        #endregion


        public override void InsertNewRecord()
        {
       //     parishEditorModel.DataSaved += new EventHandler(parishEditorModel_DataSaved);
            base.SetDataSaved(new EventHandler(parishEditorModel_DataSaved));

            this.ShowDialogInsert(this);
        }

        void parishEditorModel_DataSaved(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public override void DeleteSelectedRecords()
        {

            if (!IsValidDelete()) return;

            BLL.ParishsBLL parishsBll = new GedItter.BLL.ParishsBLL();

            foreach (Guid parishIdx in this.SelectedRecordIds)
            {
                parishsBll.DeleteParishById(parishIdx);
            }

            Refresh();
        }

        public override void EditSelectedRecord()
        {

            if (!IsvalidSelect()) return;

            parishEditorModel.SetSelectedRecordId(this.SelectedParishId);



            this.ShowDialogEdit(this);
        }







        public IParishEditorUI IParishEditorUI
        {
            get { throw new NotImplementedException(); }
        }
        public void SetEditorUI()
        {
            throw new NotImplementedException();
        }



        public void ClearMappings()
        {
            this.sourceMappingParishsDataTable = new List<TDBCore.EntityModel.Parish>();
            Refresh();

        }
    }
}
