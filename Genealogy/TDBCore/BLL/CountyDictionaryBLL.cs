using System.Linq;
using TDBCore.EntityModel;

namespace TDBCore.BLL
{
    public class CountyDictionaryBll : BaseBll
    {

        public CountyDictionaryBll()
        {

        }

       


     

        public IQueryable<CountyDictionary> GetDictionary2()
        {
            return ModelContainer.CountyDictionaries;
        }

        public CountyDictionary GetDictionary2(string birthLocation)
        {

            CountyDictionary entry = ModelContainer.CountyDictionaries.Where(cd => cd.dictPlace.Contains(birthLocation)).FirstOrDefault();

            if (entry != null)
            {
                return entry;
            }
            else
            {
                return new CountyDictionary();
            }
        }
    }
}
