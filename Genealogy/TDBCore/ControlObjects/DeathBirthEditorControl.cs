using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.ModelObjects;

namespace GedItter.BirthDeathRecords
{
    public class DeathBirthEditorControl: EditorBaseControl<Guid>, IDeathBirthEditorControl
    {
        protected new IDeathBirthEditorModel Model;
        protected new IDeathBirthEditorView View;

        public DeathBirthEditorControl(IDeathBirthEditorModel paramModel, IDeathBirthEditorView paramView)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }

        public DeathBirthEditorControl()
        {

        }

        #region IDeathBirthEditorControl Members

        public void RequestSetEditorReferenceLocationId(Guid param)
        {
            if (Model != null)
            {
                Model.SetEditorReferenceLocationId(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorBirthLocationId(Guid param)
        {
            if (Model != null)
            {
                Model.SetEditorBirthLocationId(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorDeathLocationId(Guid param)
        {
            if (Model != null)
            {
                Model.SetEditorDeathLocationId(param);

                if (View != null) SetView();
            }
        }



        


        public void RequestSetSelectedId(Guid DeathBirthId)
        {
            if (Model != null)
            {
                Model.SetSelectedRecordId(DeathBirthId);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorDateBirthString(string dBirth)
        {
            if (Model != null)
            {
                Model.SetEditorDateBirthString(dBirth);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorDateDeathString(string dDeath)
        {
            if (Model != null)
            {
                Model.SetEditorDateDeathString(dDeath);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorDateBapString(string dBap)
        {
            if (Model != null)
            {
                Model.SetEditorDateBapString(dBap);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorChristianName(string cName)
        {
            if (Model != null)
            {
                Model.SetEditorChristianName(cName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorSurnameName(string sName)
        {
            if (Model != null)
            {
                Model.SetEditorSurnameName(sName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorFatherChristianName(string fCName)
        {
            if (Model != null)
            {
                Model.SetEditorFatherChristianName(fCName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorFatherSurname(string fSName)
        {
            if (Model != null)
            {
                Model.SetEditorFatherSurname(fSName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMotherChristianName(string mCName)
        {
            if (Model != null)
            {
                Model.SetEditorMotherChristianName(mCName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMotherSurname(string mSName)
        {
            if (Model != null)
            {
                Model.SetEditorMotherSurname(mSName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorIsMale(bool isMale)
        {
            if (Model != null)
            {
                Model.SetEditorIsMale(isMale);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorBirthLocation(string bLocation)
        {
            if (Model != null)
            {
                Model.SetEditorBirthLocation(bLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorDeathLocation(string dLocation)
        {
            if (Model != null)
            {
                Model.SetEditorDeathLocation(dLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorSource(string source)
        {
            if (Model != null)
            {
                Model.SetEditorSource(source);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorNotes(string notes)
        {
            if (Model != null)
            {
                Model.SetEditorNotes(notes);

                if (View != null) SetView();
            }
        }

        public void RequestRefresh()
        {
            if (Model != null)
            {
                Model.Refresh();

                if (View != null) SetView();
            }
        }

        public void RequestDelete()
        {
            if (Model != null)
            {
                Model.DeleteSelectedRecords();

                if (View != null) SetView();
            }
        }

        public void RequestUpdate()
        {
            if (Model != null)
            {

                if (Model.SelectedRecordId != Guid.Empty)
                    Model.EditSelectedRecord();
                else
                    Model.InsertNewRecord();

                if (View != null) SetView();
            }
        }

        public void RequestInsert()
        {
            if (Model != null)
            {
                Model.InsertNewRecord();

                if (View != null) SetView();

                //if (Model.IsDataInserted)
                //    View.CloseView();
            }
        }

        public void RequestSetEditorBirthCountyLocation(string bLocation)
        {
            if (Model != null)
            {
                Model.SetEditorBirthCountyLocation(bLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorDeathCountyLocation(string dLocation)
        {
            if (Model != null)
            {
                Model.SetEditorDeathCountyLocation(dLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorReferenceLocation(string paramReferenceLocation)
        {
            if (Model != null)
            {
                Model.SetEditorReferenceLocation(paramReferenceLocation);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorReferenceDate(string paramReferenceDate)
        {
            if (Model != null)
            {
                Model.SetEditorReferenceDate(paramReferenceDate);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorOccupation(string paramOccupation)
        {
            if (Model != null)
            {
                Model.SetEditorOccupation(paramOccupation);

                if (View != null) SetView();

            }
        }

        public void RequestSetFilterOriginalName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterOriginalName(param);

                if (View != null) SetView();

            }
        }
        //150
        public void RequestSetFilterOriginalFatherName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterOriginalFatherName(param);

                if (View != null) SetView();

            }
        }
        //150
        public void RequestSetFilterOriginalMotherName(string param)
        {
            if (Model != null)
            {
                Model.SetFilterOriginalMotherName(param);

                if (View != null) SetView();

            }
        }

        //public void SetModel(IDeathBirthEditorModel paramModel)
        //{
        //    base.Model = (EditorBaseModel<Guid>)paramModel;
        //    this.Model = paramModel;
        //}

        //public void SetView(IDeathBirthEditorView paramView)
        //{
        //    this.View = paramView;
        //}

        #endregion


        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IDeathBirthEditorModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IDeathBirthEditorView)paramView;


        }



        public void SetView()
        {
            //View.ShowInvalidFemaleName(Model.IsValidFemaleName);
            //View.ShowInvalidMaleName(Model.IsValidMaleName);
            //View.ShowInvalidMarriageDate(Model.IsValidMarriageDate);
          //  View.DisableAddition(Model.IsDataInserted);

            // true when button is disabled
            // false when enabled
          //  View.DisableUpdating(Model.IsDataUpdated);

            View.ShowValidBapDate(Model.IsValidBapDate);
            View.ShowValidBirthDate(Model.IsValidBirthDate);
            View.ShowValidBirthCountyLocation(Model.IsValidBirthCountyLocation);
            View.ShowValidBirthLocationId(Model.IsValidBirthLocationId);
            View.ShowValidDeathCountyLocation(Model.IsValidDeathCountyLocation);
            View.ShowValidDeathDate(Model.IsValidDeathDate);
            View.ShowValidDeathLocation(Model.IsValidDeathLocation);
            View.ShowValidDeathLocationId(Model.IsValidDeathLocationId);
            View.ShowValidFatherChristianName(Model.IsValidFatherChristianName);
            View.ShowValidFatherOccupation(Model.IsValidFatherOccupation);
            View.ShowValidFatherSurname(Model.IsValidFatherSurname);
            View.ShowValidLocation(Model.IsValidLocation);
            View.ShowValidMotherChristianName(Model.IsValidMotherChristianName);
            View.ShowValidMotherSurname(Model.IsValidMotherSurname);
            View.ShowValidName(Model.IsValidName);
            View.ShowValidNotes(Model.IsValidNotes);
            View.ShowValidOccupation(Model.IsValidOccupation);

            View.ShowValidOriginalFatherName(Model.IsValidOriginalFatherName);
            View.ShowValidOriginalMotherName(Model.IsValidOriginalMotherName);
            View.ShowValidOriginalName(Model.IsValidOriginalName);

            View.ShowValidReferenceDate(Model.IsValidReferenceDate);
            View.ShowValidReferenceLocation(Model.IsValidReferenceLocation);
            View.ShowValidReferenceLocationId(Model.IsValidReferenceLocationId);
            View.ShowValidSource(Model.IsValidSource);
            View.ShowValidSpouseCName(Model.IsValidSpouseCName);
            View.ShowValidSpouseSName(Model.IsValidSpouseSName);
            View.ShowValidSurname(Model.IsValidSurname);
            View.ShowValidUniqueRef(Model.IsValidUniqueRef);
            

            
        }








      


        public void RequestSetEditorSpouseCName(string param)
        {
             if (Model != null)
            {
                Model.SetEditorSpouseCName(param);

                if (View != null) SetView();

            }
        }

        public void RequestSetEditorSpouseSName(string param)
        {
              if (Model != null)
            {
                Model.SetEditorSpouseSName(param);

                if (View != null) SetView();

            }
        }

        public void RequestSetEditorFatherOccupation(string param)
        {
             if (Model != null)
            {
                Model.SetEditorFatherOccupation(param);

                if (View != null) SetView();

            }
        }



       
    }
}
