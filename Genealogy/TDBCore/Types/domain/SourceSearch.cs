using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web.UI.WebControls;
using TDBCore.BLL;
using TDBCore.Interfaces;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace TDBCore.Types.domain
{
    public class SourceSearch 
    {
        readonly ISourceDal _sourceDal;
        readonly ISourceTypesDal _sourceTypesDal;
        readonly IFilesDal _filesDal;
        readonly ISourceMappingParishsDal _sourceMappingParishsDal;
        readonly ISourceMappingsDal _smDal;


        private readonly ISecurity _security;

               


        public SourceSearch(ISecurity security)
        {
            _sourceDal = new SourceDal();
            _sourceTypesDal = new SourceTypesDal();
            _filesDal = new FilesDal();
            _sourceMappingParishsDal = new SourceMappingParishsDal();
            _smDal = new SourceMappingsDal();
            _security = security;
        }


        public ServiceSourceObject Search(SourceSearchTypes param, SourceSearchFilter sourceSearchFilter,DataShaping shaper, IValidator validator = null)
        {
            var sourcesDataTable = new ServiceSourceObject();

            if(validator== null)
                validator = new Validator();

            if (!_security.IsvalidSelect()) throw new SecurityException("Missing select permission");

            if (!validator.ValidEntry()) throw new InvalidDataException(validator.GetErrors());

            switch (param)
            { 
                case SourceSearchTypes.Standard:
                    sourcesDataTable = _sourceDal.FillSourceTableByFilter(sourceSearchFilter, shaper).ToServiceSourceObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart, shaper.TotalRecords);
                    break;
                case SourceSearchTypes.Treesources:
                    sourcesDataTable = _sourceDal.FillTreeSources(sourceSearchFilter).ToServiceSourceObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart);
                    break;             
                case SourceSearchTypes.Censussource:
                    sourcesDataTable.CensusSources = _sourceDal.Get1841CensuSources(!sourceSearchFilter.Sources.IsNullOrBelowMinSize() ? sourceSearchFilter.Sources.First() : Guid.Empty);
                    break;
                case SourceSearchTypes.SourceIds:
                    sourcesDataTable = _sourceDal.FillSourceTableBySourceIds(!sourceSearchFilter.Sources.IsNullOrBelowMinSize() ? sourceSearchFilter.Sources : new List<Guid>())
                        .ToServiceSourceObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart);
                    break;
            }

            return sourcesDataTable;
        }

        public void DeleteRecords(SourceSearchFilter sourceSearchFilter)
        {
            if (!_security.IsValidDelete()) throw new SecurityException("Missing delete permission");
         
            sourceSearchFilter.Sources.ForEach(s => _sourceDal.DeleteSource2(s));
                       
        }

        public string AddSources(List<Guid> records, List<Guid> sources,SourceTypes sourceTypes)
        {

            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");

            if (sources.IsNullOrBelowMinSize(1))
            {
                throw new InvalidDataException("Invalid number of supplied sources");
            }


            switch (sourceTypes)
            {
                case SourceTypes.Person:
                    records.ForEach(p => _smDal.WritePersonSources2(p, sources, _security.UserId()));
                    break;
                case SourceTypes.Marriage:
                    records.ForEach(p => _smDal.WriteMarriageSources(p, sources, _security.UserId()));
                    break;
               
                default:
                    throw new ArgumentOutOfRangeException("sourceTypes");
            }

            return "";
        }

        public string RemoveTreeSources(List<Guid> records)
        {

            if (!_security.IsValidEdit()) return "You dont have permission to edit!";

            records.ForEach(p => _smDal.DeleteSourcesForPersonOrMarriage(p, 87));

            return "";
        }


        public void DeleteRecords(SourceDto sourceDto)
        {
            if (!_security.IsValidDelete()) throw new SecurityException("Missing delete permission");

            _sourceDal.DeleteSource2(sourceDto.SourceId);

        }

        public SourceDto Get(SourceDto sourceDto)
        {
            if (sourceDto == null)
                sourceDto = new SourceDto();

            if (!_security.IsvalidSelect()) throw new SecurityException("Missing select permission");

            sourceDto = _sourceDal.GetSource(sourceDto.SourceId);

            sourceDto.Files = _filesDal.GetFilesByParent(sourceDto.SourceId);

            sourceDto.SourceTypes = _sourceTypesDal.GetSourceTypeIds(sourceDto.SourceId);

            sourceDto.Parishs = _sourceMappingParishsDal.GetParishIds(sourceDto.SourceId);

            return sourceDto;
        }

        private void Edit(SourceDto sourceDto)
        {
            if (!_security.IsValidEdit()) throw new SecurityException("Missing edit permission");

            _sourceDal.UpdateSource(sourceDto);

            UpdateRelatedData(sourceDto);
        }

        private void Insert(SourceDto sourceDto)
        {

            if (!_security.IsValidInsert()) throw new SecurityException("Missing insert permission");

            _sourceDal.InsertSource(sourceDto);

            UpdateRelatedData(sourceDto);
        }

        private void UpdateRelatedData(SourceDto sourceDto)
        {
            _smDal.WriteFilesToSource(sourceDto.SourceId, sourceDto.Files, sourceDto.UserId);

            _smDal.WriteSourceTypesToSource(sourceDto.SourceId, sourceDto.SourceTypes, sourceDto.UserId);

            _smDal.WriteParishsToSource(sourceDto.SourceId, sourceDto.Parishs, sourceDto.UserId);
        }

        public void Update(SourceDto sourceDto, IValidator validator = null)
        {

            if (validator == null)
                validator = new Validator();

            if (!validator.ValidEntry()) throw new InvalidDataException(validator.GetErrors());

            if (sourceDto.SourceId == Guid.Empty)
            {
                Insert(sourceDto);
            }
            else
            {
                Edit(sourceDto);
            }

        }
    
    
    



    }


    
}


