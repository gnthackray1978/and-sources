using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.DTOs
{
    public class FileBasicInfo
    {
        public Guid FileId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}
