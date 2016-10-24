using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using System.Diagnostics;
using TDBCore.Types.DTOs;
using TDBCore.Types.libs;

namespace TDBCore.BLL
{
    public class SourceMappingsDal : BaseBll, ISourceMappingsDal
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
            using (var context = new GeneralModelContainer())
            {
                int retid = 0;
                IEnumerable<SourceMapping> retTab = null;

                if (sourceId != null && mapTypeId != null)
                    retTab = GetBySourceIdAndMapTypeId2(sourceId, mapTypeId);

                if (sourceId != null && personId != null) // || marriageId != null))
                    retTab = GetByPersonOrMarriageIdAndSourceId2(sourceId, personId);

                if (sourceId != null && marriageId != null) // || marriageId != null))
                    retTab = GetByPersonOrMarriageIdAndSourceId2(sourceId, marriageId);

                if (retTab == null || !retTab.Any())
                {
                    var file = context.Files.FirstOrDefault(o => o.FiletId == fileId);
                    var marriage = context.Marriages.FirstOrDefault(o => o.Marriage_Id == marriageId);
                    var person = context.Persons.FirstOrDefault(o => o.Person_id == personId);
                    var mapType = context.SourceTypes.FirstOrDefault(o => o.SourceTypeId == mapTypeId);
                    var source = context.Sources.FirstOrDefault(o => o.SourceId == sourceId);

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

                    context.SourceMappings.Add(sourceMapping);
                    context.SaveChanges();

                    retid = sourceMapping.MappingId;
                }

                return retid;
            }
        }


        public int UpdateDefaultPerson(int mappingId, Guid sourceId, Guid personId)
        {
            using (var context = new GeneralModelContainer())
            {

                SourceMapping sourceMapping =
                    context.SourceMappings.FirstOrDefault(sm => sm.MappingId == mappingId);
                Source source = context.Sources.FirstOrDefault(so => so.SourceId == sourceId);
                Person person = context.Persons.FirstOrDefault(po => po.Person_id == personId);
                SourceType sourceType = context.SourceTypes.FirstOrDefault(st => st.SourceTypeId == 39);

                if (sourceMapping != null &&
                    source != null)
                {

                    sourceMapping.DateAdded = DateTime.Today;
                    sourceMapping.Source = source;
                    sourceMapping.Person = person;
                    sourceMapping.SourceType = sourceType;



                    context.SaveChanges();
                }



                return mappingId;
            }
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
            using (var context = new GeneralModelContainer())
            {
                var copyList = parishIdList.ToList();

                // return ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId);

                foreach (var sRow in context.SourceMappingParishs.Where(o => o.Source.SourceId == sourceRecordId) )
                {
                    if (!copyList.Contains(sRow.Parish.ParishId) || copyList.Count == 0)
                    {
                        SourceMappingParish sourceMapping =
                            context.SourceMappingParishs.FirstOrDefault(
                                sm => sm.SourceMappingParishsRowId == sRow.SourceMappingParishsRowId);

                        if (sourceMapping != null)
                        {
                            context.SourceMappingParishs.Remove(sourceMapping);
                        }
                    }
                    else
                    {
                        copyList.Remove(sRow.Parish.ParishId);
                    }
                }

                context.SaveChanges();

                foreach (Guid parishId in copyList)
                {
                    _sourceMappingParishsDal.InsertSourceMappingParish2(parishId, sourceRecordId, userId);
                }
            }
        }


        public void WriteFilesIdsToSource(Guid sourceId, List<Guid> fileIdList, int userId)
        {

            using (var context = new GeneralModelContainer())
            {
                //    SourceDal sourceBll = new SourceDal();
                var copyList = fileIdList;

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
                        SourceMapping sourceMapping =
                            context.SourceMappings.FirstOrDefault(sm => sm.MappingId == sRow.MappingId);

                        if (sourceMapping != null)
                        {
                            context.SourceMappings.Remove(sourceMapping);
                        }
                    }
                    else
                    {
                        copyList.Remove(sRow.File.FiletId);
                    }
                }

                context.SaveChanges();

                foreach (var fileTypeId in copyList)
                {
                    Insert(sourceId, fileTypeId, null, userId, null, DateTime.Today.ToShortDateString(), null);
                }
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

            using (var context = new GeneralModelContainer())
            {

                var deletionList =
                    fileIdList.Where(p => p.FileDescription == "" && p.FileLocation == "")
                        .Select(p => p.FileId)
                        .ToList();

                foreach (var sourceMapping in GetSourceMappingsWithFiles(sourceId)
                    .Where(sRow => deletionList.Contains(sRow.File.FiletId))
                    .Select(sRow => ModelContainer.SourceMappings.FirstOrDefault(sm => sm.MappingId == sRow.MappingId))
                    .Where(sourceMapping => sourceMapping != null))
                {
                    context.SourceMappings.Remove(sourceMapping);
                }

                context.SaveChanges();

                foreach (
                    var file in
                        deletionList.Select(guid => context.Files.First(p => p.FiletId == guid))
                            .Where(file => file != null))
                {
                    context.Files.Remove(file);
                }

                context.SaveChanges();


                // do edits

                var newFiles = new List<File>();

                foreach (var guid in fileIdList.Where(p => p.FileDescription != "" && p.FileLocation != ""))
                {
                    var edittingFile = context.Files.FirstOrDefault(f => f.FiletId == guid.FileId);

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

                        context.Files.Add(newFile);


                    }

                }

                context.SaveChanges();

                foreach (var fileTypeId in newFiles)
                {
                    Insert(sourceId, fileTypeId.FiletId, null, userId, null, DateTime.Today.ToShortDateString(), null);
                }

                context.SaveChanges();
            }
        }
    
        public void WriteSourceTypesToSource(Guid sourceId, List<int> sourceTypeIdList, int userId)
        {


            using (var context = new GeneralModelContainer())
            {

                List<int> copyList = sourceTypeIdList.ToList();

                foreach (var sRow in GetBySourceTypesBySourceId2(sourceId))
                {
                    if (!copyList.Contains(sRow.SourceType.SourceTypeId) || copyList.Count == 0)
                    {
                        SourceMapping sourceMapping =
                            context.SourceMappings.FirstOrDefault(sm => sm.MappingId == sRow.MappingId);

                        if (sourceMapping != null)
                        {
                            context.SourceMappings.Remove(sourceMapping);
                        }
                    }
                    else
                    {
                        copyList.Remove(sRow.SourceType.SourceTypeId);
                    }
                }

                context.SaveChanges();

                foreach (int sourceTypeId in copyList)
                {
                    Insert(sourceId, null, null, userId, null, DateTime.Today.ToShortDateString(), sourceTypeId);
                }

            }
        }

   
        public void WriteSourceMappings2(Guid recordId, IList<Guid> selectedSourceGuids, int userId, bool isMarriage)
        {
            
            Debug.WriteLine("WriteSourceMappings2");

            var workingList = new List<Guid>();
            selectedSourceGuids.RemoveDuplicates();

            workingList.AddRange(selectedSourceGuids);


            // get current sources for record (marriage or person)
            var sourcesDataTable = GetByMarriageIdOrPersonId2(recordId);

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
            using (var context = new GeneralModelContainer())
            {
                var mappingIds =
                    context.SourceMappings.Where(sm => sm.Source.SourceId == sourceId && sm.File != null)
                        .Select(s => s.MappingId)
                        .ToList();

                var fileIds =
                    context.SourceMappings.Where(sm => sm.Source.SourceId == sourceId && sm.File != null)
                        .Select(s => s.File.FiletId)
                        .ToList();

                foreach (int mapping in mappingIds)
                {
                    var sourcemapping = context.SourceMappings.FirstOrDefault(sm => sm.MappingId == mapping);

                    if (sourcemapping != null)
                    {
                        context.SourceMappings.Remove(sourcemapping);

                    }
                }


                fileIds.ForEach(f =>
                {
                    var file = context.Files.FirstOrDefault(fi => fi.FiletId == f);
                    if (file != null)
                        context.Files.Remove(file);
                });

                context.SaveChanges();

            }

        }

        public void DeleteByMappingId(int mappingId)
        {
            using (var context = new GeneralModelContainer())
            {
                var sourcemapping = context.SourceMappings.FirstOrDefault(sm => sm.MappingId == mappingId);

                if (sourcemapping != null)
                {
                    context.SourceMappings.Remove(sourcemapping);
                    context.SaveChanges();
                }
            }
        }

        public void DeleteByMapTypeIdAndSourceId(Guid sourceId, int mapTypeId)
        {
            using (var context = new GeneralModelContainer())
            {
                var sourcemapping = GetBySourceIdAndMapTypeId2(sourceId, mapTypeId).FirstOrDefault();

                if (sourcemapping != null)
                {
                    context.SourceMappings.Remove(sourcemapping);


                    context.SaveChanges();
                }

            }

            //  Adapter.DeleteBySourceIdAndMapTypeId(sourceId, mapTypeId);
        }



        public void DeleteByFileIdAndSourceId(Guid? sourceId, Guid? fileId)
        {
            using (var context = new GeneralModelContainer())
            {
                var sourcemapping = GetByFileIdAndSourceId2(sourceId, fileId).FirstOrDefault();

                if (sourcemapping != null)
                {
                    context.SourceMappings.Remove(sourcemapping);
                    context.SaveChanges();
                }
            }
        }

        public void DeleteBySourceIdMarriageIdOrPersonId(Guid? sourceId, Guid? recordId)
        {
            using (var context = new GeneralModelContainer())
            {
                var sourcemapping = GetByPersonOrMarriageIdAndSourceId2(sourceId, recordId).FirstOrDefault();

                if (sourcemapping != null)
                {
                    // because source mappings are added and removed in disconnected state from the db
                    // and the source mapping might not be in the entity model by this point BUT it could still be in the db
                    if (context.SourceMappings.Count(sm => sm.MappingId == sourcemapping.MappingId) > 0)
                        context.SourceMappings.Remove(sourcemapping);

                    context.SaveChanges();

                }
            }
        }

        public void DeleteSourcesForPersonOrMarriage(Guid recordId)
        {
            using (var context = new GeneralModelContainer())
            {
                foreach (
                    SourceMapping smap in
                        context.SourceMappings.Where(
                            sm => sm.Marriage.Marriage_Id == recordId || sm.Person.Person_id == recordId).ToList())
                {
                    if (smap.Source != null)
                        context.SourceMappings.Remove(smap);
                }

                context.SaveChanges();
            }
        }


        public void DeleteSourcesForPersonOrMarriage(Guid recordId, int? mapTypeId)
        {
            using (var context = new GeneralModelContainer())
            {
                var effectedSources =
                    context.SourceMappings.Where(sm => sm.SourceType.SourceTypeId == mapTypeId)
                        .Select(p => p.Source.SourceId)
                        .AsEnumerable();

                foreach (
                    var smap in
                        context.SourceMappings.Where(
                            sm => (sm.Marriage.Marriage_Id == recordId || sm.Person.Person_id == recordId)
                                  && effectedSources.Contains(sm.Source.SourceId))
                            .ToList()
                            .Where(smap => smap.Source != null))
                {
                    context.SourceMappings.Remove(smap);
                }

                context.SaveChanges();
            }
        }


        #endregion

        #region select methods

    

        public IEnumerable<SourceMapping> GetBySourceIdAndMapTypeId2(Guid? sourceId, int? mapTypeId)
        {

            using (var context = new GeneralModelContainer())
            {
                var retTab =
                    context.SourceMappings.Where(
                        o => o.Source.SourceId == sourceId && o.SourceType.SourceTypeId == mapTypeId);
                 
                return retTab.ToList();
            }
        }


        public IEnumerable<SourceMapping> GetByFileIdAndSourceId2(Guid? sourceId, Guid? fileId)
        {
            using (var context = new GeneralModelContainer())
            {
                var retTab =
                    context.SourceMappings.Where(o => o.Source.SourceId == sourceId && o.File.FiletId == fileId);

                return retTab.ToList();
            }
        }


        public IEnumerable<SourceMapping> GetByPersonOrMarriageIdAndSourceId2(Guid? sourceId, Guid? recordId)
        {
            using (var context = new GeneralModelContainer())
            {
                var retTab =
                    context.SourceMappings.Where(
                        o =>
                            (o.Marriage.Marriage_Id == recordId || o.Person.Person_id == recordId) &&
                            o.Source.SourceId == sourceId);

                // retTab = Adapter.GetDataByMarriageIdOrRecordId(sourceId, recordId);

                return retTab.ToList();
            }
        }

        public IEnumerable<SourceMapping> GetByMarriageIdOrPersonId2(Guid? recordId)
        {
            using (var context = new GeneralModelContainer())
            {
                var retTab =
                    context.SourceMappings.Where(
                        o => o.Person.Person_id == recordId || o.Marriage.Marriage_Id == recordId);

                return retTab.ToList();
            }
        }


        public string GetSourceGuidList(Guid? recordId)
        {
            using (var context = new GeneralModelContainer())
            {
                var retTab =
                    context.SourceMappings.Where(
                        o => (o.Person.Person_id == recordId || o.Marriage.Marriage_Id == recordId) && o.Source != null)
                        .Select(_ => _.Source.SourceId)
                        .ToList();

                string retVal = retTab.Aggregate("", (current, g) => current + ("," + g.ToString()));

                if (retVal.StartsWith(",")) retVal = retVal.Remove(0, 1);

                return retVal;
            }
        }

        public IEnumerable<SourceMapping> GetSourceMappingsWithFiles(Guid? recordId)
        {
            using (var context = new GeneralModelContainer())
            {
                var retTab =
                    context.SourceMappings.Where(o => o.Source.SourceId == recordId && o.File != null);

                return retTab.ToList();
            }
        }

        public IEnumerable<SourceMapping> GetBySourceTypesBySourceId2(Guid? recordId)
        {
            using (var context = new GeneralModelContainer())
            {
                var retTab =
                    context.SourceMappings.Where(o => o.Source.SourceId == recordId && o.SourceType != null);


                return retTab.ToList();
            }
        }


        #endregion 
    }
}
