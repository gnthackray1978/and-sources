using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using System.Text.RegularExpressions;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;

namespace TDBCore.BLL
{
    public class SourceDal : BaseBll, ISourceDal
    {
        private readonly SourceTypesDal _sourceTypes;
       

        public SourceDal()
        {
            _sourceTypes = new SourceTypesDal();
           
        }

        public List<CensusSource> Get1841CensuSources(Guid sourceId)
        {   
            var ret = new List<CensusSource>();
                                
            foreach (var entry in ModelContainer.uvw_1841Census.Where(c => c.ParishId == sourceId))
            {
                var censusPersons = new List<CensusPerson>();


                var personResults = ModelContainer.Persons.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == entry.SourceId)).OrderBy(p => p.BirthDateStr).ToList();

                
                personResults.ForEach(o => censusPersons.Add(new CensusPerson
                {
                        BirthCounty = o.BirthCounty,
                        BirthYear = o.BirthDateStr.ToInt32(),
                        CName = o.ChristianName,
                        SName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(o.Surname.ToLower())  
                    }));


                var cs = new CensusSource
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
            return Enumerable.Aggregate(FillSourceTableByPersonOrMarriageId2(person), "", (current, source) => current + (Environment.NewLine + source.SourceRef));

        }

        public List<Person> GetPersonsForSource(Guid sourceId)
        {
            return ModelContainer.SourceMappings.Where(sm => sm.Source.SourceId == sourceId && sm.Person != null).Select(p => p.Person).ToList();
        }

        public Guid InsertSource(SourceDto sourceAjaxDto)
        {       
            var source = new Source
            {
                SourceDescription = sourceAjaxDto.SourceDesc,
                OriginalLocation = sourceAjaxDto.OriginalLocation,
                IsCopyHeld = sourceAjaxDto.IsCopyHeld,
                IsViewed = sourceAjaxDto.IsViewed,
                IsThackrayFound = sourceAjaxDto.IsThackrayFound,
                UserId = 0,
                SourceDate = sourceAjaxDto.SourceDateStr.ParseToValidYear(),
                SourceDateTo = sourceAjaxDto.SourceDateStrTo.ParseToValidYear(),
                SourceDateStr = sourceAjaxDto.SourceDateStr,
                SourceDateStrTo = sourceAjaxDto.SourceDateStrTo,
                SourceRef = sourceAjaxDto.SourceRef,
                SourceFileCount = sourceAjaxDto.SourceFileCount,
                SourceNotes = sourceAjaxDto.SourceNotes,
                SourceId = Guid.NewGuid(),
                DateAdded = DateTime.Today
            };

            ModelContainer.Sources.Add(source);

            ModelContainer.SaveChanges();

            return source.SourceId;
        }

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
            source.SourceDate = sourceAjaxDto.SourceDateStr.ParseToValidYear();
            source.SourceDateTo = sourceAjaxDto.SourceDateStrTo.ParseToValidYear();
            source.SourceDateStr = sourceAjaxDto.SourceDateStr;
            source.SourceDateStrTo = sourceAjaxDto.SourceDateStrTo;
            source.SourceRef = sourceAjaxDto.SourceRef;
            source.SourceFileCount = sourceAjaxDto.SourceFileCount;
            source.SourceNotes = sourceAjaxDto.SourceNotes;

            source.DateAdded = DateTime.Today;



            ModelContainer.SaveChanges();
        }
  
        public void DeleteSource2(Guid sourceId)
        {         
            var source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);

            if (source == null) return;

            ModelContainer.Sources.Remove(source);

            ModelContainer.SaveChanges();
        }
 
        public List<ServiceSource> FillSourceTableBySourceIds(List<Guid> ssf)
        {
            var result = ModelContainer.Sources.Where(o => ssf.Contains(o.SourceId)).ToList().Select(p => new ServiceSource
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

            List<ServiceSource> result;
            #region booleans
            bool isCopyHoldLocal;
            bool isCopyHoldLocalAlt;

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

            bool isViewedLocal;
            bool isViewedLocalAlt;

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

            bool isThackrayLocalFound;
            bool isThackrayLocalFoundAlt;

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

                                            o.SourceRef.Contains(ssf.Ref)).Select(p=> new ServiceSource
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
                    listOfSourcesStr).Select(p => new ServiceSource
                    {
                        SourceDesc = p.SourceDescription,
                        SourceId = p.SourceId,
                        SourceRef = p.SourceRef,
                        SourceYear = p.SourceDate.GetValueOrDefault(),
                        SourceYearTo = p.SourceDateTo.GetValueOrDefault()
                    }).ToList();
            }
          
            return ssf.FileCount.ToInt32() == 0 ? result.Where(o => o.FileCount == 0).ToList() : result.Where(o => o.FileCount >= ssf.FileCount.ToInt32()).ToList();
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
          
        public List<SourceRecord> GetParishSourceRecords(Guid parishId)
        {
            var sources = ModelContainer.Sources.Where(o => o.SourceDate != null 
                && o.SourceDateTo != null 
                && o.SourceMappingParishs.Any(a => a.Parish.ParishId == parishId));

            var sourceRecords = sources.Select(srow => new SourceRecord
            {
                SourceId = srow.SourceId,
                IsCopyHeld = srow.IsCopyHeld.GetValueOrDefault(),
                IsThackrayFound = srow.IsThackrayFound.GetValueOrDefault(),
                IsViewed = srow.IsViewed.GetValueOrDefault(),
                OriginalLocation = srow.OriginalLocation,
                SourceDesc = srow.SourceDescription,
                SourceRef = srow.SourceRef,
                YearStart = srow.SourceDate.Value,
                YearEnd = srow.SourceDateTo.Value,
                sourceTYpes = _sourceTypes.GetSourceTypeBySourceId2(srow.SourceId).Select(st => st.SourceTypeId).ToList()
                 
            }).ToList();

            sourceRecords.ForEach(sr => sr.sourceTYpes.ForEach(type =>                 
            {
                // parish regs
                if (type >= 40 && type <= 43)
                    sr.DisplayOrder = 1;
                // parish reg
                if (type == 1)
                    sr.DisplayOrder = 1;
                // igi records
                if (type == 36)
                    sr.DisplayOrder = 2;
                // wills etc
                if (type >= 2 && type <= 33)
                    sr.DisplayOrder = 3;

                if (type >= 37 && type <= 39)
                    sr.DisplayOrder = 3;
            }));                  
            return sourceRecords;
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
                return new SourceDto
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
      
        public IQueryable<Source> FillSourceTableByPersonOrMarriageId2(Guid recordId)
        {
            return ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.Marriage.Marriage_Id == recordId || p.Person.Person_id == recordId)); 
        }

        public List<ServiceSource> FillTreeSources(SourceSearchFilter description)
        {          
            List<ServiceSource> retVal;

            if(description.Description=="")
                retVal= ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.SourceType.SourceTypeId == 39)).Select(s=> new ServiceSource
                    {
                        SourceDesc = s.SourceDescription,
                        SourceId = s.SourceId,
                        SourceRef = s.SourceRef,
                        SourceYear = s.SourceDate.GetValueOrDefault(),
                        SourceYearTo = s.SourceDateTo.GetValueOrDefault(),
                        UserId = s.UserId                        
                    }).ToList();
            else
                retVal= ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.SourceType.SourceTypeId == 39) && o.SourceDescription.Contains(description.Description)).Select(s => new ServiceSource
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
                        ss.DefaultPerson = sourceMap.Person != null ? sourceMap.Person.Person_id : Guid.Empty;
                    }
                    else
                        ss.DefaultPerson = Guid.Empty;
                }

            }


            return retVal;
        }
      
        public string GetSourcesRef(Guid recordId)
        {
           // var result = ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.Marriage.Marriage_Id == recordId || p.Person.Person_id == recordId)).Select(s=>s.SourceRef);

            string result = string.Join(Environment.NewLine, ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.Marriage.Marriage_Id == recordId || p.Person.Person_id == recordId)).Select(s => s.SourceRef).ToList());


            if(result.Length>49)
                result = result.Substring(0,49);

            return result;
 
        }
 
    }
}
