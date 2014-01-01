using System;
 
using System.ServiceModel;
using System.ServiceModel.Activation;
using ANDServices;
 
using TDBCore.Types.DTOs;
using TDBCore.Types.domain;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace MarriageService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class MarriageService : IMarriageService
    {


        public ServiceMarriage GetMarriage(string id)
        {

            // ahouls use search function here
            var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
            string retVal = "";
            
            var serviceMarriage = new ServiceMarriage();

            try
            {

                serviceMarriage = iModel.Get(id.ToGuid());

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
          

           serviceMarriage.ErrorStatus = retVal;

           return serviceMarriage;
        }

        public ServiceMarriageObject GetMarriages(string uniqref, string malecname, string malesname, string femalecname,
            string femalesname, string location, string lowerDate, string upperDate, string sourceFilter, string parishFilter, string marriageWitness,
            string page_number, string page_size, string sort_col)
        {                  
            var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
          

            var serviceMarriageObject = new ServiceMarriageObject();

            string retVal = "";

            try
            {
                       
                Guid parentId = uniqref.ToGuid();
              

                if (parentId == Guid.Empty)
                {
                    var marriageFilter = new MarriageSearchFilter()
                        {
                            MaleCName = malecname,
                            MaleSName = malesname,
                            FemaleCName = femalecname,
                            FemaleSName = femalesname,
                            Location = location,
                            LowerDate = lowerDate.ToInt32(),
                            UpperDate = upperDate.ToInt32(),
                            Witness = marriageWitness,
                            Parish = parishFilter,
                            Source = sourceFilter,
                            ParentId = Guid.Empty
                        };

                    var marriageValidation = new MarriageSearchValidator(marriageFilter);


                    serviceMarriageObject = iModel.Search(MarriageFilterTypes.Standard, marriageFilter,
                                  new DataShaping()
                                      {
                                          Column = sort_col,
                                          RecordPageSize = page_size.ToInt32(),
                                          RecordStart = page_number.ToInt32()
                                      }, marriageValidation);

                  
                }
                else
                {                       
                    serviceMarriageObject = iModel.Search(MarriageFilterTypes.Duplicates, new MarriageSearchFilter() { ParentId = parentId }, 
                                  new DataShaping()
                                  {
                                      Column = sort_col,
                                      RecordPageSize = page_size.ToInt32(),
                                      RecordStart = 0
                                  });

                }
                 
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                serviceMarriageObject.ErrorStatus = retVal;               
            }

            return serviceMarriageObject;
        }

        public string DeleteMarriages(string marriageIds)
        {
            var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
    
            string retVal = "";

            try
            {                          
                iModel.DeleteRecords(marriageIds.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           

            return WebHelper.MakeReturn(marriageIds, retVal);
        }

        public string SetMarriageDuplicate(string marriages)
        {     
            var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));     
            string retVal = "";

            try
            {                       
                iModel.SetSelectedDuplicateMarriage(marriages.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           

            return WebHelper.MakeReturn(marriages, retVal);

        }

        public string RemoveMarriageLink(string marriage)
        {
            var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));    
            string retVal = "";

            try
            {              
                iModel.SetRemoveSelectedFromDuplicateList(marriage.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
          

            return WebHelper.MakeReturn(marriage, retVal);

        }

        public string ReorderMarriages(string marriage)
        {
            var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
           
            string retVal = "";

            try
            {           
                iModel.SetReorderDupes(marriage.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           

            return WebHelper.MakeReturn(marriage, retVal);

        }

        public string MergeMarriage(string marriage)
        {
            var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
            string retVal = "";

            try
            {     
                iModel.SetMergeSources(marriage.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           

            return WebHelper.MakeReturn(marriage, retVal);
        }




        public string AddMarriage(
                    string FemaleLocationId, string LocationId, string MaleLocationId, string SourceDescription, string Sources, string MarriageId, string IsBanns, string IsLicense, string IsWidow, string IsWidower,
                    string FemaleBirthYear, string FemaleCName, string FemaleLocation, string FemaleNotes, string FemaleOccupation, string FemaleSName, string LocationCounty, string MaleBirthYear, string MaleCName,
                    string MaleLocation, string MaleNotes, string MaleOccupation, string MaleSName, string MarriageDate, string MarriageLocation, string MarriageWitnesses)
        {


            WebHelper.WriteParams(FemaleLocationId, LocationId, MaleLocationId, SourceDescription, Sources, MarriageId, IsBanns, IsLicense, IsWidow, IsWidower,
                     FemaleBirthYear, FemaleCName, FemaleLocation, FemaleNotes, FemaleOccupation, FemaleSName, LocationCounty, MaleBirthYear, MaleCName,
                     MaleLocation, MaleNotes, MaleOccupation, MaleSName, MarriageDate, MarriageLocation);

            string retVal = "";

              var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
         



                var serviceMarriage = new ServiceMarriage
                    {
                        MarriageId = MarriageId.ToGuid(),
                        MarriageDate = MarriageDate,
                        MaleCName = MaleCName,
                        MaleSName = MaleSName,
                        FemaleCName = FemaleCName,
                        FemaleSName = FemaleSName,
                        MaleNotes = MaleNotes,
                        FemaleNotes = FemaleNotes,
                        MarriageLocation = MarriageLocation,
                        LocationId = LocationId,
                        LocationCounty = LocationCounty,
                        MaleLocation = MaleLocation,
                        FemaleLocation = FemaleLocation,
                        IsBanns = IsBanns.ToBool(),
                        IsLicense = IsLicense.ToBool(),
                        IsWidow = IsWidow.ToBool(),
                        IsWidower = IsWidower.ToBool(),
                        MaleOccupation = MaleOccupation,
                        FemaleOccupation = FemaleOccupation,
                        MaleBirthYear = MaleBirthYear.ToInt32(),
                        FemaleBirthYear = FemaleBirthYear.ToInt32(),
                        SourceDescription = SourceDescription 
                    };

            try
            {        
                iModel.Save(serviceMarriage, Sources.ParseToGuidList(), MarriageWitness.DeSerializeWitnesses(MarriageWitnesses, MarriageDate, MarriageLocation,
                                                     LocationId.ToGuid()),new MarriageValidator(serviceMarriage));                            
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(serviceMarriage.MarriageId.ToString(), retVal);

        }



    }
}