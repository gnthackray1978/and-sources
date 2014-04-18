using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.Types.DTOs;

namespace TDBCore.BLL
{
    public interface IFilesDal
    {
        Guid AddFile2(string description, string filePath, int userId, string thumbPath);
        IQueryable<EntityModel.File> GetFilesByParentId2(Guid parentId);
        List<ServiceFile> GetFilesByParent(Guid parentId);
    }
}