using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TancWebApp.Services.UriMappingConstants
{
    public static class BatchMappings
    {
        public const string GetBatches = "/GetBatches?0={batch_ref}&1={page_number}&2={page_size}&3={sort_col}";

        public const string AddPersons = "/AddPersons";

        public const string AddMarriages = "/AddMarriages";

        public const string AddParishs = "/AddParishs";

        public const string AddSources = "/AddSources";

        public const string RemoveBatch = "/RemoveBatch";
    }
}
