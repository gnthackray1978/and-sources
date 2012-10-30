using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RestURIMapping
/// </summary>
/// 

namespace ANDServices
{

    public static class URIMappingsDiag
    {

        //tree

        public const string GetJSONTreeSources = "/GetTreeSources/Select?0={description}&1={page_number}&2={page_size}";
        public const string GetJSONTreePersons = "/GetTreePersons/Select?0={sourceId}&1={start}&2={end}";


        public const string SetJSONTreeDefaultPerson = "/settreepersons/Set";//?0={sourceId}&1={personId}
        public const string SaveTree = "/SaveTree/Save";

        public const string GetTreeDiagPerson = "/Trees/GetTreeDiagPerson?0={treeId}&1={personId}";
        public const string GetTreeDiag = "/Trees/GetTreeDiag?0={treeId}";
        public const string DeleteTree = "/Trees/DeleteTree";
        public const string GetAncTreeDiag = "/Trees/GetAncTreeDiag?0={treeId}";

    }

}