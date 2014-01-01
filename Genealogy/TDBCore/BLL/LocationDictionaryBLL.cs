using System.Linq;
using TDBCore.EntityModel;


namespace TDBCore.BLL
{
    public class LocationDictionaryBll : BaseBll
    {
      
        public LocationDictionaryBll()
        {

        }

      

        public LocationDictionary GetEntryByLocatAndCounty(string locat, string county)
        {
            LocationDictionary retVal = ModelContainer.LocationDictionaries.Where(o => o.LocationName == locat && o.LocationCounty == county).FirstOrDefault();

            return retVal;

        }
       

    }
}
