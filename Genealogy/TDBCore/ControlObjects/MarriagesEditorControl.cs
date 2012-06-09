using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GedItter.Interfaces;
using GedItter.ModelObjects;

namespace GedItter.MarriageRecords
{
    public class MarriagesEditorControl : EditorBaseControl<Guid>, IMarriageEditorControl
    {
        protected new IMarriageEditorModel Model;
        protected new IMarriageEditorView View; 



        public MarriagesEditorControl(IMarriageEditorModel paramModel, 
                                      IMarriageEditorView paramView)
        {

            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = paramModel;
            this.View = paramView;
        }

        public MarriagesEditorControl()
        {

        }

        #region iMarriageEditorControl Members

        public void RequestSetSelectedId(Guid marriageId)
        {
            if (Model != null)
            {
                Model.SetSelectedRecordId(marriageId);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMaleName(string cName, string sName)
        {
            if (Model != null)
            {
                Model.SetEditorMaleCName(cName);
                Model.SetEditorMaleSName(sName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorFemaleName(string cName, string sName)
        {
            if (Model != null)
            {
                Model.SetEditorFemaleCName(cName);
                Model.SetEditorFemaleSName(sName);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMarriageDate(string date)
        {
            if (Model != null)
            {
                Model.SetEditorMarriageDate(date);
                 

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorLocation(string location)
        {
            if (Model != null)
            {
                Model.SetEditorLocation(location);
                 

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMaleLocation(string maleLocation)
        {
            if (Model != null)
            {
                Model.SetEditorMaleLocation(maleLocation);
                if (View != null) SetView();
            }
        }

        public void RequestSetEditorFemaleLocation(string femaleLocation)
        {
            if (Model != null)
            {
                Model.SetEditorFemaleLocation(femaleLocation);
                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMarriageCounty(string marriageCounty)
        {
            if (Model != null)
            {
                Model.SetEditorMarriageCounty(marriageCounty);
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

        public void RequestSetEditorWitness1(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness1(witness);
                if (View != null) SetView();
            }
        }

        public void RequestSetEditorWitness2(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness2(witness);
                if (View != null) SetView();
            }
        }

        public void RequestSetEditorWitness3(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness3(witness);
                if (View != null) SetView();
            }
        }

        public void RequestSetEditorWitness4(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness4(witness);
                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMaleInfo(string minfo)
        {
            if (Model != null)
            {
                Model.SetEditorMaleInfo(minfo);
                if (View != null) SetView();
            }
        }

        public void RequestSetEditorFemaleInfo(string finfo)
        {
            if (Model != null)
            {
                Model.SetEditorFemaleInfo(finfo);
                if (View != null) SetView();
            }
        }


        #endregion


        public override void RequestInsert()
        {

            base.RequestInsert();
            
            //if (Model.IsDataInserted)
            //    View.CloseView();

        }

        public void SetModel(Interfaces.IDBRecordModel<Guid> paramModel)
        {
            base.Model = (EditorBaseModel<Guid>)paramModel;
            this.Model = (IMarriageEditorModel)paramModel;
        }

        public void SetView(Interfaces.IDBRecordView paramView)
        {
            this.View = (IMarriageEditorView)paramView;
           // base.View = paramView;
        }

        public override void SetView()
        {
            if (this.View != null)
            {
         

                
                View.ShowInvalidFemaleBirthYear(Model.IsValidFemaleBirthYear);
                View.ShowInvalidFemaleInfo(Model.IsValidFemaleInfo);
                View.ShowInvalidFemaleLocation(Model.IsValidFemaleLocation);
                View.ShowInvalidFemaleName(Model.IsValidFemaleName);
                View.ShowInvalidFemaleOccupation(Model.IsValidFemaleOccupation);
                View.ShowInvalidFemaleSurname(Model.IsValidFemaleSurname);
                View.ShowInvalidLocation(Model.IsValidLocation);
                View.ShowInvalidMaleBirthYear(Model.IsValidMaleBirthYear);
                View.ShowInvalidMaleInfo(Model.IsValidMaleInfo);
                View.ShowInvalidMaleLocation(Model.IsValidMaleLocation);
                View.ShowInvalidMaleName(Model.IsValidMaleName);
                View.ShowInvalidMaleSurname(Model.IsValidMaleSurname);
                View.ShowInvalidMaleOccupation(Model.IsValidMaleOccupation);
                View.ShowInvalidMarriageDate(Model.IsValidMarriageDate);
                View.ShowInvalidMarriageCounty(Model.IsValidMarriageCounty);
                View.ShowInvalidOriginalFemaleName(Model.IsValidOriginalFemaleName);
                View.ShowInvalidOriginalName(Model.IsValidOriginalName);
                View.ShowInvalidSource(Model.IsValidSource);
                View.ShowInvalidWitnessCName1(Model.IsValidWitnessCName1);
                View.ShowInvalidWitnessCName2(Model.IsValidWitnessCName2);
                View.ShowInvalidWitnessCName3(Model.IsValidWitnessCName3);
                View.ShowInvalidWitnessCName4(Model.IsValidWitnessCName4);
                View.ShowInvalidWitnessSName1(Model.IsValidWitnessSName1);
                View.ShowInvalidWitnessSName2(Model.IsValidWitnessSName2);
                View.ShowInvalidWitnessSName3(Model.IsValidWitnessSName3);
                View.ShowInvalidWitnessSName4(Model.IsValidWitnessSName4);
                //View.DisableAddition(Model.IsDataInserted);
                //View.DisableUpdating(Model.IsDataUpdated);
            }
        }



        #region IMarriageEditorControl Members

        public void RequestSetEditorMaleOccupation(string paramOccupation)
        {
            if (Model != null)
            {
                Model.SetEditorMaleOccupation(paramOccupation);
                if (this.View != null) SetView();
            }
        }

        public void RequestSetEditorFemaleOccupation(string paramOccupation)
        {
            if (Model != null)
            {
                Model.SetEditorFemaleOccupation(paramOccupation);
                if (this.View != null) SetView();
            }
        }

        public void RequestSetEditorIsWidow(bool paramIsWidow)
        {
            if (Model != null)
            {
                Model.SetEditorIsWidow(paramIsWidow);
                if (this.View != null) SetView();
            }
        }

        public void RequestSetEditorIsWidower(bool paramIsWidower)
        {
            if (Model != null)
            {
                Model.SetEditorIsWidower(paramIsWidower);
                if (this.View != null) SetView();
            }
        }

        public void RequestSetEditorIsLicence(bool paramIsLicence)
        {
            if (Model != null)
            {
                Model.SetEditorIsLicence(paramIsLicence);
                if (this.View != null) SetView();
            }
        }

        public void RequestSetEditorIsBanns(bool paramBanns)
        {
            if (Model != null)
            {
                Model.SetEditorIsBanns(paramBanns);
                if (this.View != null) SetView();
            }
        }

        #endregion

       

        public void RequestSetEditorMarriageLocationId(Guid param)
        {
            if (Model != null)
            {
                Model.SetMarriageLocationId(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMaleLocationId(Guid param)
        {
            if (Model != null)
            {
                Model.SetMaleLocationId(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorFemaleLocationId(Guid param)
        {
            if (Model != null)
            {
                Model.SetFemaleLocationId(param);

                if (View != null) SetView();
            }
        }



        


        public void RequestSetEditorFemaleBirthYear(string param)
        {
            if (Model != null)
            {
                Model.SetEditorFemaleBirthYear(param);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorMaleBirthYear(string param)
        {
            if (Model != null)
            {
                Model.SetEditorMaleBirthYear(param);

                if (View != null) SetView();
            }
        }



        


        public void RequestSetEditorWitness1CName(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness1CName(witness);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorWitness2CName(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness2CName(witness);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorWitness3CName(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness3CName(witness);

                if (View != null) SetView();
            }
        }

        public void RequestSetEditorWitness4CName(string witness)
        {
            if (Model != null)
            {
                Model.SetEditorWitness4CName(witness);

                if (View != null) SetView();
            }
        }

       
    }
}
