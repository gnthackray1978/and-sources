﻿using System;
using System.Collections.Generic;
using TDBCore.EntityModel;
using TDBCore.Types.DTOs;
using TDBCore.Types.filters;
using TDBCore.Types.libs;

namespace TDBCore.Interfaces
{
    public interface IParishsDal
    {
        List<CensusPlace> Get1841Census();
        List<ParishCounter> GetParishCounter();

        Guid AddParish(string parishName, string parishNotes, 
            string deposited, string parentParish, int startYear, string parishCounty, int endYear, decimal parishX, decimal parishY);

        Parish GetParishById2(Guid parishId);
        ServiceParish GetParishById(Guid parishId);
        List<ServiceParish> GetParishByFilter(ParishSearchFilter parishSearchFilter, DataShaping dataShaping);
        List<RectangleD> GetLocationList(string param);
        ParishCollection GetParishDetail(Guid parishId);
        List<SilverParish> GetParishsByLocationString(string locations);
        List<ParishDataType> GetParishTypes();
        List<SilverParish> GetParishsByLocation(List<RectangleD> locations);
        IEnumerable<Parish> GetParishsByLocationBox3(double xD, double yD, double boxlenD);
        List<string> GetParishNames(List<Guid> parishIds);
        IEnumerable<Parish> GetParishsByCounty2(string county);
        IEnumerable<ParishRecordSource> GetParishRecordSources();
        void DeleteParishs(List<Guid> parishIds);
        void UpdateParish(ServiceParish serviceParish);
        Guid InsertParish(ServiceParish serviceParish);
    }
}