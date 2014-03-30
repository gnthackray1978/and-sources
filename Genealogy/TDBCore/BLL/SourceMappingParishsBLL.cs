using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using System.Diagnostics;
//using TDBCore.Datasets.DsSourceMappingParishsTableAdapters;
//using TDBCore.Datasets;


namespace TDBCore.BLL
{
    public class SourceMappingParishsBll : BaseBll
    {
 
        public IQueryable<SourceMappingParish> GetData2()
        {

            return ModelContainer.SourceMappingParishs;
        }
         
        public IQueryable<SourceMappingParish> GetDataBySourceId2(Guid sourceId)
        {

            return ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId);
        }


        public List<Guid> GetParishIds(Guid sourceId)
        {
            return ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId).Select(p=>p.Parish.ParishId).ToList();
        }
         
        public IQueryable<SourceMappingParish> GetDataSourceMappingParishRowId2(int sourceMappingParishRowId)
        {

            return ModelContainer.SourceMappingParishs.Where(o => o.SourceMappingParishsRowId == sourceMappingParishRowId);
        }
         
        public int? InsertSourceMappingParish2(Guid SourceMappingParishId, Guid SourceMappingSourceId, int? userId)
        {
            SourceMappingParish smp = new SourceMappingParish();

            var source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == SourceMappingSourceId);
            var parish = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == SourceMappingParishId);

            if (source != null && parish != null)
            {
                smp.Parish = parish;
                smp.Source = source;
                smp.SourceMappingDateAdded = DateTime.Today;
                smp.SourceMappingUser = userId;


                ModelContainer.SourceMappingParishs.Add(smp);

                ModelContainer.SaveChanges();
            }

            return smp.SourceMappingParishsRowId;
             
        }
 

        public void UpdateSourceMappingParish2(int SourceMappingParishRowId, Guid SourceMappingParishId, Guid SourceMappingSourceId, int? userId)
        {

            SourceMappingParish smp = ModelContainer.SourceMappingParishs.FirstOrDefault(o => o.SourceMappingParishsRowId == SourceMappingParishRowId);

            var source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == SourceMappingSourceId);
            var parish = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == SourceMappingParishId);

            if (smp != null && source != null && parish != null)
            {
                smp.Parish = parish;
                smp.Source = source;
                smp.SourceMappingDateAdded = DateTime.Today;
                smp.SourceMappingUser = userId;
                ModelContainer.SaveChanges();
            }
            else
            {
                this.ErrorCondition = "Source Mapping Parish didnt update because the system couldnt find the SourceMappingParish, the Source or the Parish " + Environment.NewLine +
                    "SourceMappingParishRowId: " + SourceMappingParishRowId.ToString() + Environment.NewLine +
                    "sourcemappingparishid: " + SourceMappingParishId.ToString() + Environment.NewLine +
                    "sourcemappingsourceid: " + SourceMappingSourceId.ToString();
            }

        }
 


    }
}
