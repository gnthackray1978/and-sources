using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBCore.BLL;
using TDBCore.Interfaces;

namespace CSVImporter
{
    public class SourcePersonWriter
    {
        private readonly ISourceMappingsDal _sourceMappingDal;
        private readonly IPersonDal _personDal;
        private readonly IBatchDal _batchDal;
        private readonly ISourceDal _sourceDal;

        List<Source> _sources = new List<Source>();

        public SourcePersonWriter(List<Source> sources)
        {
            _sources = sources;             
            _batchDal = new BatchDal();
            _sourceDal = new SourceDal();      
            _sourceMappingDal = new SourceMappingsDal();
            _personDal = new PersonDal();         
        }

        public void WriteToDB()
        {
            
            //


            foreach (var s in _sources)
            {
                var source = _sourceDal.FillSourceTableById2(s.SourceId);

                if (source != null)
                {

                }
                else
                {
                    source = new TDBCore.EntityModel.Source();

                    source.SourceDate = s.FromYear;
                    source.SourceDateTo = s.ToYear;
                    source.SourceDateStr = s.From;
                    source.SourceDateStrTo = s.To;
                    source.SourceRef = s.SourceRef;
                    source.OriginalLocation = s.Location;
                    source.DateAdded = DateTime.Now;
                    source.SourceId = s.SourceId;



                }


            }
        }


    }
}
