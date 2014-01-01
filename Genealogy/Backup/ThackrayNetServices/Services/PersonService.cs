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


namespace PersonService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PersonService :IPersonService
    {



        public void AssignLocations()
        {
            var deathBirthFilterModel = new DeathBirthFilterModel();
            var deathBirthFilterControl = new DeathBirthFilterControl();

            deathBirthFilterControl.SetModel(deathBirthFilterModel);

            deathBirthFilterControl.RequestUpdateLocationsFromParishList();
        }

        public void UpdateDateEstimates()
        {
            var deathBirthFilterModel = new DeathBirthFilterModel();
            var deathBirthFilterControl = new DeathBirthFilterControl();
            deathBirthFilterControl.SetModel(deathBirthFilterModel);

            deathBirthFilterControl.RequestUpdateDateEstimates();
        }



        public string SetDuplicate(string persons)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();

            try
            {




                DeathBirthFilterControl iControl = new DeathBirthFilterControl();

                List<Guid> selection = new List<Guid>();


                persons.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetRelationTypeId(1);
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

            return WebHelper.MakeReturn(persons, retVal);
        }


        public string MergeSources(string person)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();

            try
            {
                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(person.ToGuid());

                iControl.RequestMergeDuplicates();
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


            return WebHelper.MakeReturn(person, retVal);
        }

        public string RemoveLink(string person)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            List<Guid> selection = new List<Guid>();

            try
            {

                person.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestRemoveRelationType();

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

            return WebHelper.MakeReturn(person, retVal);
        }

        public string SetPersonRelation(string persons, string relationType)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            List<Guid> selection = new List<Guid>();

            try
            {
                persons.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedIds(selection);

                iControl.RequestSetRelationTypeId(relationType.ToInt32());
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

            return WebHelper.MakeReturn(persons, retVal);
        }

        public string DeletePerson(string personId)
        {
            string retVal = "";

            DeathBirthFilterModel iModel = new DeathBirthFilterModel();
            DeathBirthFilterControl iControl = new DeathBirthFilterControl();
            List<Guid> selection = new List<Guid>();

            try
            {

                iControl.SetModel(iModel);

                iControl.RequestSetUser(WebHelper.GetUser());

                personId.Split(',').ToList().ForEach(s => selection.Add(s.ToGuid()));

                iControl.RequestSetSelectedIds(selection);

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



            return WebHelper.MakeReturn(personId, retVal);
        }

        public string AddPerson(string personId, string birthparishId, string deathparishId, string referenceparishId, string sources, string christianName, string surname, string fatherchristianname,
            string fathersurname, string motherchristianname, string mothersurname,
            string source, string ismale, string occupation, string datebirthstr,
            string datebapstr, string birthloc, string birthcounty, string datedeath,
            string deathloc, string deathcounty, string notes, string refdate,
            string refloc, string fatheroccupation, string spousesurname, string spousechristianname, string years, string months, string weeks, string days)
        {
            string retVal = "";

            WebHelper.WriteParams(birthparishId, deathparishId, referenceparishId, sources, christianName, surname, fatherchristianname,
             fathersurname, motherchristianname, mothersurname,
             source, ismale, occupation, datebirthstr,
             datebapstr, birthloc, birthcounty, datedeath,
             deathloc, deathcounty, notes, refdate,
             refloc, fatheroccupation, spousesurname, spousechristianname, years, months, weeks, days);


            if (datebapstr == "" && datebirthstr == "" && datedeath != "" && (years != "" || months != "" || weeks != "" || days != ""))
            {
                DateTime deathDate = new DateTime(2100, 1, 1);

                if (!DateTime.TryParse(datedeath, out deathDate))
                {
                    int deathYear = CsUtils.GetDateYear(datedeath);
                    if (deathYear != 0)
                    {
                        deathDate = new DateTime(deathYear, 1, 1);
                    }
                }

                if (deathDate.Year != 2100)
                {
                    int iyears = 0;
                    int imonths = 0;
                    int iweeks = 0;
                    int idays = 0;

                    Int32.TryParse(years, out iyears);
                    Int32.TryParse(months, out imonths);
                    Int32.TryParse(weeks, out iweeks);
                    Int32.TryParse(days, out idays);

                    idays = (iyears * 365) + (imonths * 28) + (iweeks * 7) + idays;

                    TimeSpan ts = new TimeSpan(idays, 1, 1, 1);


                    DateTime birthDate = deathDate.Subtract(ts);

                    datebirthstr = birthDate.ToString("dd MMM yyyy");

                }
            }



            IDeathBirthEditorModel iModel = null;
            IDeathBirthEditorControl iControl = null;

            iModel = new DeathBirthEditorModel();
            iControl = new DeathBirthEditorControl();

            try
            {


                if (iModel != null)
                    iControl.SetModel(iModel);

                List<Guid> sourceList = new List<Guid>();

                if (sources != null)
                    sources.Split(',').Where(s => s != null).ToList().ForEach(s => sourceList.Add(s.ToGuid()));

                iControl.RequestSetUser(WebHelper.GetUser());

                iControl.RequestSetSelectedId(personId.ToGuid());

                iControl.RequestSetSourceGuidsList(sourceList);

                ServicePerson person = new ServicePerson()
                {
                    PersonId = personId.ToGuid(),
                    ChristianName = christianName,
                    Surname = surname,
                    FatherChristianName = fatherchristianname,
                    FatherSurname = fathersurname,
                    MotherChristianName = motherchristianname,
                    MotherSurname = mothersurname,
                    IsMale = ismale,
                    Occupation = occupation,
                    Birth = datebirthstr,
                    Baptism = datebapstr,
                    Death = datedeath,
                    BirthLocation = birthloc,
                    DeathLocation = deathloc,
                    BirthCounty = birthcounty,
                    DeathCounty = deathcounty,
                    Notes = notes,
                    ReferenceDate = refdate,
                    FatherOccupation = fatheroccupation,
                    SpouseChristianName = spousechristianname,
                    SpouseSurname = spousesurname,
                    ReferenceLocation = refloc,
                    BirthLocationId = birthparishId,
                    ReferenceLocationId = referenceparishId,
                    DeathLocationId = deathparishId,
                    SourceDescription = source,
                    BirthYear = CsUtils.GetDateYear(datebirthstr),
                    BaptismYear = CsUtils.GetDateYear(datebapstr),
                    DeathYear = CsUtils.GetDateYear(datedeath)
                };

                iControl.RequestSetEditorPerson(person);


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





        public ServicePerson GetPerson(string id)
        {
            string retVal = "";

            IDeathBirthEditorModel iModel = new DeathBirthEditorModel();
            IDeathBirthEditorControl iControl = new DeathBirthEditorControl();

            ServicePerson sp = new ServicePerson();

            try
            {

                iControl.SetModel(iModel);
                iControl.RequestSetUser(WebHelper.GetUser());
                iControl.RequestSetSelectedId(id.ToGuid());

                iControl.RequestRefresh();

                sp = iModel.ServicePerson;

                //sp.Baptism = iModel.EditorDateBapString;
                //sp.Birth = iModel.EditorDateBirthString;
                //sp.BirthCounty = iModel.EditorBirthCountyLocation;
                //sp.BirthLocation = iModel.EditorBirthLocation;
                //sp.BirthLocationId = iModel.EditorBirthLocationId.ToString();
                //sp.BirthYear = iModel.EstBirthInt;
                //sp.ChristianName = iModel.EditorChristianName;
                //sp.Death = iModel.EditorDateDeathString;
                //sp.DeathCounty = iModel.EditorDeathCountyLocation;
                //sp.DeathLocation = iModel.EditorDeathLocation;
                //sp.DeathLocationId = iModel.EditorDeathLocationId.ToString();
                //sp.DeathYear = iModel.EstDeathInt;
                //sp.FatherChristianName = iModel.EditorFatherChristianName;
                //sp.FatherSurname = iModel.EditorFatherSurname;
                //sp.FatherOccupation = iModel.EditorFatherOccupation;
                //sp.IsMale = iModel.EditorIsMale.ToString();
                //sp.MotherChristianName = iModel.EditorMotherChristianName;
                //sp.MotherSurname = iModel.EditorMotherSurname;
                //sp.Notes = iModel.EditorNotes;
                //sp.Occupation = iModel.EditorOccupation;
                //sp.PersonId = iModel.SelectedRecordId;
                //sp.ReferenceLocation = iModel.EditorReferenceLocation;
                //sp.ReferenceLocationId = iModel.EditorReferenceLocationId.ToString();
                //sp.ReferenceDate = iModel.EditorReferenceDateString;
                //sp.SourceDescription = iModel.EditorSource;
                //sp.Sources = iModel.SourceGuidListAsString;
                //sp.SpouseChristianName = iModel.EditorSpouseCName;
                //sp.SpouseSurname = iModel.EditorSpouseSName;
                //sp.Surname = iModel.EditorSurnameName;
                //sp.XREF = "";

            }
            catch (Exception ex1)
            {
                retVal = "Exception: " + ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;

                sp.ErrorStatus = retVal;
            }



            return sp;
        }

        public ServicePersonObject GetPersons(string _parentId,
            string christianName,
            string surname,
            string fatherChristianName,
            string fatherSurname,
            string motherChristianName,
            string motherSurname,
            string location,
            string county,
            string lowerDate,
            string upperDate,
            string filterTreeResults,
            string filterIncludeBirths,
            string filterIncludeDeaths,
            string filterSource,
            string spouse,
            string parishFilter,

            string page_number,
            string page_size,
            string sort_col)
        {
            string retVal = "";

            Guid parentId = _parentId.ToGuid();

            ServicePersonObject spo = new ServicePersonObject();


            var iModel = new DeathBirthFilterModel();
            var iControl = new DeathBirthFilterControl();

            try
            {
                iControl.SetModel(iModel);

                if (parentId == Guid.Empty)
                {
                    iControl.RequestSetFilterMode(DeathBirthFilterTypes.Simple);
                    iControl.RequestSetSelectedIds(new List<Guid>());
                    iControl.RequestSetUser(WebHelper.GetUser());

                    iControl.RequestSetSearchFilter(new PersonSearchFilter()
                    {
                        CName = christianName,
                        Surname = surname,
                        FatherChristianName = fatherChristianName,
                        FatherSurname = fatherSurname,
                        MotherChristianName = motherChristianName,
                        MotherSurname = motherSurname,
                        Location = location,
                        LowerDate = lowerDate.ToInt32(),
                        UpperDate = upperDate.ToInt32(),
                        IsIncludeBirths = filterIncludeBirths.ToBool(),
                        IsIncludeDeaths = filterIncludeDeaths.ToBool(),
                        SpouseChristianName = spouse,
                        SourceString = filterSource,
                        ParishString = parishFilter
                    });

                    iControl.RequestSetRecordStart(page_number.ToInt32());

                    iControl.RequestSetRecordPageSize(page_size.ToInt32());
                }
                else
                {
                    iControl.RequestSetSelectedIds(new List<Guid>());
                    iControl.RequestSetParentRecordIds(parentId);
                    iControl.RequestSetFilterMode(DeathBirthFilterTypes.Duplicates);
                }


                iControl.RequestRefresh();


                spo = iModel.PersonsDataTable;

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
                retVal += iModel.StatusMessage;

                spo.ErrorStatus = retVal;
            }

            return spo;
        }


    }
}