namespace TancWebApp.URIMappings
{

    public static class UriMappingsDiag
    {

        //tree

        public const string GetJsonTreeSources = "/GetTreeSources/Select?0={description}&1={pageNumber}&2={pageSize}";
        public const string GetJsonTreePersons = "/GetTreePersons/Select?0={sourceId}&1={start}&2={end}";


        public const string SetJsonTreeDefaultPerson = "/settreepersons/Set";//?0={sourceId}&1={personId}
        public const string SaveTree = "/SaveTree/Save";

        public const string GetTreeDiagPerson = "/Trees/GetTreeDiagPerson?0={treeId}&1={personId}";
        public const string GetTreeDiag = "/Trees/GetTreeDiag?0={treeId}";
        public const string DeleteTree = "/Trees/DeleteTree";
        public const string GetAncTreeDiag = "/Trees/GetAncTreeDiag?0={treeId}";

    }

}