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
            using (var context = new GeneralModelContainer())
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

                context.BatchLog.Add(newBatch);

                context.SaveChanges();

                return newBatch.Id;
            }
        }

        public void RemoveBatch(Guid batchId)
        {
            using (var context = new GeneralModelContainer())
            {
                var batchLogs = context.BatchLog.Where(c => batchId == c.BatchId).ToList();

                foreach (var batchLog in batchLogs.Where(b => b != null))
                {
                    var found = false;

                    if (batchLog.PersonId != null)
                    {

                        var person = context.Persons.FirstOrDefault(p => p.Person_id == batchLog.PersonId);

                        if (person != null)
                        {
                            context.Persons.Remove(person);
                            found = true;
                        }
                    }

                    if (batchLog.MarriageId != null)
                    {
                        var marriage = context.Marriages.FirstOrDefault(p => p.Marriage_Id == batchLog.MarriageId);

                        if (marriage != null)
                        {
                            context.Marriages.Remove(marriage);
                            found = true;
                        }
                    }

                    if (found)
                    {
                        batchLog.IsDeleted = true;
                        context.SaveChanges();
                    }

                }
            }
        }

        public List<BatchDto> GetBatchsAndContents(BatchSearchFilter searchFilter)
        {
            using (var context = new GeneralModelContainer())
            {
                //search filter current unused

                return context.BatchLog.ToList().Select(b => new BatchDto
                {
                    Id = b.Id, BatchId = b.BatchId, PersonId = b.PersonId, MarriageId = b.MarriageId, SourceId = b.SourceId, ParishId = b.ParishId, TimeRun = b.TimeRun, Ref = b.Ref, IsDeleted = b.IsDeleted.GetValueOrDefault()
                }).ToList();
            }
        }

        public List<ShortBatch> GetBatchList(BatchSearchFilter searchFilter)
        {
            using (var context = new GeneralModelContainer())
            {
                //search filter current unused
                return context.BatchLog.ToList().GroupBy(g => g.BatchId).Select(b => new ShortBatch
                {
                    BatchId = b.First().BatchId,
                    TimeRun = b.First().TimeRun,
                    Ref = b.First().Ref,
                    IsDeleted = b.First().IsDeleted.GetValueOrDefault()
                }).ToList();
            }
        }
    }
}
