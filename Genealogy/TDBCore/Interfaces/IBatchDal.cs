using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;

namespace TDBCore.Interfaces
{
    public interface IBatchDal
    {
        Guid AddRecord(BatchDto batchDto);

        void RemoveBatch(Guid batchId);

        List<BatchDto> GetBatchs(BatchSearchFilter searchFilter);

     
    }
}
