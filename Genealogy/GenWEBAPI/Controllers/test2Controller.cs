﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using GenOnline.Helpers;
using GenOnline.Services.UriMappingConstants;
using TDBCore.BLL;
using TDBCore.Types.domain;
using TDBCore.Types.DTOs;
using TDBCore.Types.enums;
using TDBCore.Types.filters;
using TDBCore.Types.libs;
using TDBCore.Types.security;
using TDBCore.Types.validators;

namespace GenWEBAPI.Controllers
{

    //test comment

    public class TestController2 : ApiController
    {

        private readonly MarriageSearch _marriageSearch;

        public TestController2(IMarriagesDal iMarriagesDal,
            IMarriageWitnessesDal iMarriageWitnessesDal,
            ISourceDal iSourceDal,
            ISourceMappingsDal iSourceMappingsDal,
            IPersonDal iPersonDal,
            ISecurity iSecurity)
        {
            _marriageSearch = new MarriageSearch(iSecurity,
                iMarriagesDal,
                iMarriageWitnessesDal, iSourceDal, iSourceMappingsDal, iPersonDal);
        }
        
    }
}