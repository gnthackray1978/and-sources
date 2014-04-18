using System;
using System.Collections.Generic;
using TDBCore.Types.DTOs;

namespace TDBCore.BLL
{
    public interface IMarriageWitnessesDal
    {
        List<MarriageWitness> GetWitnessesForMarriage(Guid marriageId);
        string GetWitnesseStringForMarriage(Guid marriageId);
        void InsertWitnessesForMarriage(Guid marriageId, IList<MarriageWitness> persons);
        void DeleteWitnessesForMarriage(Guid marriageId);
    }
}