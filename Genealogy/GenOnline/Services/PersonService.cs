﻿using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GenOnline.Helpers;
using GenOnline.Services.Contracts;
using TDBCore.Types;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenOnline.Services
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PersonService :IPersonService
    {

        public string TestPerson()
        {
            string error = "xxx";

            //try
            //{
            //    var sp = new ServicePerson();
            //    var iModel = new PersonSearch(new Security(new WebUser()));

            //    Guid g = Guid.Parse("de9937f4-5850-463f-af99-25c381fe5f3");
                
            //    sp = iModel.Get(g);

            //    error = sp.ChristianName + " " + sp.Surname;
            //}
            //catch (Exception ex1)
            //{
            //    error = "Exception: " + ex1.Message;
            //}

            return error;
        }

        public void AssignLocations()
        {
            var iModel = new PersonSearch(new Security(new WebUser()));

            iModel.UpdateLocationsFromParishList();
        }

        

        public void UpdateDateEstimates()
        {
            var iModel = new PersonSearch(new Security(new WebUser()));

            iModel.UpdateDateEstimates();
        }



        public string SetDuplicate(string persons)
        {
            string retVal = "";

            var iModel = new PersonSearch(new Security(new WebUser()));

            try
            {
                retVal = iModel.SetDuplicateRelation(persons.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
          
            return WebHelper.MakeReturn(persons, retVal);
        }


        public string MergeSources(string person)
        {
            string retVal = "";

            var iModel = new PersonSearch(new Security(new WebUser()));
           
            try
            {
                retVal = iModel.MergeDuplicates(person.ToGuid());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
             
            return WebHelper.MakeReturn(person, retVal);
        }

        public string RemoveLink(string person)
        {
            string retVal = "";

            var iModel = new PersonSearch(new Security(new WebUser()));
         
            try
            {
                retVal = iModel.DelinkPersons(person.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            
            return WebHelper.MakeReturn(person, retVal);
        }

        public string SetPersonRelation(string persons, string relationType)
        {
            string retVal = "";
            var iModel = new PersonSearch(new Security(new WebUser()));
    
            try
            {                               
                iModel.SetDuplicateRelation(persons.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
           
            return WebHelper.MakeReturn(persons, retVal);
        }

 

        public string DeletePerson(string personId)
        {
            string retVal = "";

            var iModel = new PersonSearch(new Security(new WebUser()));
           
            try
            {                                  
                iModel.DeleteRecords(personId.ParseToGuidList());
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
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

          
          var iModel = new PersonSearch(new Security(new WebUser()));


            datebirthstr = DateTools.MakeDateString(datebapstr, datebirthstr, datedeath, years, months, weeks, days);

            var sp = new ServicePerson
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
                    BirthYear = datebirthstr.ParseToValidYear(),//DateTools.GetDateYear(datebirthstr),
                    BaptismYear =datebapstr.ParseToValidYear(),// DateTools.GetDateYear(),
                    DeathYear =datedeath.ParseToValidYear()// DateTools.GetDateYear(datedeath)
                };
                                            
                                            
            try
            {                         
                iModel.Save(sp,sources.ParseToGuidList(),new PersonValidator(sp));
            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }

            return WebHelper.MakeReturn(sp.PersonId.ToString(), retVal);
        }





        public ServicePerson GetPerson(string id)
        {
            var sp = new ServicePerson();
            var iModel = new PersonSearch(new Security(new WebUser()));
                     
            try
            {
                 sp = iModel.Get(id.ToGuid());
            }
            catch (Exception ex1) 
            {
                sp.ErrorStatus = "Exception: " + ex1.Message; 
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
            string filterIncludeSources,
            string filterSource,
            string spouse,
            string parishFilter,

            string page_number,
            string page_size,
            string sort_col)
        {
            string retVal = "";

            Guid parentId = _parentId.ToGuid();

            var spo = new ServicePersonObject();


            var iModel = new PersonSearch(new Security(new WebUser()));
           
            try
            {
            
                if (parentId == Guid.Empty)
                {
             
                    var searchParams = new PersonSearchFilter
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
                            IsIncludeSources = filterIncludeSources.ToBool(),
                            SpouseChristianName = spouse,
                            SourceString = filterSource,
                            ParishString = parishFilter
                        };


                    spo = iModel.Search(PersonSearchTypes.Simple, searchParams,
                                  new DataShaping
                                      {
                                          RecordStart = page_number.ToInt32(),
                                          RecordPageSize = page_size.ToInt32()
                                      }, new PersonSearchValidator(searchParams));
                }
                else
                {
                    
                    spo = iModel.Search(PersonSearchTypes.Duplicates, new PersonSearchFilter {ParentId = parentId},
                                        new DataShaping
                                        {
                                            RecordStart = 0,
                                            RecordPageSize=25
                                        });

                }
                  

            }
            catch (Exception ex1)
            {
                retVal = ex1.Message;
            }
            finally
            {
                if (retVal != "") retVal += Environment.NewLine;
            
                spo.ErrorStatus = retVal;
            }

            return spo;
        }






      
    }
}