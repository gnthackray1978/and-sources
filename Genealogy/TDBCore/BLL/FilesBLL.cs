using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using TDBCore.Types.DTOs;

////using TDBCore.Datasets;


namespace TDBCore.BLL
{
    public class FilesBll : BaseBll
    {
 

        private string root = "";


        public FilesBll()
        {

        }
 

        public FilesBll(string rootLocation)
        {
            this.root = rootLocation;
        }

        public static string DirContentThumbs()
        {
            //needs trailing seperator
            return @"thumbs\";
        }

        public static string ImageLocationURL()
        {
            return "~/Images/content/";
        }

        public static string ImageLocationUNC()
        {
            return @"\Images\content\";
        }



        public Guid AddFile2(string description, string filePath, int userId, string thumbPath)
        {
            //Guid newId = System.Guid.NewGuid();

            //Adapter.Insert(newId, description, filePath, "", DateTime.Today, DateTime.Today, userId, thumbPath);

            //return newId;

            TDBCore.EntityModel.File _file = new TDBCore.EntityModel.File();

            _file.FileContent = "";
            _file.FileDate = DateTime.Today;
            _file.FileDescription = description;
            _file.FileEntryAdded = DateTime.Today;
            _file.FileLocation = filePath;
            _file.FilerUserAdded = userId;
            _file.FileThumbLocation = thumbPath;
            _file.FiletId = System.Guid.NewGuid();
            ModelContainer.Files.Add(_file);

            ModelContainer.SaveChanges();

            return _file.FiletId;
        }


        public void UpdateFile2(Guid fileId, string description, string filePath, int userId, string thumbPath)
        {
           // Adapter.Update(description, filePath, DateTime.Today, DateTime.Now, userId, thumbPath, fileId);

            var _file = ModelContainer.Files.FirstOrDefault(o => o.FiletId == fileId);

            if (_file != null)
            {
                _file.FileDescription = description;
                _file.FileLocation = filePath;
                _file.FilerUserAdded = userId;
                _file.FileThumbLocation = thumbPath;

                ModelContainer.SaveChanges();
            }
        }


        public void DeleteFile2(Guid fileId)
        {
            var _file = ModelContainer.Files.FirstOrDefault(o => o.FiletId == fileId);
            if (_file != null)
            {
                ModelContainer.Files.Remove(_file);
                ModelContainer.SaveChanges();
            }

        }


        public TDBCore.EntityModel.File GetFile2(Guid fileId)
        {

            var _file = ModelContainer.Files.Where(o => o.FiletId == fileId).FirstOrDefault();

        
            return _file;
        }

        public IQueryable<TDBCore.EntityModel.File> GetFileByName2(string filename)
        {
            return ModelContainer.Files.Where(o => o.FileLocation.Contains(filename));
        }

    

        public IQueryable<TDBCore.EntityModel.File> GetFiles2(string filename,
          string descrip,
          DateTime editDateUpper,
          DateTime editDateLower,
          DateTime addDateUpper,
          DateTime addDateLower)
        {
            filename = filename.Replace("%", "");
            descrip = descrip.Replace("%", "");

            return ModelContainer.Files.Where(f => f.FileDescription.Contains(descrip) && f.FileLocation.Contains(filename) && f.FileDate >= editDateLower && f.FileDate <= editDateUpper && f.FileEntryAdded >= addDateLower && f.FileEntryAdded <= addDateUpper);
        }



        public IQueryable<TDBCore.EntityModel.File> GetFiles2(string filename, string descrip)
        {
        
            DateTime editDateUpper = new DateTime(2050, 1, 1);
            DateTime editDateLower = new DateTime(2008, 1, 1);
            DateTime addDateLower = new DateTime(2008, 1, 1);
            DateTime addDateUpper = new DateTime(2050, 1, 1);

       

            return ModelContainer.Files.Where(f => f.FileDescription.Contains(descrip) && f.FileLocation.Contains(filename) && f.FileDate >= editDateLower && f.FileDate <= editDateUpper && f.FileEntryAdded >= addDateLower && f.FileEntryAdded <= addDateUpper);
        
        }



        public IQueryable<TDBCore.EntityModel.File> GetFilesByParentId2(Guid parentId)
        {
        
            return ModelContainer.Files.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == parentId));
        }

        public List<Guid> GetFileIdsByParent(Guid parentId)
        {

            return ModelContainer.Files.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == parentId)).Select(p=>p.FiletId).ToList();
        }

        public List<ServiceFile> GetFilesByParent(Guid parentId)
        {

            return ModelContainer.Files.Where(s => s.SourceMappings.Any(o => o.Source.SourceId == parentId)).Select(p => new ServiceFile()
                {
                    FileId = p.FiletId,
                    FileDescription =p.FileDescription,
                    FileLocation =  p.FileLocation          
                }).ToList();
        }
    }
}
