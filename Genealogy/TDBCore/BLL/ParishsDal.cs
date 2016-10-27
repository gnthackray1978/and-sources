using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.EntityModel;
using System.Diagnostics;
using TDBCore.Interfaces;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using ParishRecord = TDBCore.Types.DTOs.ParishRecord;


namespace TDBCore.BLL
{
    public class ParishsDal : BaseBll, IParishsDal
    {
             
        public List<CensusPlace> Get1841Census()
        {
            using (var context = new GeneralModelContainer())
            {
                return context.uvw_1841Census.ToList().Select(entry => new CensusPlace
                {
                    ParishId = entry.ParishId,
                    PlaceName = entry.ParishName,
                    LocX = entry.ParishX.GetValueOrDefault(),
                    LocY = entry.ParishY.GetValueOrDefault()
                }).ToList();
            }
        }


        public List<ParishCounter> GetParishCounter()
        {
            using (var context = new GeneralModelContainer())
            {
                return context.ParishCounter.ToList();
            }
        }


        public Guid AddParish(string parishName, string parishNotes, 
            string deposited, string parentParish, int startYear, string parishCounty, int endYear, decimal parishX, decimal parishY)
        {

            Guid parishId = Guid.NewGuid();

            parishName = parishName.Trim();
            parentParish = parentParish.Trim();

            using (var context = new GeneralModelContainer())
            {
                var parishs =
                    context.Parishs.Where(
                        o =>
                            o.ParishName.ToLower().Contains(parishName) &&
                            o.ParishRegistersDeposited.ToLower().Contains(deposited.ToLower()));


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

                    context.Parishs.Add(parish);
                }
                else
                {
                    parishId = parishs.First().ParishId;
                }


                context.SaveChanges();

                return parishId;
            }
        }


        public Parish GetParishById2(Guid parishId)
        {
            using (var context = new GeneralModelContainer())
            {
                var parishEntity = context.Parishs.FirstOrDefault(o => o.ParishId == parishId);

                return parishEntity;
            }
        }

        public ServiceParish GetParishById(Guid parishId)
        {
            using (var context = new GeneralModelContainer())
            {
                var pe = context.Parishs.FirstOrDefault(o => o.ParishId == parishId);

                if (pe == null) return new ServiceParish();

                var ret = new ServiceParish
                {
                    ParishCounty = pe.ParishCounty,
                    ParishDeposited = pe.ParishRegistersDeposited,
                    ParishId = pe.ParishId,
                    ParishLat = (double) pe.ParishX.GetValueOrDefault(),
                    ParishLong = (double) pe.ParishY.GetValueOrDefault(),
                    ParishEndYear = pe.ParishEndYear,
                    ParishStartYear = pe.ParishStartYear,
                    ParishName = pe.ParishName,
                    ParishNote = pe.ParishNotes,
                    ParishParent = pe.ParentParish
                };

                return ret;
            }
        }


        public List<ServiceParish> GetParishByFilter(ParishSearchFilter parishSearchFilter, DataShaping dataShaping)
        {
            using (var context = new GeneralModelContainer())
            {
                IQueryable<Parish> parishDataTable;

                if (parishSearchFilter.Name == "%" && parishSearchFilter.Deposited == "%" &&
                    parishSearchFilter.County == "%")
                {
                    parishDataTable = context.Parishs;
                }
                else
                {
                    parishSearchFilter.Name = parishSearchFilter.Name.Replace('%', ' ').Trim();
                    parishSearchFilter.Deposited = parishSearchFilter.Deposited.Replace('%', ' ').Trim();
                    parishSearchFilter.County = parishSearchFilter.County.Replace('%', ' ').Trim();

                    parishDataTable =
                        context.Parishs.Where(o => o.ParishName.Contains(parishSearchFilter.Name) &&
                                                          o.ParishRegistersDeposited.Contains(
                                                              parishSearchFilter.Deposited) &&
                                                          o.ParishCounty.Contains(parishSearchFilter.County));
                }

                dataShaping.TotalRecords = parishDataTable.Count();

                return
                    parishDataTable.OrderBy(o => o.ParishName)
                        .Skip(dataShaping.RecordStart*dataShaping.RecordPageSize)
                        .Take(dataShaping.RecordPageSize)
                        .Select(p => new ServiceParish
                        {
                            ParishCounty = p.ParishCounty,
                            ParishDeposited = p.ParishRegistersDeposited,
                            ParishId = p.ParishId,
                            ParishEndYear = p.ParishEndYear,
                            ParishStartYear = p.ParishStartYear,
                            ParishName = p.ParishName,
                            ParishParent = p.ParentParish,
                            ParishNote = p.ParishNotes


                        }).ToList();
            }
            //serviceParishObject.serviceParishs.Skip(shaper.RecordStart * shaper.RecordPageSize).Take(shaper.RecordPageSize).ToList();
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

            using (var context = new GeneralModelContainer())
            {
                var parishCollection = new ParishCollection();


                var parishRecordsDataTable = context.ParishRecords.Where(o => o.Parish.ParishId == parishId);
                ; // _parishRecordsBll.GetParishRecordsById2(parishId);

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
                                prr.RecordType.ToUpper().Trim() + " " + prr.Year.ToString() + "-" +
                                prr.YearEnd.ToString() +
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


                var parishTranscriptDataTable =
                    context.ParishTranscriptionDetails.Where(o => o.Parish.ParishId == parishId);


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

        public IEnumerable<Parish> GetParishsByLocationBox3(double xD, double yD, double boxlenD)
        {
            using (var context = new GeneralModelContainer())
            {
                var x = Convert.ToDecimal(xD);
                var y = Convert.ToDecimal(yD);
                var boxlen = Convert.ToDecimal(boxlenD);

                var parishDataTable = context.Parishs.Where(o => o.ParishX >= x
                                                                        && o.ParishX <= (x + boxlen)
                                                                        && o.ParishY >= y
                                                                        && o.ParishY <= (y + boxlen));

                return parishDataTable;
            }
        }

        public List<string> GetParishNames(List<Guid> parishIds)
        {
            using (var context = new GeneralModelContainer())
            {
                var parishDataTable =
                    context.Parishs.Where(p => parishIds.Contains(p.ParishId)).Select(s => s.ParishName).ToList();

                return parishDataTable;
            }
        }

        public IEnumerable<Parish> GetParishsByCounty2(string county)
        {
            using (var context = new GeneralModelContainer())
            {
                var parishDataTable =
                    context.Parishs.Where(o => o.ParishCounty.Trim().ToLower().Contains(county.ToLower()));

                return parishDataTable.ToList();
            }
        }

        public IEnumerable<ParishRecordSource> GetParishRecordSources()
        {
            using (var context = new GeneralModelContainer())
            {
                return context.ParishRecordSources;
            }
        }



        public void DeleteParishs(List<Guid> parishIds)
        {
            using (var context = new GeneralModelContainer())
            {
                var customer = context.Parishs.Where(c => parishIds.Contains(c.ParishId) && c != null).ToList();

                foreach (var parish in customer)
                {
                    context.Parishs.Remove(parish);
                    context.SaveChanges();
                }
            }

        }

        public void UpdateParish(ServiceParish serviceParish)
        {
            using (var context = new GeneralModelContainer())
            {
                var parish = context.Parishs.First(o => o.ParishId == serviceParish.ParishId);

                if (parish != null)
                {
                    parish.ParishName = serviceParish.ParishName;
                    parish.ParentParish = serviceParish.ParishParent;
                    parish.ParishCounty = serviceParish.ParishCounty;
                    parish.ParishEndYear = serviceParish.ParishEndYear;
                    parish.ParishStartYear = serviceParish.ParishStartYear;
                    parish.ParishNotes = serviceParish.ParishNote;
                    parish.ParishX = (decimal) serviceParish.ParishLat;
                    parish.ParishY = (decimal) serviceParish.ParishLong;
                    parish.ParishRegistersDeposited = serviceParish.ParishDeposited;
                }

                context.SaveChanges();
            }
        }

        public Guid InsertParish(ServiceParish serviceParish)
        {
            using (var context = new GeneralModelContainer())
            {
                var parishId = Guid.NewGuid();
                serviceParish.ParishId = parishId;
                serviceParish.ParishName = serviceParish.ParishName.Trim();
                serviceParish.ParishParent = serviceParish.ParishParent.Trim();

                var parishs =
                    context.Parishs.Where(
                        o =>
                            o.ParishName.ToLower().Contains(serviceParish.ParishName) &&
                            o.ParishRegistersDeposited.ToLower().Contains(serviceParish.ParishDeposited.ToLower()));


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
                        ParishX = (decimal) serviceParish.ParishLat,
                        ParishY = (decimal) serviceParish.ParishLong
                    };

                    context.Parishs.Add(parish);
                }
                else
                {
                    parishId = parishs.First().ParishId;
                    serviceParish.ParishId = parishId;
                }


                context.SaveChanges();

                return parishId;
            }
        }
         
    }


}
