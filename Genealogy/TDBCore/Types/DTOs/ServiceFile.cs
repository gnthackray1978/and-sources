using System;
using TDBCore.Types.libs;

namespace TDBCore.Types.DTOs
{
    public class ServiceFile : ServiceBase
    {
        public Guid FileId { get; set; }
        public string FileDescription { get; set; }
        public string FileLocation { get; set; }
        public string FileThumbLocation { get; set; }

        public ServiceFile() {}

        public ServiceFile(string file, string desc, string id)
        {
            FileDescription = desc;
            FileId = id.ToGuid();
            FileLocation = file;

        }
    }
}