using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;



namespace TDBCore.BLL
{
    public class SourceMappingParishsDal : BaseBll, ISourceMappingParishsDal
    {
        
        public List<Guid> GetParishIds(Guid sourceId)
        {
            using (var context = new GeneralModelContainer())
            {
                return
                    context.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId)
                        .Select(p => p.Parish.ParishId)
                        .ToList();
            }
        }        
         
        public int? InsertSourceMappingParish2(Guid sourceMappingParishId, Guid sourceMappingSourceId, int? userId)
        {
            using (var context = new GeneralModelContainer())
            {
                var smp = new SourceMappingParish();

                var source = context.Sources.FirstOrDefault(o => o.SourceId == sourceMappingSourceId);
                var parish = context.Parishs.FirstOrDefault(o => o.ParishId == sourceMappingParishId);

                if (source == null || parish == null) return smp.SourceMappingParishsRowId;

                smp.Parish = parish;
                smp.Source = source;
                smp.SourceMappingDateAdded = DateTime.Today;
                smp.SourceMappingUser = userId;


                context.SourceMappingParishs.Add(smp);

                context.SaveChanges();

                return smp.SourceMappingParishsRowId;
            }
        }
 
    }
}
