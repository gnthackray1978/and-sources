using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GenOnline.Helpers;
using GenOnline.Services.Contracts;
using TDBCore.BLL;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenOnline.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class MarriageService : IMarriageService
    {
        private readonly MarriageSearch _marriageSearch;

        public MarriageService(IMarriagesDal iMarriagesDal,
            IMarriageWitnessesDal iMarriageWitnessesDal,
            ISourceDal iSourceDal, 
            ISourceMappingsDal iSourceMappingsDal,
            IPersonDal iPersonDal,
            ISecurity iSecurity)
        {
            _marriageSearch = new MarriageSearch(iSecurity,
                iMarriagesDal,
                iMarriageWitnessesDal,iSourceDal, iSourceMappingsDal,iPersonDal);            
        }

        public ServiceMarriage GetMarriage(string id)
        {

            // ahouls use search function here     
            string retVal = "";
            
            var serviceMarriage = new ServiceMarriage();

            try
            {

                serviceMarriage = _marriageSearch.Get(id.ToGuid());

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
            //var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
          

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


                    serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.Standard, marriageFilter,
                                  new DataShaping()
                                      {
                                          Column = sort_col,
                                          RecordPageSize = page_size.ToInt32(),
                                          RecordStart = page_number.ToInt32()
                                      }, marriageValidation);

                  
                }
                else
                {
                    serviceMarriageObject = _marriageSearch.Search(MarriageFilterTypes.Duplicates, new MarriageSearchFilter() { ParentId = parentId }, 
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
            string retVal = "";

            try
            {
                _marriageSearch.DeleteRecords(marriageIds.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           

            return WebHelper.MakeReturn(marriageIds, retVal);
        }

        public string SetMarriageDuplicate(string marriages)
        {                
            string retVal = "";

            try
            {
                _marriageSearch.SetSelectedDuplicateMarriage(marriages.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           

            return WebHelper.MakeReturn(marriages, retVal);

        }

        public string RemoveMarriageLink(string marriage)
        {
           // var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));    
            string retVal = "";

            try
            {
                _marriageSearch.SetRemoveSelectedFromDuplicateList(marriage.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
          

            return WebHelper.MakeReturn(marriage, retVal);

        }

        public string ReorderMarriages(string marriage)
        {
          //  var iModel = new MarriageSearch(new Security(WebHelper.GetUser()));
           
            string retVal = "";

            try
            {
                _marriageSearch.SetReorderDupes(marriage.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           

            return WebHelper.MakeReturn(marriage, retVal);

        }

        public string SwitchSpouses(string marriage)
        {
    
            string retVal = "";

            try
            {
                _marriageSearch.SwitchSpouses(marriage.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }


            return WebHelper.MakeReturn(marriage, retVal);
        }

       

        public string MergeMarriage(string marriage)
        {
      
            string retVal = "";

            try
            {
                _marriageSearch.SetMergeSources(marriage.ToGuid());
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
                _marriageSearch.Save(serviceMarriage, Sources.ParseToGuidList(), MarriageWitness.DeSerializeWitnesses(MarriageWitnesses, MarriageDate, MarriageLocation,
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