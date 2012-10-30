using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
////using TDBCore.Datasets;
using TDBCore.EntityModel;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;

namespace TDBCore.BLL
{
    public class CountyDictionaryBLL : BaseBLL
    {

        public CountyDictionaryBLL()
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
