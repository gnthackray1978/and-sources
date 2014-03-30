using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types;
using System.Text.RegularExpressions;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;

namespace TDBCore.BLL
{
    public class SourceBll : BaseBll
    {

        public List<CensusSource> Get1841CensuSources(Guid sourceId)
        {   
            var ret = new List<CensusSource>();
                                
            foreach (var entry in ModelContainer.uvw_1841Census.Where(c => c.ParishId == sourceId))
            {
                var censusPersons = new List<CensusPerson>();


                var personResults = ModelContainer.Persons.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == entry.SourceId)).OrderBy(p => p.BirthDateStr).ToList();

                
                personResults.ForEach(o => censusPersons.Add(new CensusPerson()
                    {
                        BirthCounty = o.BirthCounty,
                        BirthYear = o.BirthDateStr.ToInt32(),
                        CName = o.ChristianName,
                        SName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(o.Surname.ToLower())  
                    }));


                var cs = new CensusSource()
                {
                    CensusDesc = entry.SourceDescription,
                    CensusRef = entry.SourceRef,
                    CensusYear = 1841,
                    SourceId = entry.SourceId,
                    attachedPersons = censusPersons
                };


                //parse address
                string namesPattern = @"(?<=Address).*(?=Civil Parish)";
                var regex = new Regex(namesPattern, RegexOptions.Singleline);

                Match result = regex.Match(entry.SourceDescription);

                if (result.Success)
                    cs.Address = result.Value.Trim();

                //parse civil address
                namesPattern = @"(?<=Civil Parish).*(?=County)";
                regex = new Regex(namesPattern, RegexOptions.Singleline);

                result = regex.Match(entry.SourceDescription);

                if (result.Success)
                    cs.Civil_Parish = result.Value;

                //parse county
                namesPattern = @"(?<=County).*(?=Municipal Borough)";
                regex = new Regex(namesPattern, RegexOptions.Singleline);
                result = regex.Match(entry.SourceDescription);

                if (result.Success)
                    cs.County = result.Value.Trim();


                  //parse municipal borough
                namesPattern = @"(?<=Municipal Borough).*(?=Registration District)";
                regex = new Regex(namesPattern, RegexOptions.Singleline);
                result = regex.Match(entry.SourceDescription);

                if (result.Success)
                    cs.Municipal_Borough = result.Value.Trim();

                //parse Registration_District
                namesPattern = @"(?<=Registration District).*(?=Page)";
                regex = new Regex(namesPattern, RegexOptions.Singleline);
                result = regex.Match(entry.SourceDescription);

                if (result.Success)
                    cs.Registration_District = result.Value.Trim();


                //parse page
                namesPattern = @"(?<=Page).*(?=Piece)";
                regex = new Regex(namesPattern, RegexOptions.Singleline);
                result = regex.Match(entry.SourceDescription);

                if (result.Success)
                    cs.Page = result.Value.Trim();



                //parse piece
                namesPattern = @"(?<=Piece).*";
                regex = new Regex(namesPattern, RegexOptions.Singleline);
                result = regex.Match(entry.SourceDescription);

                if (result.Success)
                    cs.Piece = result.Value.Trim();



                ret.Add(cs);

                
                
            }

            return ret;
        }


        public string MakeSourceString(Guid person)
        {
            var sourceBll = new SourceBll();

            return Enumerable.Aggregate(sourceBll.FillSourceTableByPersonOrMarriageId2(person), "", (current, source) => current + (Environment.NewLine + source.SourceRef));

        }

        public List<Person> GetPersonsForSource(Guid sourceId)
        {
            return ModelContainer.SourceMappings.Where(sm => sm.Source.SourceId == sourceId && sm.Person != null).Select(p => p.Person).ToList();
        }

        public Source CreateBasicSource(Guid sourceId, string sourceRef, int sourceDateInt, int sourceDateToInt, string sourceDesc)
        {
            Source newSource = new Source();
            newSource.SourceId = sourceId;
            newSource.IsCopyHeld = false;
            newSource.IsThackrayFound = false;
            newSource.IsViewed = false;
            newSource.OriginalLocation = "";
            newSource.SourceDate = sourceDateInt;
            newSource.SourceDateTo = sourceDateToInt;

            newSource.SourceDateStr = sourceDateInt.ToString();
            newSource.SourceDateStrTo = sourceDateToInt.ToString();
            newSource.SourceDescription = sourceDesc;
            newSource.SourceRef = sourceRef;
            newSource.DateAdded = DateTime.Today;

            ModelContainer.Sources.Add(newSource);

            ModelContainer.SaveChanges();

            return newSource;
        }

        public Source CreateBasicSource(string sourceRef, int sourceDateInt, string sourceDesc)
        {
            Source newSource = new Source();
            newSource.SourceId = Guid.NewGuid();
            newSource.IsCopyHeld = false;
            newSource.IsThackrayFound = false;
            newSource.IsViewed = false;
            newSource.OriginalLocation = "";
            newSource.SourceDate = sourceDateInt;
            newSource.SourceDateTo = sourceDateInt;
            newSource.SourceDateStr = sourceDateInt.ToString();
            newSource.SourceDateStrTo = sourceDateInt.ToString();
            newSource.SourceDescription = sourceDesc;
            newSource.SourceRef = sourceRef;
            newSource.DateAdded = DateTime.Today;

            ModelContainer.Sources.Add(newSource);

            ModelContainer.SaveChanges();

            return newSource;
        }



        public Guid InsertSource(SourceDto sourceAjaxDto)
        {       
            var source = new Source();
            source.SourceDescription = sourceAjaxDto.SourceDesc;
            source.OriginalLocation = sourceAjaxDto.OriginalLocation;
            source.IsCopyHeld = sourceAjaxDto.IsCopyHeld;
            source.IsViewed = sourceAjaxDto.IsViewed;
            source.IsThackrayFound = sourceAjaxDto.IsThackrayFound;
            source.UserId = 0;
            source.SourceDate = CsUtils.GetDateYear(sourceAjaxDto.SourceDateStr);
            source.SourceDateTo = CsUtils.GetDateYear(sourceAjaxDto.SourceDateStrTo);
            source.SourceDateStr = sourceAjaxDto.SourceDateStr;
            source.SourceDateStrTo = sourceAjaxDto.SourceDateStrTo;
            source.SourceRef = sourceAjaxDto.SourceRef;
            source.SourceFileCount = sourceAjaxDto.SourceFileCount;
            source.SourceNotes = sourceAjaxDto.SourceNotes;
            source.SourceId = System.Guid.NewGuid();
            source.DateAdded = DateTime.Today;

            ModelContainer.Sources.Add(source);

            ModelContainer.SaveChanges();

            return source.SourceId;
        }



        public Guid InsertSource2(string sourceDesc, string sourceOrigLoc, bool isCopyHeld, bool isViewed, bool isThackrayFound, int userId,
                                    string sourceDateStr, string sourceDateStrTo, int sourceDateInt, int sourceDateIntTo , string sourceRef, int sourceFileCount, string sourceNotes)
        {

          //  ModelContainer.AcceptAllChanges();

            Source _source = new Source();
            _source.SourceDescription = sourceDesc;
            _source.OriginalLocation = sourceOrigLoc;
            _source.IsCopyHeld = isCopyHeld;
            _source.IsViewed = isViewed;
            _source.IsThackrayFound = isThackrayFound;
            _source.UserId = userId;
            _source.SourceDate = sourceDateInt;
            _source.SourceDateTo = sourceDateIntTo;
            _source.SourceDateStr = sourceDateStr;
            _source.SourceDateStrTo = sourceDateStrTo;
            _source.SourceRef = sourceRef;
            _source.SourceFileCount = sourceFileCount;
            _source.SourceNotes = sourceNotes;
            _source.SourceId = System.Guid.NewGuid();
            _source.DateAdded = DateTime.Today;


            ModelContainer.Sources.Add(_source);

            //Debug.WriteLine("modified");
            //foreach (var _entry in ModelContainer.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Modified))
            //{
            //    Debug.WriteLine(_entry.ToString());
            //}
            //Debug.WriteLine("added");
            //foreach (var _entry in ModelContainer.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added))
            //{
            //    Debug.WriteLine(_entry.ToString());
            //}
            //Debug.WriteLine("unchanged");
            //foreach (var _entry in ModelContainer.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Unchanged))
            //{
            //    Debug.WriteLine(_entry.ToString());
            //}


            
            ModelContainer.SaveChanges();
     
             
            return _source.SourceId;
        }

        public Source GetNewSource(string sourceDesc, string sourceOrigLoc, bool isCopyHeld, bool isViewed, bool isThackrayFound, int userId,
                            string sourceDateStr, string sourceDateStrTo, int sourceDateInt, int sourceDateIntTo, string sourceRef, int sourceFileCount, string sourceNotes)
        {

            //  ModelContainer.AcceptAllChanges();

            Source _source = new Source();
            _source.SourceDescription = sourceDesc;
            _source.OriginalLocation = sourceOrigLoc;
            _source.IsCopyHeld = isCopyHeld;
            _source.IsViewed = isViewed;
            _source.IsThackrayFound = isThackrayFound;
            _source.UserId = userId;
            _source.SourceDate = sourceDateInt;
            _source.SourceDateTo = sourceDateIntTo;
            _source.SourceDateStr = sourceDateStr;
            _source.SourceDateStrTo = sourceDateStrTo;
            _source.SourceRef = sourceRef;
            _source.SourceFileCount = sourceFileCount;
            _source.SourceNotes = sourceNotes;
            _source.SourceId = System.Guid.NewGuid();
            _source.DateAdded = DateTime.Today;


            ModelContainer.Sources.Add(_source);

 

            ModelContainer.SaveChanges();


            return _source;
        }
 
        public void UpdateSource2(Guid sourceId, string sourceDesc, string sourceOrigLoc,
         bool isCopyHeld, bool isViewed, bool isThackrayFound, int userId,
         string sourceDateStr, string sourceDateStrTo, int sourceDateInt, int sourceDateIntTo,
         string sourceRef, int sourceFileCount, string sourceNotes)
        {
            Source _source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);


            if (_source != null)
            {
                _source.SourceDescription = sourceDesc;
                _source.OriginalLocation = sourceOrigLoc;
                _source.IsCopyHeld = isCopyHeld;
                _source.IsViewed = isViewed;
                _source.IsThackrayFound = isThackrayFound;
                _source.UserId = userId;
                _source.SourceDate = sourceDateInt;
                _source.SourceDateTo = sourceDateIntTo;
                _source.SourceDateStr = sourceDateStr;
                _source.SourceDateStrTo = sourceDateStrTo;
                _source.SourceRef = sourceRef;
                _source.SourceFileCount = sourceFileCount;
                _source.SourceNotes = sourceNotes;
             
                _source.DateAdded = DateTime.Today;

                ModelContainer.SaveChanges();
            }



      
        }


        //SourceAjaxDto

        public void UpdateSource(SourceDto sourceAjaxDto)
        {
           
            var source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceAjaxDto.SourceId);


            if (source == null) return;


            source.SourceDescription = sourceAjaxDto.SourceDesc;
            source.OriginalLocation = sourceAjaxDto.OriginalLocation;
            source.IsCopyHeld = sourceAjaxDto.IsCopyHeld;
            source.IsViewed = sourceAjaxDto.IsViewed;
            source.IsThackrayFound = sourceAjaxDto.IsThackrayFound;
            source.UserId = 1;
            source.SourceDate = CsUtils.GetDateYear(sourceAjaxDto.SourceDateStr);
            source.SourceDateTo = CsUtils.GetDateYear(sourceAjaxDto.SourceDateStrTo);
            source.SourceDateStr = sourceAjaxDto.SourceDateStr;
            source.SourceDateStrTo = sourceAjaxDto.SourceDateStrTo;
            source.SourceRef = sourceAjaxDto.SourceRef;
            source.SourceFileCount = sourceAjaxDto.SourceFileCount;
            source.SourceNotes = sourceAjaxDto.SourceNotes;

            source.DateAdded = DateTime.Today;



            ModelContainer.SaveChanges();
        }

        public void UpdateBasic(Guid sourceId, string sourceRef, string sourceDesc, int sourceYear, int sourceYearTo)
        {
            Source _source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);


            if (_source != null)
            {
                _source.SourceDescription = sourceDesc;
              
                _source.SourceRef = sourceRef;
        
                _source.DateAdded = DateTime.Today;


                _source.SourceDate = sourceYear;
                _source.SourceDateStr = sourceYear.ToString();

                _source.SourceDateTo = sourceYearTo;
                _source.SourceDateStrTo = sourceYearTo.ToString();
                 

                ModelContainer.SaveChanges();
            }
        }

        public void UpdateNotes(Guid sourceId, string sourceNotes)
        {
            Source _source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);
            if (_source != null)
            {
                _source.SourceNotes += sourceNotes;
                ModelContainer.SaveChanges();
            }
        }
 
        public void DeleteSource2(Guid sourceId)
        {
          

            var _source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);

            if (_source != null)
            {
                ModelContainer.Sources.Remove(_source);

                ModelContainer.SaveChanges();
            }
        }

        public string GetPersonSources(Guid _personId, List<Guid> sources)
        {
            //SimpleTimer _st = new SimpleTimer();


            //_st.StartTimer();


            var results = ModelContainer.GetPersonSources(_personId);
            string retVal = "";
            foreach (var _source in results)
            {
                retVal += _source.SourceRef + " ";
                sources.Add(_source.SourceId.Value);
            }


//            Debug.WriteLine( _st.EndTimer("1"));

            return retVal;
        }

        public string GetMarriageSources(Guid _marriageId, List<Guid> sources)
        {
            string retVal = "";
            var results = ModelContainer.GetMarriageSources(_marriageId);

            foreach (var source in results)
            {
                retVal += source.SourceRef;
                sources.Add(source.SourceId.Value);
            }

            return retVal;
        }

        public List<ServiceSource> FillSourceTableBySourceIds(List<Guid> ssf)
        {

            List<ServiceSource> result = null;
           
            result = ModelContainer.Sources.Where(o => ssf.Contains(o.SourceId)).ToList().Select(p => new ServiceSource()
                                        {
                                            SourceDesc = p.SourceDescription,
                                            SourceId = p.SourceId,
                                            SourceRef = p.SourceRef,
                                            SourceYear = p.SourceDate.GetValueOrDefault(),
                                            SourceYearTo = p.SourceDateTo.GetValueOrDefault()
                                        }).ToList();
 
            
            return result;

        }

        public List<ServiceSource> FillSourceTableByFilter(SourceSearchFilter ssf)
        {

            List<ServiceSource> result = null;
            #region booleans
            bool isCopyHoldLocal = false;
            bool isCopyHoldLocalAlt = false;

            if (ssf.CopyHeld == null)
            {
                isCopyHoldLocal = false;
                isCopyHoldLocalAlt = true;
            }
            else
            {
                isCopyHoldLocal = ssf.CopyHeld.Value;
                isCopyHoldLocalAlt = ssf.CopyHeld.Value;
            }

            bool isViewedLocal = false;
            bool isViewedLocalAlt = false;

            if (ssf.Viewed == null)
            {
                isViewedLocal = false;
                isViewedLocalAlt = true;
            }
            else
            {
                isViewedLocal = ssf.Viewed.Value;
                isViewedLocalAlt = ssf.Viewed.Value;
            }

            bool isThackrayLocalFound = false;
            bool isThackrayLocalFoundAlt = false;

            if (ssf.ThackrayFound == null)
            {
                isThackrayLocalFound = false;
                isThackrayLocalFoundAlt = true;
            }
            else
            {
                isThackrayLocalFound = ssf.ThackrayFound.Value;
                isThackrayLocalFoundAlt = ssf.ThackrayFound.Value;
            }

            #endregion

            ssf.SourceTypes.RemoveAll(p => p == 0);

            string listOfSourcesStr =ssf.SourceTypes.ParseToCSV();

            
            int sourceL = ssf.FromYear;
            int sourceU = ssf.ToYear;

            int sourceToL = ssf.FromYear;

            int sourceToU = ssf.ToYear;




            if (listOfSourcesStr == "")
            {
                result = ModelContainer.Sources.Where(o => (o.IsCopyHeld == isCopyHoldLocal || o.IsCopyHeld == isCopyHoldLocalAlt) &&
                                            (o.IsThackrayFound == isThackrayLocalFound || o.IsThackrayFound == isThackrayLocalFoundAlt) &&
                                            o.SourceDescription.Contains(ssf.Description) &&
                                            (o.IsViewed == isViewedLocal || o.IsViewed == isViewedLocalAlt) &&
                                            o.OriginalLocation.Contains(ssf.OriginalLocation) &&
                                            
                                            o.SourceDate >= sourceL && o.SourceDate <= sourceU &&
                                            o.SourceDateTo >= sourceToL && o.SourceDateTo <= sourceToU &&

                                            o.SourceRef.Contains(ssf.Ref)).Select(p=> new ServiceSource()
                                                {
                                                    SourceDesc = p.SourceDescription,
                                                    SourceId = p.SourceId,
                                                    SourceRef = p.SourceRef,
                                                    SourceYear = p.SourceDate,
                                                    SourceYearTo = p.SourceDateTo                                                    
                                                }).ToList();
            }
            else
            {
                result = ModelContainer.GetSourcesBySourceTypes(
                    ssf.Ref,
                    sourceToU, sourceL, sourceToL, sourceU,
                    1,
                    ssf.OriginalLocation,
                    new DateTime(2050,1,1),
                    new DateTime(1920, 1, 1), 
                    isThackrayLocalFound,
                    isThackrayLocalFoundAlt,
                    isViewedLocal,
                    isViewedLocalAlt,
                    isCopyHoldLocal,
                    isCopyHoldLocalAlt,
                    listOfSourcesStr).Select(p => new ServiceSource()
                    {
                        SourceDesc = p.SourceDescription,
                        SourceId = p.SourceId,
                        SourceRef = p.SourceRef,
                        SourceYear = p.SourceDate.GetValueOrDefault(),
                        SourceYearTo = p.SourceDateTo.GetValueOrDefault()
                    }).ToList();
            }

            int intSourceFileCount = 0;

     
            if (Int32.TryParse( ssf.FileCount, out intSourceFileCount))
            {
                if (intSourceFileCount == 0)
                {
                    result = result.Where(o => o.FileCount == 0).ToList();
                }
                else
                {
                    result = result.Where(o => o.FileCount >= intSourceFileCount).ToList();
                }
            }


            return result;

        }


        public IQueryable<Source> FillSourceTableByFilter2(string sourceDesc, string sourceOrigLoc,
            bool? isCopyHeld, bool? isViewed, bool? isThackrayFound,
            int sourceU, int sourceL, int sourceToU, int sourceToL,
            int? userId, List<int> listOfSources, DateTime addedFrom, DateTime addedTo, string sourceRef,
            string sourceFileCount)
        {
     //       ModelContainer.Sources.MergeOption = System.Data.Objects.MergeOption.NoTracking;

            IQueryable<Source> result = null;
            #region booleans
            bool isCopyHoldLocal = false;
            bool isCopyHoldLocalAlt = false;

            if (isCopyHeld == null)
            {
                isCopyHoldLocal = false;
                isCopyHoldLocalAlt = true;
            }
            else
            {
                isCopyHoldLocal = isCopyHeld.Value;
                isCopyHoldLocalAlt = isCopyHeld.Value;
            }

            bool isViewedLocal = false;
            bool isViewedLocalAlt = false;

            if (isViewed == null)
            {
                isViewedLocal = false;
                isViewedLocalAlt = true;
            }
            else
            {
                isViewedLocal = isViewed.Value;
                isViewedLocalAlt = isViewed.Value;
            }

            bool isThackrayLocalFound = false;
            bool isThackrayLocalFoundAlt = false;

            if (isThackrayFound == null)
            {
                isThackrayLocalFound = false;
                isThackrayLocalFoundAlt = true;
            }
            else
            {
                isThackrayLocalFound = isThackrayFound.Value;
                isThackrayLocalFoundAlt = isThackrayFound.Value;
            }

            #endregion

            string listOfSourcesStr = "";

            if (listOfSources != null && listOfSources.Count > 0)
            {
                foreach (int _id in listOfSources)
                {
                    listOfSourcesStr += "," + _id.ToString();
                }

                listOfSourcesStr = listOfSourcesStr.Remove(0, 1);
            }


            if (listOfSourcesStr == "")
            {
                result = ModelContainer.Sources.Where(o => (o.IsCopyHeld == isCopyHoldLocal || o.IsCopyHeld == isCopyHoldLocalAlt) &&
                                            (o.IsThackrayFound == isThackrayLocalFound || o.IsThackrayFound == isThackrayLocalFoundAlt) &&
                                            o.SourceDescription.Contains(sourceDesc) &&
                                            (o.IsViewed == isViewedLocal || o.IsViewed == isViewedLocalAlt) && 
                                            o.OriginalLocation.Contains(sourceOrigLoc) && 
                                            o.SourceDate >= sourceL &&  o.SourceDate <= sourceU &&
                                            o.SourceDateTo >= sourceToL &&  o.SourceDateTo <= sourceToU && 
                                            o.SourceRef.Contains(sourceRef));
            }
            else
            {                           
                result =ModelContainer.GetSourcesBySourceTypes(
                    sourceRef,
                    sourceToU, sourceL, sourceToL, sourceU,
                    userId.Value,
                    sourceOrigLoc,
                    addedTo,
                    addedFrom,
                    isThackrayLocalFound,
                    isThackrayLocalFoundAlt,
                    isViewedLocal,
                    isViewedLocalAlt,
                    isCopyHoldLocal,
                    isCopyHoldLocalAlt,
                    listOfSourcesStr).AsQueryable();
            }

            int intSourceFileCount = 0;

            if (Int32.TryParse(sourceFileCount, out intSourceFileCount))
            {
                if (intSourceFileCount == 0)
                {
                    result = result.Where(o => o.SourceFileCount == 0);
                }
                else
                {
                    result = result.Where(o => o.SourceFileCount >= intSourceFileCount);
                }
            }


            return result;

        }



        public List<ServiceSearchResult> GetSourceByParishString(string parishs, int startYear, int endYear)
        {
            var ssresults = new List<ServiceSearchResult>();

            var parishGroups = ModelContainer.uspGetParishSources(parishs, startYear, endYear).ToList();

            foreach (var parish in parishGroups)
            {

                var ssr = new ServiceSearchResult
                    {
                        ParishId = parish.SourceMappingParishId.GetValueOrDefault(),
                        IsMarriage = true,
                        IsBaptism = true,
                        IsBurial = true
                    };

                var parishResult = parishGroups.Where(ps => ps.SourceMappingParishId == ssr.ParishId).ToList();

                

                if (parishResult.Count > 0 && !parishResult.Exists(pr => pr.MapTypeId == 43))
                {
                    ssr.IsMarriage = parishResult.Exists(pr => pr.MapTypeId == 40);
                    //baptisms
                    ssr.IsBaptism = parishResult.Exists(pr => pr.MapTypeId == 41);
                    //burials
                    ssr.IsBurial = parishResult.Exists(pr => pr.MapTypeId == 42);                    
                }


                ssresults.Add(ssr);

            }


            return ssresults;
        }

        public List<string> GetsourceRefs2(List<Guid> sourceId)
        {
             return ModelContainer.Sources.Where(o => sourceId.Contains(o.SourceId)).Select(o => o.SourceRef).ToList();
        }

        public IQueryable<Source> FillSourceTableByParishId2(Guid parishId)
        {
            return ModelContainer.Sources.Where(o => o.SourceMappingParishs.Any(a => a.Parish.ParishId == parishId)); 
        }

        public Source FillSourceTableById2(Guid sourceId)
        {
            return ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);
        }

        public SourceDto GetSource(Guid sourceId)
        {
            var tp = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);

            if (tp != null)
            {
                return new SourceDto()
                {
                   IsCopyHeld = tp.IsCopyHeld.GetValueOrDefault(),
                   IsThackrayFound = tp.IsThackrayFound.GetValueOrDefault(),
                   IsViewed = tp.IsViewed.GetValueOrDefault(),
                   OriginalLocation = tp.OriginalLocation,
                   SourceDateStr = tp.SourceDateStr,
                   SourceDateStrTo = tp.SourceDateStrTo,
                   SourceDesc = tp.SourceDescription,
                   SourceFileCount = tp.SourceFileCount.GetValueOrDefault(),
                   SourceNotes = tp.SourceNotes,
                   SourceRef = tp.SourceRef,
                   SourceId = tp.SourceId
                };
            }

            return new SourceDto();
        }


        public IQueryable<Source> FillSources()
        {
            return ModelContainer.Sources; 
        }

        public IQueryable<Source> FillSourceTableByPersonOrMarriageId2(Guid recordId)
        {
            return ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.Marriage.Marriage_Id == recordId || p.Person.Person_id == recordId)); 
        }

        public List<ServiceSource> FillTreeSources(SourceSearchFilter description)
        {          
            var retVal = new List<ServiceSource>();

            if(description.Description=="")
                retVal= ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.SourceType.SourceTypeId == 39)).Select(s=> new ServiceSource()
                    {
                        SourceDesc = s.SourceDescription,
                        SourceId = s.SourceId,
                        SourceRef = s.SourceRef,
                        SourceYear = s.SourceDate.GetValueOrDefault(),
                        SourceYearTo = s.SourceDateTo.GetValueOrDefault(),
                        UserId = s.UserId                        
                    }).ToList();
            else
                retVal= ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.SourceType.SourceTypeId == 39) && o.SourceDescription.Contains(description.Description)).Select(s => new ServiceSource()
                {
                    SourceDesc = s.SourceDescription,
                    SourceId = s.SourceId,
                    SourceRef = s.SourceRef,
                    SourceYear = s.SourceDate.GetValueOrDefault(),
                    SourceYearTo = s.SourceDateTo.GetValueOrDefault(),
                    UserId = s.UserId
                }).ToList();

            if (description.IncludeDefaultPerson)
            {             
                foreach (var ss in retVal)
                {
                    var sourceMap = ModelContainer.SourceMappings.FirstOrDefault(o => o.Source.SourceId == ss.SourceId && o.SourceType.SourceTypeId == 39);
               
                    if (sourceMap != null)
                    {
                        if (sourceMap.Person != null)
                            ss.DefaultPerson = sourceMap.Person.Person_id;
                        else
                            ss.DefaultPerson = Guid.Empty;
                    }
                    else
                        ss.DefaultPerson = Guid.Empty;
                }

            }


            return retVal;
        }

        public bool DeleteTree(Guid sourceId)
        {
            if (sourceId != null
                && sourceId != Guid.Empty)
            {
                ModelContainer.DeleteTree(sourceId);

                var row = ModelContainer.Sources.FirstOrDefault(s => s.SourceId == sourceId);


                if (row == null)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Get Source Reference string for marriage or person
        /// ref is a maximum of 49 character anything after that
        /// will be cut off.
        /// </summary>          
        public string GetSourcesRef(Guid recordId)
        {
           // var result = ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.Marriage.Marriage_Id == recordId || p.Person.Person_id == recordId)).Select(s=>s.SourceRef);

            string result = string.Join(Environment.NewLine, ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.Marriage.Marriage_Id == recordId || p.Person.Person_id == recordId)).Select(s => s.SourceRef).ToList());


            if(result.Length>49)
                result = result.Substring(0,49);

            return result;
 
        }

       

        public IQueryable<Source> FillSourceTableBySourceRef2(string sourceRef)
        {
            return ModelContainer.Sources.Where(o => o.SourceRef.Contains(sourceRef));
        }
    
    
    
    }
}
