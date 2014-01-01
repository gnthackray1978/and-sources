using System.Collections.Generic;

namespace TDBCore.Types.DTOs
{
    //used with google maps


    public class ServiceEventObject : ServiceBase
    {
        public List<ServiceEvent> serviceEvents { get; set; }
        public int Batch { get; set; }
        public int Total { get; set; }
        public int BatchLength { get; set; }
    }


}

 