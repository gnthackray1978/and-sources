using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.security
{
    public class NoSecurity :ISecurity
    {
        public bool IsValidEdit()
        {
            return true;
        }

        public bool IsValidDelete()
        {
            return true;
        }

        public bool IsValidInsert()
        {
            return true;
        }

        public bool IsvalidSelect()
        {
            return true;
        }

        public int UserId()
        {
            return 1;
        }

        public string PermissionState()
        {
            return "";
        }
    }
}
