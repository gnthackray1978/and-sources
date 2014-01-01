using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.BLL;
 
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
        readonly SourceBll _sourceBll = new SourceBll();
        readonly SourceTypesBll _sourceTypesBll = new SourceTypesBll();
        readonly FilesBll _filesBll = new FilesBll();
        readonly SourceMappingParishsBll _sourceMappingParishsBll = new SourceMappingParishsBll();
        readonly SourceMappingsBll _smBll = new SourceMappingsBll();


        private readonly ISecurity _security;

               


        public SourceSearch(ISecurity security)
        {
          
            _security = security;
        }


        public ServiceSourceObject Search(SourceSearchTypes param, SourceSearchFilter sourceSearchFilter,DataShaping shaper, IValidator validator = null)
        {
            var sourcesDataTable = new ServiceSourceObject();

            if(validator== null)
                validator = new Validator();



            if (!_security.IsvalidSelect()) return sourcesDataTable;

            if (!validator.ValidEntry()) return sourcesDataTable;

            switch (param)
            { 
                case SourceSearchTypes.Standard:
                    sourcesDataTable = _sourceBll.FillSourceTableByFilter(sourceSearchFilter).ToServiceSourceObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart);
                    break;
                case SourceSearchTypes.Treesources:
                    sourcesDataTable = _sourceBll.FillTreeSources(sourceSearchFilter).ToServiceSourceObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart);
                    break;             
                case SourceSearchTypes.Censussource:
                    sourcesDataTable.CensusSources = _sourceBll.Get1841CensuSources(!sourceSearchFilter.Sources.IsNullOrBelowMinSize() ? sourceSearchFilter.Sources.First() : Guid.Empty);
                    break;
                case SourceSearchTypes.SourceIds:
                    sourcesDataTable = _sourceBll.FillSourceTableBySourceIds(!sourceSearchFilter.Sources.IsNullOrBelowMinSize() ? sourceSearchFilter.Sources : new List<Guid>())
                        .ToServiceSourceObject(shaper.Column, shaper.RecordPageSize, shaper.RecordStart);
                    break;
            }

            return sourcesDataTable;
        }

        public void DeleteRecords(SourceSearchFilter sourceSearchFilter)
        {
            if (!_security.IsValidDelete()) return;
         
            sourceSearchFilter.Sources.ForEach(s => _sourceBll.DeleteSource2(s));
                       
        }


        public void DeleteRecords(SourceDto sourceDto)
        {
            if (!_security.IsValidDelete()) return;

            _sourceBll.DeleteSource2(sourceDto.SourceId);

        }

        public SourceDto Get(SourceDto sourceDto)
        {
            if (sourceDto == null)
                sourceDto = new SourceDto();

            if (!_security.IsvalidSelect()) return sourceDto;

            sourceDto = _sourceBll.GetSource(sourceDto.SourceId);

            sourceDto.Files = _filesBll.GetFilesByParent(sourceDto.SourceId);

            sourceDto.SourceTypes = _sourceTypesBll.GetSourceTypeIds(sourceDto.SourceId);

            sourceDto.Parishs = _sourceMappingParishsBll.GetParishIds(sourceDto.SourceId);

            return sourceDto;
        }

        private void Edit(SourceDto sourceDto)
        {
            if (!_security.IsValidEdit()) return;

            _sourceBll.UpdateSource(sourceDto);

            UpdateRelatedData(sourceDto);
        }

        private void Insert(SourceDto sourceDto)
        {

            if (!_security.IsValidInsert()) return;

            UpdateRelatedData(sourceDto);
        }

        private void UpdateRelatedData(SourceDto sourceDto)
        {
            _smBll.WriteFilesToSource(sourceDto.SourceId, sourceDto.Files, sourceDto.UserId);

            _smBll.WriteSourceTypesToSource(sourceDto.SourceId, sourceDto.SourceTypes, sourceDto.UserId);

            _smBll.WriteParishsToSource(sourceDto.SourceId, sourceDto.Parishs, sourceDto.UserId);
        }

        public void Update(SourceDto sourceDto, IValidator validator = null)
        {

            if (validator == null)
                validator = new Validator();

            if (!validator.ValidEntry()) return;

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


