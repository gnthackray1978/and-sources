using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using TDBCore.Datasets;
using TDBCore.EntityModel;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;
using TDBCore.BLL;


namespace TDBCore.BLL
{
    public class LocationDictionaryBLL : BaseBLL
    {
      
        public LocationDictionaryBLL()
        {

        }

      

        public LocationDictionary GetEntryByLocatAndCounty(string locat, string county)
        {
            LocationDictionary retVal = ModelContainer.LocationDictionaries.Where(o => o.LocationName == locat && o.LocationCounty == county).FirstOrDefault();

            return retVal;

        }
       

    }
}
