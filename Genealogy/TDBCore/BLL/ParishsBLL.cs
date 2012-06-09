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
using TDBCore.Types;


namespace GedItter.BLL
{
    public class ParishsBLL : BaseBLL
    {


        public List<ParishCounter> GetParishCounter()
        {
            return ModelContainer.ParishCounter.ToList();
        }


        public Guid AddParish(string parishName, string parishNotes, 
            string deposited, string parentParish, int startYear, string parishCounty, int endYear, decimal parishX, decimal parishY)
        {

            //SalesOrderDetail detail = new SalesOrderDetail
            //{
            //    SalesOrderID = 1,
            //    SalesOrderDetailID = 0,
            //    OrderQty = 2,
            //    ProductID = 750,
            //    SpecialOfferID = 1,
            //    UnitPrice = (decimal)2171.2942,
            //    UnitPriceDiscount = 0,
            //    LineTotal = 0,
            //    rowguid = Guid.NewGuid(),
            //    ModifiedDate = DateTime.Now
            //};



            Guid parishId = System.Guid.NewGuid();

            parishName = parishName.Trim();
            parentParish = parentParish.Trim();

            var parishs = ModelContainer.Parishs.Where(o => o.ParishName.ToLower().Contains(parishName) && o.ParishRegistersDeposited.ToLower().Contains(deposited.ToLower()));

    
            if (parishs.Count() == 0)
            {
                Parish _parish = new Parish();

                _parish.ParishId = parishId;
                _parish.ParentParish = parentParish;
                _parish.ParishCounty = parishCounty;
                _parish.ParishEndYear = endYear;
                _parish.ParishName = parishName;
                _parish.ParishNotes = parishNotes;
                _parish.ParishRegistersDeposited = deposited;
                _parish.ParishStartYear = startYear;
                _parish.ParishX = parishX;
                _parish.ParishY = parishY;

                ModelContainer.AddToParishs(_parish);
            }
            else
            {
                parishId = parishs.First().ParishId;
            }


            ModelContainer.SaveChanges();

            return parishId;
        }


        public Parish GetParishById2(Guid parishId)
        {           
            Parish parishEntity = ModelContainer.Parishs.Where(o => o.ParishId == parishId).FirstOrDefault();

            //if (parishEntity == null)
            //    parishEntity = new Parish();
 
            return parishEntity;
        }

      

        public IQueryable<Parish> GetParishByFilter2(string name, string deposited, string parentName)
        {
            IQueryable<Parish> parishDataTable = null;

            if (name == "%" && deposited == "%" && parentName == "%")
            {
                parishDataTable = ModelContainer.Parishs;
            }
            else
            { 
                parishDataTable = ModelContainer.Parishs.Where(o => o.ParishName.Contains(name) &&
                    o.ParishRegistersDeposited.Contains(deposited) && o.ParishCounty.Contains(parentName));            
            }

            return parishDataTable;
        }

      
        public IQueryable<Parish> GetParishByNameFilter2(string name)
        {
            IQueryable<Parish> parishDataTable = null;

            parishDataTable = ModelContainer.Parishs.Where(o=>o.ParishName.Contains(name));

            return parishDataTable;
        }
 
        public IQueryable<Parish> GetParishsByLocationBox2(double x, double y, double boxlen)
        {
            IQueryable<Parish> parishDataTable = null;

         
            SourceMappingParish smp = new SourceMappingParish();


            parishDataTable = ModelContainer.Parishs.Where(o => o.ParishX >= (decimal)x
                                                            && o.ParishX <= (decimal)(x + boxlen)
                                                            && o.ParishY >= (decimal)y
                                                            && o.ParishY <= (decimal)(y + boxlen));




            return parishDataTable;
        }


        public List<RectangleD> GetLocationList(string param)
        {
            List<RectangleD> locations = new List<RectangleD>();

            List<string> parts = new List<string>(param.Split(','));
        

            int idx = 2;

            if (parts.Count > 2)
            {

                while (idx < parts.Count)
                {
                    RectangleD _rect = new RectangleD();
                    _rect.X = Convert.ToDouble(parts[idx - 2]);
                    _rect.Y = Convert.ToDouble(parts[idx - 1]);
                    _rect.Width = Convert.ToDouble(parts[idx]);
                    _rect.Height = Convert.ToDouble(parts[idx]);

                    locations.Add(_rect);
                    idx += 3;
                }

         
            }

            return locations;
        }

        private string MakeLocation(double latx, double laty)
        {
            List<RectangleD> newArea = new List<RectangleD>();

            //double latx = 53.957700;
            // double laty = -1.082290;

            int xcount = 10;
            int ycount = 10;

            double boxlen = 0.1;
            //Math.Round(latx * 10, MidpointRounding.ToEven) / 10;

            latx = Math.Round((latx * 10), 0) / 10;
            laty = Math.Round((laty * 10), 0) / 10;



            int XIdx = 0;
            int YIdx = 0;

            latx -= boxlen * (xcount / 2);
            laty -= boxlen * (ycount / 2);

            while (YIdx < ycount)
            {
                XIdx = 0;

                while (XIdx < xcount)
                {


                    RectangleD _rec = new RectangleD(latx, laty);

                    //if (!downloadedArea.Contains(_rec))
                    newArea.Add(_rec);

                    latx += 0.1;
                    XIdx++;
                }


                laty += 0.1;

                YIdx++;
            }

            string locationString = "";
            foreach (RectangleD _lrect in newArea)
            {
                locationString += "," + _lrect.X + "," + _lrect.Y + "," + boxlen;
            }

            locationString = locationString.Remove(0, 1);

            return locationString;
        }

        public ParishCollection GetParishDetail(Guid _parishId)
        {
            ParishCollection parishCollection = new ParishCollection();

            BLL.ParishRecordsBLL parishRecordsBll = new BLL.ParishRecordsBLL();
            ParishTranscriptionsBLL parishTranBll = new ParishTranscriptionsBLL();


            var parishRecordsDataTable = parishRecordsBll.GetParishRecordsById2(_parishId);

            parishCollection.parishRecords.Clear();
            parishCollection.parishTranscripts.Clear();

            foreach (var prr in parishRecordsDataTable)
            {
                TDBCore.Types.ParishRecord parishRecord = new TDBCore.Types.ParishRecord();


                if (prr.ParishRecordSource.RecordTypeId == 1 || prr.ParishRecordSource.RecordTypeId == 2)
                {
                    ParishTranscript parishTranscript = new ParishTranscript();

                    parishTranscript.ParishTranscriptRecord = prr.RecordType.ToUpper().Trim() + " " + prr.Year.ToString() + "-" + prr.YearEnd.ToString() + " " + prr.ParishRecordSource.RecordTypeName.ToUpper();

                    parishTranscript.ParishId = prr.Parish.ParishId;

                    parishCollection.parishTranscripts.Add(parishTranscript);
                }
                else
                {
                    parishRecord.dataType = prr.ParishRecordSource.RecordTypeId;
                    parishRecord.endYear = prr.YearEnd.Value;
                    parishRecord.startYear = prr.Year.Value;
                    parishRecord.parishRecordType = prr.RecordType;
                    parishRecord.parishId = prr.Parish.ParishId;

                    parishCollection.parishRecords.Add(parishRecord);

                }
            }


            var parishTranscriptDataTable = parishTranBll.GetParishTranscriptionsByTranscriptionId2(_parishId);
            foreach (var ptranrow in parishTranscriptDataTable)
            {
                ParishTranscript parishTranscript = new ParishTranscript();

                parishTranscript.ParishTranscriptRecord = ptranrow.ParishDataString;
                parishTranscript.ParishId = ptranrow.Parish.ParishId;

                parishCollection.parishTranscripts.Add(parishTranscript);

            }


            return parishCollection;
        }

        public List<SourceRecord> GetParishSourceRecords(Guid _parishId)
        { 
            SourceBLL sourceBll = new SourceBLL();
              SourceTypesBLL _sourceTypes = new SourceTypesBLL();

            List<SourceRecord> sourceRecords = new List<SourceRecord> ();

            foreach (var srow in sourceBll.FillSourceTableByParishId2(_parishId))
            {
                SourceRecord sourceRecord = new SourceRecord();
                sourceRecord.SourceId = srow.SourceId;
                sourceRecord.IsCopyHeld = srow.IsCopyHeld.Value;
                sourceRecord.IsThackrayFound = srow.IsThackrayFound.Value;
                sourceRecord.IsViewed = srow.IsViewed.Value;
                sourceRecord.OriginalLocation = srow.OriginalLocation;
                sourceRecord.SourceDesc = srow.SourceDescription;
                sourceRecord.SourceRef = srow.SourceRef;
                sourceRecord.YearStart = srow.SourceDate.Value;
                sourceRecord.YearEnd = srow.SourceDateTo.Value;

                sourceRecord.sourceTYpes = _sourceTypes.GetSourceTypeBySourceId2(srow.SourceId).Select(st => st.SourceTypeId).ToList();

                // bit of a hack this
                // at some point revisit this and do it properly.
                foreach (int _type in sourceRecord.sourceTYpes)
                {
                    // parish regs
                    if (_type >= 40 && _type <= 43)
                        sourceRecord.DisplayOrder = 1;
                    // parish reg
                    if (_type == 1)
                        sourceRecord.DisplayOrder = 1;
                    // igi records
                    if (_type == 36)
                        sourceRecord.DisplayOrder = 2;
                    // wills etc
                    if (_type >= 2 && _type <= 33)
                        sourceRecord.DisplayOrder = 3;

                    if (_type >= 37 && _type <= 39)
                        sourceRecord.DisplayOrder = 3;

                }
             
                sourceRecords.Add(sourceRecord);
            }

            return sourceRecords;
        }

        public List<SilverParish> GetParishsByLocationString(string locations)
        {
            return GetParishsByLocation(GetLocationList(locations));
        }

        public List<ParishDataType> GetParishTypes()
        {
            List<ParishDataType> listParishDataType = new List<ParishDataType>();
       
            foreach (var prsr in this.GetParishRecordSources())
            {
                ParishDataType parishDataType = new ParishDataType();
                parishDataType.dataTypeId = prsr.RecordTypeId;
                parishDataType.description = prsr.RecordTypeName;
                listParishDataType.Add(parishDataType);
            }

            return listParishDataType;
        }

        public List<SilverParish> GetParishsByLocation(List<RectangleD> locations)
        {
            List<SilverParish> results = new List<SilverParish>();

            foreach (RectangleD _rect in locations)
            {

                if(double.IsNaN(_rect.X) ||
                    double.IsNaN(_rect.Y) ||
                     double.IsNaN(_rect.Height) ||
                        double.IsNaN(_rect.Width)) continue;

                foreach (var _group in this.GetParishsByLocationBox3(_rect.X, _rect.Y, _rect.Width).GroupBy(x => new { x.ParishX, x.ParishY }))
                {

                    int _order = 0;
                    Guid _uniqId = Guid.NewGuid();
                    foreach (var _row in _group)
                    {
                        SilverParish _parish = new SilverParish();
                        _parish.County = _row.ParishCounty;
                        _parish.Deposited = _row.ParishRegistersDeposited;
                        _parish.Name = _row.ParishName;
                        _parish.LocationCount = _group.Count();
                        _parish.LocationOrder = _order;
                        _parish.groupRef = _uniqId.ToString();
                        _parish.ParishId = _row.ParishId;

                        try
                        {

                            _parish.ParishX = Convert.ToDouble(_row.ParishX);
                            _parish.ParishY = Convert.ToDouble(_row.ParishY);
                        }
                        catch (Exception ex1)
                        {
                            Debug.WriteLine(ex1.Message);
                        }

                        results.Add(_parish);
                        _order++;
                    }

                }

            }


            return results;
        }

        public IQueryable<Parish> GetParishsByLocationBox3(double x_d, double y_d, double boxlen_d)
        {
            IQueryable<Parish> parishDataTable = null;


            SourceMappingParish smp = new SourceMappingParish();

            decimal x = Convert.ToDecimal(x_d);
            decimal y = Convert.ToDecimal(y_d);
            decimal boxlen = Convert.ToDecimal(boxlen_d);


            //Debug.WriteLine("searching for box:" + x.ToString() + "," + y.ToString() + "," + (x + boxlen).ToString() + "," + (y + boxlen).ToString());

            parishDataTable = ModelContainer.Parishs.Where(o => o.ParishX >= x
                                                            && o.ParishX <= (x + boxlen)
                                                            && o.ParishY >= y
                                                            && o.ParishY <= (y + boxlen));




            return parishDataTable;
        }

        public IQueryable<Parish> GetParishs2()
        {
            IQueryable<Parish> parishDataTable = null;

            parishDataTable = ModelContainer.Parishs;

            return parishDataTable;
        }

        public IQueryable<Parish> GetParishBySourceId2(Guid sourceId)
        {
            IQueryable<Parish> parishDataTable = null;

         
            parishDataTable = from c in ModelContainer.Parishs where c.SourceMappingParishs.Any(m => m.Source.SourceId == sourceId) select c;
         
            return parishDataTable;
        }

        public IQueryable<Parish> GetParishsByCounty2(string county)
        {
            IQueryable<Parish> parishDataTable = null;


            parishDataTable = ModelContainer.Parishs.Where(o => o.ParishCounty.Trim().ToLower().Contains(county.ToLower())); 

            return parishDataTable;
        }

        public IQueryable<ParishRecordSource> GetParishRecordSources()
        {
            return ModelContainer.ParishRecordSources;
        }

        public void DeleteParishById(Guid parishId)
        {
            //hackray1978@gmail.com's googlecode.com password: SG2fZ8wM3MZ9 
            Parish customer = ModelContainer.Parishs.First(c => c.ParishId == parishId);
            if (customer != null)
            {
                ModelContainer.DeleteObject(customer);
                ModelContainer.SaveChanges();
            } 

        }

        public void UpdateParish2(Guid parishId, string parishName,
           string parishNotes,
           string deposited,
           string parentParish,
           int startYear,
           int endYear,
           string parishCounty,
           decimal parishX,
           decimal parishY)
        {

       
            Parish _parish = ModelContainer.Parishs.First(o => o.ParishId == parishId);
       
            if (_parish != null)
            {
     
                _parish.ParishName = parishName;
                _parish.ParentParish = parentParish;
                _parish.ParishCounty = parishCounty;
                _parish.ParishEndYear = endYear;
                _parish.ParishStartYear = startYear;
                _parish.ParishNotes = parishNotes;
                _parish.ParishX = parishX;
                _parish.ParishY = parishY;
                _parish.ParishRegistersDeposited = deposited;


            }
            
            ModelContainer.SaveChanges();
        }
    }
}
