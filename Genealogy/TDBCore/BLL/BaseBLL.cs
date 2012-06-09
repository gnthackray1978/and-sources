using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDBCore.EntityModel;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace TDBCore.BLL
{
    public class BaseBLL
    {

        private string connectionString = "";
        private static TDBCore.EntityModel.GeneralModelContainer generalModelContainer = null; 

        public BaseBLL()
        {
            System.Data.EntityClient.EntityConnection connStr = this.GetConn();//new System.Data.EntityClient.EntityConnection(@"metadata=res://*/CustomSearches.csdl|res://*/CustomSearches.ssdl|res://*/CustomSearches.msl;provider=System.Data.SqlClient;provider connection string="";Data Source=GRN-P005718\;Initial Catalog=ThackrayDB;Integrated Security=True;MultipleActiveResultSets=True"";");

            if (BaseBLL.generalModelContainer == null)
            {
                Debug.WriteLine("Created new entity container: " + connStr);
                BaseBLL.generalModelContainer = new GeneralModelContainer(connStr);
            }
            
            
            connectionString = TDBCore.Properties.Settings.Default.ThackrayDBConnectionString;
        }


        public TDBCore.EntityModel.GeneralModelContainer ModelContainer
        {
            get
            {


                return BaseBLL.generalModelContainer;
            }
            set
            {
                BaseBLL.generalModelContainer = value;
            }
        }

        public bool LazyLoading
        {
            get
            {
                return this.ModelContainer.ContextOptions.LazyLoadingEnabled;
            }
            set
            {
                this.ModelContainer.ContextOptions.LazyLoadingEnabled = value;
            }
            
        }

        private System.Data.EntityClient.EntityConnection GetConn()
        {
            //  (@"metadata=res://*/CustomSearches.csdl|res://*/CustomSearches.ssdl|res://*/CustomSearches.msl;provider=System.Data.SqlClient;
            //provider connection string="";Data Source=GRN-P005718\;Initial Catalog=ThackrayDB;Integrated Security=True;MultipleActiveResultSets=True"";");


            //metadata=res://*/EntityModel.GeneralModel.csdl|res://*/EntityModel.GeneralModel.ssdl|
            //res://*/EntityModel.GeneralModel.msl;provider=System.Data.SqlClient;provider connection string=
            //&quot;Data Source=GEORGE-PC\SQLEXPRESS;Initial Catalog=ThackrayDB;Integrated Security=True;MultipleActiveResultSets=True&quot;"
  

          //  SqlConnectionStringBuilder sconStrBuilder = new SqlConnectionStringBuilder(TDBCore.Properties.Settings.Default.WorkConnString);




           // SqlConnectionStringBuilder sconStrBuilder = new SqlConnectionStringBuilder(TDBCore.Properties.Settings.Default.ThackrayDBConnectionString);
            
            //SqlServices
            //;User Id=myUsername;Password=myPassword;

          

            // looks for this stuff in the tdbcore entitymodel folder
            //string entConStr = @"metadata=res://*/TDBCore.EntityModel.GeneralModel.csdl|res://*/TDBCore.EntityModel.GeneralModel.ssdl|res://*/TDBCore.EntityModel.GeneralModel.msl;provider=System.Data.SqlClient;provider connection string="";Data Source=" +
            //    sconStrBuilder.DataSource + @";Initial Catalog=" + sconStrBuilder.InitialCatalog + @";Integrated Security=" + sconStrBuilder.IntegratedSecurity + @";MultipleActiveResultSets=True"";";

            string conString = ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString;

            SqlConnectionStringBuilder sconStrBuilder = new SqlConnectionStringBuilder(conString);
            string entConStr = @"metadata=res://*/EntityModel.GeneralModel.csdl|res://*/EntityModel.GeneralModel.ssdl|res://*/EntityModel.GeneralModel.msl;provider=System.Data.SqlClient;provider connection string="";Data Source=" +
              sconStrBuilder.DataSource + @";Initial Catalog=" + sconStrBuilder.InitialCatalog + @";Integrated Security="
              + sconStrBuilder.IntegratedSecurity + @";User Id=" + sconStrBuilder.UserID + @";Password=" + sconStrBuilder.Password + @";MultipleActiveResultSets=True"";";



            System.Data.EntityClient.EntityConnection connStr = new System.Data.EntityClient.EntityConnection(entConStr);



            return connStr;
        }

        private string errorCondition;

        public string ErrorCondition
        {
            get { return errorCondition; }
            set { errorCondition = value; }
        }

        public SqlConnection GetConnection()
        {
            SqlConnection SQLConnection = new SqlConnection();

            try
            {
                SQLConnection.ConnectionString = connectionString;
                SQLConnection.Open();

                // You can get the server version 
                // SQLConnection.ServerVersion
            }
            catch (Exception Ex)
            {
                // Try to close the connection
                if (SQLConnection != null)
                    SQLConnection.Dispose();

                // Create a (useful) error message
                string ErrorMessage = "A error occurred while trying to connect to the server.";
                ErrorMessage += Environment.NewLine;
                ErrorMessage += Environment.NewLine;
                ErrorMessage += Ex.Message;

                // Show error message (this = the parent Form object)


                // Stop here

            }
            return SQLConnection;
        }
    }
}
