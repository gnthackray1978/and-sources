using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.Types.DTOs;

 
namespace TDBCore.BLL
{
    public class FilesDal : BaseBll, IFilesDal
    {  
        public Guid AddFile2(string description, string filePath, int userId, string thumbPath)
        {            
            var file = new EntityModel.File
            {
                FileContent = "",
                FileDate = DateTime.Today,
                FileDescription = description,
                FileEntryAdded = DateTime.Today,
                FileLocation = filePath,
                FilerUserAdded = userId,
                FileThumbLocation = thumbPath,
                FiletId = Guid.NewGuid()
            };

            ModelContainer.Files.Add(file);

            ModelContainer.SaveChanges();

            return file.FiletId;
        }

        public IQueryable<EntityModel.File> GetFilesByParentId2(Guid parentId)
        {        
            return ModelContainer.Files.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == parentId));
        }

        public List<ServiceFile> GetFilesByParent(Guid parentId)
        {
            return ModelContainer.Files.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == parentId)).Select(p => new ServiceFile
                {
                    FileId = p.FiletId,
                    FileDescription =p.FileDescription,
                    FileLocation =  p.FileLocation          
                }).ToList();
        }
    }
}
