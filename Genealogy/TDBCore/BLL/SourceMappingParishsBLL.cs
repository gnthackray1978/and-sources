﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TDBCore.Datasets.DsSourceMappingParishsTableAdapters;
//using TDBCore.Datasets;
using TDBCore.EntityModel;
using TDBCore.BLL;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;


namespace GedItter.BLL
{
    public class SourceMappingParishsBLL : BaseBLL
    {
 
        public IQueryable<SourceMappingParish> GetData2()
        {

            return ModelContainer.SourceMappingParishs;
        }
         
        public IQueryable<SourceMappingParish> GetDataBySourceId2(Guid sourceId)
        {

            return ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId);
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


                ModelContainer.SourceMappingParishs.AddObject(smp);

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
 
        public void DeleteBySourceIdAndParishId2(Guid sourceId, Guid parishId)
        {

            var ent = ModelContainer.SourceMappingParishs.Where(o => o.Source.SourceId == sourceId && o.Parish.ParishId == parishId).FirstOrDefault();


            if (ent != null)
            {
                // because source mappings are added and removed in disconnected state from the db
                // and the source mapping might not be in the entity model by this point BUT it could still be in the db

                if (ModelContainer.SourceMappingParishs.Where(sm => sm.SourceMappingParishsRowId == ent.SourceMappingParishsRowId).Count() > 0)
                    ModelContainer.DeleteObject(ent);


                ModelContainer.SaveChanges();
            }
   
        }
 

        public void DeleteBySourceMappingParishsRowId2(int sourceMappingParishsRowId)
        {
    
            ModelContainer.SaveChanges();
            SourceMappingParish ent = ModelContainer.SourceMappingParishs.FirstOrDefault(o => o.SourceMappingParishsRowId == sourceMappingParishsRowId);

            if (ent != null)
            {
                ModelContainer.DeleteObject(ent);
                ModelContainer.SaveChanges();
            }
            else
            {
                Debug.Assert(false);
            }
        }

    }
}
