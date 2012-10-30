using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

////using TDBCore.Datasets;
using TDBCore.EntityModel;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;
using TDBCore.BLL;



namespace GedItter.BLL
{
    public class ParishTranscriptionsBLL : BaseBLL
    {


        public IQueryable<ParishTranscriptionDetail> GetParishTranscriptionsByLoomes2( )
        {
            IQueryable<ParishTranscriptionDetail> parishTranscriptionDetailsDataTable = null;
            //
            parishTranscriptionDetailsDataTable = ModelContainer.ParishTranscriptionDetails.Where(o => o.ParishDataString.Contains("lymi"));

            return parishTranscriptionDetailsDataTable;
        }



        public IQueryable<ParishTranscriptionDetail> GetParishTranscriptionsByTranscriptionId2(int transcriptionId)
        {
            IQueryable<ParishTranscriptionDetail> parishTranscriptionDetailsDataTable = null;
            //
            parishTranscriptionDetailsDataTable = ModelContainer.ParishTranscriptionDetails.Where(o => o.ParishTranscriptionId == transcriptionId);

            return parishTranscriptionDetailsDataTable;
        }

        public IQueryable<ParishTranscriptionDetail> GetParishTranscriptionsByTranscriptionId2(Guid parishId)
        {
            IQueryable<ParishTranscriptionDetail> parishTranscriptionDetailsDataTable = null;
            //
            parishTranscriptionDetailsDataTable = ModelContainer.ParishTranscriptionDetails.Where(o => o.Parish.ParishId == parishId);

            return parishTranscriptionDetailsDataTable;
        }



        public int InsertTranscription2(Guid parishId, string transcriptionString)
        {
            //return Adapter.InsertTranscription(parishId, transcriptionString);

            var parish = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == parishId);


            if (parish != null)
            {
                ParishTranscriptionDetail parishTranscriptionDetail = new ParishTranscriptionDetail();
                parishTranscriptionDetail.Parish = parish;
                parishTranscriptionDetail.ParishDataString = transcriptionString;

                ModelContainer.ParishTranscriptionDetails.AddObject(parishTranscriptionDetail);
                ModelContainer.SaveChanges();

                return parishTranscriptionDetail.ParishTranscriptionId;
            }
            else
            {
                this.ErrorCondition = "couldnt add parishtranscript as couldnt find parish ";
                return 0;
            }
        }



        public void DeleteTranscription2(int transcriptionId)
        {
           // Adapter.Delete(transcriptionId);
            var ptrans = ModelContainer.ParishTranscriptionDetails.FirstOrDefault(o => o.ParishTranscriptionId == transcriptionId);

            if (ptrans != null)
            {
                ModelContainer.ParishTranscriptionDetails.DeleteObject(ptrans);
                ModelContainer.SaveChanges();
            }
        }
    }
}
