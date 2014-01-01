using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Script.Serialization;
using ANDServices;
using TDBCore;
using TDBCore.ControlObjects;
using TDBCore.EntityModel;
using TDBCore.Interfaces;
using TDBCore.Types;
using TDBCore.BLL;
using TDBCore.ModelObjects;
using System.Web.Security;
using System.ServiceModel.Web;
using System.Diagnostics;
using System.Reflection;
using Facebook;
using PersonService = PersonService.PersonService;

namespace MarriageService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class MarriageService : IMarriageService
    {


        public ServiceMarriage GetMarriage(string id)
        {
            MarriagesEditorModel iModel = new MarriagesEditorModel();
            MarriagesEditorControl iControl = new MarriagesEditorControl();
            string retVal = "";

            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(id.ToGuid());

                iControl.RequestRefresh();

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;


            }


            ServiceMarriage serviceMarriage = new ServiceMarriage()
            {
                FemaleBirthYear = iModel.EditorFemaleBirthYear.ToInt32(),
                FemaleCName = iModel.EditorFemaleCName,
                FemaleLocation = iModel.EditorFemaleLocation,
                FemaleLocationId = iModel.EditorFemaleLocationId.ToString(),
                FemaleNotes = iModel.EditorFemaleInfo,
                FemaleOccupation = iModel.EditorFemaleOccupation,
                FemaleSName = iModel.EditorFemaleSName,
                IsBanns = iModel.EditorIsBanns,
                IsLicense = iModel.EditorIsLicence,
                IsWidow = iModel.EditorIsWidow,
                IsWidower = iModel.EditorIsWidower,
                LocationCounty = iModel.EditorMarriageCounty,
                LocationId = iModel.EditorMarriageLocationId.ToString(),
                MaleBirthYear = iModel.EditorMaleBirthYear.ToInt32(),
                MaleCName = iModel.EditorMaleCName,
                MaleLocation = iModel.EditorMaleLocation,
                MaleLocationId = iModel.EditorMaleLocationId.ToString(),
                MaleNotes = iModel.EditorMaleInfo,
                MaleOccupation = iModel.EditorMaleOccupation,
                MaleSName = iModel.EditorMaleSName,
                MarriageDate = iModel.EditorDateMarriageString,
                MarriageId = iModel.SelectedRecordId,
                MarriageLocation = iModel.EditorLocation,
                Sources = iModel.SourceGuidListAsString,
                SourceDescription = iModel.EditorSource,
                Witness1ChristianName = iModel.EditorWitnesses.GetXName(0),
                Witness1Surname = iModel.EditorWitnesses.GetXSurname(0),
                Witness1Description = iModel.EditorWitnesses.GetXDescription(0),
                Witness2ChristianName = iModel.EditorWitnesses.GetXName(1),
                Witness2Surname = iModel.EditorWitnesses.GetXSurname(1),
                Witness2Description = iModel.EditorWitnesses.GetXDescription(1),
                Witness3ChristianName = iModel.EditorWitnesses.GetXName(2),
                Witness3Surname = iModel.EditorWitnesses.GetXSurname(2),
                Witness3Description = iModel.EditorWitnesses.GetXDescription(2),
                Witness4ChristianName = iModel.EditorWitnesses.GetXName(3),
                Witness4Surname = iModel.EditorWitnesses.GetXSurname(3),
                Witness4Description = iModel.EditorWitnesses.GetXDescription(3),
                Witness5ChristianName = iModel.EditorWitnesses.GetXName(4),
                Witness5Surname = iModel.EditorWitnesses.GetXSurname(4),
                Witness5Description = iModel.EditorWitnesses.GetXDescription(4),
                Witness6ChristianName = iModel.EditorWitnesses.GetXName(5),
                Witness6Surname = iModel.EditorWitnesses.GetXSurname(5),
                Witness6Description = iModel.EditorWitnesses.GetXDescription(5),
                Witness7ChristianName = iModel.EditorWitnesses.GetXName(6),
                Witness7Surname = iModel.EditorWitnesses.GetXSurname(6),
                Witness7Description = iModel.EditorWitnesses.GetXDescription(6),
                Witness8ChristianName = iModel.EditorWitnesses.GetXName(7),
                Witness8Surname = iModel.EditorWitnesses.GetXSurname(7),
                Witness8Description = iModel.EditorWitnesses.GetXDescription(7)




            };

            serviceMarriage.ErrorStatus = retVal;
            return serviceMarriage;
        }

        public ServiceMarriageObject GetMarriages(string uniqref, string malecname, string malesname, string femalecname,
            string femalesname, string location, string lowerDate, string upperDate, string sourceFilter, string parishFilter, string marriageWitness,
            string page_number, string page_size, string sort_col)
        {


            ServiceMarriageObject serviceMarriageObject = new ServiceMarriageObject();
            ParishsBll parishsBll = new ParishsBll();


            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();
            string retVal = "";

            try
            {


                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                Guid parishId = parishFilter.ToGuid();

                if (parishId != Guid.Empty)
                {

                    sourceFilter = "";

                    parishsBll.GetParishSourceRecords(parishId).ForEach(p => sourceFilter += "," + p.SourceId.ToString());

                    sourceFilter = sourceFilter.Substring(1);
                }


                Guid parentId = uniqref.ToGuid();

                if (parentId == Guid.Empty)
                {
                    iControl.RequestSetFilterMode(MarriageFilterTypes.STANDARD);
                    iControl.RequestSetSelectedIds(new List<Guid>());
                    iControl.RequestSetFilterMaleName((malecname ?? ""), (malesname ?? ""));
                    iControl.RequestSetFilterFemaleName((femalecname ?? ""), (femalesname ?? ""));
                    iControl.RequestSetFilterLocation(location ?? "");
                    iControl.RequestSetFilterMarriageBoundLower(lowerDate ?? "");
                    iControl.RequestSetFilterMarriageBoundUpper(upperDate ?? "");
                    iControl.RequestSetFilterWitness(marriageWitness ?? "");
                }
                else
                {
                    iControl.RequestSetSelectedIds(new List<Guid>());
                    iControl.RequestSetParentRecordIds(parentId);
                    //hack
                    iControl.RequestSetFilterSource("dupes");
                    iControl.RequestSetIsDataChanged(true);
                    iControl.RequestSetFilterMode(MarriageFilterTypes.DUPLICATES);

                }

                if (sourceFilter != "")
                {
                    iControl.RequestSetFilterSource(sourceFilter);

                    List<Guid> sources = new List<Guid>();

                    foreach (string _guid in sourceFilter.Split(','))
                    {
                        sources.Add(_guid.ToGuid());
                    }

                    iControl.RequestSetSourceGuidsList(sources);

                }

                iControl.RequestRefresh();

                if (sort_col.Contains("MarriageDate DESC"))
                {
                    sort_col = "MarriageYear DESC";
                }
                else if (sort_col.Contains("MarriageDate"))
                {
                    sort_col = "MarriageYear";
                }
            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }



            serviceMarriageObject.serviceMarriages = iModel.MarriagesTable.OrderBy(sort_col).Select(p => new ServiceMarriageLookup()
            {
                Events = p.MarriageTotalEvents.ToString(),
                FemaleCName = p.FemaleCName,
                FemaleSName = p.FemaleSName,
                MaleCName = p.MaleCName,
                MaleSName = p.MaleSName,
                MarriageDate = p.MarriageYear.ToString(),
                MarriageId = p.MarriageId,
                MarriageLocation = p.MarriageLocation,
                Sources = p.MarriageSource,
                Witnesses = p.Witnesses,
                XREF = p.UniqueRef.ToString(),
                LinkedTrees = p.SourceTrees
            }).ToList();


            serviceMarriageObject.Batch = page_number.ToInt32();
            serviceMarriageObject.BatchLength = page_size.ToInt32();
            serviceMarriageObject.Total = serviceMarriageObject.serviceMarriages.Count;

            serviceMarriageObject.serviceMarriages = serviceMarriageObject.serviceMarriages.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();
            serviceMarriageObject.ErrorStatus = retVal;

            return serviceMarriageObject;
        }

        public string DeleteMarriages(string marriageIds)
        {
            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();
            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                List<Guid> selection = new List<Guid>();

                marriageIds.Split(',').ToList().ForEach(p => selection.Add(p.ToGuid()));


                iControl.RequestSetSelectedIds(selection);
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestDelete();
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }

            return WebHelper.MakeReturn(marriageIds, retVal);
        }

        public string SetMarriageDuplicate(string marriages)
        {
            //RequestSetSelectedDuplicateMarriage
            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();

            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                List<Guid> selection = new List<Guid>();


                string[] persons = marriages.Split(',');

                foreach (string _person in persons)
                {
                    selection.Add(_person.ToGuid());
                }

                iControl.RequestSetSelectedIds(selection);
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetSelectedDuplicateMarriage();

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }

            return WebHelper.MakeReturn(marriages, retVal);

        }

        public string RemoveMarriageLink(string marriage)
        {

            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();

            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                List<Guid> selection = new List<Guid>();


                string[] persons = marriage.Split(',');

                foreach (string _person in persons)
                {
                    selection.Add(_person.ToGuid());
                }

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetRemoveSelectedFromDuplicateList();

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }

            return WebHelper.MakeReturn(marriage, retVal);

        }

        public string ReorderMarriages(string marriage)
        {

            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();

            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                List<Guid> selection = new List<Guid>();


                string[] marriages = marriage.Split(',');

                foreach (string _marriage in marriages)
                {
                    selection.Add(_marriage.ToGuid());
                }

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetReorderDupes();

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }

            return WebHelper.MakeReturn(marriage, retVal);

        }

        public string MergeMarriage(string marriage)
        {
            MarriagesFilterModel iModel = new MarriagesFilterModel();
            MarriagesFilterControl iControl = new MarriagesFilterControl();

            string retVal = "";

            try
            {
                iControl.SetModel(iModel);

                iControl.RequestSetSelectedId(marriage.ToGuid());
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetMergeSources();

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
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
            IMarriageEditorModel iModel = new MarriagesEditorModel();
            IMarriageEditorControl iControl = new MarriagesEditorControl();


            try
            {
                if (iModel != null)
                    iControl.SetModel(iModel);

                List<Guid> sourceList = new List<Guid>();

                if (Sources != null)
                    Sources.Split(',').ToList().ForEach(s => sourceList.Add(s.ToGuid()));

                var witnesses = new List<marriageWitness>();

                //  string example = @"[{""name"":""john"",""surname"":""smith"",""description"":""witness""},{""name"":""chris"",""surname"":""jones"",""description"":""witness""},{""name"":""allen"",""surname"":""bond"",""description"":""vicar""}]";

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                // Deserialize



                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(MarriageId.ToGuid());
                iControl.RequestSetSourceGuidsList(sourceList);

                iControl.RequestSetEditorMarriageDate(MarriageDate);
                iControl.RequestSetEditorSource(SourceDescription);
                iControl.RequestSetEditorMaleName(MaleCName, MaleSName);

                iControl.RequestSetEditorFemaleName(FemaleCName, FemaleSName);

                iControl.RequestSetEditorMaleInfo(MaleNotes);
                iControl.RequestSetEditorFemaleInfo(FemaleNotes);
                iControl.RequestSetEditorLocation(MarriageLocation);
                iControl.RequestSetEditorMarriageLocationId(LocationId.ToGuid());
                iControl.RequestSetEditorMarriageCounty(LocationCounty);
                iControl.RequestSetEditorMaleLocation(MaleLocation);
                iControl.RequestSetEditorFemaleLocation(FemaleLocation);
                //iControl.RequestSetEditorWitnesses();



                iControl.RequestSetEditorIsWidow(IsWidow.ToBool());
                iControl.RequestSetEditorIsWidower(IsWidower.ToBool());
                iControl.RequestSetEditorIsBanns(IsBanns.ToBool());
                iControl.RequestSetEditorIsLicence(IsLicense.ToBool());



                iControl.RequestSetEditorMaleOccupation(MaleOccupation);
                iControl.RequestSetEditorFemaleOccupation(FemaleOccupation);
                iControl.RequestSetEditorMaleBirthYear(MaleBirthYear);
                iControl.RequestSetEditorFemaleBirthYear(FemaleBirthYear);



                var marriages = serializer.DeserializeToMarriageWitnesses(MarriageWitnesses, iModel.EditorMarriageYear, iModel.EditorDateMarriageString,
                                                          iModel.EditorLocation, iModel.EditorMarriageLocationId);



                iControl.RequestSetEditorWitnesses(marriageWitness.AddWitnesses(marriages));

                iControl.RequestUpdate();

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;
            }


            return WebHelper.MakeReturn(iModel.SelectedRecordId.ToString(), retVal);

        }



    }
}