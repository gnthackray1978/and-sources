using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using System.Diagnostics;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;

namespace TDBCore.BLL
{
    public class SourceMappingsDal : BaseBll
    {
   
        private readonly SourceMappingParishsDal _sourceMappingParishsDal;
       
      
    

        public SourceMappingsDal()
        {
            _sourceMappingParishsDal = new SourceMappingParishsDal();                        
        }

        public bool SetDefaultTreePerson(Guid sourceId, Guid personId)
        {
            
            var smap = GetBySourceIdAndMapTypeId2(sourceId, 39).FirstOrDefault();

            if (smap != null)
            {
                UpdateDefaultPerson(smap.MappingId, sourceId, personId);
            }

            return true;
        }

        public int Insert(Guid? sourceId, Guid? fileId, Guid? marriageId, int userId, Guid? personId, string dateAdded, int? mapTypeId)
        {
            int retid = 0;
            IQueryable<SourceMapping> retTab = null;
        
            if (sourceId != null && mapTypeId != null)
                retTab = GetBySourceIdAndMapTypeId2(sourceId, mapTypeId);

            if (sourceId != null && personId != null)// || marriageId != null))
                retTab = GetByPersonOrMarriageIdAndSourceId2(sourceId, personId);

            if (sourceId != null && marriageId != null)// || marriageId != null))
                retTab = GetByPersonOrMarriageIdAndSourceId2(sourceId, marriageId);

            if (retTab == null || !retTab.Any())
            {
                var file = ModelContainer.Files.FirstOrDefault(o => o.FiletId == fileId);
                var marriage = ModelContainer.Marriages.FirstOrDefault(o => o.Marriage_Id == marriageId);
                var person = ModelContainer.Persons.FirstOrDefault(o => o.Person_id == personId);
                var mapType = ModelContainer.SourceTypes.FirstOrDefault(o => o.SourceTypeId == mapTypeId);
                var source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceId);

               // Adapter.Insert(searchId, fileId, marriageId, userId, personId, dateAdded, mapTypeId);

                var sourceMapping = new SourceMapping
                {
                    File = file,
                    DateAdded = DateTime.Today,
                    Source = source,
                    Person = person,
                    SourceType = mapType,
                    UserId = userId,
                    Marriage = marriage
                };

                ModelContainer.SourceMappings.Add(sourceMapping);
                ModelContainer.SaveChanges();

                retid = sourceMapping.MappingId;
            }

            return retid;
        }


        public int UpdateDefaultPerson(int mappingId, Guid sourceId, Guid personId)
        {
            SourceMapping sourceMapping = ModelContainer.SourceMappings.FirstOrDefault(sm => sm.MappingId == mappingId);
            Source source = ModelContainer.Sources.FirstOrDefault(so => so.SourceId == sourceId);
            Person person = ModelContainer.Persons.FirstOrDefault(po => po.Person_id == personId);
            SourceType sourceType = ModelContainer.SourceTypes.FirstOrDefault(st => st.SourceTypeId == 39);

            if (sourceMapping != null &&
                source != null)
            {
              
                sourceMapping.DateAdded = DateTime.Today;
                sourceMapping.Source = source;
                sourceMapping.Person = person;
                sourceMapping.SourceType = sourceType;
           

    
                ModelContainer.SaveChanges();
            }



            return mappingId;
        }

        public void WriteMarriageSources(Guid recordId, List<Guid> selectedSourceGuids, int userId)
        { 
            WriteSourceMappings2( recordId,selectedSourceGuids,  userId,  true);
        
        }

        public void WritePersonSources2(Guid recordId, List<Guid> selectedSourceGuids, int userId)
        {
            WriteSourceMappings2(recordId, selectedSourceGuids, userId, false);

        }

      

        public void WriteParishsToSource(Guid sourceRecordId, List<Guid> parishIdList, int userId)
        {

            List<Guid> copyList = parishIdList.ToList();

            // return ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId);

            foreach (var sRow in ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceRecordId))
            {
                if (!copyList.Contains(sRow.Parish.ParishId) || copyList.Count == 0)
                {
                    SourceMappingParish sourceMapping = ModelContainer.SourceMappingParishs.FirstOrDefault(sm => sm.SourceMappingParishsRowId == sRow.SourceMappingParishsRowId);
                    
                    if (sourceMapping != null)
                    {
                        ModelContainer.SourceMappingParishs.Remove(sourceMapping);
                    }
                }
                else
                {
                    copyList.Remove(sRow.Parish.ParishId);
                }
            }

            ModelContainer.SaveChanges();

            foreach (Guid parishId in copyList)
            {                 
                _sourceMappingParishsDal.InsertSourceMappingParish2(parishId, sourceRecordId, userId);
            }     
        }


        public void WriteFilesIdsToSource(Guid sourceId, List<Guid> fileIdList, int userId)
        {
        //    SourceDal sourceBll = new SourceDal();
            List<Guid> copyList = fileIdList;

            // if there are no selected sources and there are some in the source table
            // then delete them from the database
            //  other wise just delete the missing entries 
            // we dont want to perform any unnessecary writes to the db if we can help it
            // so check if there is already a record if there is remove it from the list
            // of records that need to be written


            foreach (var sRow in GetSourceMappingsWithFiles(sourceId))
            {
                if (!copyList.Contains(sRow.File.FiletId) || copyList.Count == 0)
                {
                    SourceMapping sourceMapping = ModelContainer.SourceMappings.FirstOrDefault(sm => sm.MappingId == sRow.MappingId);

                    if (sourceMapping != null)
                    {
                        ModelContainer.SourceMappings.Remove(sourceMapping);
                    }
                }
                else
                {
                    copyList.Remove(sRow.File.FiletId);
                }
            }

            ModelContainer.SaveChanges();

            foreach (var fileTypeId in copyList)
            {
                Insert(sourceId, fileTypeId, null, userId, null, DateTime.Today.ToShortDateString(), null);
            }

        }

        public void WriteFilesToSource(Guid sourceId, List<ServiceFile> fileIdList, int userId)
        {
            // if there are no selected sources and there are some in the source table
            // then delete them from the database
            //  other wise just delete the missing entries 
            // we dont want to perform any unnessecary writes to the db if we can help it
            // so check if there is already a record if there is remove it from the list
            // of records that need to be written


            // ok so do the deletions to start with

           
            var deletionList = fileIdList.Where(p => p.FileDescription == "" && p.FileLocation == "").Select(p => p.FileId).ToList();
 
            foreach (var sourceMapping in GetSourceMappingsWithFiles(sourceId)
                .Where(sRow => deletionList.Contains(sRow.File.FiletId))
                .Select(sRow => ModelContainer.SourceMappings.FirstOrDefault(sm => sm.MappingId == sRow.MappingId)).Where(sourceMapping => sourceMapping != null))
            {
                ModelContainer.SourceMappings.Remove(sourceMapping);
            }

            ModelContainer.SaveChanges();

            foreach (var file in deletionList.Select(guid => ModelContainer.Files.First(p => p.FiletId == guid)).Where(file => file != null))
            {
                ModelContainer.Files.Remove(file);
            }

            ModelContainer.SaveChanges();
            

            // do edits

            var newFiles = new List<File>();

            foreach (var guid in fileIdList.Where(p => p.FileDescription != "" && p.FileLocation != ""))
            {
               var edittingFile = ModelContainer.Files.FirstOrDefault(f => f.FiletId == guid.FileId);

               if (edittingFile != null)
               {
                   edittingFile.FileDescription = guid.FileDescription;
                   edittingFile.FileLocation = guid.FileLocation;
               }
               else
               {
                   var newFile = new File
                       {
                           FiletId = guid.FileId,
                           FileDescription = guid.FileDescription,
                           FileLocation = guid.FileLocation
                       };

                   newFiles.Add(newFile);

                   ModelContainer.Files.Add(newFile);


               }

            }

            ModelContainer.SaveChanges();

            foreach (var fileTypeId in newFiles)
            {
                Insert(sourceId, fileTypeId.FiletId, null, userId, null, DateTime.Today.ToShortDateString(), null);
            }

            ModelContainer.SaveChanges();
        }
    
        public void WriteSourceTypesToSource(Guid sourceId, List<int> sourceTypeIdList, int userId)
        {           

         
     

            List<int> copyList = sourceTypeIdList.ToList();

            foreach (var sRow in GetBySourceTypesBySourceId2(sourceId))
            {
                if (!copyList.Contains(sRow.SourceType.SourceTypeId) || copyList.Count == 0)
                {
                    SourceMapping sourceMapping = ModelContainer.SourceMappings.FirstOrDefault(sm => sm.MappingId == sRow.MappingId);

                    if (sourceMapping != null)
                    {
                        ModelContainer.SourceMappings.Remove(sourceMapping);
                    }
                }
                else
                {
                    copyList.Remove(sRow.SourceType.SourceTypeId);
                }
            }

            ModelContainer.SaveChanges();

            foreach (int sourceTypeId in copyList)
            {
                Insert(sourceId, null, null, userId, null, DateTime.Today.ToShortDateString(), sourceTypeId);
            }

        }

   
        public void WriteSourceMappings2(Guid recordId, IList<Guid> selectedSourceGuids, int userId, bool isMarriage)
        {
            
            Debug.WriteLine("WriteSourceMappings2");

            var workingList = new List<Guid>();
            selectedSourceGuids.RemoveDuplicates();

            workingList.AddRange(selectedSourceGuids);


            // get current sources for record (marriage or person)
            IQueryable<SourceMapping> sourcesDataTable = GetByMarriageIdOrPersonId2(recordId);

            // if there are no selected sources and there are some in the source table
            // then delete them from the database
            //  other wise just delete the missing entries
            var rowsToDelete = new List<Guid>();

            // if there are any of these delete them 
            // shouldnt be but sometimes things can get screwed up
            // so if thats happened clean up
            List<int> invalidmappings = sourcesDataTable.Where(o => o.File == null && o.Source == null && o.SourceType == null).Select(p => p.MappingId).ToList();


            List<Guid> validSources = sourcesDataTable.Where(o => o.Source != null).Select(p => p.Source.SourceId).ToList();



            if (workingList.Count == 0 && validSources.Any())
            {
                validSources.ForEach(rowsToDelete.Add);                
            }
            else
            {
                if (workingList.Count > 0)
                {
                    validSources.ForEach(o =>
                    {
                        if (!workingList.Contains(o)) rowsToDelete.Add(o);
                    });
                }

                // we dont want to perform any unnessecary writes to the db if we can help it
                // so check if there is already a record if there is remove it from the list
                // of records that need to be written
                if (workingList.Count > 0)
                {
                    foreach (var sRow in validSources)
                    {
                        if (workingList.Contains(sRow))
                        {
                            workingList.Remove(sRow);
                        }
                    }
                }

            }

            foreach (Guid deleteId in rowsToDelete)
            {

                DeleteBySourceIdMarriageIdOrPersonId(deleteId, recordId);
            }
            
            //delete the invalid mappings if they exist.
            foreach (int mappingid in invalidmappings)
            {
                DeleteByMappingId(mappingid);
            }

            // if we've got something to write
            // we shouldnt need to do an update for this, as its purely a mapping table
            // if i remember correctly!! 
            if (workingList.Count > 0)
            {
             
                foreach (Guid sourceId in workingList)
                {

                    if (isMarriage)
                        Insert(sourceId, null, recordId, userId, null, DateTime.Today.ToShortDateString(), null);
                    else
                        Insert(sourceId, null, null, userId, recordId, DateTime.Today.ToShortDateString(), null);

                }

            }
        }

   

        #region delete methods



        // get files for source 
        // get mappings for source which are a file


        public void DeleteFilesForSource(Guid sourceId)
        {
            var mappingIds = ModelContainer.SourceMappings.Where(sm => sm.Source.SourceId == sourceId && sm.File != null).Select(s=>s.MappingId).ToList();

            var fileIds = ModelContainer.SourceMappings.Where(sm => sm.Source.SourceId == sourceId && sm.File != null).Select(s => s.File.FiletId).ToList();

            foreach (int mapping in mappingIds)
            {
                var sourcemapping = ModelContainer.SourceMappings.FirstOrDefault(sm => sm.MappingId == mapping);

                if (sourcemapping != null)
                {
                    ModelContainer.SourceMappings.Remove(sourcemapping);
                    
                }
            }


            fileIds.ForEach(f =>
            {
               var file = ModelContainer.Files.FirstOrDefault(fi => fi.FiletId == f);
               if (file != null)
                   ModelContainer.Files.Remove(file);
            });

            ModelContainer.SaveChanges();



        }

        public void DeleteByMappingId(int mappingId)
        {
            var sourcemapping = ModelContainer.SourceMappings.FirstOrDefault(sm => sm.MappingId == mappingId);

            if (sourcemapping != null)
            {
                ModelContainer.SourceMappings.Remove(sourcemapping);
                ModelContainer.SaveChanges();
            }
        }

        public void DeleteByMapTypeIdAndSourceId(Guid sourceId, int mapTypeId)
        {

            var sourcemapping= GetBySourceIdAndMapTypeId2(sourceId, mapTypeId).FirstOrDefault();

            if (sourcemapping != null)
            {
                ModelContainer.SourceMappings.Remove(sourcemapping);


                ModelContainer.SaveChanges();
            }



          //  Adapter.DeleteBySourceIdAndMapTypeId(sourceId, mapTypeId);
        }



        public void DeleteByFileIdAndSourceId(Guid? sourceId, Guid? fileId)
        {
            var sourcemapping = GetByFileIdAndSourceId2(sourceId, fileId).FirstOrDefault();

            if (sourcemapping != null)
            {
                ModelContainer.SourceMappings.Remove(sourcemapping);
                ModelContainer.SaveChanges();
            }
        }

        public void DeleteBySourceIdMarriageIdOrPersonId(Guid? sourceId, Guid? recordId)
        {

            var sourcemapping = GetByPersonOrMarriageIdAndSourceId2(sourceId,recordId).FirstOrDefault();

            if (sourcemapping != null)
            {
                // because source mappings are added and removed in disconnected state from the db
                // and the source mapping might not be in the entity model by this point BUT it could still be in the db
                if(ModelContainer.SourceMappings.Count(sm => sm.MappingId == sourcemapping.MappingId) >0)
                    ModelContainer.SourceMappings.Remove(sourcemapping);
                
                ModelContainer.SaveChanges();

            }
        }

        public void DeleteSourcesForPersonOrMarriage(Guid recordId)
        {

            foreach (SourceMapping smap in ModelContainer.SourceMappings.Where(sm => sm.Marriage.Marriage_Id == recordId || sm.Person.Person_id == recordId).ToList())
            {
                if (smap.Source != null)
                    ModelContainer.SourceMappings.Remove(smap);
            }

            ModelContainer.SaveChanges();

        }


        public void DeleteSourcesForPersonOrMarriage(Guid recordId, int? mapTypeId)
        {

            var effectedSources =
                ModelContainer.SourceMappings.Where( sm => sm.SourceType.SourceTypeId == mapTypeId).Select(p=>p.Source.SourceId).AsEnumerable() ;

            foreach (var smap in ModelContainer.SourceMappings.Where(sm => (sm.Marriage.Marriage_Id == recordId || sm.Person.Person_id == recordId)
                                                                           && effectedSources.Contains(sm.Source.SourceId)).ToList().Where(smap => smap.Source != null))
            {
                ModelContainer.SourceMappings.Remove(smap);
            }

            ModelContainer.SaveChanges();

        }


        #endregion

        #region select methods

    

        public IQueryable<SourceMapping> GetBySourceIdAndMapTypeId2(Guid? sourceId, int? mapTypeId)
        {
            IQueryable<SourceMapping> retTab = ModelContainer.SourceMappings.Where(o => o.Source.SourceId == sourceId && o.SourceType.SourceTypeId == mapTypeId);


            //  retTab = Adapter.GetDataBySourceIdAndMapTypeId(mapTypeId, sourceId);

            return retTab;
        }


        public IQueryable<SourceMapping> GetByFileIdAndSourceId2(Guid? sourceId, Guid? fileId)
        {
            IQueryable<SourceMapping> retTab = ModelContainer.SourceMappings.Where(o => o.Source.SourceId == sourceId && o.File.FiletId == fileId);

            return retTab;
        }


        public IQueryable<SourceMapping> GetByPersonOrMarriageIdAndSourceId2(Guid? sourceId, Guid? recordId)
        {
            IQueryable<SourceMapping> retTab = ModelContainer.SourceMappings.Where(o => (o.Marriage.Marriage_Id == recordId || o.Person.Person_id == recordId) && o.Source.SourceId == sourceId);

            // retTab = Adapter.GetDataByMarriageIdOrRecordId(sourceId, recordId);

            return retTab;
        }

        public IQueryable<SourceMapping> GetByMarriageIdOrPersonId2(Guid? recordId)
        {
            IQueryable<SourceMapping> retTab = ModelContainer.SourceMappings.Where(o => o.Person.Person_id == recordId || o.Marriage.Marriage_Id == recordId);

            return retTab;
        }


        public string GetSourceGuidList(Guid? recordId)
        {
            List<Guid> retTab = ModelContainer.SourceMappings.Where(o => o.Person.Person_id == recordId || o.Marriage.Marriage_Id == recordId).Select(_ => _.Source.SourceId).ToList();

            string retVal = retTab.Aggregate("", (current, g) => current + ("," + g.ToString()));

            if (retVal.StartsWith(",")) retVal = retVal.Remove(0, 1);

            return retVal;
        }

        public IQueryable<SourceMapping> GetSourceMappingsWithFiles(Guid? recordId)
        {
           
            IQueryable<SourceMapping> retTab = ModelContainer.SourceMappings.Where(o => o.Source.SourceId == recordId && o.File != null);

            return retTab;
        }

        public IQueryable<SourceMapping> GetBySourceTypesBySourceId2(Guid? recordId)
        {
          
            IQueryable<SourceMapping> retTab = ModelContainer.SourceMappings.Where(o => o.Source.SourceId == recordId && o.SourceType != null);
           

            return retTab;
        }


        #endregion 
    }
}
