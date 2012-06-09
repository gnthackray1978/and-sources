using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
////using TDBCore.Datasets;
using System.Data.SqlClient;
//using TDBCore.Datasets.DsSourceTypesTableAdapters;
 
using TDBCore.BLL;
using TDBCore.EntityModel;
using System.Diagnostics;
using TDBCore.Types;

namespace GedItter.BLL
{
    public class SourceBLL : BaseBLL
    {

        public ServiceSourceObject GetTreeSources(string description, string page_number, string page_size)
        {
            ServiceSourceObject ssobj = new ServiceSourceObject();

            ssobj.serviceSources = new List<ServiceSource>();



           // SourceBLL _sources = new SourceBLL();

            SourceMappingsBLL sourceMappingsBll = new SourceMappingsBLL();



            ssobj.serviceSources = this.FillTreeSources().Where(s => s.SourceDescription.Contains(description)).Select(s => new ServiceSource()
            {
                SourceDesc = s.SourceDescription,
                SourceId = s.SourceId,
                SourceRef = s.SourceRef,
                SourceYear = s.SourceDate.Value,
                SourceYearTo = s.SourceDateTo.Value
                //  DefaultPerson = sourceMappingsBll.GetBySourceIdAndMapTypeId2(s.SourceId,39).Select(o=>o.Person.Person_id).FirstOrDefault()
            }).ToList();

            foreach (ServiceSource ss in ssobj.serviceSources)
            {
                var sourceMap = sourceMappingsBll.GetBySourceIdAndMapTypeId2(ss.SourceId, 39).FirstOrDefault();


                if (sourceMap != null)
                {
                    if (sourceMap.Person != null && sourceMap.Person.Person_id != null)
                        ss.DefaultPerson = sourceMap.Person.Person_id;
                    else
                        ss.DefaultPerson = Guid.Empty;
                }
                else
                    ss.DefaultPerson = Guid.Empty;
            }

            ssobj.Batch = page_number.ToInt32();
            ssobj.BatchLength = page_size.ToInt32();
            ssobj.Total = ssobj.serviceSources.Count;

            ssobj.serviceSources = ssobj.serviceSources.Skip(page_number.ToInt32() * page_size.ToInt32()).Take(page_size.ToInt32()).ToList();

            return ssobj;
        }


        public List<Person> GetPersonsForSource(Guid sourceId)
        {
            return ModelContainer.SourceMappings.Where(sm => sm.Source.SourceId == sourceId).Select(p => p.Person).ToList();
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

            ModelContainer.Sources.AddObject(newSource);

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

            ModelContainer.Sources.AddObject(newSource);

            ModelContainer.SaveChanges();

            return newSource;
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


            ModelContainer.Sources.AddObject(_source);

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


            ModelContainer.Sources.AddObject(_source);

 

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
          

            Source _source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);

            if (_source != null)
            {
                ModelContainer.DeleteObject(_source);

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



        public List<uspGetParishSources_Result> GetSourceByParishString(string parishs, int startYear, int endYear)
        {
            return ModelContainer.uspGetParishSources(parishs, startYear, endYear).ToList();
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
            return ModelContainer.Sources.Where(o => o.SourceId == sourceId).FirstOrDefault();
        }

        public IQueryable<Source> FillSources()
        {
            return ModelContainer.Sources; 
        }


        public IQueryable<Source> FillSourceTableByPersonOrMarriageId2(Guid recordId)
        {
            return ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.Marriage.Marriage_Id == recordId || p.Person.Person_id == recordId)); 
        }


        public IQueryable<Source> FillTreeSources()
        {
            return ModelContainer.Sources.Where(o => o.SourceMappings.Any(p => p.SourceType.SourceTypeId == 39));
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
