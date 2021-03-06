﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;


namespace TDBCore.BLL
{
 

    public class MarriagesDal : BaseBll, IMarriagesDal
    {
        readonly SourceDal _sourceDal;

        public MarriagesDal()
        {
            Debug.WriteLine("hello");

            _sourceDal = new SourceDal();
            // this.Restart();
        }

        public Guid ReorderMarriages(Guid marriageId)
        {
            using (var context = new GeneralModelContainer())
            {

                var result = context.ReorderMarriages(marriageId);
                Guid retVal = Guid.Empty;


                foreach (var re in result.ToList())
                {
                    string c1 = re.Column1.ToString();

                    retVal = new Guid(c1);
                }

                return retVal;
            }
        }


        public string SwapSpouses(List<Guid> marriageIds)
        {
            using (var context = new GeneralModelContainer())
            {

                foreach (Guid marriageId in marriageIds)
                {
                    var result = context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

                    if (result != null)
                    {
                        var oCname = result.MaleCName;
                        var oSname = result.MaleSName;

                        result.MaleCName = result.FemaleCName;
                        result.MaleSName = result.FemaleSName;


                        result.FemaleCName = oCname;
                        result.FemaleSName = oSname;

                        //  ModelContainer.Marriages.Add(result);

                        context.SaveChanges();


                    }
                }

                return "Success";
            }
        }

        public List<Guid> GetDeletedMarriages()
        {
            using (var context = new GeneralModelContainer())
            {
                return context.Marriages.Where(m => m.IsDeleted == true).Select(p => p.Marriage_Id).ToList();
            }
        }
 
        public Guid GetMarriageUniqueRef(Guid marriageId)
        {
            using (var context = new GeneralModelContainer())
            {
                var a = context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId && m.IsDeleted == false);

                return a.UniqueRef.GetValueOrDefault();
            }
        }
        public IList<Guid> GetMarriageUniqueRefs(List<Guid> marriageIds, bool returnEmpty = false)
        {

            using (var context = new GeneralModelContainer())
            {
                var a =
                    context.Marriages.Where(m => marriageIds.Contains(m.Marriage_Id))
                        .ToList()
                        .Select(p => p.UniqueRef.GetValueOrDefault())
                        .ToList();

                return returnEmpty ? a.Where(m => m == Guid.Empty).ToList() : a;

            }
        }

        public ServiceMarriage GetMarriageById2(Guid marriageId)
        {
            using (var context = new GeneralModelContainer())
            {
                var ma =
                    context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId && m.IsDeleted == false) ??
                    new Marriage();

                return ma.ToServiceMarriage();
            }
        }

        public IList<Guid> GetMarriageIdsByUniqueRef(Guid uniqueRef)
        {
            using (var context = new GeneralModelContainer())
            {
                return context.Marriages.Where(m => m.UniqueRef == uniqueRef).Select(p => p.Marriage_Id).ToList();
            }
        }

        public IList<MarriageResult> GetDataByUniqueRef(Guid uniqueRef)
        {
            

            using (var context = new GeneralModelContainer())
            {
                IList<Marriage> retVal = context.Marriages.Where(m => m.UniqueRef == uniqueRef).ToList();

                var selectPred = new Func<Marriage, MarriageResult>(m => new MarriageResult()
                {
                    UniqueRefStr = m.UniqueRef.GetValueOrDefault(),
                    UniqueRef = m.UniqueRef.GetValueOrDefault(),
                    MarriageSource = m.Source,
                    MarriageId = m.Marriage_Id,
                    FemaleCName = m.FemaleCName,
                    FemaleSName = m.FemaleSName,
                    MaleCName = m.MaleCName,
                    MaleSName = m.MaleSName,
                    MarriageLocation = m.MarriageLocation,
                    MarriageTotalEvents = m.TotalEvents.Value,
                    MarriageYear = m.YearIntVal.GetValueOrDefault()
                });

                int idx = 0;
                while (idx < retVal.Count)
                {
                    retVal[idx].Source = _sourceDal.GetSourcesRef(retVal[idx].Marriage_Id);
                    idx++;
                }

                return retVal.Select(selectPred).ToList();
            }
        }

        public IList<Guid> GetDataByDupeRefByMarriageId(Guid marriageId)
        {
            using (var context = new GeneralModelContainer())
            {
                var marriage = context.Marriages.FirstOrDefault(p => p.Marriage_Id == marriageId);

                if (marriage != null)
                {


                    return
                        context.Marriages.Where(o => o.UniqueRef == marriage.UniqueRef && o.IsDeleted == false)
                            .Select(p => p.Marriage_Id)
                            .ToList();
                }

                return new List<Guid>();
            }
        }

        public List<MarriageResult> GetFilteredMarriages(MarriageSearchFilter m)
        {
            using (var context = new GeneralModelContainer())
            {
                if (m.Parish.ToGuid() != Guid.Empty)
                {
                    m.Source = "";

                    var sources =
                        context.Sources.Where(
                            o => o.SourceMappingParishs.Any(a => a.Parish.ParishId == m.Parish.ToGuid())).ToList();

                    if (sources.Count > 0)
                        sources.ForEach(p => m.Source += "," + p.SourceId.ToString());

                    m.Source = m.Source.Substring(1);
                }


                var result =
                    context.USP_Marriages_Filtered(m.Witness, m.Source, m.FemaleCName, m.FemaleSName, m.MaleCName,
                        m.MaleSName, m.County, m.Location, m.LowerDate,
                        m.UpperDate).Select(p => new MarriageResult()
                        {
                            FemaleCName = p.FemaleCName,
                            FemaleSName = p.FemaleSName,
                            MaleCName = p.MaleCName,
                            MaleSName = p.MaleSName,
                            MarriageId = p.Marriage_Id,
                            MarriageLocation = p.MarriageLocation,
                            MarriageSource = p.Source,
                            MarriageTotalEvents = p.TotalEvents.GetValueOrDefault(),
                            MarriageYear = p.YearIntVal.GetValueOrDefault(),
                            UniqueRef = p.UniqueRef.GetValueOrDefault(),
                            UniqueRefStr = p.UniqueRef.GetValueOrDefault(),
                            SourceTrees = p.links,
                            Notes = p.MaleInfo + Environment.NewLine + p.FemaleInfo
                        }).ToList();


                foreach (var mr in result)
                {
                    mr.Witnesses = string.Join(" ",
                        context.MarriageMapWitness.Where(mw => mw.Marriages.Marriage_Id == mr.MarriageId)
                            .Select(p => p.Persons.Surname)
                            .ToArray());
                }



                return result;
            }
        }

        public List<MarriageResult> GetByListId(List<Guid> marriageIds)
        {
            using (var context = new GeneralModelContainer())
            {
                var results = context.Marriages.Where(m => marriageIds.Contains(m.Marriage_Id)).ToList();

                return results.Select(r => new MarriageResult()
                {
                    MarriageId = r.Marriage_Id,
                    FemaleCName = r.FemaleCName,
                    FemaleSName = r.FemaleSName,
                    MaleCName = r.MaleCName,
                    MaleSName = r.MaleSName,
                    MarriageLocation = r.MarriageLocation,
                    MarriageTotalEvents = r.TotalEvents.GetValueOrDefault(),
                    MarriageYear = r.YearIntVal.GetValueOrDefault(),
                    UniqueRef = r.UniqueRef.GetValueOrDefault()
                }).ToList();
            }
        }

        public void MergeMarriages(Guid marriageToMergeIntoId, Guid marriageToMergeId)
        {
            using (var context = new GeneralModelContainer())
            {
                var m1 = context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageToMergeIntoId);

                var m2 = context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageToMergeId);

                m1.MergeInto(m2);


                context.SaveChanges();
            }
        }
   
        public Guid InsertMarriage(ServiceMarriage sm)
        {

            using (var context = new GeneralModelContainer())
            {
      


                var marriage = new Marriage();

                if (sm.MarriageId == Guid.Empty) marriage.Marriage_Id = Guid.NewGuid();

                marriage.MaleCName = sm.MaleCName;
                marriage.MaleSName = sm.MaleSName;
                marriage.MaleLocation = sm.MaleLocation;
                marriage.MaleInfo = sm.MaleNotes;
                marriage.FemaleId = Guid.Empty;
                marriage.FemaleCName = sm.FemaleCName;
                marriage.FemaleSName = sm.FemaleSName;
                marriage.FemaleLocation = sm.FemaleLocation;
                marriage.FemaleInfo = sm.FemaleNotes;
                marriage.Date = sm.MarriageDate;
                marriage.MarriageLocation = sm.MarriageLocation;
                marriage.YearIntVal = sm.MarriageDate.ParseToValidYear();
                marriage.MarriageCounty = sm.LocationCounty;
                marriage.Source = sm.SourceDescription;       
                marriage.IsLicence = sm.IsLicense;
                marriage.IsBanns = sm.IsBanns;
                marriage.MaleIsKnownWidower = sm.IsWidower;
                marriage.FemaleIsKnownWidow = sm.IsWidow;
                marriage.FemaleOccupation = sm.FemaleOccupation;
                marriage.MaleOccupation = sm.MaleOccupation;
                marriage.MarriageLocationId = sm.LocationId.ToGuid();
                marriage.MaleLocationId = Guid.Empty;
                marriage.FemaleLocationId = Guid.Empty;
                marriage.UserId = sm.UserId;
                marriage.MaleBirthYear = sm.MaleBirthYear;
                marriage.FemaleBirthYear = sm.FemaleBirthYear;
                marriage.UniqueRef = sm.UniqueRef.ToGuid() == Guid.Empty ? Guid.NewGuid() : sm.UniqueRef.ToGuid();
                marriage.TotalEvents = sm.TotalEvents.ToInt32();
                marriage.EventPriority = sm.Priority.ToInt32();
                marriage.Marriage_Id = System.Guid.NewGuid();
                marriage.IsDeleted = false;
                marriage.MaleId = Guid.Empty;
           
                marriage.IsComposite = false;
                marriage.DateAdded = DateTime.Today;
                marriage.DateLastEdit = DateTime.Today;
          
                context.Marriages.Add(marriage);


                context.SaveChanges();

                //ModelContainer.Detach(_marriage);

                return marriage.Marriage_Id;

            }
        }

        public void UpdateMarriageUniqRef(Guid marriageId,
                                Guid uniqueRef,
                                int totalEvents,
                                int eventPriority)
        {
            using (var context = new GeneralModelContainer())
            {

                var marriage = context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

                if (marriage == null) return;

                marriage.UniqueRef = uniqueRef;
                marriage.TotalEvents = totalEvents;
                marriage.EventPriority = eventPriority;

                context.SaveChanges();
            }
        }

        public void UpdateMarriage(ServiceMarriage serviceMarriage)
        {
            using (var context = new GeneralModelContainer())
            {

                context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                var marriage = context.Marriages.FirstOrDefault(m => m.Marriage_Id == serviceMarriage.MarriageId);

                if (marriage != null)
                {
                    marriage.MaleCName = serviceMarriage.MaleCName;
                    marriage.MaleSName = serviceMarriage.MaleSName;
                    marriage.MaleLocation = serviceMarriage.MaleLocation;
                    marriage.MaleInfo = serviceMarriage.MaleNotes;
                    marriage.FemaleId = Guid.Empty;
                    marriage.FemaleCName = serviceMarriage.FemaleCName;
                    marriage.FemaleSName = serviceMarriage.FemaleSName;
                    marriage.FemaleLocation = serviceMarriage.FemaleLocation;
                    marriage.FemaleInfo = serviceMarriage.FemaleNotes;
                    marriage.Date = serviceMarriage.MarriageDate;
                    marriage.MarriageLocation = serviceMarriage.MarriageLocation;
                    marriage.YearIntVal = serviceMarriage.MarriageDate.ParseToValidYear();
                    marriage.MarriageCounty = serviceMarriage.LocationCounty;
                    marriage.Source = serviceMarriage.SourceDescription;
                    marriage.IsLicence = serviceMarriage.IsLicense;
                    marriage.IsBanns = serviceMarriage.IsBanns;
                    marriage.MaleIsKnownWidower = serviceMarriage.IsWidower;
                    marriage.FemaleIsKnownWidow = serviceMarriage.IsWidow;
                    marriage.FemaleOccupation = serviceMarriage.FemaleOccupation;
                    marriage.MaleOccupation = serviceMarriage.MaleOccupation;
                    marriage.MarriageLocationId = serviceMarriage.LocationId.ToGuid();
                    marriage.MaleLocationId = serviceMarriage.MaleLocationId.ToGuid();
                    marriage.FemaleLocationId = serviceMarriage.FemaleLocationId.ToGuid();
                    marriage.MaleBirthYear = serviceMarriage.MaleBirthYear;
                    marriage.FemaleBirthYear = serviceMarriage.FemaleBirthYear;


                    context.SaveChanges();
                }
            }
        }

        public void DeleteMarriageTemp2(Guid marriageId)
        {

            using (var context = new GeneralModelContainer())
            {

                var marriage = context.Marriages.FirstOrDefault(m => m.Marriage_Id == marriageId);

                if (marriage != null)
                {
                    marriage.IsDeleted = true;
                    context.SaveChanges();
                }

            }
        }
  

  

    
    }
}
