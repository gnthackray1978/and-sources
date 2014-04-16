using System;
using TDBCore.EntityModel;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace TDBCore.BLL
{
    public class BaseBll
    {
        private string _errorCondition;
        private string _connectionString = "";
        private static GeneralModelContainer _generalModelContainer; 

        public BaseBll()
        {
            //System.Data.EntityClient.EntityConnection connStr = GetConn();//new System.Data.EntityClient.EntityConnection(@"metadata=res://*/CustomSearches.csdl|res://*/CustomSearches.ssdl|res://*/CustomSearches.msl;provider=System.Data.SqlClient;provider connection string="";Data Source=GRN-P005718\;Initial Catalog=ThackrayDB;Integrated Security=True;MultipleActiveResultSets=True"";");
            
            if (_generalModelContainer == null)
            {
               // Debug.WriteLine("Created new entity container: " + connStr);
                _generalModelContainer = new GeneralModelContainer();

              //  _generalModelContainer.Database.Connection.ConnectionString = GetNormalConn();

                //BaseBll.generalModelContainer.Configuration.
            }

            
            _connectionString = Properties.Settings.Default.ThackrayDBConnectionString;
        }


        public void Reset()
        {
          //  System.Data.EntityClient.EntityConnection connStr = this.GetConn();//new System.Data.EntityClient.EntityConnection(@"metadata=res://*/CustomSearches.csdl|res://*/CustomSearches.ssdl|res://*/CustomSearches.msl;provider=System.Data.SqlClient;provider connection string="";Data Source=GRN-P005718\;Initial Catalog=ThackrayDB;Integrated Security=True;MultipleActiveResultSets=True"";");

            if (_generalModelContainer == null)
            {
                //Debug.WriteLine("Created new entity container: " + connStr);
                _generalModelContainer = new GeneralModelContainer();

              //  _generalModelContainer.Database.Connection.ConnectionString = GetNormalConn();
            }


            _connectionString = Properties.Settings.Default.ThackrayDBConnectionString;

        }

        public GeneralModelContainer ModelContainer
        {
            get
            {
                return _generalModelContainer;
            }
            set
            {
                _generalModelContainer = value;
            }
        }

      

        public string ErrorCondition
        {
            get { return _errorCondition; }
            set { _errorCondition = value; }
        }

        public SqlConnection GetConnection()
        {
            var SQLConnection = new SqlConnection();

            try
            {
                SQLConnection.ConnectionString = _connectionString;
                SQLConnection.Open();

                // You can get the server version 
                // SQLConnection.ServerVersion
            }
            catch (Exception Ex)
            {
                // Try to close the connection
                if (SQLConnection != null)
                    SQLConnection.Dispose();              
            }
            return SQLConnection;
        }
    }
}
