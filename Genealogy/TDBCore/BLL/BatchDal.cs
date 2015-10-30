using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.EntityModel;
using TDBCore.Interfaces;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.BLL
{
    public class BatchDal : BaseBll, IBatchDal
    {
        public Guid AddRecord(BatchDto batchDto)
        {
            
            var newBatch = new BatchLog
            {
                BatchId = batchDto.BatchId,
                Id = batchDto.Id,
                PersonId = batchDto.PersonId,
                MarriageId = batchDto.MarriageId,
                SourceId = batchDto.SourceId,
                ParishId = batchDto.ParishId,
                TimeRun = batchDto.TimeRun,
                IsDeleted = batchDto.IsDeleted,
                Ref = batchDto.Ref
            };

            ModelContainer.BatchLog.Add(newBatch);

            ModelContainer.SaveChanges();

            return newBatch.Id;
      }

        public void RemoveBatch(Guid batchId)
        {
            var batch = ModelContainer.BatchLog.Where(c => batchId == c.BatchId).ToList();

            foreach (var _batch in batch.Where(b => b != null))
            {
                var found = false;

                if (_batch.PersonId != null)
                {

                    var person = ModelContainer.Persons.FirstOrDefault(p => p.Person_id == _batch.PersonId);

                    if (person != null)
                    {
                        ModelContainer.Persons.Remove(person);
                        found = true;
                    }
                }

                if (_batch.MarriageId != null)
                {
                    var marriage = ModelContainer.Marriages.FirstOrDefault(p => p.Marriage_Id == _batch.MarriageId);

                    if (marriage != null)
                    {
                        ModelContainer.Marriages.Remove(marriage);
                        found = true;
                    }
                }

                if (found)
                {
                    _batch.IsDeleted = true;
                    ModelContainer.SaveChanges();
                }
                 
            }
        }

        public List<BatchDto> GetBatchsAndContents(BatchSearchFilter searchFilter)
        {

            //search filter current unused

            var retList = new List<BatchDto>();

            foreach (var b in ModelContainer.BatchLog.ToList()) {
                retList.Add(new BatchDto
                {
                    Id = b.Id,
                    BatchId = b.BatchId,
                    PersonId = b.PersonId,
                    MarriageId = b.MarriageId,
                    SourceId = b.SourceId,
                    ParishId = b.ParishId,
                    TimeRun = b.TimeRun,
                    Ref = b.Ref,
                    IsDeleted = b.IsDeleted.GetValueOrDefault()
                });
            }

            return retList;
        }

        public List<ShortBatch> GetBatchList(BatchSearchFilter searchFilter)
        {
            //search filter current unused
            return ModelContainer.BatchLog.ToList().GroupBy(g => g.BatchId).Select(b => new ShortBatch
            {
                BatchId = b.First().BatchId, TimeRun = b.First().TimeRun, Ref = b.First().Ref, IsDeleted = b.First().IsDeleted.GetValueOrDefault()
            }).ToList();
        }
    }
}
