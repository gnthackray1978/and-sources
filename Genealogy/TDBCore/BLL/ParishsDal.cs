using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using System.Diagnostics;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using ParishRecord = TDBCore.Types.DTOs.ParishRecord;


namespace TDBCore.BLL
{
    public class ParishsDal : BaseBll, IParishsDal
    {
             
        public List<CensusPlace> Get1841Census()
        {
            return ModelContainer.uvw_1841Census.ToList().Select(entry => new CensusPlace
                {
                    ParishId = entry.ParishId, PlaceName = entry.ParishName, LocX = entry.ParishX.GetValueOrDefault(), LocY = entry.ParishY.GetValueOrDefault()
                }).ToList();
        }


        public List<ParishCounter> GetParishCounter()
        {
            return ModelContainer.ParishCounter.ToList();
        }


        public Guid AddParish(string parishName, string parishNotes, 
            string deposited, string parentParish, int startYear, string parishCounty, int endYear, decimal parishX, decimal parishY)
        {

            Guid parishId = Guid.NewGuid();

            parishName = parishName.Trim();
            parentParish = parentParish.Trim();

            var parishs = ModelContainer.Parishs.Where(o => o.ParishName.ToLower().Contains(parishName) && o.ParishRegistersDeposited.ToLower().Contains(deposited.ToLower()));

    
            if (!parishs.Any())
            {
                var parish = new Parish
                {
                    ParishId = parishId,
                    ParentParish = parentParish,
                    ParishCounty = parishCounty,
                    ParishEndYear = endYear,
                    ParishName = parishName,
                    ParishNotes = parishNotes,
                    ParishRegistersDeposited = deposited,
                    ParishStartYear = startYear,
                    ParishX = parishX,
                    ParishY = parishY
                };

                ModelContainer.Parishs.Add(parish);
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
            Parish parishEntity = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == parishId);
             
            return parishEntity;
        }

        public ServiceParish GetParishById(Guid parishId)
        {
            var pe = ModelContainer.Parishs.FirstOrDefault(o => o.ParishId == parishId);

            if (pe == null) return new ServiceParish();

            var ret = new ServiceParish
                {
                    ParishCounty = pe.ParishCounty,
                    ParishDeposited = pe.ParishRegistersDeposited,
                    ParishId =  pe.ParishId,
                    ParishLat = (double)pe.ParishX.GetValueOrDefault(),
                    ParishLong = (double)pe.ParishY.GetValueOrDefault(),
                    ParishEndYear = pe.ParishEndYear.GetValueOrDefault(),
                    ParishStartYear = pe.ParishStartYear.GetValueOrDefault(),
                    ParishName = pe.ParishName,
                    ParishNote = pe.ParishNotes,
                    ParishParent = pe.ParentParish                       
                };

            return ret;
        }


        public List<ServiceParish> GetParishByFilter(ParishSearchFilter parishSearchFilter)
        {
            IQueryable<Parish> parishDataTable;

            if (parishSearchFilter.Name == "%" && parishSearchFilter.Deposited == "%" && parishSearchFilter.County == "%")
            {
                parishDataTable = ModelContainer.Parishs;
            }
            else
            {
                parishSearchFilter.Name = parishSearchFilter.Name.Replace('%', ' ').Trim();
                parishSearchFilter.Deposited = parishSearchFilter.Deposited.Replace('%', ' ').Trim();
                parishSearchFilter.County = parishSearchFilter.County.Replace('%', ' ').Trim();

                parishDataTable = ModelContainer.Parishs.Where(o => o.ParishName.Contains(parishSearchFilter.Name) &&
                    o.ParishRegistersDeposited.Contains(parishSearchFilter.Deposited) && o.ParishCounty.Contains(parishSearchFilter.County));
            }

            return parishDataTable.ToList().Select(p=> new ServiceParish
                {
                    ParishCounty = p.ParishCounty,
                    ParishDeposited = p.ParishRegistersDeposited,
                    ParishId = p.ParishId,
                    ParishEndYear = p.ParishEndYear.GetValueOrDefault(),
                    ParishStartYear = p.ParishStartYear.GetValueOrDefault(),
                    ParishName = p.ParishName,
                    ParishParent = p.ParentParish,
                    ParishNote = p.ParishNotes
                   

                }).ToList();
        }
      
 


        public List<RectangleD> GetLocationList(string param)
        {
            var locations = new List<RectangleD>();

            var parts = new List<string>(param.Split(','));
        

            var idx = 2;

            if (parts.Count <= 2) return locations;

            while (idx < parts.Count)
            {
                var rect = new RectangleD
                {
                    X = Convert.ToDouble(parts[idx - 2]),
                    Y = Convert.ToDouble(parts[idx - 1]),
                    Width = Convert.ToDouble(parts[idx]),
                    Height = Convert.ToDouble(parts[idx])
                };

                locations.Add(rect);
                idx += 3;
            }

            return locations;
        }

        public ParishCollection GetParishDetail(Guid parishId)
        {
            var parishCollection = new ParishCollection();


            var parishRecordsDataTable = ModelContainer.ParishRecords.Where(o => o.Parish.ParishId == parishId);
            ;// _parishRecordsBll.GetParishRecordsById2(parishId);

            parishCollection.parishRecords.Clear();
            parishCollection.parishTranscripts.Clear();

            foreach (var prr in parishRecordsDataTable)
            {
                var parishRecord = new ParishRecord();


                if (prr.ParishRecordSource.RecordTypeId == 1 || prr.ParishRecordSource.RecordTypeId == 2)
                {
                    var parishTranscript = new ParishTranscript
                    {
                        ParishTranscriptRecord =
                            prr.RecordType.ToUpper().Trim() + " " + prr.Year.ToString() + "-" + prr.YearEnd.ToString() +
                            " " + prr.ParishRecordSource.RecordTypeName.ToUpper(),
                        ParishId = prr.Parish.ParishId
                    };

                    parishCollection.parishTranscripts.Add(parishTranscript);
                }
                else
                {
                    parishRecord.dataType = prr.ParishRecordSource.RecordTypeId;
                    if (prr.YearEnd != null) parishRecord.endYear = prr.YearEnd.Value;
                    if (prr.Year != null) parishRecord.startYear = prr.Year.Value;
                    parishRecord.parishRecordType = prr.RecordType;
                    parishRecord.parishId = prr.Parish.ParishId;

                    parishCollection.parishRecords.Add(parishRecord);

                }
            }


            var parishTranscriptDataTable = ModelContainer.ParishTranscriptionDetails.Where(o => o.Parish.ParishId == parishId); 


            foreach (var ptranrow in parishTranscriptDataTable)
            {
                var parishTranscript = new ParishTranscript
                {
                    ParishTranscriptRecord = ptranrow.ParishDataString,
                    ParishId = ptranrow.Parish.ParishId
                };

                parishCollection.parishTranscripts.Add(parishTranscript);

            }


            return parishCollection;
        }
 
        public List<SilverParish> GetParishsByLocationString(string locations)
        {
            return GetParishsByLocation(GetLocationList(locations));
        }

        public List<ParishDataType> GetParishTypes()
        {
            return GetParishRecordSources().Select(prsr => new ParishDataType
            {
                dataTypeId = prsr.RecordTypeId, description = prsr.RecordTypeName
            }).ToList();
        }

        public List<SilverParish> GetParishsByLocation(List<RectangleD> locations)
        {
            var results = new List<SilverParish>();

            foreach (var rect in locations)
            {

                if(double.IsNaN(rect.X) ||
                    double.IsNaN(rect.Y) ||
                     double.IsNaN(rect.Height) ||
                        double.IsNaN(rect.Width)) continue;

                foreach (var group in GetParishsByLocationBox3(rect.X, rect.Y, rect.Width).GroupBy(x => new { x.ParishX, x.ParishY }))
                {

                    var order = 0;
                    var uniqId = Guid.NewGuid();
                    foreach (var row in group)
                    {
                        var parish = new SilverParish
                        {
                            County = row.ParishCounty,
                            Deposited = row.ParishRegistersDeposited,
                            Name = row.ParishName,
                            LocationCount = @group.Count(),
                            LocationOrder = order,
                            groupRef = uniqId.ToString(),
                            ParishId = row.ParishId
                        };

                        try
                        {

                            parish.ParishX = Convert.ToDouble(row.ParishX);
                            parish.ParishY = Convert.ToDouble(row.ParishY);
                        }
                        catch (Exception ex1)
                        {
                            Debug.WriteLine(ex1.Message);
                        }

                        results.Add(parish);
                        order++;
                    }

                }

            }


            return results;
        }

        public IQueryable<Parish> GetParishsByLocationBox3(double xD, double yD, double boxlenD)
        {            
            var x = Convert.ToDecimal(xD);
            var y = Convert.ToDecimal(yD);
            var boxlen = Convert.ToDecimal(boxlenD);

            var parishDataTable = ModelContainer.Parishs.Where(o => o.ParishX >= x
                                                                           && o.ParishX <= (x + boxlen)
                                                                           && o.ParishY >= y
                                                                           && o.ParishY <= (y + boxlen));

            return parishDataTable;
        }

        public List<string> GetParishNames(List<Guid> parishIds)
        {
            var parishDataTable = ModelContainer.Parishs.Where(p => parishIds.Contains(p.ParishId)).Select(s => s.ParishName).ToList();

            return parishDataTable;
        }

        public IQueryable<Parish> GetParishsByCounty2(string county)
        {
            var parishDataTable = ModelContainer.Parishs.Where(o => o.ParishCounty.Trim().ToLower().Contains(county.ToLower()));

            return parishDataTable;
        }

        public IQueryable<ParishRecordSource> GetParishRecordSources()
        {
            return ModelContainer.ParishRecordSources;
        }



        public void DeleteParishs(List<Guid> parishIds)
        {
            //hackray1978@gmail.com's googlecode.com password: SG2fZ8wM3MZ9 
            var customer = ModelContainer.Parishs.Where(c => parishIds.Contains(c.ParishId));

            foreach (var parish in customer.Where(parish => parish != null))
            {
                ModelContainer.Parishs.Remove(parish);
                ModelContainer.SaveChanges();
            }
        }

        public void UpdateParish(ServiceParish serviceParish)
        {
            var parish = ModelContainer.Parishs.First(o => o.ParishId == serviceParish.ParishId);

            if (parish != null)
            {
                parish.ParishName = serviceParish.ParishName;
                parish.ParentParish = serviceParish.ParishParent;
                parish.ParishCounty = serviceParish.ParishCounty;
                parish.ParishEndYear = serviceParish.ParishEndYear;
                parish.ParishStartYear = serviceParish.ParishStartYear;
                parish.ParishNotes = serviceParish.ParishNote;
                parish.ParishX = (decimal)serviceParish.ParishLat;
                parish.ParishY = (decimal)serviceParish.ParishLong;
                parish.ParishRegistersDeposited = serviceParish.ParishDeposited;
            }

            ModelContainer.SaveChanges();
        }

        public Guid InsertParish(ServiceParish serviceParish)
        {

            var parishId = Guid.NewGuid();
            serviceParish.ParishId = parishId;
            serviceParish.ParishName = serviceParish.ParishName.Trim();
            serviceParish.ParishParent = serviceParish.ParishParent.Trim();

            var parishs = ModelContainer.Parishs.Where(o => o.ParishName.ToLower().Contains(serviceParish.ParishName) && o.ParishRegistersDeposited.ToLower().Contains(serviceParish.ParishDeposited.ToLower()));


            if (!parishs.Any())
            {
                var parish = new Parish
                    {
                        ParishId = serviceParish.ParishId,
                        ParentParish = serviceParish.ParishParent,
                        ParishCounty = serviceParish.ParishCounty,
                        ParishEndYear = serviceParish.ParishEndYear,
                        ParishName = serviceParish.ParishName,
                        ParishNotes = serviceParish.ParishNote,
                        ParishRegistersDeposited = serviceParish.ParishDeposited,
                        ParishStartYear = serviceParish.ParishStartYear,
                        ParishX = (decimal)serviceParish.ParishLat,
                        ParishY = (decimal)serviceParish.ParishLong
                    };

                ModelContainer.Parishs.Add(parish);
            }
            else
            {
                parishId = parishs.First().ParishId;
            }


            ModelContainer.SaveChanges();

            return parishId;
        }
         
    }


}
