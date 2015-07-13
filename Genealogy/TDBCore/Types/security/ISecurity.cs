using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDBCore.Types.security
{
    public interface ISecurity
    {
         bool IsValidEdit();
         bool IsValidDelete();
         bool IsValidInsert();     
         bool IsvalidSelect();
         int UserId();
         string PermissionState();
    }

    public interface IUser
    {
        string GetUser();
    }
}
