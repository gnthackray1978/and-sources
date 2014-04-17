using System;
using System.Collections.Generic;
using System.Linq;
using TDBCore.BLL;
 
using TDBCore.Types.DTOs;
 
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace TDBCore.Types.domain
{
    public class MarriageSearch 
    {
        readonly MarriagesDal _marriagesDll = new MarriagesDal();
        readonly MarriageWitnessesDal _marriageWitnessesDll = new MarriageWitnessesDal();
        readonly SourceDal _sourceDll = new SourceDal();
        readonly SourceMappingsDal _sourceMappingsDll = new SourceMappingsDal();
        readonly MarriageWitnessesDal _marriageWitnessDal = new MarriageWitnessesDal();
        readonly SourceMappingsDal _sourceMappingsDal = new SourceMappingsDal();
        readonly PersonDal _personDal = new PersonDal();



        private readonly ISecurity _security;
 
        public MarriageSearch(ISecurity security)
        {
            _security = security;
        }



        public ServiceMarriageObject Search(MarriageFilterTypes filterMode, MarriageSearchFilter marriageSearchFilter, DataShaping shaping, IValidator validator = null)
        {     
            var serviceMarriageObject = new ServiceMarriageObject();

            if (!_security.IsvalidSelect())  serviceMarriageObject.ErrorStatus = "Invalid permission to select";

            if (validator != null && !validator.ValidEntry())  serviceMarriageObject.ErrorStatus += " Validation failed: " + validator.GetErrors();

            if (serviceMarriageObject.ErrorStatus.Length == 0)
            {

                if (shaping.Column.Contains("MarriageDate DESC"))
                {
                    shaping.Column = "MarriageYear DESC";
                }
                else if (shaping.Column.Contains("MarriageDate"))
                {
                    shaping.Column = "MarriageYear";
                }


                switch (filterMode)
                {
                    case MarriageFilterTypes.Duplicates:
                        serviceMarriageObject = marriageSearchFilter.ParentId != Guid.Empty
                                                    ? _marriagesDll.GetDataByUniqueRef(marriageSearchFilter.ParentId)
                                                                   .ToServiceMarriageObject(shaping.Column, shaping.RecordPageSize,
                                                                                            shaping.RecordStart)
                                                    : new ServiceMarriageObject();
                        break;
                    case MarriageFilterTypes.Standard:
                        serviceMarriageObject =
                            _marriagesDll.GetFilteredMarriages(marriageSearchFilter)
                                         .ToServiceMarriageObject(shaping.Column, shaping.RecordPageSize, shaping.RecordStart);
                        break;
                }

            }
            return serviceMarriageObject;
        }



        public void SetMergeSources(Guid marriageId)
        {
            if (marriageId == Guid.Empty) return;

            var sourceList = new List<Guid>();
            var witnessList = new List<MarriageWitness>();

            witnessList.AddRange(_marriageWitnessesDll.GetWitnessesForMarriage(marriageId));

            foreach (var dupePerson in _marriagesDll.GetDataByDupeRefByMarriageId(marriageId))
            {
                sourceList.AddRange(_sourceDll.FillSourceTableByPersonOrMarriageId2(dupePerson).Select(dp => dp.SourceId).ToList());

                witnessList.AddRange(_marriageWitnessesDll.GetWitnessesForMarriage(dupePerson));


                _marriagesDll.MergeMarriages(marriageId, dupePerson);

            }

            _marriageWitnessesDll.DeleteWitnessesForMarriage(marriageId);

            witnessList.RemoveDuplicates();

            _marriageWitnessesDll.InsertWitnessesForMarriage(marriageId, witnessList);

            _sourceMappingsDll.WriteMarriageSources(marriageId, sourceList, 1);
        }

        public void SetReorderDupes(Guid marriageId )
        {
            if (marriageId != Guid.Empty)
                _marriagesDll.ReorderMarriages(marriageId);
        }


        public void SwitchSpouses(List<Guid> marriageIds)
        {
            if (!marriageIds.IsNullOrEmpty())
                _marriagesDll.SwapSpouses(marriageIds);
        }

        public string RemoveTreeSources(Guid marriageId)
        {

            if (!_security.IsValidEdit()) return "You dont have permission to edit!";

            _sourceMappingsDal.DeleteSourcesForPersonOrMarriage(marriageId, 87);//.WritePersonSources2(personId, sources, _security.UserId());

            return "";
        }


        /// <summary>
        /// Deletes selected record(s) then calls refresh
        /// </summary>
        public void DeleteRecords(List<Guid> marriages)
        {
            if (!_security.IsValidDelete()) return;

            marriages.ForEach(p => _marriagesDll.DeleteMarriageTemp2(p));
         
        }

        public void UpdateDeletedMarriages()
        {
            foreach (var marriageId in _marriagesDll.GetDeletedMarriages())
            {
                var peopleToKeep = new List<Guid>();

                var uniqueRef = _marriagesDll.GetMarriageUniqueRef(marriageId);

                peopleToKeep.AddRange(_marriagesDll.GetMarriageIdsByUniqueRef(uniqueRef));
            
                Guid newRef = Guid.NewGuid();

                int evtCount = 1;
                foreach (var id in peopleToKeep)
                {

                    _marriagesDll.UpdateMarriageUniqRef(id, newRef, peopleToKeep.Count, evtCount);
                    evtCount++;
                }

                newRef = Guid.NewGuid();
                _marriagesDll.UpdateMarriageUniqRef(marriageId, newRef, 1, 1);
            }

        }

        /// <summary>
        /// selected records must all be duplicate
        /// function removes the first record from the selected records list
        /// </summary>
        public void SetRemoveSelectedFromDuplicateList(List<Guid> marriages)
        {
            // marriage filter screen display the marriage starting at zero!!!!!
            int evtCount = 0;

            if (!_security.IsValidEdit()) return;

            if (marriages.Count <= 0) return;

            var selectedRecord = marriages[0];

            var marriagesToKeep = _marriagesDll.GetDataByDupeRefByMarriageId(selectedRecord).Where(groupedMarriage => !marriages.Contains(groupedMarriage)).ToList();

            Guid newRef = Guid.NewGuid();
         
            marriagesToKeep.ForEach(p =>
                {
                    _marriagesDll.UpdateMarriageUniqRef(p, newRef, marriagesToKeep.Count, evtCount);
                    evtCount++;
                });
           
            // records to remove done here
            foreach (Guid marriageToRemove in marriages)
            {
                newRef = Guid.NewGuid();
                _marriagesDll.UpdateMarriageUniqRef(marriageToRemove, newRef, 1, 0);
            }

 
        }

 

        public void SetSelectedDuplicateMarriage(List<Guid> marriages)
        {
            Guid newRef = Guid.NewGuid();
            int evtCount = 0;

            if (!_security.IsValidEdit()) return;


            if (marriages.Count <= 1) return;



            var marriageList = _marriagesDll.GetMarriageUniqueRefs(marriages);

            var listToUpdate = marriageList.Where(m => m == Guid.Empty).ToList();
                              
            foreach (var dupeMarriage in marriageList.Where(m => m != Guid.Empty))
            {
                listToUpdate.AddRange(_marriagesDll.GetMarriageIdsByUniqueRef(dupeMarriage));
                    
            }

            listToUpdate.ForEach(m =>
            {
                _marriagesDll.UpdateMarriageUniqRef(m, newRef, listToUpdate.Count, evtCount);
                evtCount++;
            });

        }



        public ServiceMarriage Get(Guid marriageId)
        {
            var marriage = new ServiceMarriage();


            if (!_security.IsvalidSelect()) return marriage;

            if (marriageId == Guid.Empty) return marriage;

            //get wits
            marriage = _marriagesDll.GetMarriageById2(marriageId);
            marriage.Sources = _sourceMappingsDal.GetSourceGuidList(marriageId);

            var mw = _marriageWitnessDal.GetWitnessesForMarriage(marriageId);

            mw.PopulateServiceMarriage(marriage);


            return marriage;
        }



        private void Edit(ServiceMarriage pmarriage, List<Guid> sources, List<MarriageWitness> witnesses)
        {

            _marriagesDll.UpdateMarriage(pmarriage);

            _sourceMappingsDal.WriteMarriageSources(pmarriage.MarriageId, sources, 1);

            SetWitnesses(pmarriage.MarriageId, witnesses);


        }

        private void Insert(ServiceMarriage pmarriage, List<Guid> sources, List<MarriageWitness> witnesses)
        {
            if (! _security.IsValidInsert()) return;

            if (pmarriage.TotalEvents.ToInt32() == 0 && pmarriage.Priority.ToInt32() == 0)
                pmarriage.TotalEvents = "1";

            pmarriage.MarriageId = _marriagesDll.InsertMarriage(pmarriage);

            if(sources.Count>0)
                _sourceMappingsDal.WriteMarriageSources(pmarriage.MarriageId, sources, 1);


            SetWitnesses(pmarriage.MarriageId, witnesses);
        }


        private void SetWitnesses(Guid marriageId,  List<MarriageWitness> witnesses)
        {

            //delete existing entries
            _marriageWitnessDal.DeleteWitnessesForMarriage(marriageId);

            foreach (var marriages in witnesses)
            {
                _personDal.Insert(marriages.Person);
            }

            _marriageWitnessDal.InsertWitnessesForMarriage(marriageId, witnesses);

        }

        public void Save(ServiceMarriage pmarriage, List<Guid> sources, List<MarriageWitness> witnesses, IValidator marriageValidation)
        {
            if (!marriageValidation.ValidEntry()) return;

            if (!_security.IsValidEdit()) return;

            if (pmarriage.MarriageId == Guid.Empty)
            {
                Insert(pmarriage,sources, witnesses);
            }
            else
            {
                Edit(pmarriage,sources, witnesses);
            }

        }
    }

   

}
