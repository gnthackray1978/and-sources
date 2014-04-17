using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;



namespace TDBCore.BLL
{
    public class SourceMappingParishsDal : BaseBll
    {
        
        public List<Guid> GetParishIds(Guid sourceId)
        {
            return ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId).Select(p=>p.Parish.ParishId).ToList();
        }        
         
        public int? InsertSourceMappingParish2(Guid sourceMappingParishId, Guid sourceMappingSourceId, int? userId)
        {
            var smp = new SourceMappingParish();

            var source = ModelContainer.Sources.FirstOrDefault(o => o.SourceId == sourceMappingSourceId);
            var parish = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == sourceMappingParishId);

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
 
    }
}
